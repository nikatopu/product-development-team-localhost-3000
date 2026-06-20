using ApiDocGen.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApiDocGen.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Repository> Repositories => Set<Repository>();
    public DbSet<RepositoryScan> RepositoryScans => Set<RepositoryScan>();
    public DbSet<BreakingChangeRecord> BreakingChanges => Set<BreakingChangeRecord>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<Notification> Notifications => Set<Notification>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(e =>
        {
            e.HasKey(u => u.Id);
            e.HasIndex(u => u.GithubId).IsUnique();
            e.HasIndex(u => u.Username);
            e.Property(u => u.GithubAccessToken).IsRequired(false);
        });

        modelBuilder.Entity<Repository>(e =>
        {
            e.HasKey(r => r.Id);
            e.HasIndex(r => new { r.UserId, r.GithubRepoId }).IsUnique();
            e.HasOne(r => r.User)
                .WithMany(u => u.Repositories)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            e.Property(r => r.Status).HasConversion<string>();
        });

        modelBuilder.Entity<RepositoryScan>(e =>
        {
            e.HasKey(s => s.Id);
            e.HasIndex(s => s.RepositoryId);
            e.HasIndex(s => s.StartedAt);
            e.HasOne(s => s.Repository)
                .WithMany(r => r.Scans)
                .HasForeignKey(s => s.RepositoryId)
                .OnDelete(DeleteBehavior.Cascade);
            e.Property(s => s.Status).HasConversion<string>();
            e.Property(s => s.ResultJson).HasColumnType("text");
            e.Property(s => s.EnumsJson).HasColumnType("text");
        });

        modelBuilder.Entity<BreakingChangeRecord>(e =>
        {
            e.HasKey(b => b.Id);
            e.HasIndex(b => b.ScanId);
            e.HasOne(b => b.Scan)
                .WithMany(s => s.BreakingChanges)
                .HasForeignKey(b => b.ScanId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<RefreshToken>(e =>
        {
            e.HasKey(t => t.Id);
            e.HasIndex(t => t.TokenHash).IsUnique();
            e.HasIndex(t => t.UserId);
            e.HasOne(t => t.User)
                .WithMany(u => u.RefreshTokens)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Notification>(e =>
        {
            e.HasKey(n => n.Id);
            e.HasIndex(n => new { n.UserId, n.IsRead });
            e.HasOne(n => n.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            e.Property(n => n.Type).HasConversion<string>();
        });
    }
}
