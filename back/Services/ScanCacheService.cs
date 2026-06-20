using ApiDocGen.Models.Responses;
using Microsoft.Extensions.Caching.Memory;

namespace ApiDocGen.Services;

public class ScanCacheService : IScanCacheService
{
    private readonly IMemoryCache _cache;

    public ScanCacheService(IMemoryCache cache) => _cache = cache;

    public AnalysisResult? GetLastScan(string repoUrl) =>
        _cache.TryGetValue(CacheKey(repoUrl), out AnalysisResult? result) ? result : null;

    public void StoreScan(string repoUrl, AnalysisResult result) =>
        _cache.Set(CacheKey(repoUrl), result, TimeSpan.FromHours(24));

    private static string CacheKey(string repoUrl) =>
        $"scan:{repoUrl.Trim().ToLowerInvariant()}";
}
