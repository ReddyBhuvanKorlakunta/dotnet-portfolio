using EnterprisePortal.Domain.Entities;
namespace EnterprisePortal.Tests;
public class CaseDomainTests
{
    private static Case NewCase() => new("CA-2026-1", "Test", "Desc", CaseType.General, "Alex", "alex@example.test", "test");
    [Fact] public void Draft_Can_Submit() { var c = NewCase(); c.Submit("test"); Assert.Equal(CaseStatus.Submitted, c.Status); }
    [Fact] public void Draft_Cannot_Approve_Directly()
    {
        var c = NewCase();
        Assert.Throws<InvalidOperationException>(() => c.Approve("test"));
    }
    [Fact] public void Reject_Requires_Reason()
    {
        var c = NewCase(); c.Submit("t"); c.StartReview("t");
        Assert.Throws<ArgumentException>(() => c.Reject("t", ""));
    }
    [Fact] public void Full_Happy_Path()
    {
        var c = NewCase(); c.Submit("t"); c.StartReview("t"); c.Approve("t"); c.Close("t");
        Assert.Equal(CaseStatus.Closed, c.Status);
    }
}
