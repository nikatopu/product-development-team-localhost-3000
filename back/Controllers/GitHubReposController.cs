using ApiDocGen.Data;
using ApiDocGen.Data.Entities;
using ApiDocGen.Models.Requests;
using ApiDocGen.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Octokit;
using System.Security.Claims;
using System.Text.Json;

namespace ApiDocGen.Controllers;

[ApiController]
[Route("api/repos")]
[Authorize]
public class GitHubReposController : ControllerBase
{
    private readonly ApplicationDbContext _db;
    private readonly IGitService _gitService;
    private readonly IAnalysisService _analysisService;
    private readonly IBreakingChangeService _breakingChangeService;
    private readonly IScanCacheService _scanCache;
    private readonly IAnalysisNotifier _notifier;
    private readonly ILogger<GitHubReposController> _logger;

    public GitHubReposController(
        ApplicationDbContext db,
        IGitService gitService,
        IAnalysisService analysisService,
        IBreakingChangeService breakingChangeService,
        IScanCacheService scanCache,
        IAnalysisNotifier notifier,
        ILogger<GitHubReposController> logger)
    {
        _db = db;
        _gitService = gitService;
        _analysisService = analysisService;
        _breakingChangeService = breakingChangeService;
        _scanCache = scanCache;
        _notifier = notifier;
        _logger = logger;
    }

    private Guid CurrentUserId =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue("sub")
            ?? throw new UnauthorizedAccessException());

    /// <summary>List all GitHub repos available to the authenticated user</summary>
    [HttpGet("available")]
    public async Task<IActionResult> ListAvailableRepos()
    {
        var user = await _db.Users.FindAsync(CurrentUserId);
        if (user?.GithubAccessToken == null) return Unauthorized();

        var github = new GitHubClient(new ProductHeaderValue("Driftless"))
        {
            Credentials = new Credentials(user.GithubAccessToken)
        };

        var repos = await github.Repository.GetAllForCurrent(
            new ApiOptions { PageSize = 100, PageCount = 1 });

        return Ok(repos.Select(r => new
        {
            githubRepoId = r.Id,
            owner = r.Owner.Login,
            name = r.Name,
            fullName = r.FullName,
            description = r.Description,
            htmlUrl = r.HtmlUrl,
            defaultBranch = r.DefaultBranch,
            isPrivate = r.Private,
            language = r.Language,
            updatedAt = r.UpdatedAt,
        }));
    }

    /// <summary>List repos connected to Driftless by the current user</summary>
    [HttpGet]
    public async Task<IActionResult> ListConnectedRepos()
    {
        var repos = await _db.Repositories
            .Where(r => r.UserId == CurrentUserId)
            .OrderByDescending(r => r.ConnectedAt)
            .Select(r => new
            {
                r.Id, r.Owner, r.Name, r.FullName, r.DefaultBranch, r.IsPrivate,
                r.Description, r.HtmlUrl,
                status = r.Status.ToString(),
                r.ConnectedAt, r.LastScannedAt,
                lastScan = r.LastScannedAt != null ? new
                {
                    totalRoutes = r.LastScanTotalRoutes,
                    totalControllers = r.LastScanTotalControllers,
                    apiType = r.LastScanApiType,
                    breakingChangeCount = r.LastScanBreakingChangeCount,
                } : null,
            })
            .ToListAsync();

        return Ok(repos);
    }

    /// <summary>Connect a GitHub repository to Driftless</summary>
    [HttpPost("connect")]
    public async Task<IActionResult> ConnectRepo([FromBody] ConnectRepoRequest request)
    {
        var userId = CurrentUserId;

        var existing = await _db.Repositories.FirstOrDefaultAsync(r =>
            r.UserId == userId && r.GithubRepoId == request.GithubRepoId);
        if (existing != null)
            return Conflict("Repository already connected.");

        var repo = new Data.Entities.Repository
        {
            UserId = userId,
            GithubRepoId = request.GithubRepoId,
            Owner = request.Owner,
            Name = request.Name,
            FullName = request.FullName,
            DefaultBranch = request.DefaultBranch,
            IsPrivate = request.IsPrivate,
            Description = request.Description,
            HtmlUrl = request.HtmlUrl,
            Status = ScanStatus.Connected,
        };

        _db.Repositories.Add(repo);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetRepo), new { id = repo.Id }, new { repo.Id, repo.FullName });
    }

    /// <summary>Get a single connected repository</summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetRepo(Guid id)
    {
        var repo = await _db.Repositories
            .Include(r => r.Scans.OrderByDescending(s => s.StartedAt).Take(10))
            .FirstOrDefaultAsync(r => r.Id == id && r.UserId == CurrentUserId);

        if (repo == null) return NotFound();
        return Ok(repo);
    }

    /// <summary>Disconnect a repository</summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DisconnectRepo(Guid id)
    {
        var repo = await _db.Repositories.FirstOrDefaultAsync(r =>
            r.Id == id && r.UserId == CurrentUserId);
        if (repo == null) return NotFound();

        _db.Repositories.Remove(repo);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>Trigger a scan for a connected repository</summary>
    [HttpPost("{id:guid}/scan")]
    public async Task<IActionResult> TriggerScan(Guid id)
    {
        var repo = await _db.Repositories.FirstOrDefaultAsync(r =>
            r.Id == id && r.UserId == CurrentUserId);
        if (repo == null) return NotFound();

        var user = await _db.Users.FindAsync(CurrentUserId);
        if (user == null) return Unauthorized();

        // Set scanning status
        repo.Status = ScanStatus.Scanning;
        var scan = new RepositoryScan
        {
            RepositoryId = repo.Id,
            Status = ScanStatus.Scanning,
            TriggerSource = "manual",
        };
        _db.RepositoryScans.Add(scan);
        await _db.SaveChangesAsync();

        // Run scan in background
        _ = Task.Run(async () => await RunScanAsync(repo, scan, user));

        return Accepted(new { scanId = scan.Id, status = "Scanning" });
    }

    /// <summary>Get scan history for a repository</summary>
    [HttpGet("{id:guid}/scans")]
    public async Task<IActionResult> GetScanHistory(Guid id, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var repoExists = await _db.Repositories.AnyAsync(r =>
            r.Id == id && r.UserId == CurrentUserId);
        if (!repoExists) return NotFound();

        var scans = await _db.RepositoryScans
            .Where(s => s.RepositoryId == id)
            .OrderByDescending(s => s.StartedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(s => new
            {
                s.Id, s.StartedAt, s.CompletedAt,
                status = s.Status.ToString(),
                s.TotalRoutes, s.TotalControllers, s.ApiType,
                s.ErrorMessage, s.TriggerSource, s.CommitSha,
                breakingChanges = s.BreakingChanges.Select(b => new
                {
                    b.ChangeType, b.Severity, b.AffectedEndpoint, b.AffectedField,
                    b.OldValue, b.NewValue
                })
            })
            .ToListAsync();

        return Ok(scans);
    }

    /// <summary>Update notification settings for a repository</summary>
    [HttpPatch("{id:guid}/notifications")]
    public async Task<IActionResult> UpdateNotificationSettings(
        Guid id, [FromBody] NotificationSettingsRequest request)
    {
        var repo = await _db.Repositories.FirstOrDefaultAsync(r =>
            r.Id == id && r.UserId == CurrentUserId);
        if (repo == null) return NotFound();

        repo.SlackWebhookUrl = request.SlackWebhookUrl;
        repo.DiscordWebhookUrl = request.DiscordWebhookUrl;
        await _db.SaveChangesAsync();

        return NoContent();
    }

    private async Task RunScanAsync(Data.Entities.Repository repo, RepositoryScan scan, Data.Entities.User user)
    {
        string? localPath = null;
        var repoUrl = repo.HtmlUrl ?? $"https://github.com/{repo.FullName}";

        try
        {
            await _notifier.NotifyStarted(repoUrl);
            localPath = await _gitService.CloneRepositoryAsync(repoUrl, repo.DefaultBranch);
            await _notifier.NotifyCloningComplete(repoUrl);

            var result = await _analysisService.AnalyzeRepositoryAsync(localPath, repoUrl);

            var previousScan = _scanCache.GetLastScan(repoUrl);
            var breakingChanges = previousScan != null
                ? _breakingChangeService.DetectChanges(previousScan, result)
                : new List<Models.Responses.BreakingChangeInfo>();

            _scanCache.StoreScan(repoUrl, result);

            // Persist results
            scan.Status = ScanStatus.Ready;
            scan.CompletedAt = DateTime.UtcNow;
            scan.TotalRoutes = result.Metadata.TotalRoutes;
            scan.TotalControllers = result.Metadata.TotalControllers;
            scan.ApiType = result.Metadata.ApiType;
            scan.ResultJson = JsonSerializer.Serialize(result);
            scan.EnumsJson = JsonSerializer.Serialize(result.Enums);

            scan.BreakingChanges = breakingChanges.Select(b => new BreakingChangeRecord
            {
                ScanId = scan.Id,
                ChangeType = b.ChangeType,
                Severity = b.Severity,
                AffectedEndpoint = b.AffectedEndpoint,
                AffectedField = b.AffectedField,
                OldValue = b.OldValue,
                NewValue = b.NewValue,
            }).ToList();

            repo.Status = ScanStatus.Ready;
            repo.LastScannedAt = DateTime.UtcNow;
            repo.LastScanTotalRoutes = result.Metadata.TotalRoutes;
            repo.LastScanTotalControllers = result.Metadata.TotalControllers;
            repo.LastScanApiType = result.Metadata.ApiType;
            repo.LastScanBreakingChangeCount = breakingChanges.Count(c => c.Severity == "Breaking");

            await _db.SaveChangesAsync();

            await _notifier.NotifyComplete(repoUrl, result, breakingChanges);

            // Send external notifications if breaking changes found
            if (breakingChanges.Any(c => c.Severity == "Breaking"))
                await SendExternalNotificationsAsync(repo, breakingChanges);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Scan failed for repository {RepoId}", repo.Id);
            scan.Status = ScanStatus.Failed;
            scan.CompletedAt = DateTime.UtcNow;
            scan.ErrorMessage = ex.Message;
            repo.Status = ScanStatus.Failed;
            await _db.SaveChangesAsync();
            await _notifier.NotifyFailed(repoUrl, ex.Message);
        }
        finally
        {
            if (localPath != null) _gitService.Cleanup(localPath);
        }
    }

    private async Task SendExternalNotificationsAsync(
        Data.Entities.Repository repo,
        List<Models.Responses.BreakingChangeInfo> changes)
    {
        var breakingCount = changes.Count(c => c.Severity == "Breaking");
        var message = $"⚠️ {breakingCount} breaking change(s) detected in *{repo.FullName}*";

        var http = new HttpClient();

        if (!string.IsNullOrEmpty(repo.SlackWebhookUrl))
        {
            try
            {
                await http.PostAsJsonAsync(repo.SlackWebhookUrl,
                    new { text = message });
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Slack notification failed for {RepoId}", repo.Id);
            }
        }

        if (!string.IsNullOrEmpty(repo.DiscordWebhookUrl))
        {
            try
            {
                await http.PostAsJsonAsync(repo.DiscordWebhookUrl,
                    new { content = message });
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Discord notification failed for {RepoId}", repo.Id);
            }
        }
    }
}

public record ConnectRepoRequest(
    long GithubRepoId,
    string Owner,
    string Name,
    string FullName,
    string DefaultBranch,
    bool IsPrivate,
    string? Description,
    string? HtmlUrl);

public record NotificationSettingsRequest(
    string? SlackWebhookUrl,
    string? DiscordWebhookUrl);
