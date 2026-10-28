#if SQLSERVER
using System.Data;
using System.Text.Json;
using Microsoft.Data.SqlClient;
namespace EnterprisePortal.Infrastructure.Data;

public sealed class SqlServerStorage(string connectionString, EnterprisePortal.Application.Cases.IEncryptionService crypto) : ICaseStorage, IAsyncDisposable
{
    private SqlConnection? connection;
    private SqlTransaction? transaction;
    public async Task<List<StoredCase>> ReadAsync(CancellationToken ct)
    {
        connection = new SqlConnection(connectionString);
        await connection.OpenAsync(ct);
        transaction = (SqlTransaction)await connection.BeginTransactionAsync(IsolationLevel.Serializable, ct);
        await using var command = new SqlCommand("SELECT Payload FROM dbo.CaseStore WITH (UPDLOCK,HOLDLOCK) WHERE Id=1", connection, transaction);
        var value = await command.ExecuteScalarAsync(ct);
        return value is string json ? JsonSerializer.Deserialize<List<StoredCase>>(crypto.Decrypt(json)) ?? [] : [];
    }
    public async Task WriteAsync(List<StoredCase> cases, CancellationToken ct)
    {
        await using var command = new SqlCommand("UPDATE dbo.CaseStore SET Payload=@payload, UpdatedAt=SYSUTCDATETIME() WHERE Id=1; IF @@ROWCOUNT=0 INSERT dbo.CaseStore(Id,Payload) VALUES(1,@payload);", connection, transaction);
        command.Parameters.Add("@payload", SqlDbType.NVarChar, -1).Value = crypto.Encrypt(JsonSerializer.Serialize(cases));
        await command.ExecuteNonQueryAsync(ct);
        await transaction!.CommitAsync(ct);
    }
    public async ValueTask DisposeAsync()
    {
        if (transaction is not null) await transaction.DisposeAsync();
        if (connection is not null) await connection.DisposeAsync();
    }
}
#endif
