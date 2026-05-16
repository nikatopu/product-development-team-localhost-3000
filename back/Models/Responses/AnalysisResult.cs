namespace ApiDocGen.Models.Responses;

public class AnalysisResult
{
    public string RepoUrl { get; set; } = string.Empty;
    public string ProjectName { get; set; } = string.Empty;
    public DateTime AnalyzedAt { get; set; }
    public List<RouteInfo> Routes { get; set; } = new();
    public AnalysisMetadata Metadata { get; set; } = new();
}

public class AnalysisMetadata
{
    public int TotalRoutes { get; set; }
    public int TotalControllers { get; set; }
    public List<string> DetectedFrameworks { get; set; } = new();
    public string ApiType { get; set; } = string.Empty; 
}