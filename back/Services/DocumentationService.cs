using System.Text;
using ApiDocGen.Models.Responses;
using System.Text.RegularExpressions;
using ApiDocGen.Models.Responses;

namespace ApiDocGen.Services;

public class DocumentationService : IDocumentationService
{
    public DocumentationResult GenerateMarkdown(AnalysisResult analysis)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"# {analysis.ProjectName} API Documentation");
        sb.AppendLine($"> Generated at {analysis.AnalyzedAt:yyyy-MM-dd HH:mm} UTC");
        sb.AppendLine();
        sb.AppendLine("## Overview");
        sb.AppendLine($"- **Total Routes:** {analysis.Metadata.TotalRoutes}");
        sb.AppendLine($"- **Controllers:** {analysis.Metadata.TotalControllers}");
        sb.AppendLine($"- **API Type:** {analysis.Metadata.ApiType}");
        sb.AppendLine($"- **Frameworks:** {string.Join(", ", analysis.Metadata.DetectedFrameworks)}");
        sb.AppendLine();

        var grouped = analysis.Routes.GroupBy(r => r.ControllerName);
        foreach (var group in grouped)
        {
            sb.AppendLine($"## {group.Key}");
            sb.AppendLine();
            foreach (var route in group)
            {
                sb.AppendLine($"### `{route.HttpMethod}` {route.Path}");
                if (!string.IsNullOrEmpty(route.Summary))
                    sb.AppendLine($"> {route.Summary}");
                sb.AppendLine();

                if (route.Parameters.Count > 0)
                {
                    sb.AppendLine("**Parameters**");
                    sb.AppendLine("| Name | Type | Source | Required |");
                    sb.AppendLine("|------|------|--------|----------|");
                    foreach (var p in route.Parameters)
                        sb.AppendLine($"| {p.Name} | `{p.Type}` | {p.Source} | {(p.IsRequired ? "✅" : "❌")} |");
                    sb.AppendLine();
                }

                if (route.RequestBody != null)
                {
                    sb.AppendLine($"**Request Body:** `{route.RequestBody.TypeName}`");
                    if (route.RequestBody.Properties.Count > 0)
                    {
                        sb.AppendLine();
                        sb.AppendLine("| Property | Type | Required | Description |");
                        sb.AppendLine("|----------|------|----------|-------------|");
                        foreach (var p in route.RequestBody.Properties)
                            sb.AppendLine($"| {p.Name} | `{p.Type}` | {(p.IsRequired ? "✅" : "❌")} | {p.Summary ?? "-"} |");
                    }
                    sb.AppendLine();
                }

                if (route.Responses.Count > 0)
                {
                    sb.AppendLine("**Responses**");
                    foreach (var r in route.Responses)
                    {
                        sb.AppendLine($"| {r.StatusCode} | `{r.TypeName}` | {r.Description} |");
                        if (r.Properties.Count > 0)
                        {
                            sb.AppendLine();
                            sb.AppendLine($"  *`{r.TypeName}` shape:*");
                            sb.AppendLine("  | Property | Type | Required |");
                            sb.AppendLine("  |----------|------|----------|");
                            foreach (var p in r.Properties)
                                sb.AppendLine($"  | {p.Name} | `{p.Type}` | {(p.IsRequired ? "✅" : "❌")} |");
                        }
                    }
                    sb.AppendLine();
                }

                if (route.Attributes.Count > 0)
                    sb.AppendLine($"**Attributes:** {string.Join(", ", route.Attributes.Select(a => $"`{a}`"))}");

                sb.AppendLine("---");
                sb.AppendLine();
            }
        }

        return new DocumentationResult
        {
            Format = "markdown",
            Content = sb.ToString(),
            GeneratedAt = DateTime.UtcNow
        };
    }

    public DocumentationResult GenerateOpenApi(AnalysisResult analysis)
    {
        var sb = new StringBuilder();
        sb.AppendLine("openapi: 3.0.0");
        sb.AppendLine("info:");
        sb.AppendLine($"  title: {analysis.ProjectName}");
        sb.AppendLine("  version: 1.0.0");
        sb.AppendLine("paths:");

        var groupedByPath = analysis.Routes.GroupBy(r => r.Path);
        foreach (var pathGroup in groupedByPath)
        {
            sb.AppendLine($"  {pathGroup.Key}:");
            foreach (var route in pathGroup)
            {
                sb.AppendLine($"    {route.HttpMethod.ToLower()}:");
                sb.AppendLine($"      summary: {route.Summary ?? route.ActionName}");
                sb.AppendLine($"      operationId: {route.ActionName}");
                sb.AppendLine("      responses:");
                foreach (var resp in route.Responses)
                    sb.AppendLine($"        '{resp.StatusCode}':");
                if (route.Responses.Count == 0)
                    sb.AppendLine("        '200':");
                sb.AppendLine("          description: Success");
            }
        }

        return new DocumentationResult
        {
            Format = "openapi",
            Content = sb.ToString(),
            GeneratedAt = DateTime.UtcNow
        };
    }
    public DocumentationResult GenerateTypeScript(AnalysisResult analysis)
    {
        var sb = new StringBuilder();

        sb.AppendLine($"// ========================================");
        sb.AppendLine($"// {analysis.ProjectName} — Auto-generated TypeScript");
        sb.AppendLine($"// Generated at {analysis.AnalyzedAt:yyyy-MM-dd HH:mm} UTC");
        sb.AppendLine($"// DO NOT EDIT MANUALLY");
        sb.AppendLine($"// ========================================");
        sb.AppendLine();

        var allTypes = new Dictionary<string, List<Models.Responses.PropertyInfo>>();

        foreach (var route in analysis.Routes)
        {
            if (route.RequestBody?.Properties.Count > 0)
                allTypes.TryAdd(route.RequestBody.TypeName, route.RequestBody.Properties);

            foreach (var response in route.Responses)
            {
                if (response.Properties.Count > 0)
                {
                    var typeName = UnwrapGenericTypeName(response.TypeName);
                    allTypes.TryAdd(typeName, response.Properties);
                }

                foreach (var prop in response.Properties)
                    CollectNestedTypes(prop, allTypes);
            }

            if (route.RequestBody != null)
                foreach (var prop in route.RequestBody.Properties)
                    CollectNestedTypes(prop, allTypes);
        }

        // ── 2. Emit TypeScript interfaces ──
        sb.AppendLine("// ----------------------------------------");
        sb.AppendLine("// Interfaces");
        sb.AppendLine("// ----------------------------------------");
        sb.AppendLine();

        foreach (var (typeName, properties) in allTypes)
        {
            var cleanName = UnwrapGenericTypeName(typeName);
            sb.AppendLine($"export interface {cleanName} {{");
            foreach (var prop in properties)
            {
                var tsType = MapToTypeScriptType(prop.Type);
                var optional = !prop.IsRequired ? "?" : "";
                if (!string.IsNullOrEmpty(prop.Summary))
                    sb.AppendLine($"  /** {prop.Summary} */");
                sb.AppendLine($"  {ToCamelCase(prop.Name)}{optional}: {tsType};");
            }
            sb.AppendLine("}");
            sb.AppendLine();
        }

        sb.AppendLine("// ----------------------------------------");
        sb.AppendLine("// Route-specific Request & Response Types");
        sb.AppendLine("// ----------------------------------------");
        sb.AppendLine();

        foreach (var route in analysis.Routes)
        {
            var fnName = ToFunctionName(route.HttpMethod, route.Path);
            sb.AppendLine($"// {route.HttpMethod} {route.Path}");

            if (route.RequestBody != null)
            {
                var requestTypeName = $"{fnName}Request";
                var bodyType = UnwrapGenericTypeName(route.RequestBody.TypeName);

                if (allTypes.ContainsKey(route.RequestBody.TypeName))
                {
                    sb.AppendLine($"export type {requestTypeName} = {bodyType};");
                }
                else
                {
                    sb.AppendLine($"export type {requestTypeName} = {MapToTypeScriptType(route.RequestBody.TypeName)};");
                }
            }

            if (route.Parameters.Any(p => p.Source == "Query"))
            {
                var queryTypeName = $"{fnName}QueryParams";
                sb.AppendLine($"export interface {queryTypeName} {{");
                foreach (var p in route.Parameters.Where(p => p.Source == "Query"))
                {
                    var tsType = MapToTypeScriptType(p.Type);
                    var optional = !p.IsRequired ? "?" : "";
                    sb.AppendLine($"  {ToCamelCase(p.Name)}{optional}: {tsType};");
                }
                sb.AppendLine("}");
            }

            foreach (var response in route.Responses.Where(r => r.StatusCode is >= 200 and < 300))
            {
                var responseTypeName = $"{fnName}Response";
                var bodyType = UnwrapGenericTypeName(response.TypeName);
                var isList = IsListType(response.TypeName);

                if (allTypes.ContainsKey(bodyType))
                    sb.AppendLine($"export type {responseTypeName} = {bodyType}{(isList ? "[]" : "")};");
                else
                    sb.AppendLine($"export type {responseTypeName} = {MapToTypeScriptType(response.TypeName)};");
            }

            sb.AppendLine();
        }

        sb.AppendLine("// ----------------------------------------");
        sb.AppendLine("// Typed Fetch Helpers");
        sb.AppendLine("// ----------------------------------------");
        sb.AppendLine();
        sb.AppendLine("const BASE_URL = process.env.NEXT_PUBLIC_API_URL ?? 'http://localhost:5000';");
        sb.AppendLine();

        foreach (var route in analysis.Routes)
        {
            var fnName = ToFunctionName(route.HttpMethod, route.Path);
            var routeParams = route.Parameters.Where(p => p.Source == "Route").ToList();
            var queryParams = route.Parameters.Where(p => p.Source == "Query").ToList();
            var hasBody = route.RequestBody != null;
            var hasQuery = queryParams.Count > 0;

            var successResponse = route.Responses.FirstOrDefault(r => r.StatusCode is >= 200 and < 300);
            var responseType = successResponse != null
                ? $"{fnName}Response"
                : "void";

            var paramParts = new List<string>();
            foreach (var p in routeParams)
                paramParts.Add($"{ToCamelCase(p.Name)}: {MapToTypeScriptType(p.Type)}");
            if (hasBody)
                paramParts.Add($"body: {fnName}Request");
            if (hasQuery)
                paramParts.Add($"params?: {fnName}QueryParams");

            var signature = string.Join(", ", paramParts);

            var urlPath = route.Path;
            foreach (var p in routeParams)
                urlPath = urlPath.Replace($"{{{p.Name}}}", $"${{{ToCamelCase(p.Name)}}}");

            if (!string.IsNullOrEmpty(route.Summary))
                sb.AppendLine($"/** {route.Summary} */");

            sb.AppendLine($"export async function {fnName}({signature}): Promise<{responseType}> {{");

            if (hasQuery)
            {
                sb.AppendLine("  const query = params");
                sb.AppendLine("    ? '?' + new URLSearchParams(params as Record<string, string>).toString()");
                sb.AppendLine("    : '';");
            }

            var urlVar = hasQuery ? $"`${{BASE_URL}}{urlPath}${{query}}`" : $"`${{BASE_URL}}{urlPath}`";

            sb.AppendLine($"  const res = await fetch({urlVar}, {{");
            sb.AppendLine($"    method: '{route.HttpMethod}',");

            if (hasBody)
            {
                sb.AppendLine("    headers: { 'Content-Type': 'application/json' },");
                sb.AppendLine("    body: JSON.stringify(body),");
            }

            sb.AppendLine("  });");
            sb.AppendLine();
            sb.AppendLine("  if (!res.ok) throw new Error(`HTTP ${res.status}: ${res.statusText}`);");

            if (responseType != "void")
                sb.AppendLine($"  return res.json() as Promise<{responseType}>;");

            sb.AppendLine("}");
            sb.AppendLine();
        }

        return new DocumentationResult
        {
            Format = "typescript",
            Content = sb.ToString(),
            GeneratedAt = DateTime.UtcNow
        };
    }


    private static void CollectNestedTypes(
        Models.Responses.PropertyInfo prop,
        Dictionary<string, List<Models.Responses.PropertyInfo>> allTypes)
    {
        if (prop.NestedProperties.Count > 0)
        {
            var typeName = UnwrapGenericTypeName(prop.Type);
            allTypes.TryAdd(typeName, prop.NestedProperties);
            foreach (var nested in prop.NestedProperties)
                CollectNestedTypes(nested, allTypes);
        }
    }

    private static string MapToTypeScriptType(string csharpType)
    {
        var unwrapped = UnwrapGenericTypeName(csharpType);
        var isList = IsListType(csharpType);
        var isNullable = csharpType.EndsWith("?");

        var tsBase = unwrapped.TrimEnd('?') switch
        {
            "int" or "long" or "short" or "byte" or "float" or "double" or "decimal" => "number",
            "bool" => "boolean",
            "string" or "char" => "string",
            "Guid" => "string",
            "DateTime" or "DateTimeOffset" => "string", 
            "TimeSpan" => "string",
            "object" or "dynamic" => "unknown",
            "void" or "IActionResult" or "ActionResult" => "void",
            _ => unwrapped.TrimEnd('?') 
        };

        var result = isList ? $"{tsBase}[]" : tsBase;
        return isNullable ? $"{result} | null" : result;
    }

    private static string UnwrapGenericTypeName(string typeName)
    {
        var match = Regex.Match(typeName,
            @"^(?:List|IList|IEnumerable|ICollection|Task|ActionResult)<(.+)>$");
        return match.Success ? match.Groups[1].Value.Trim() : typeName;
    }

    private static bool IsListType(string typeName) =>
        Regex.IsMatch(typeName, @"^(?:List|IList|IEnumerable|ICollection)<.+>$");

    private static string ToFunctionName(string httpMethod, string path)
    {
        var verb = httpMethod.ToLower() switch
        {
            "get" => "get",
            "post" => "create",
            "put" => "update",
            "patch" => "patch",
            "delete" => "delete",
            _ => httpMethod.ToLower()
        };

        var parts = path.Split('/', StringSplitOptions.RemoveEmptyEntries)
            .Select(p => p.StartsWith("{") && p.EndsWith("}")
                ? "By" + char.ToUpper(p[1]) + p[2..^1]
                : char.ToUpper(p[0]) + p[1..]);

        return verb + string.Join("", parts);
    }

    private static string ToCamelCase(string name) =>
        string.IsNullOrEmpty(name) ? name : char.ToLower(name[0]) + name[1..];
    public DocumentationResult GenerateRoutesJson(AnalysisResult analysis)
    {
        var routes = analysis.Routes.Select(route => new
        {
            method = route.HttpMethod,
            path = route.Path,
            controller = route.ControllerName,
            action = route.ActionName,
            summary = route.Summary,
            attributes = route.Attributes,
            parameters = route.Parameters.Select(p => new
            {
                name = p.Name,
                type = p.Type,
                source = p.Source,
                isRequired = p.IsRequired
            }),
            requestBody = route.RequestBody == null ? null : new
            {
                typeName = route.RequestBody.TypeName,
                properties = MapProperties(route.RequestBody.Properties)
            },
            responses = route.Responses.Select(r => new
            {
                statusCode = r.StatusCode,
                description = r.Description,
                typeName = r.TypeName,
                properties = MapProperties(r.Properties)
            })
        });

        return new DocumentationResult
        {
            Format = "json-routes",
            Content = System.Text.Json.JsonSerializer.Serialize(new
            {
                projectName = analysis.ProjectName,
                analyzedAt = analysis.AnalyzedAt,
                metadata = new
                {
                    totalRoutes = analysis.Metadata.TotalRoutes,
                    totalControllers = analysis.Metadata.TotalControllers,
                    apiType = analysis.Metadata.ApiType,
                    detectedFrameworks = analysis.Metadata.DetectedFrameworks
                },
                routes
            }, new System.Text.Json.JsonSerializerOptions { WriteIndented = true }),
            GeneratedAt = DateTime.UtcNow
        };
    }

    public DocumentationResult GenerateTypeScriptJson(AnalysisResult analysis)
    {
        var interfaces = new Dictionary<string, object>();

        foreach (var route in analysis.Routes)
        {
            if (route.RequestBody?.Properties.Count > 0)
            {
                var name = UnwrapGenericTypeName(route.RequestBody.TypeName);
                interfaces.TryAdd(name, MapPropertiesAsTs(route.RequestBody.Properties));
            }

            foreach (var response in route.Responses)
            {
                if (response.Properties.Count > 0)
                {
                    var name = UnwrapGenericTypeName(response.TypeName);
                    interfaces.TryAdd(name, MapPropertiesAsTs(response.Properties));
                }
            }
        }

        var routeTypes = analysis.Routes.Select(route =>
        {
            var fnName = ToFunctionName(route.HttpMethod, route.Path);
            var successResponse = route.Responses.FirstOrDefault(r => r.StatusCode is >= 200 and < 300);

            string? responseType = null;
            if (successResponse != null)
            {
                var baseType = UnwrapGenericTypeName(successResponse.TypeName);
                var isList = IsListType(successResponse.TypeName);
                responseType = isList ? $"{baseType}[]" : MapToTypeScriptType(successResponse.TypeName);
            }

            string? requestType = null;
            if (route.RequestBody != null)
                requestType = UnwrapGenericTypeName(route.RequestBody.TypeName);

            var queryParams = route.Parameters
                .Where(p => p.Source == "Query")
                .Select(p => new
                {
                    name = ToCamelCase(p.Name),
                    type = MapToTypeScriptType(p.Type),
                    isRequired = p.IsRequired
                }).ToList();

            var routeParams = route.Parameters
                .Where(p => p.Source == "Route")
                .Select(p => new
                {
                    name = ToCamelCase(p.Name),
                    type = MapToTypeScriptType(p.Type),
                    isRequired = p.IsRequired
                }).ToList();

            var fetchParams = new List<object>();
            foreach (var p in routeParams)
                fetchParams.Add(new { name = p.name, type = p.type, source = "route" });
            if (route.RequestBody != null)
                fetchParams.Add(new { name = "body", type = requestType ?? "unknown", source = "body" });
            if (queryParams.Count > 0)
                fetchParams.Add(new { name = "params", type = $"{fnName}QueryParams", source = "query" });

            return new
            {
                functionName = fnName,
                method = route.HttpMethod,
                path = route.Path,
                summary = route.Summary,
                requestTypeName = requestType,
                responseTypeName = responseType,
                routeParams,
                queryParams,
                fetchParams,
                hasBody = route.RequestBody != null,
                hasQueryParams = queryParams.Count > 0
            };
        });

        return new DocumentationResult
        {
            Format = "json-typescript",
            Content = System.Text.Json.JsonSerializer.Serialize(new
            {
                projectName = analysis.ProjectName,
                analyzedAt = analysis.AnalyzedAt,
                interfaces,
                routeTypes
            }, new System.Text.Json.JsonSerializerOptions { WriteIndented = true }),
            GeneratedAt = DateTime.UtcNow
        };
    }


    private static IEnumerable<object> MapProperties(List<Models.Responses.PropertyInfo> props) =>
        props.Select(p => new
        {
            name = p.Name,
            type = p.Type,
            isRequired = p.IsRequired,
            summary = p.Summary,
            nestedProperties = p.NestedProperties.Count > 0
                ? MapProperties(p.NestedProperties)
                : Enumerable.Empty<object>()
        });

    private static IEnumerable<object> MapPropertiesAsTs(List<Models.Responses.PropertyInfo> props) =>
        props.Select(p => new
        {
            name = ToCamelCase(p.Name),
            type = MapToTypeScriptType(p.Type),
            isRequired = p.IsRequired,
            summary = p.Summary,
            nestedProperties = p.NestedProperties.Count > 0
                ? MapPropertiesAsTs(p.NestedProperties)
                : Enumerable.Empty<object>()
        });


}





