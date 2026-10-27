-- SQL Server — run in order. Dummy data only.
CREATE TABLE dbo.Cases (
  Id INT IDENTITY(1,1) PRIMARY KEY,
  Reference NVARCHAR(50) NOT NULL,
  Status NVARCHAR(50) NOT NULL,
  EncryptedNotes NVARCHAR(MAX) NULL,
  CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);
GO
INSERT INTO dbo.Cases (Reference, Status) VALUES
  ('CA-1042', 'Approved'), ('CA-1043', 'In review');
GO
-- Read pattern used by the API: parameterised, least-privilege login only.
-- SELECT Id, Reference, Status, CreatedAt FROM dbo.Cases WHERE Status = @status;
