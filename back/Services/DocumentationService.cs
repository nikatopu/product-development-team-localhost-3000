using System.Text;
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
}