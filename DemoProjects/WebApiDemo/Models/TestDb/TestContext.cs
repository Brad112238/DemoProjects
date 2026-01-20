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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("");

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

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
