using System;
using System.Collections.Generic;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public partial class AccessDbContext : DbContext
{
    public AccessDbContext()
    {
    }

    public AccessDbContext(DbContextOptions<AccessDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AccessGroup> AccessGroups { get; set; }

    public virtual DbSet<AccessRequest> AccessRequests { get; set; }

    public virtual DbSet<AccessRequestHistory> AccessRequestHistories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Connection string will be configured via dependency injection in Program.cs
        // This method is left empty to avoid hardcoded connection strings
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AccessGroup>(entity =>
        {
            entity.HasKey(e => e.GroupId).HasName("PK__AccessGr__149AF36A4CAD0FF6");

            entity.ToTable("AccessGroup", tb => tb.HasComment("Stores access groups for profile authorization"));

            entity.HasIndex(e => e.GroupCode, "IX_AccessGroup_GroupCode").IsUnique();

            entity.Property(e => e.CreatedByNum).HasMaxLength(50);
            entity.Property(e => e.GroupName).HasMaxLength(255);
        });

        modelBuilder.Entity<AccessRequest>(entity =>
        {
            entity.HasKey(e => e.RequestId);

            entity.ToTable("AccessRequest", tb => tb.HasTrigger("TR_AccessRequestHistory"));

            entity.HasIndex(e => e.EmployeeNum, "Index_EmployeeNum").IsUnique();

            entity.HasIndex(e => e.RequestCode, "Index_RequestCode").IsUnique();

            entity.Property(e => e.AccessExpiresAt).HasColumnType("datetime");
            entity.Property(e => e.ApproverNum)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.CreatedByNum)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EmployeeNum)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.JobManufacturingSiteCode)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.JobSiteCode)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedByNum)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.UtcCreatedAt).HasColumnType("datetime");
            entity.Property(e => e.UtcUpdatedAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<AccessRequestHistory>(entity =>
        {
            entity.HasKey(e => e.AccessRequestHistoryId).HasName("PK__AccessRe__3AF1115C2B4781BE");

            entity.ToTable("AccessRequestHistory");

            entity.Property(e => e.AccessExpiresAt).HasColumnType("datetime");
            entity.Property(e => e.ApproverNum)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.CreatedByNum)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EmployeeNum)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.JobManufacturingSiteCode)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.JobSiteCode)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedByNum)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.UtcCreatedAt).HasColumnType("datetime");
            entity.Property(e => e.UtcUpdatedAt).HasColumnType("datetime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
