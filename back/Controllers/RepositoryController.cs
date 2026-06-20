using ApiDocGen.Models.Requests;
using ApiDocGen.Models.Responses;
using ApiDocGen.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiDocGen.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RepositoryController : ControllerBase
{
    private readonly IGitService _gitService;
    private readonly IAnalysisService _analysisService;
    private readonly IBreakingChangeService _breakingChangeService;
    private readonly IScanCacheService _scanCache;
    private readonly IAnalysisNotifier _notifier;
    private readonly ILogger<RepositoryController> _logger;

    public RepositoryController(
        IGitService gitService,
        IAnalysisService analysisService,
        IBreakingChangeService breakingChangeService,
        IScanCacheService scanCache,
        IAnalysisNotifier notifier,
        ILogger<RepositoryController> logger)
    {
        _gitService = gitService;
        _analysisService = analysisService;
        _breakingChangeService = breakingChangeService;
        _scanCache = scanCache;
        _notifier = notifier;
        _logger = logger;
    }

    /// <summary>Analyze an ASP.NET repository and extract all routes</summary>
    [HttpPost("analyze")]
    [ProducesResponseType(typeof(AnalysisResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<AnalysisResult>> AnalyzeRepository(
        [FromBody] AnalyzeRepoRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.RepoUrl))
            return BadRequest("RepoUrl is required.");

        string? localPath = null;
        try
        {
            await _notifier.NotifyStarted(request.RepoUrl);

            localPath = await _gitService.CloneRepositoryAsync(request.RepoUrl, request.Branch);
            await _notifier.NotifyCloningComplete(request.RepoUrl);

            await _notifier.NotifyAnalysisProgress(request.RepoUrl, "Parsing source files", 60);
            var result = await _analysisService.AnalyzeRepositoryAsync(localPath, request.RepoUrl);

            var previousScan = _scanCache.GetLastScan(request.RepoUrl);
            if (previousScan != null)
            {
                result.BreakingChanges = _breakingChangeService.DetectChanges(previousScan, result);
                _logger.LogInformation(
                    "Breaking changes detected: {Count} for {RepoUrl}",
                    result.BreakingChanges.Count(c => c.Severity == "Breaking"),
                    request.RepoUrl);
            }

            _scanCache.StoreScan(request.RepoUrl, result);
            await _notifier.NotifyComplete(request.RepoUrl, result, result.BreakingChanges);

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to analyze repository {RepoUrl}", request.RepoUrl);
            await _notifier.NotifyFailed(request.RepoUrl, ex.Message);
            return StatusCode(500, $"Analysis failed: {ex.Message}");
        }
        finally
        {
            if (localPath != null)
                _gitService.Cleanup(localPath);
        }
    }
}
