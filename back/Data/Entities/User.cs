namespace ApiDocGen.Data.Entities;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public long GithubId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string AvatarUrl { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? GithubAccessToken { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Repository> Repositories { get; set; } = new List<Repository>();
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}
