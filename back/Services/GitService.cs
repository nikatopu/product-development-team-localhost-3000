using LibGit2Sharp;

namespace ApiDocGen.Services;

public class GitService : IGitService
{
    private readonly ILogger<GitService> _logger;
    private readonly string _workingDirectory;

    public GitService(ILogger<GitService> logger, IConfiguration config)
    {
        _logger = logger;
        _workingDirectory = config["WorkingDirectory"]
            ?? Path.Combine(Path.GetTempPath(), "apidocgen");
        Directory.CreateDirectory(_workingDirectory);
    }

    public Task<string> CloneRepositoryAsync(string repoUrl, string? branch = "main")
    {
        var repoId = Guid.NewGuid().ToString("N")[..8];
        var localPath = Path.Combine(_workingDirectory, repoId);

        _logger.LogInformation("Cloning {RepoUrl} into {LocalPath}", repoUrl, localPath);

        var options = new CloneOptions
        {
            BranchName = branch,
            RecurseSubmodules = false
        };

        Repository.Clone(repoUrl, localPath, options);
        _logger.LogInformation("Clone complete: {LocalPath}", localPath);

        return Task.FromResult(localPath);
    }

    public void Cleanup(string localPath)
    {
        if (!Directory.Exists(localPath)) return;
        // Git repos have read-only files — need to force delete
        foreach (var file in Directory.GetFiles(localPath, "*", SearchOption.AllDirectories))
            File.SetAttributes(file, FileAttributes.Normal);
        Directory.Delete(localPath, recursive: true);
        _logger.LogInformation("Cleaned up {LocalPath}", localPath);
    }
}