namespace ApiDocGen.Models.Responses;

public class BreakingChangeInfo
{
    public string ChangeType { get; set; } = string.Empty;
    public string Severity { get; set; } = "Breaking";
    public string AffectedEndpoint { get; set; } = string.Empty;
    public string? AffectedField { get; set; }
    public string? OldValue { get; set; }
    public string? NewValue { get; set; }
}
