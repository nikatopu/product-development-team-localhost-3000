namespace ApiDocGen.Models.Responses;

public class DocumentationResult
{
    public string Format { get; set; } = string.Empty; 
    public string Content { get; set; } = string.Empty;
    public DateTime GeneratedAt { get; set; }
}