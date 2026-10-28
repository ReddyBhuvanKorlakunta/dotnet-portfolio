namespace EnterprisePortal.Domain.Entities;

public class Case : Entity
{
    private static readonly Dictionary<CaseStatus, CaseStatus[]> Allowed = new()
    {
        [CaseStatus.Draft] = new[] { CaseStatus.Submitted, CaseStatus.Closed },
        [CaseStatus.Submitted] = new[] { CaseStatus.InReview, CaseStatus.Closed },
        [CaseStatus.InReview] = new[] { CaseStatus.Approved, CaseStatus.Rejected, CaseStatus.Closed },
        [CaseStatus.Approved] = new[] { CaseStatus.Closed },
        [CaseStatus.Rejected] = new[] { CaseStatus.Submitted, CaseStatus.Closed },
        [CaseStatus.Closed] = Array.Empty<CaseStatus>(),
    };
    public string Reference { get; private set; } = "";
    public string Title { get; private set; } = "";
    public string Description { get; private set; } = "";
    public CaseType Type { get; private set; }
    public CaseStatus Status { get; private set; } = CaseStatus.Draft;
    public CasePriority Priority { get; private set; } = CasePriority.Normal;
    public string ApplicantName { get; private set; } = "";
    public string ApplicantEmail { get; private set; } = "";
    public string? EncryptedNotes { get; private set; }
    public string? AssignedTo { get; private set; }
    public string? RejectionReason { get; private set; }
    public List<CaseNote> Notes { get; private set; } = new();
    public Case() { }
    public Case(string reference, string title, string description, CaseType type,
        string applicantName, string applicantEmail, string createdBy)
    {
        if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Title is required.", nameof(title));
        Reference = reference; Title = title; Description = description; Type = type;
        ApplicantName = applicantName; ApplicantEmail = applicantEmail; CreatedBy = createdBy;
    }
    public void SetPriority(CasePriority priority) { if (!Enum.IsDefined(priority)) throw new ArgumentException("Invalid priority."); Priority = priority; }
    public void Submit(string by) => Transition(CaseStatus.Submitted, by);
    public void StartReview(string by) => Transition(CaseStatus.InReview, by);
    public void Approve(string by) => Transition(CaseStatus.Approved, by);
    public void Reject(string by, string reason)
    {
        if (string.IsNullOrWhiteSpace(reason)) throw new ArgumentException("Rejection reason is required.", nameof(reason));
        Transition(CaseStatus.Rejected, by); RejectionReason = reason;
    }
    public void Close(string by) => Transition(CaseStatus.Closed, by);
    public void AssignTo(string user, string by) { AssignedTo = user; Touch(by); }
    public void SetEncryptedNotes(string cipher, string by) { EncryptedNotes = cipher; Touch(by); }
    public void AddNote(string author, string text) => Notes.Add(new CaseNote(Id, author, text));
    private void Transition(CaseStatus next, string by)
    {
        if (!Allowed[Status].Contains(next))
            throw new InvalidOperationException($"Cannot move a case from {Status} to {next}.");
        Status = next; Touch(by);
    }
}
