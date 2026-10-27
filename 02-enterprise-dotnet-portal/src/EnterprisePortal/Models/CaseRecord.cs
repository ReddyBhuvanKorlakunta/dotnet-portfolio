namespace EnterprisePortal.Models;

public class CaseRecord
{
    public int Id { get; set; }
    public string Reference { get; set; } = ""; // e.g. CA-1042 (dummy)
    public string Status { get; set; } = "In review";
    public string? EncryptedNotes { get; set; } // AES-GCM ciphertext, see EncryptionService
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
