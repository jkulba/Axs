using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class AccessDbContext : DbContext
{
    public AccessDbContext(DbContextOptions<AccessDbContext> options) : base(options)
    {
    }

    public DbSet<AccessRequest> AccessRequests { get; set; }
    public DbSet<Authorization> Authorizations { get; set; }
    public DbSet<Activity> Activities { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserGroup> UserGroups { get; set; }
    public DbSet<UserGroupMember> UserGroupMembers { get; set; }
    public DbSet<GroupAuthorization> GroupAuthorizations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure AccessRequest
        modelBuilder.Entity<AccessRequest>(entity =>
        {
            entity.ToTable("AccessRequest");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.RequestCode).IsRequired();
            entity.Property(e => e.UserName).IsRequired().HasMaxLength(255);
            entity.Property(e => e.ActivityCode).HasMaxLength(50);
            entity.Property(e => e.ApplicationName).HasMaxLength(50);
            entity.Property(e => e.Workstation).HasMaxLength(50);

            // Create unique index on RequestCode
            entity.HasIndex(e => e.RequestCode).IsUnique();

            // Create indexes for common queries
            entity.HasIndex(e => e.JobNumber);
            entity.HasIndex(e => e.UserName);
        });

        // Configure Authorization
        modelBuilder.Entity<Authorization>(entity =>
        {
            entity.HasKey(e => e.AuthorizationId);
            entity.Property(e => e.UserId).IsRequired().HasMaxLength(255);
            entity.Property(e => e.CreatedByNum).HasMaxLength(50);
            entity.Property(e => e.UpdatedByNum).HasMaxLength(50);

            // Configure relationship with Activity
            entity.HasOne(e => e.Activity)
                  .WithMany(a => a.Authorizations)
                  .HasForeignKey(e => e.ActivityId)
                  .OnDelete(DeleteBehavior.Restrict);

            // Configure relationship with User (optional, since UserId might reference external system)
            entity.HasOne(e => e.User)
                  .WithMany(u => u.Authorizations)
                  .HasForeignKey(e => e.UserId)
                  .HasPrincipalKey(u => u.UserId)
                  .OnDelete(DeleteBehavior.SetNull);

            // Create indexes
            entity.HasIndex(e => new { e.JobNumber, e.UserId, e.ActivityId }).IsUnique();
        });

        // Configure Activity
        modelBuilder.Entity<Activity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ActivityCode).IsRequired().HasMaxLength(50);
            entity.Property(e => e.ActivityName).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Description).HasMaxLength(1000);

            // Create unique index on ActivityCode
            entity.HasIndex(e => e.ActivityCode).IsUnique();
        });

        // Configure UserGroup
        modelBuilder.Entity<UserGroup>(entity =>
        {
            entity.HasKey(e => e.GroupId);
            entity.Property(e => e.GroupName).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.GroupOwner).HasMaxLength(255);

            // Create unique index on GroupName
            entity.HasIndex(e => e.GroupName).IsUnique();
        });

        // Configure UserGroupMember
        modelBuilder.Entity<UserGroupMember>(entity =>
        {
            entity.HasKey(e => new { e.GroupId, e.UserId });
            entity.Property(e => e.UserId).IsRequired().HasMaxLength(255);

            // Configure relationship with UserGroup
            entity.HasOne(e => e.Group)
                  .WithMany(g => g.Members)
                  .HasForeignKey(e => e.GroupId)
                  .OnDelete(DeleteBehavior.Cascade);

            // Configure relationship with User (optional, since UserId might reference external system)
            entity.HasOne(e => e.User)
                  .WithMany(u => u.UserGroupMemberships)
                  .HasForeignKey(e => e.UserId)
                  .HasPrincipalKey(u => u.UserId)
                  .OnDelete(DeleteBehavior.SetNull);
        });

        // Configure GroupAuthorization
        modelBuilder.Entity<GroupAuthorization>(entity =>
        {
            entity.HasKey(e => e.AuthorizationId);
            entity.Property(e => e.CreatedByNum).HasMaxLength(50);
            entity.Property(e => e.UpdatedByNum).HasMaxLength(50);

            // Configure relationship with UserGroup
            entity.HasOne(e => e.Group)
                  .WithMany(g => g.GroupAuthorizations)
                  .HasForeignKey(e => e.GroupId)
                  .OnDelete(DeleteBehavior.Restrict);

            // Configure relationship with Activity
            entity.HasOne(e => e.Activity)
                  .WithMany(a => a.GroupAuthorizations)
                  .HasForeignKey(e => e.ActivityId)
                  .OnDelete(DeleteBehavior.Restrict);

            // Create unique index
            entity.HasIndex(e => new { e.JobNumber, e.GroupId, e.ActivityId }).IsUnique();
        });

        // Configure User
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.GraphId).IsRequired().HasMaxLength(255);
            entity.Property(e => e.UserId).IsRequired().HasMaxLength(255);
            entity.Property(e => e.DisplayName).IsRequired().HasMaxLength(255);
            entity.Property(e => e.PrincipalName).IsRequired().HasMaxLength(255);
            entity.Property(e => e.CreatedByNum).HasMaxLength(50);
            entity.Property(e => e.UpdatedByNum).HasMaxLength(50);

            // Create unique indexes
            entity.HasIndex(e => e.GraphId).IsUnique();
            entity.HasIndex(e => e.UserId).IsUnique();
            entity.HasIndex(e => e.PrincipalName).IsUnique();
        });
    }
}
