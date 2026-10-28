using EnterprisePortal.Application.Common;
using System.ComponentModel.DataAnnotations;
using EnterprisePortal.Domain.Entities;
namespace EnterprisePortal.Application.Cases;

public class CaseService(ICaseRepository repo, IUnitOfWork uow, IEncryptionService crypto, IReferenceGenerator refs)
{
    public async Task<PagedResult<CaseListItem>> ListAsync(CaseQuery q, CancellationToken ct = default)
    {
        q = q with { Page = Math.Max(1, q.Page), PageSize = Math.Clamp(q.PageSize, 1, 100) };
        var (items, total) = await repo.SearchAsync(q, ct);
        return new PagedResult<CaseListItem>(items.Select(ToListItem).ToList(), total, q.Page, q.PageSize);
    }
    public async Task<Result<CaseDetail>> GetAsync(int id, CancellationToken ct = default)
    {
        var c = await repo.GetByIdAsync(id, ct);
        return c is null ? Result<CaseDetail>.Fail("Case not found.") : Result<CaseDetail>.Success(ToDetail(c));
    }
    public async Task<Result<CaseDetail>> CreateAsync(CreateCaseRequest req, string user, CancellationToken ct = default)
    {
        Validator.ValidateObject(req, new ValidationContext(req), true);
        var c = new Case(refs.NextReference(), req.Title, req.Description ?? "", req.Type, req.ApplicantName, req.ApplicantEmail, user);
        c.SetPriority(req.Priority);
        if (!string.IsNullOrWhiteSpace(req.Notes)) c.SetEncryptedNotes(crypto.Encrypt(req.Notes), user);
        c.Submit(user);
        await repo.AddAsync(c, ct);
        await uow.SaveChangesAsync(ct);
        return Result<CaseDetail>.Success(ToDetail(c));
    }
    public async Task<Result<CaseDetail>> ChangeStatusAsync(int id, ChangeStatusRequest req, string user, CancellationToken ct = default)
    {
        var c = await repo.GetByIdAsync(id, ct);
        if (c is null) return Result<CaseDetail>.Fail("Case not found.");
        try
        {
            switch (req.Status)
            {
                case CaseStatus.Submitted: c.Submit(user); break;
                case CaseStatus.InReview: c.StartReview(user); break;
                case CaseStatus.Approved: c.Approve(user); break;
                case CaseStatus.Rejected: c.Reject(user, req.Reason ?? ""); break;
                case CaseStatus.Closed: c.Close(user); break;
                default: return Result<CaseDetail>.Fail($"Status change to {req.Status} is not supported here.");
            }
        }
        catch (InvalidOperationException ex) { return Result<CaseDetail>.Fail(ex.Message); }
        catch (ArgumentException ex) { return Result<CaseDetail>.Fail(ex.Message); }
        await uow.SaveChangesAsync(ct);
        return Result<CaseDetail>.Success(ToDetail(c));
    }
    private static CaseListItem ToListItem(Case c) => new(c.Id, c.Reference, c.Title, c.Type, c.Status, c.Priority, c.AssignedTo, c.CreatedAt);
    private static CaseDetail ToDetail(Case c) => new(c.Id, c.Reference, c.Title, c.Description, c.Type, c.Status, c.Priority, c.ApplicantName, c.AssignedTo, c.CreatedAt, c.Notes.Select(n => n.Text).ToList());
}
