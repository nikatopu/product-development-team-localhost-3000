using ApiDocGen.Models.Responses;

namespace ApiDocGen.Services;

public interface IDocumentationService
{
    DocumentationResult GenerateMarkdown(AnalysisResult analysis);
    DocumentationResult GenerateOpenApi(AnalysisResult analysis);
    DocumentationResult GenerateTypeScript(AnalysisResult analysis);
    DocumentationResult GenerateRoutesJson(AnalysisResult analysis);
    DocumentationResult GenerateTypeScriptJson(AnalysisResult analysis);
}