using ApiDocGen.Models.Responses;

namespace ApiDocGen.Services;

public interface IDocumentationService
{
    DocumentationResult GenerateMarkdown(AnalysisResult analysis);
    DocumentationResult GenerateOpenApi(AnalysisResult analysis);
}