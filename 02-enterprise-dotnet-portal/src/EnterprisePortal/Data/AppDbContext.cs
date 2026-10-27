using EnterprisePortal.Models;
using Microsoft.EntityFrameworkCore;

namespace EnterprisePortal.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<CaseRecord> Cases => Set<CaseRecord>();
}
