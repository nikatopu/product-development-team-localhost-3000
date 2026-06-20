namespace ApiDocGen.Data.Entities;

public class RepositoryScan
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid RepositoryId { get; set; }
    public Repository Repository { get; set; } = null!;

    public DateTime StartedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }
    public ScanStatus Status { get; set; } = ScanStatus.Scanning;

    public int TotalRoutes { get; set; }
    public int TotalControllers { get; set; }
    public string? ApiType { get; set; }
    public string? ErrorMessage { get; set; }
    public string? ResultJson { get; set; }
    public string? EnumsJson { get; set; }

    public string? TriggerSource { get; set; } // "manual" | "webhook" | "scheduled"
    public string? CommitSha { get; set; }

    public ICollection<BreakingChangeRecord> BreakingChanges { get; set; } = new List<BreakingChangeRecord>();
}
