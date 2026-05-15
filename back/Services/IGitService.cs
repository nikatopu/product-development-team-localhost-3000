namespace ApiDocGen.Services;

public interface IGitService
{
    Task<string> CloneRepositoryAsync(string repoUrl, string? branch = "main");
    void Cleanup(string localPath);
}