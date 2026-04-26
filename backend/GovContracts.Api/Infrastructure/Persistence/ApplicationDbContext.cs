using Microsoft.EntityFrameworkCore;
using GovContracts.Api.Domain.Entities;

namespace GovContracts.Api.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Contract> Contracts { get; set; }
    public DbSet<AttachmentTemplate> AttachmentTemplates { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Contract>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ContractName).IsRequired().HasMaxLength(200);
            
            // تخزين المرفقات كـ JSON في SQL Server (EF Core feature)
            entity.OwnsMany(e => e.Attachments, a =>
            {
                a.ToJson();
            });
        });

        modelBuilder.Entity<AttachmentTemplate>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
        });
    }
}