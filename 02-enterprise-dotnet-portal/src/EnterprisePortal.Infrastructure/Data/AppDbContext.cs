using EnterprisePortal.Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace EnterprisePortal.Infrastructure.Data;
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Case> Cases => Set<Case>();
    public DbSet<CaseNote> CaseNotes => Set<CaseNote>();
    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Case>(e =>
        {
            e.HasIndex(c => c.Reference).IsUnique();
            e.HasIndex(c => c.Status);
            e.HasIndex(c => c.CreatedAt);
            e.Property(c => c.RowVersion).IsRowVersion();
            e.HasQueryFilter(c => !c.IsDeleted); // soft delete is invisible to every query
            e.HasMany(c => c.Notes).WithOne().HasForeignKey(n => n.CaseId);
        });
        b.Entity<CaseNote>(e => { e.HasQueryFilter(n => !n.IsDeleted); });
    }
}
