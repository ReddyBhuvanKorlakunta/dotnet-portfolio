using EnterprisePortal.Application.Cases;
using EnterprisePortal.Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace EnterprisePortal.Infrastructure.Data;
public class CaseRepository(AppDbContext db) : ICaseRepository
{
    public Task<Case?> GetByIdAsync(int id, CancellationToken ct = default)
        => db.Cases.Include(c => c.Notes).FirstOrDefaultAsync(c => c.Id == id, ct);
    public async Task<(IReadOnlyList<Case> Items, int Total)> SearchAsync(CaseQuery q, CancellationToken ct = default)
    {
        var query = db.Cases.Include(c => c.Notes).AsQueryable();
        if (q.Status.HasValue) query = query.Where(c => c.Status == q.Status.Value);
        if (q.Type.HasValue) query = query.Where(c => c.Type == q.Type.Value);
        if (!string.IsNullOrWhiteSpace(q.Search))
            query = query.Where(c => c.Title.Contains(q.Search) || c.Reference.Contains(q.Search) || c.ApplicantName.Contains(q.Search));
        var total = await query.CountAsync(ct);
        query = q.Sort switch
        {
            "Title_asc" => query.OrderBy(c => c.Title),
            "Title_desc" => query.OrderByDescending(c => c.Title),
            _ => query.OrderByDescending(c => c.CreatedAt),
        };
        var items = await query.Skip((q.Page - 1) * q.PageSize).Take(q.PageSize).ToListAsync(ct);
        return (items, total);
    }
    public Task AddAsync(Case c, CancellationToken ct = default) => db.Cases.AddAsync(c, ct).AsTask();
    public Task<int> CountByStatusAsync(CaseStatus status, CancellationToken ct = default)
        => db.Cases.CountAsync(c => c.Status == status, ct);
}
public class UnitOfWork(AppDbContext db) : IUnitOfWork
{
    public Task<int> SaveChangesAsync(CancellationToken ct = default) => db.SaveChangesAsync(ct);
}
public class ReferenceGenerator : IReferenceGenerator
{
    // Production: sequence / database-generated reference. This format is kept
    // stable so existing references never change shape.
    public string NextReference() => $"CA-{DateTime.UtcNow:yyyy}-{Random.Shared.Next(10000, 99999)}";
}
