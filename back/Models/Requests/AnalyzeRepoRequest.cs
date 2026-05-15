namespace ApiDocGen.Models.Requests;

public class AnalyzeRepoRequest
{
    public string RepoUrl { get; set; } = string.Empty;
    public string? Branch { get; set; } = "main";
}