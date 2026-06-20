using ApiDocGen.Models.Responses;

namespace ApiDocGen.Services;

public interface IScanCacheService
{
    AnalysisResult? GetLastScan(string repoUrl);
    void StoreScan(string repoUrl, AnalysisResult result);
}
