using System.Text.RegularExpressions;
using ApiDocGen.Models.Responses;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ApiDocGen.Services;

public class AnalysisService : IAnalysisService
{
    private readonly ILogger<AnalysisService> _logger;

    private static readonly string[] HttpMethodAttributes =
        ["HttpGet", "HttpPost", "HttpPut", "HttpDelete", "HttpPatch", "HttpHead", "HttpOptions"];

    private Dictionary<string, ClassDeclarationSyntax> _typeRegistry = new();

    public AnalysisService(ILogger<AnalysisService> logger) => _logger = logger;

    public async Task<AnalysisResult> AnalyzeRepositoryAsync(string localPath, string repoUrl)
    {
        var csFiles = Directory.GetFiles(localPath, "*.cs", SearchOption.AllDirectories)
            .Where(f => !f.Contains("obj") && !f.Contains("bin"))
            .ToList();

        _logger.LogInformation("Analyzing {Count} .cs files", csFiles.Count);

        _typeRegistry = await BuildTypeRegistryAsync(csFiles);
        _logger.LogInformation("Type registry built with {Count} types", _typeRegistry.Count);

        var routes = new List<RouteInfo>();
        var controllerNames = new HashSet<string>();

        foreach (var file in csFiles)
        {
            var code = await File.ReadAllTextAsync(file);
            var tree = CSharpSyntaxTree.ParseText(code);
            var root = await tree.GetRootAsync();

            var controllers = root.DescendantNodes()
                .OfType<ClassDeclarationSyntax>()
                .Where(IsController);

            foreach (var controller in controllers)
            {
                controllerNames.Add(controller.Identifier.Text);
                var controllerRoute = ExtractControllerRoute(controller);
                var extracted = ExtractRoutes(controller, controllerRoute);
                routes.AddRange(extracted);
            }
        }

        var minimalApiRoutes = await ExtractMinimalApiRoutesAsync(csFiles);
        routes.AddRange(minimalApiRoutes);

        var apiType = (routes.Count > 0 && minimalApiRoutes.Count > 0) ? "Mixed"
            : minimalApiRoutes.Count > 0 ? "Minimal API"
            : "Controller-based";

        return new AnalysisResult
        {
            RepoUrl = repoUrl,
            ProjectName = Path.GetFileName(localPath),
            AnalyzedAt = DateTime.UtcNow,
            Routes = routes,
            Metadata = new AnalysisMetadata
            {
                TotalRoutes = routes.Count,
                TotalControllers = controllerNames.Count,
                DetectedFrameworks = DetectFrameworks(localPath),
                ApiType = apiType
            }
        };
    }

    private async Task<Dictionary<string, ClassDeclarationSyntax>> BuildTypeRegistryAsync(
        List<string> csFiles)
    {
        var registry = new Dictionary<string, ClassDeclarationSyntax>();

        foreach (var file in csFiles)
        {
            var code = await File.ReadAllTextAsync(file);
            var tree = CSharpSyntaxTree.ParseText(code);
            var root = await tree.GetRootAsync();

            var classes = root.DescendantNodes().OfType<ClassDeclarationSyntax>();
            foreach (var cls in classes)
            {
                var name = cls.Identifier.Text;
                if (!registry.ContainsKey(name))
                    registry[name] = cls;
            }

            var records = root.DescendantNodes().OfType<RecordDeclarationSyntax>();
            foreach (var record in records)
            {
                var name = record.Identifier.Text;
                if (!registry.ContainsKey(name))
                {
                   
                    registry[$"record:{name}"] = null!;
                }
            }
        }

        return registry;
    }

    private List<Models.Responses.PropertyInfo> ResolveTypeProperties(
        string typeName, int depth = 0)
    {
        if (depth > 3) return [];

        var inner = UnwrapGenericType(typeName);
        if (inner != typeName)
            return ResolveTypeProperties(inner, depth);

        if (IsPrimitiveType(typeName)) return [];

        if (!_typeRegistry.TryGetValue(typeName, out var classDef) || classDef == null)
            return [];

        var properties = new List<Models.Responses.PropertyInfo>();

        var propDeclarations = classDef.Members
            .OfType<PropertyDeclarationSyntax>()
            .Where(p => p.Modifiers.Any(m => m.IsKind(SyntaxKind.PublicKeyword)));

        foreach (var prop in propDeclarations)
        {
            var propTypeName = prop.Type.ToString();
            var isRequired = !propTypeName.EndsWith("?")
                && prop.Modifiers.Any(m => m.ToString() == "required");
            var summary = ExtractXmlSummaryFromNode(prop);

            var nestedProperties = IsPrimitiveType(UnwrapGenericType(propTypeName))
                ? []
                : ResolveTypeProperties(UnwrapGenericType(propTypeName), depth + 1);

            properties.Add(new Models.Responses.PropertyInfo
            {
                Name = prop.Identifier.Text,
                Type = propTypeName,
                IsRequired = isRequired,
                Summary = summary,
                NestedProperties = nestedProperties
            });
        }

        var fieldDeclarations = classDef.Members
            .OfType<FieldDeclarationSyntax>()
            .Where(f => f.Modifiers.Any(m => m.IsKind(SyntaxKind.PublicKeyword)));

        foreach (var field in fieldDeclarations)
        {
            var fieldTypeName = field.Declaration.Type.ToString();
            foreach (var variable in field.Declaration.Variables)
            {
                properties.Add(new Models.Responses.PropertyInfo
                {
                    Name = variable.Identifier.Text,
                    Type = fieldTypeName,
                    IsRequired = false,
                    NestedProperties = []
                });
            }
        }

        return properties;
    }

    private static string UnwrapGenericType(string typeName)
    {
        
        var match = Regex.Match(typeName,
            @"^(?:List|IList|IEnumerable|ICollection|Task|ActionResult|IActionResult)<(.+)>$");
        return match.Success ? match.Groups[1].Value.Trim() : typeName;
    }

    private static bool IsPrimitiveType(string typeName)
    {
        var primitives = new HashSet<string>
        {
            "int", "long", "short", "byte", "float", "double", "decimal", "bool",
            "string", "char", "Guid", "DateTime", "DateTimeOffset", "TimeSpan",
            "int?", "long?", "bool?", "double?", "decimal?", "Guid?", "DateTime?",
            "object", "dynamic", "void", "IActionResult", "ActionResult"
        };
        return primitives.Contains(typeName);
    }

    private static bool IsController(ClassDeclarationSyntax cls)
    {
        var name = cls.Identifier.Text;
        if (name.EndsWith("Controller")) return true;
        return cls.AttributeLists
            .SelectMany(al => al.Attributes)
            .Any(a => a.Name.ToString() is "ApiController" or "Controller");
    }

    private static string ExtractControllerRoute(ClassDeclarationSyntax controller)
    {
        var routeAttr = controller.AttributeLists
            .SelectMany(al => al.Attributes)
            .FirstOrDefault(a => a.Name.ToString().StartsWith("Route"));

        if (routeAttr?.ArgumentList?.Arguments.FirstOrDefault()?.Expression
            is LiteralExpressionSyntax lit)
        {
            var route = lit.Token.ValueText;
            var controllerName = controller.Identifier.Text.Replace("Controller", "");
            return route.Replace("[controller]", controllerName.ToLower());
        }

        var name = controller.Identifier.Text.Replace("Controller", "").ToLower();
        return $"api/{name}";
    }

    private List<RouteInfo> ExtractRoutes(
        ClassDeclarationSyntax controller, string controllerRoute)
    {
        var routes = new List<RouteInfo>();
        var controllerName = controller.Identifier.Text;
        var methods = controller.Members.OfType<MethodDeclarationSyntax>();

        foreach (var method in methods)
        {
            var httpAttr = method.AttributeLists
                .SelectMany(al => al.Attributes)
                .FirstOrDefault(a => HttpMethodAttributes.Any(
                    h => a.Name.ToString().StartsWith(h)));

            if (httpAttr == null) continue;

            var httpVerb = HttpMethodAttributes.FirstOrDefault(
                h => httpAttr.Name.ToString().StartsWith(h))
                ?.Replace("Http", "").ToUpper() ?? "GET";

            var actionRoute = httpAttr.ArgumentList?.Arguments
                .FirstOrDefault()?.Expression is LiteralExpressionSyntax routeLit
                ? routeLit.Token.ValueText
                : string.Empty;

            var fullPath = string.IsNullOrEmpty(actionRoute)
                ? $"/{controllerRoute}"
                : $"/{controllerRoute}/{actionRoute}".Replace("//", "/");

            var summary = ExtractXmlSummary(method);
            var parameters = ExtractParameters(method);
            var requestBody = ExtractRequestBody(method);  
            var responses = ExtractResponses(method);       

            var attributes = method.AttributeLists
                .SelectMany(al => al.Attributes)
                .Select(a => a.Name.ToString())
                .Where(a => !HttpMethodAttributes.Any(h => a.StartsWith(h)))
                .ToList();

            routes.Add(new RouteInfo
            {
                HttpMethod = httpVerb,
                Path = fullPath,
                ControllerName = controllerName,
                ActionName = method.Identifier.Text,
                Summary = summary,
                Parameters = parameters,
                RequestBody = requestBody,
                Responses = responses,
                Attributes = attributes
            });
        }

        return routes;
    }

    private static string? ExtractXmlSummary(MethodDeclarationSyntax method)
    {
        var trivia = method.GetLeadingTrivia()
            .FirstOrDefault(t => t.IsKind(SyntaxKind.SingleLineDocumentationCommentTrivia));
        if (trivia == default) return null;
        var xml = trivia.ToString();
        var match = Regex.Match(xml, @"<summary>(.*?)</summary>",
            RegexOptions.Singleline | RegexOptions.IgnoreCase);
        return match.Success
            ? Regex.Replace(match.Groups[1].Value.Trim(), @"^\s*///\s*", "",
                RegexOptions.Multiline).Trim()
            : null;
    }

    private static string? ExtractXmlSummaryFromNode(SyntaxNode node)
    {
        var trivia = node.GetLeadingTrivia()
            .FirstOrDefault(t => t.IsKind(SyntaxKind.SingleLineDocumentationCommentTrivia));
        if (trivia == default) return null;
        var xml = trivia.ToString();
        var match = Regex.Match(xml, @"<summary>(.*?)</summary>",
            RegexOptions.Singleline | RegexOptions.IgnoreCase);
        return match.Success
            ? Regex.Replace(match.Groups[1].Value.Trim(), @"^\s*///\s*", "",
                RegexOptions.Multiline).Trim()
            : null;
    }

    private static List<Models.Responses.ParameterInfo> ExtractParameters(
        MethodDeclarationSyntax method)
    {
        var result = new List<Models.Responses.ParameterInfo>();

        foreach (var param in method.ParameterList.Parameters)
        {
            var attrs = param.AttributeLists.SelectMany(al => al.Attributes).ToList();
            var attrName = attrs.FirstOrDefault()?.Name.ToString();

            if (attrName == "FromBody") continue;

            var typeName = param.Type?.ToString() ?? "object";
            if (attrName == null && !IsPrimitiveTypeStatic(typeName)) continue;

            var source = attrName switch
            {
                "FromRoute" => "Route",
                "FromQuery" => "Query",
                "FromHeader" => "Header",
                _ => "Route"
            };

            result.Add(new Models.Responses.ParameterInfo
            {
                Name = param.Identifier.Text,
                Type = typeName,
                Source = source,
                IsRequired = !typeName.EndsWith("?")
            });
        }

        return result;
    }

    private RequestBodyInfo? ExtractRequestBody(MethodDeclarationSyntax method)
    {
        var bodyParam = method.ParameterList.Parameters
            .FirstOrDefault(p => p.AttributeLists
                .SelectMany(al => al.Attributes)
                .Any(a => a.Name.ToString() == "FromBody"));

        if (bodyParam == null)
        {
            var httpAttr = method.AttributeLists
                .SelectMany(al => al.Attributes)
                .FirstOrDefault(a => a.Name.ToString() is "HttpPost" or "HttpPut" or "HttpPatch");

            if (httpAttr != null)
            {
                bodyParam = method.ParameterList.Parameters
                    .FirstOrDefault(p =>
                    {
                        var t = p.Type?.ToString() ?? "";
                        return !IsPrimitiveTypeStatic(t)
                            && !p.AttributeLists.SelectMany(al => al.Attributes)
                                .Any(a => a.Name.ToString() is "FromRoute" or "FromQuery" or "FromHeader");
                    });
            }
        }

        if (bodyParam == null) return null;

        var typeName = bodyParam.Type?.ToString() ?? "object";

        return new RequestBodyInfo
        {
            TypeName = typeName,
            Properties = ResolveTypeProperties(typeName)
        };
    }

    private List<ResponseInfo> ExtractResponses(MethodDeclarationSyntax method)
    {
        var responses = new List<ResponseInfo>();

        var producesAttrs = method.AttributeLists
            .SelectMany(al => al.Attributes)
            .Where(a => a.Name.ToString().StartsWith("ProducesResponseType"));

        foreach (var attr in producesAttrs)
        {
            var args = attr.ArgumentList?.Arguments.ToList() ?? [];
            if (args.Count == 0) continue;

            var statusCode = 200;
            string typeName = "void";

            foreach (var arg in args)
            {
                var text = arg.ToString();
                if (int.TryParse(text, out var code)) statusCode = code;
                else if (text.StartsWith("typeof("))
                    typeName = text.Replace("typeof(", "").TrimEnd(')');
                else if (text.StartsWith("StatusCodes."))
                    statusCode = MapStatusCodeConstant(text);
            }

            responses.Add(new ResponseInfo
            {
                StatusCode = statusCode,
                TypeName = typeName,
                Description = GetStatusDescription(statusCode),
                Properties = ResolveTypeProperties(typeName) 
            });
        }

        if (responses.Count == 0)
        {
            var returnType = method.ReturnType.ToString();
            var match = Regex.Match(returnType, @"ActionResult<(.+)>|Task<ActionResult<(.+)>>");
            var typeName = match.Success
                ? (match.Groups[1].Value is { Length: > 0 } g1 ? g1 : match.Groups[2].Value)
                : returnType.Replace("Task<", "").TrimEnd('>');

            responses.Add(new ResponseInfo
            {
                StatusCode = 200,
                TypeName = typeName,
                Description = "OK",
                Properties = ResolveTypeProperties(typeName) // ← expand
            });
        }

        return responses;
    }

    private async Task<List<RouteInfo>> ExtractMinimalApiRoutesAsync(List<string> csFiles)
    {
        var routes = new List<RouteInfo>();
        var pattern = new Regex(
            @"app\.(MapGet|MapPost|MapPut|MapDelete|MapPatch)\s*\(\s*""([^""]+)""",
            RegexOptions.Compiled);

        foreach (var file in csFiles)
        {
            var code = await File.ReadAllTextAsync(file);
            foreach (Match match in pattern.Matches(code))
            {
                routes.Add(new RouteInfo
                {
                    HttpMethod = match.Groups[1].Value.Replace("Map", "").ToUpper(),
                    Path = match.Groups[2].Value,
                    ControllerName = "MinimalAPI",
                    ActionName = match.Groups[2].Value.Trim('/').Replace("/", "_")
                });
            }
        }

        return routes;
    }

    private static List<string> DetectFrameworks(string localPath)
    {
        var frameworks = new List<string>();
        var csprojFiles = Directory.GetFiles(localPath, "*.csproj", SearchOption.AllDirectories);
        foreach (var file in csprojFiles)
        {
            var content = File.ReadAllText(file);
            if (content.Contains("Microsoft.AspNetCore")) frameworks.Add("ASP.NET Core");
            if (content.Contains("Swashbuckle")) frameworks.Add("Swashbuckle/Swagger");
            if (content.Contains("NSwag")) frameworks.Add("NSwag");
            if (content.Contains("MediatR")) frameworks.Add("MediatR");
            if (content.Contains("FluentValidation")) frameworks.Add("FluentValidation");
        }
        return frameworks.Distinct().ToList();
    }

    private static bool IsPrimitiveTypeStatic(string typeName)
    {
        var primitives = new HashSet<string>
        {
            "int", "long", "short", "byte", "float", "double", "decimal", "bool",
            "string", "char", "Guid", "DateTime", "DateTimeOffset", "TimeSpan",
            "int?", "long?", "bool?", "double?", "decimal?", "Guid?", "DateTime?",
            "object", "dynamic", "void", "IActionResult", "ActionResult"
        };
        return primitives.Contains(typeName);
    }

    private static int MapStatusCodeConstant(string constant) => constant switch
    {
        "StatusCodes.Status200OK" => 200,
        "StatusCodes.Status201Created" => 201,
        "StatusCodes.Status204NoContent" => 204,
        "StatusCodes.Status400BadRequest" => 400,
        "StatusCodes.Status401Unauthorized" => 401,
        "StatusCodes.Status403Forbidden" => 403,
        "StatusCodes.Status404NotFound" => 404,
        "StatusCodes.Status500InternalServerError" => 500,
        _ => 200
    };

    private static string GetStatusDescription(int code) => code switch
    {
        200 => "OK",
        201 => "Created",
        204 => "No Content",
        400 => "Bad Request",
        401 => "Unauthorized",
        403 => "Forbidden",
        404 => "Not Found",
        500 => "Internal Server Error",
        _ => "Unknown"
    };
}