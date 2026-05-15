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

    public AnalysisService(ILogger<AnalysisService> logger) => _logger = logger;

    public async Task<AnalysisResult> AnalyzeRepositoryAsync(string localPath, string repoUrl)
    {
        var csFiles = Directory.GetFiles(localPath, "*.cs", SearchOption.AllDirectories)
            .Where(f => !f.Contains("obj") && !f.Contains("bin"))
            .ToList();

        _logger.LogInformation("Analyzing {Count} .cs files", csFiles.Count);

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

        // Detect minimal APIs
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
            // Replace [controller] token
            var controllerName = controller.Identifier.Text.Replace("Controller", "");
            return route.Replace("[controller]", controllerName.ToLower());
        }

        var name = controller.Identifier.Text.Replace("Controller", "").ToLower();
        return $"api/{name}";
    }

    private static List<RouteInfo> ExtractRoutes(
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

    private static List<Models.Responses.ParameterInfo> ExtractParameters(
        MethodDeclarationSyntax method)
    {
        var result = new List<Models.Responses.ParameterInfo>();

        foreach (var param in method.ParameterList.Parameters)
        {
            var attrs = param.AttributeLists.SelectMany(al => al.Attributes).ToList();
            var source = attrs.FirstOrDefault()?.Name.ToString() switch
            {
                "FromRoute" => "Route",
                "FromQuery" => "Query",
                "FromHeader" => "Header",
                "FromBody" => null, // handled separately
                _ => "Route" // default assumption for simple types
            };

            if (source == null) continue; // skip body params here

            result.Add(new Models.Responses.ParameterInfo
            {
                Name = param.Identifier.Text,
                Type = param.Type?.ToString() ?? "object",
                Source = source,
                IsRequired = !param.Type?.ToString().EndsWith("?") ?? true
            });
        }

        return result;
    }

    private static RequestBodyInfo? ExtractRequestBody(MethodDeclarationSyntax method)
    {
        var bodyParam = method.ParameterList.Parameters
            .FirstOrDefault(p => p.AttributeLists
                .SelectMany(al => al.Attributes)
                .Any(a => a.Name.ToString() == "FromBody"));

        if (bodyParam == null) return null;

        return new RequestBodyInfo
        {
            TypeName = bodyParam.Type?.ToString() ?? "object",
            Properties = [] // deep type resolution needs semantic model
        };
    }

    private static List<ResponseInfo> ExtractResponses(MethodDeclarationSyntax method)
    {
        var responses = new List<ResponseInfo>();

        var producesAttrs = method.AttributeLists
            .SelectMany(al => al.Attributes)
            .Where(a => a.Name.ToString().StartsWith("ProducesResponseType"));

        foreach (var attr in producesAttrs)
        {
            var args = attr.ArgumentList?.Arguments.ToList() ?? [];
            if (args.Count == 0) continue;

            // Try to parse status code
            var statusCode = 200;
            string typeName = "void";

            foreach (var arg in args)
            {
                var text = arg.ToString();
                if (int.TryParse(text, out var code))
                    statusCode = code;
                else if (text.StartsWith("typeof("))
                    typeName = text.Replace("typeof(", "").TrimEnd(')');
                else if (text.StartsWith("StatusCodes."))
                    statusCode = MapStatusCodeConstant(text);
            }

            responses.Add(new ResponseInfo
            {
                StatusCode = statusCode,
                TypeName = typeName,
                Description = GetStatusDescription(statusCode)
            });
        }

        // Fallback — infer from return type
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
                Description = "OK"
            });
        }

        return responses;
    }

    private async Task<List<RouteInfo>> ExtractMinimalApiRoutesAsync(List<string> csFiles)
    {
        var routes = new List<RouteInfo>();
        var minimalApiPattern = new Regex(
            @"app\.(MapGet|MapPost|MapPut|MapDelete|MapPatch)\s*\(\s*""([^""]+)""",
            RegexOptions.Compiled);

        foreach (var file in csFiles)
        {
            var code = await File.ReadAllTextAsync(file);
            foreach (Match match in minimalApiPattern.Matches(code))
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