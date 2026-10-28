using System.ComponentModel.DataAnnotations;
using EnterprisePortal.Domain.Entities;
namespace EnterprisePortal.Application.Cases;

public record CaseListItem(int Id, string Reference, string Title, CaseType Type, CaseStatus Status, CasePriority Priority, string? AssignedTo, DateTime CreatedAt);
public record CaseDetail(int Id, string Reference, string Title, string Description, CaseType Type, CaseStatus Status, CasePriority Priority, string ApplicantName, string? AssignedTo, DateTime CreatedAt, IReadOnlyList<string> Notes);
public record CaseQuery(CaseStatus? Status, CaseType? Type, string? Search, int Page = 1, int PageSize = 20, string Sort = "CreatedAt_desc");
public class CreateCaseRequest
{
    [Required, StringLength(200)] public string Title { get; set; } = "";
    [StringLength(2000)] public string Description { get; set; } = "";
    [EnumDataType(typeof(CaseType))] public CaseType Type { get; set; }
    [Required, StringLength(200)] public string ApplicantName { get; set; } = "";
    [Required, EmailAddress] public string ApplicantEmail { get; set; } = "";
    [EnumDataType(typeof(CasePriority))] public CasePriority Priority { get; set; } = CasePriority.Normal;
    [StringLength(2000)] public string? Notes { get; set; }
}
public class ChangeStatusRequest
{
    [EnumDataType(typeof(CaseStatus))] public CaseStatus Status { get; set; }
    [StringLength(500)] public string? Reason { get; set; }
}
