using System.Security.Cryptography;
using EnterprisePortal.Application.Cases;
using EnterprisePortal.Domain.Entities;
using EnterprisePortal.Infrastructure.Data;
using EnterprisePortal.Infrastructure.Security;
var passed = 0;
void Check(bool value, string name) { if (!value) throw new Exception(name); Console.WriteLine("PASS " + name); passed++; }
var key = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
var crypto = new EncryptionService(key);
var cipher = crypto.Encrypt("restricted synthetic note");
Check(crypto.Decrypt(cipher) == "restricted synthetic note", "AES-GCM round trip");
Check(cipher != crypto.Encrypt("restricted synthetic note"), "Unique nonces");
var bytes = Convert.FromBase64String(cipher); bytes[^1] ^= 1;
try { crypto.Decrypt(Convert.ToBase64String(bytes)); throw new Exception("Tampering accepted"); } catch (CryptographicException) { Check(true, "Tampering rejected"); }
try { new EncryptionService(Convert.ToBase64String(new byte[10])); throw new Exception("Short key accepted"); } catch (ArgumentException) { Check(true, "Short encryption key rejected"); }
var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".enc");
var storage = new DemoFileStorage(path, crypto); var gate = new StoreGate();
int id;
await using (var repo = new CaseRepository(storage, gate))
{
    var service = new CaseService(repo, repo, crypto, new ReferenceGenerator());
    var created = await service.CreateAsync(new() { Title = "Synthetic housing support", ApplicantName = "Alex Example", ApplicantEmail = "alex@example.test", Priority = CasePriority.High, Notes = "restricted" }, "worker@example.test");
    Check(created.Ok && created.Value!.Status == CaseStatus.Submitted, "Create submits request"); id = created.Value!.Id;
    Check(created.Value.Priority == CasePriority.High, "Priority persisted");
    Check(!((await File.ReadAllTextAsync(path)).Contains("Alex Example")), "Demo file encrypted");
    var invalid = await service.ChangeStatusAsync(id, new() { Status = CaseStatus.Approved }, "worker@example.test");
    Check(!invalid.Ok, "Approval before review rejected");
}
await using (var repo = new CaseRepository(storage, gate))
{
    var service = new CaseService(repo, repo, crypto, new ReferenceGenerator());
    Check((await service.GetAsync(id)).Ok, "Durable record reload");
    Check((await service.ChangeStatusAsync(id, new() { Status = CaseStatus.InReview }, "reviewer")).Ok, "Review transition");
    Check(!(await service.ChangeStatusAsync(id, new() { Status = CaseStatus.Rejected, Reason = " " }, "reviewer")).Ok, "Rejection requires reason");
    Check((await service.ChangeStatusAsync(id, new() { Status = CaseStatus.Approved }, "reviewer")).Ok, "Approval after review");
    Check((await service.ChangeStatusAsync(id, new() { Status = CaseStatus.Closed }, "reviewer")).Ok, "Close approved case");
    Check(!(await service.ChangeStatusAsync(id, new() { Status = CaseStatus.InReview }, "reviewer")).Ok, "Closed case immutable");
}
await using (var repo = new CaseRepository(storage, gate))
{
    var service = new CaseService(repo, repo, crypto, new ReferenceGenerator());
    Check((await service.GetAsync(id)).Value!.Status == CaseStatus.Closed, "Transition history survives reload");
    var page = await service.ListAsync(new(null, null, "HOUSING", -1, 1000));
    Check(page.Total == 1 && page.Page == 1 && page.PageSize == 100, "Search and pagination bounds");
    try { await service.CreateAsync(new() { Title = "bad", ApplicantEmail = "bad" }, "worker"); throw new Exception("Invalid input accepted"); } catch (System.ComponentModel.DataAnnotations.ValidationException) { Check(true, "Application validates outside HTTP"); }
}
File.Delete(path);
Console.WriteLine($"{passed} checks passed.");

#if SQLSERVER
var sqlConnection = Environment.GetEnvironmentVariable("SQL_TEST_CONNECTION");
if (!string.IsNullOrWhiteSpace(sqlConnection))
{
    await using (var connection = new Microsoft.Data.SqlClient.SqlConnection(sqlConnection))
    {
        await connection.OpenAsync();
        await using var command = connection.CreateCommand();
        command.CommandText = "IF OBJECT_ID('dbo.CaseStore','U') IS NULL CREATE TABLE dbo.CaseStore(Id int PRIMARY KEY CHECK(Id=1), Payload nvarchar(max) NOT NULL, UpdatedAt datetime2 DEFAULT SYSUTCDATETIME(), Version rowversion); DELETE FROM dbo.CaseStore;";
        await command.ExecuteNonQueryAsync();
    }
    var sqlGate = new StoreGate();
    int sqlId;
    await using (var sqlStorage = new SqlServerStorage(sqlConnection, crypto))
    await using (var repo = new CaseRepository(sqlStorage, sqlGate))
    {
        var svc = new CaseService(repo, repo, crypto, new ReferenceGenerator());
        var result = await svc.CreateAsync(new() { Title = "SQL integration case", ApplicantName = "SQL Example", ApplicantEmail = "sql@example.test" }, "sql-test");
        sqlId = result.Value!.Id;
        Check(result.Ok, "SQL Server transactional create");
    }
    await using (var sqlStorage = new SqlServerStorage(sqlConnection, crypto))
    await using (var repo = new CaseRepository(sqlStorage, sqlGate))
    {
        var svc = new CaseService(repo, repo, crypto, new ReferenceGenerator());
        Check((await svc.GetAsync(sqlId)).Value!.Title == "SQL integration case", "SQL Server durable read");
        Check((await svc.ChangeStatusAsync(sqlId, new() { Status = CaseStatus.InReview }, "sql-test")).Ok, "SQL Server transactional update");
    }
}
#endif
