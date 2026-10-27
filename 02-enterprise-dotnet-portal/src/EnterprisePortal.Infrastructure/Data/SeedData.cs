using EnterprisePortal.Domain.Entities;
namespace EnterprisePortal.Infrastructure.Data;
/// <summary>Dummy seed only. Never seed real personal, health or classified data.
/// Run with: dotnet run -- seed  (see Program.cs) or via the migration seed.</summary>
public static class SeedData
{
    public static IEnumerable<Case> Cases() => new[]
    {
        new Case("CA-2026-1042", "Benefits review — housing support", "Dummy case for demo and tests.", CaseType.BenefitsReview, "Alex Morgan", "alex@example.test", "seed"),
        new Case("CA-2026-1043", "Document check — identity proof", "Dummy case for demo and tests.", CaseType.DocumentCheck, "Jordan Lee", "jordan@example.test", "seed"),
    };
}
