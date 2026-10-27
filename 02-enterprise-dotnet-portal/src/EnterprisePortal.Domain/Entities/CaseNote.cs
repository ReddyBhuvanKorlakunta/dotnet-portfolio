namespace EnterprisePortal.Domain.Entities;
public class CaseNote : Entity
{
    public int CaseId { get; private set; }
    public string Author { get; private set; } = "";
    public string Text { get; private set; } = "";
    public CaseNote() { }
    public CaseNote(int caseId, string author, string text)
    {
        if (string.IsNullOrWhiteSpace(text)) throw new ArgumentException("Note text is required.", nameof(text));
        CaseId = caseId; Author = author; Text = text;
    }
}
