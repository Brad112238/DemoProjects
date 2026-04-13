using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebApiDemo.Models.TestDb;

public partial class TestContext : DbContext
{
    public TestContext()
    {
    }

    public TestContext(DbContextOptions<TestContext> options)
        : base(options)
    {
    }

    public virtual DbSet<UserCredit> UserCredits { get; set; }

    public virtual DbSet<UserTopUp> UserTopUps { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Chinese_Taiwan_Stroke_CI_AS");

        modelBuilder.Entity<UserCredit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.UserCredit");

            entity.ToTable("UserCredit");

            entity.HasIndex(e => e.UserId, "IX_UserId");

            entity.Property(e => e.CreateTime).HasColumnType("datetime");
            entity.Property(e => e.ModifyTime).HasColumnType("datetime");
            entity.Property(e => e.SmsName).HasMaxLength(20);
            entity.Property(e => e.SmsPassword).HasMaxLength(50);
        });

        modelBuilder.Entity<UserTopUp>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.UserTopUp");

            entity.ToTable("UserTopUp");

            entity.HasIndex(e => e.UserId, "IX_UserTopUp_UserId");

            entity.Property(e => e.CreateTime).HasColumnType("datetime");
            entity.Property(e => e.ModifyTime).HasColumnType("datetime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
