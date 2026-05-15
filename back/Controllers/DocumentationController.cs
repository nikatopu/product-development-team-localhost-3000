using ApiDocGen.Models.Responses;
using ApiDocGen.Services;
using Microsoft.AspNetCore.Mvc;
using ApiDocGen.Models.Requests;

namespace ApiDocGen.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DocumentationController : ControllerBase
{
    private readonly IGitService _gitService;
    private readonly IAnalysisService _analysisService;
    private readonly IDocumentationService _documentationService;

    public DocumentationController(
        IGitService gitService,
        IAnalysisService analysisService,
        IDocumentationService documentationService)
    {
        _gitService = gitService;
        _analysisService = analysisService;
        _documentationService = documentationService;
    }

    [HttpGet("markdown")]
    [ProducesResponseType(typeof(DocumentationResult), StatusCodes.Status200OK)]
    public async Task<ActionResult<DocumentationResult>> GetMarkdown([FromQuery] string repoUrl,
        [FromQuery] string? branch = "main")
    {
        if (string.IsNullOrWhiteSpace(repoUrl))
            return BadRequest("repoUrl query parameter is required.");

        string? localPath = null;
        try
        {
            localPath = await _gitService.CloneRepositoryAsync(repoUrl, branch);
            var analysis = await _analysisService.AnalyzeRepositoryAsync(localPath, repoUrl);
            var doc = _documentationService.GenerateMarkdown(analysis);
            return Ok(doc);
        }
        finally
        {
            if (localPath != null) _gitService.Cleanup(localPath);
        }
    }

    [HttpGet("openapi")]
    [ProducesResponseType(typeof(DocumentationResult), StatusCodes.Status200OK)]
    public async Task<ActionResult<DocumentationResult>> GetOpenApi([FromQuery] string repoUrl,
        [FromQuery] string? branch = "main")
    {
        if (string.IsNullOrWhiteSpace(repoUrl))
            return BadRequest("repoUrl query parameter is required.");

        string? localPath = null;
        try
        {
            localPath = await _gitService.CloneRepositoryAsync(repoUrl, branch);
            var analysis = await _analysisService.AnalyzeRepositoryAsync(localPath, repoUrl);
            var doc = _documentationService.GenerateOpenApi(analysis);
            return Ok(doc);
        }
        finally
        {
            if (localPath != null) _gitService.Cleanup(localPath);
        }
    }
    [HttpPost("typescript")]
    [ProducesResponseType(typeof(DocumentationResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<DocumentationResult>> GetTypeScript([FromBody] AnalyzeRepoRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.RepoUrl))
            return BadRequest("RepoUrl is required.");

        string? localPath = null;
        try
        {
            localPath = await _gitService.CloneRepositoryAsync(request.RepoUrl, request.Branch);
            var analysis = await _analysisService.AnalyzeRepositoryAsync(localPath, request.RepoUrl);
            return Ok(_documentationService.GenerateTypeScript(analysis));
        }
        finally
        {
            if (localPath != null) _gitService.Cleanup(localPath);
        }
    }
}