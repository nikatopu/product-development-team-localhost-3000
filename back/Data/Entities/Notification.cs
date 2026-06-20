namespace ApiDocGen.Data.Entities;

public enum NotificationType { BreakingChange, ScanComplete, ScanFailed }

public class Notification
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public Guid? RepositoryId { get; set; }
    public Repository? Repository { get; set; }

    public NotificationType Type { get; set; }
    public string Message { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
