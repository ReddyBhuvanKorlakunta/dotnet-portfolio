using System.Text.Json;
using System.Text.Json.Serialization;
using EnterprisePortal.Application.Cases;
using EnterprisePortal.Domain.Entities;
namespace EnterprisePortal.Infrastructure.Data;

public sealed class StoredCase
{
    public int Id { get; set; }
    public string Reference { get; set; } = "";
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public CaseType Type { get; set; }
    public CasePriority Priority { get; set; }
    public string ApplicantName { get; set; } = "";
    public string ApplicantEmail { get; set; } = "";
    public string CreatedBy { get; set; } = "";
    public string? EncryptedNotes { get; set; }
    public CaseStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<StoredTransition> History { get; set; } = [];
    public static StoredCase From(Case c) => new() { Id = c.Id, Reference = c.Reference, Title = c.Title, Description = c.Description, Type = c.Type, Priority = c.Priority, ApplicantName = c.ApplicantName, ApplicantEmail = c.ApplicantEmail, CreatedBy = c.CreatedBy, EncryptedNotes = c.EncryptedNotes, Status = c.Status, CreatedAt = c.CreatedAt };
    public Case ToDomain()
    {
        var c = new Case(Reference, Title, Description, Type, ApplicantName, ApplicantEmail, CreatedBy);
        typeof(Entity).GetProperty(nameof(Entity.Id))!.SetValue(c, Id);
        typeof(Entity).GetProperty(nameof(Entity.CreatedAt))!.SetValue(c, CreatedAt);
        c.SetPriority(Priority);
        if (EncryptedNotes is not null) c.SetEncryptedNotes(EncryptedNotes, CreatedBy);
        foreach (var transition in History)
        {
            switch (transition.Status)
            {
                case CaseStatus.Submitted: c.Submit(transition.Actor); break;
                case CaseStatus.InReview: c.StartReview(transition.Actor); break;
                case CaseStatus.Approved: c.Approve(transition.Actor); break;
                case CaseStatus.Rejected: c.Reject(transition.Actor, transition.Reason ?? "Reason unavailable"); break;
                case CaseStatus.Closed: c.Close(transition.Actor); break;
            }
        }
        return c;
    }
}
public record StoredTransition(CaseStatus Status, string Actor, DateTime At, string? Reason);
public interface ICaseStorage
{
    Task<List<StoredCase>> ReadAsync(CancellationToken ct);
    Task WriteAsync(List<StoredCase> cases, CancellationToken ct);
}
public sealed class DemoFileStorage(string path, IEncryptionService crypto) : ICaseStorage
{
    public async Task<List<StoredCase>> ReadAsync(CancellationToken ct)
    {
        if (!File.Exists(path)) return [];
        return JsonSerializer.Deserialize<List<StoredCase>>(crypto.Decrypt(await File.ReadAllTextAsync(path, ct))) ?? [];
    }
    public async Task WriteAsync(List<StoredCase> cases, CancellationToken ct)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(path))!);
        var temp = path + ".tmp";
        await File.WriteAllTextAsync(temp, crypto.Encrypt(JsonSerializer.Serialize(cases)), ct);
        File.Move(temp, path, true);
    }
}
public sealed class StoreGate { public SemaphoreSlim Lock { get; } = new(1, 1); }
public sealed class CaseRepository(ICaseStorage storage, StoreGate gate) : ICaseRepository, IUnitOfWork, IAsyncDisposable
{
    private List<StoredCase>? snapshot;
    private readonly Dictionary<int, Case> tracked = [];
    private bool locked;
    private async Task<List<StoredCase>> Load(CancellationToken ct)
    {
        if (snapshot is not null) return snapshot;
        await gate.Lock.WaitAsync(ct);
        locked = true;
        snapshot = await storage.ReadAsync(ct);
        return snapshot;
    }
    public async Task<Case?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var item = (await Load(ct)).Find(c => c.Id == id);
        if (item is null) return null;
        var domain = item.ToDomain(); tracked[id] = domain; return domain;
    }
    public async Task<(IReadOnlyList<Case> Items, int Total)> SearchAsync(CaseQuery q, CancellationToken ct = default)
    {
        IEnumerable<StoredCase> items = await Load(ct);
        if (q.Status.HasValue) items = items.Where(c => c.Status == q.Status);
        if (q.Type.HasValue) items = items.Where(c => c.Type == q.Type);
        if (!string.IsNullOrWhiteSpace(q.Search)) items = items.Where(c => c.Title.Contains(q.Search, StringComparison.OrdinalIgnoreCase) || c.Reference.Contains(q.Search, StringComparison.OrdinalIgnoreCase));
        var total = items.Count();
        items = q.Sort switch { "Title_asc" => items.OrderBy(c => c.Title).ThenBy(c => c.Id), "Title_desc" => items.OrderByDescending(c => c.Title).ThenBy(c => c.Id), _ => items.OrderByDescending(c => c.CreatedAt).ThenByDescending(c => c.Id) };
        return (items.Skip((q.Page - 1) * q.PageSize).Take(q.PageSize).Select(c => c.ToDomain()).ToList(), total);
    }
    public async Task AddAsync(Case c, CancellationToken ct = default)
    {
        var items = await Load(ct);
        typeof(Entity).GetProperty(nameof(Entity.Id))!.SetValue(c, items.Count == 0 ? 1 : items.Max(x => x.Id) + 1);
        tracked[c.Id] = c;
    }
    public async Task<int> CountByStatusAsync(CaseStatus status, CancellationToken ct = default) => (await Load(ct)).Count(c => c.Status == status);
    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        var items = await Load(ct);
        foreach (var c in tracked.Values)
        {
            var prior = items.Find(x => x.Id == c.Id);
            var next = StoredCase.From(c);
            next.History = prior?.History.ToList() ?? [];
            if (prior is null || prior.Status != c.Status) next.History.Add(new(c.Status, c.CreatedBy, DateTime.UtcNow, c.RejectionReason));
            if (prior is not null) items.Remove(prior);
            items.Add(next);
        }
        await storage.WriteAsync(items, ct);
        return tracked.Count;
    }
    public ValueTask DisposeAsync() { if (locked) gate.Lock.Release(); return ValueTask.CompletedTask; }
}
public sealed class ReferenceGenerator : IReferenceGenerator { public string NextReference() => $"CS-{Guid.NewGuid():N}"; }
