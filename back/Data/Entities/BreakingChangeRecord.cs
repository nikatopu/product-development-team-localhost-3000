namespace ApiDocGen.Data.Entities;

public class BreakingChangeRecord
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ScanId { get; set; }
    public RepositoryScan Scan { get; set; } = null!;

    public string ChangeType { get; set; } = string.Empty;
    public string Severity { get; set; } = "Breaking";
    public string AffectedEndpoint { get; set; } = string.Empty;
    public string? AffectedField { get; set; }
    public string? OldValue { get; set; }
    public string? NewValue { get; set; }
    public DateTime DetectedAt { get; set; } = DateTime.UtcNow;
}
