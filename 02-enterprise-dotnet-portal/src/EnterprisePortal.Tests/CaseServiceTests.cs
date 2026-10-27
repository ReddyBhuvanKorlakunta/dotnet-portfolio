using EnterprisePortal.Application.Cases;
using EnterprisePortal.Domain.Entities;
namespace EnterprisePortal.Tests;
internal class FakeRepo : ICaseRepository
{
    public List<Case> Items = new();
    public Task<Case?> GetByIdAsync(int id, CancellationToken ct = default) => Task.FromResult(Items.FirstOrDefault(c => c.Id == id));
    public Task<(IReadOnlyList<Case> Items, int Total)> SearchAsync(CaseQuery q, CancellationToken ct = default)
    {
        var list = Items.AsQueryable();
        if (q.Status.HasValue) list = list.Where(c => c.Status == q.Status.Value);
        return Task.FromResult(((IReadOnlyList<Case>)list.ToList(), list.Count()));
    }
    public Task AddAsync(Case c, CancellationToken ct = default) { Items.Add(c); return Task.CompletedTask; }
    public Task<int> CountByStatusAsync(CaseStatus s, CancellationToken ct = default) => Task.FromResult(Items.Count(c => c.Status == s));
}
internal class FakeUow : IUnitOfWork { public Task<int> SaveChangesAsync(CancellationToken ct = default) => Task.FromResult(1); }
internal class FakeCrypto : IEncryptionService { public string Encrypt(string p) => "ENC:" + p; public string Decrypt(string c) => c[4..]; }
internal class FakeRefs : IReferenceGenerator { public string NextReference() => "CA-TEST-1"; }
public class CaseServiceTests
{
    private CaseService Service(FakeRepo? repo = null) => new(repo ?? new FakeRepo(), new FakeUow(), new FakeCrypto(), new FakeRefs());
    [Fact] public async Task Create_Submits_Case_And_Encrypts_Notes()
    {
        var repo = new FakeRepo();
        var r = await Service(repo).CreateAsync(new CreateCaseRequest { Title = "T", ApplicantName = "A", ApplicantEmail = "a@example.test", Notes = "secret" }, "tester");
        Assert.True(r.Ok);
        Assert.Equal(CaseStatus.Submitted, repo.Items.Single().Status);
        Assert.StartsWith("ENC:", repo.Items.Single().EncryptedNotes);
    }
    [Fact] public async Task ChangeStatus_Rejects_Illegal_Transition()
    {
        var repo = new FakeRepo();
        var svc = Service(repo);
        await svc.CreateAsync(new CreateCaseRequest { Title = "T", ApplicantName = "A", ApplicantEmail = "a@example.test" }, "tester");
        // Created case is Submitted (Id is 0 in the fake — use the stored instance)
        var stored = repo.Items.Single();
        var r = await svc.ChangeStatusAsync(stored.Id, new ChangeStatusRequest { Status = CaseStatus.Approved }, "tester");
        Assert.False(r.Ok); // Submitted -> Approved is illegal; must go via InReview
    }
}
