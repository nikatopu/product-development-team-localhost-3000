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
    private readonly ILogger<RepositoryController> _logger;

    public RepositoryController(
        IGitService gitService,
        IAnalysisService analysisService,
        ILogger<RepositoryController> logger)
    {
        _gitService = gitService;
        _analysisService = analysisService;
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
            localPath = await _gitService.CloneRepositoryAsync(request.RepoUrl, request.Branch);
            var result = await _analysisService.AnalyzeRepositoryAsync(localPath, request.RepoUrl);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to analyze repository {RepoUrl}", request.RepoUrl);
            return StatusCode(500, $"Analysis failed: {ex.Message}");
        }
        finally
        {
            if (localPath != null)
                _gitService.Cleanup(localPath);
        }
    }
}