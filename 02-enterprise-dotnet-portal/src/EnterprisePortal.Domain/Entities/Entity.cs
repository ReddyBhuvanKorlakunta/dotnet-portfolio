namespace EnterprisePortal.Domain.Entities;
public abstract class Entity
{
    public int Id { get; protected set; }
    public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; protected set; }
    public string CreatedBy { get; protected set; } = "system";
    public bool IsDeleted { get; protected set; }
    public byte[]? RowVersion { get; set; }
    protected void Touch(string by) { UpdatedAt = DateTime.UtcNow; CreatedBy = by; }
    public void SoftDelete(string by) { IsDeleted = true; Touch(by); }
}
