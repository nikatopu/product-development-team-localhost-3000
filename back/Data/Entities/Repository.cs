namespace ApiDocGen.Data.Entities;

public enum ScanStatus { Connected, Scanning, Ready, Failed }

public class Repository
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public long GithubRepoId { get; set; }
    public string Owner { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string DefaultBranch { get; set; } = "main";
    public bool IsPrivate { get; set; }
    public string? Description { get; set; }
    public string? HtmlUrl { get; set; }

    public ScanStatus Status { get; set; } = ScanStatus.Connected;
    public DateTime ConnectedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastScannedAt { get; set; }

    // Cached stats from the last scan
    public int LastScanTotalRoutes { get; set; }
    public int LastScanTotalControllers { get; set; }
    public string? LastScanApiType { get; set; }
    public int LastScanBreakingChangeCount { get; set; }

    // Webhook integration
    public string? WebhookSecret { get; set; }
    public long? GithubWebhookId { get; set; }

    // Notification config
    public string? SlackWebhookUrl { get; set; }
    public string? DiscordWebhookUrl { get; set; }

    public ICollection<RepositoryScan> Scans { get; set; } = new List<RepositoryScan>();
}
