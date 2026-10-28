SET XACT_ABORT ON;
BEGIN TRANSACTION;
IF OBJECT_ID('dbo.CaseStore','U') IS NULL
CREATE TABLE dbo.CaseStore (
    Id int NOT NULL CONSTRAINT PK_CaseStore PRIMARY KEY CONSTRAINT CK_CaseStore_Id CHECK(Id=1),
    Payload nvarchar(max) NOT NULL,
    UpdatedAt datetime2 NOT NULL CONSTRAINT DF_CaseStore_UpdatedAt DEFAULT SYSUTCDATETIME(),
    Version rowversion NOT NULL
);
COMMIT;
