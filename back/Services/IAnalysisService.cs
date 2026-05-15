using ApiDocGen.Models.Responses;

namespace ApiDocGen.Services;

public interface IAnalysisService
{
	Task<AnalysisResult> AnalyzeRepositoryAsync(string localPath, string repoUrl);
}