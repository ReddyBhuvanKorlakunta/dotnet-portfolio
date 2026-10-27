using EnterprisePortal.Domain.Entities;
namespace EnterprisePortal.Application.Cases;
public interface ICaseRepository
{
    Task<Case?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<(IReadOnlyList<Case> Items, int Total)> SearchAsync(CaseQuery q, CancellationToken ct = default);
    Task AddAsync(Case c, CancellationToken ct = default);
    Task<int> CountByStatusAsync(CaseStatus status, CancellationToken ct = default);
}
public interface IUnitOfWork { Task<int> SaveChangesAsync(CancellationToken ct = default); }
public interface IEncryptionService { string Encrypt(string plain); string Decrypt(string cipher); }
public interface IReferenceGenerator { string NextReference(); }
