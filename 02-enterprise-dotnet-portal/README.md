# Enterprise Portal — Clean Architecture .NET 8

Dummy public-services case-management system. No real personal, health or
classified data anywhere in this repo.

## Run it
```
# Fastest, no SQL Server needed for the UI shell: the tests run without a DB.
dotnet test src/EnterprisePortal.Tests

# Full stack with SQL Server (Docker):
docker compose up --build
# web: http://localhost:5000   db: localhost:1433 (sa / DevOnly123! — dev only)

# Or directly (needs .NET 8 SDK + SQL Server / LocalDB):
cd src/EnterprisePortal.Web
dotnet run -- seed          # applies migrations, seeds dummy cases
dotnet run                  # https://localhost:5001
```

## Login (dev only)
```
POST /api/v1/auth/login  {"email":"worker@example.test","password":"Worker123!"}
# -> { token, role: "CaseWorker" }  — use: Authorization: Bearer <token>
# admin@example.test / Admin123!  — production uses Entra ID, not these accounts.
```

## API
```
GET    /api/v1/cases?status=InReview&search=CA&page=1&pageSize=20&sort=CreatedAt_desc
GET    /api/v1/cases/{id}
POST   /api/v1/cases              (CaseWorker, Admin)
PATCH  /api/v1/cases/{id}/status (CaseWorker, Admin)  {"status":"Approved"} / {"status":"Rejected","reason":"..."}
GET    /health
```

## Layout
```
src/
  EnterprisePortal.Domain/        Case aggregate + state machine, no dependencies
  EnterprisePortal.Application/   CaseService, DTOs, pagination, Result, interfaces
  EnterprisePortal.Infrastructure/ EF Core, SQL Server, repos, migrations, AES-GCM
  EnterprisePortal.Web/           MVC UI + API v1, JWT auth, middleware, health
  EnterprisePortal.Tests/         xUnit: domain transitions, service rules, encryption
legacy/WebForms/                  Working legacy page + strangler migration plan
docs/  ARCHITECTURE.md is at the portal root; runbook + strangler notes here
```

## Tests
```
dotnet test src/EnterprisePortal.Tests
# Domain: illegal transitions, rejection reason, full happy path
# Service: create encrypts notes, illegal status change is rejected (not crashed)
# Security: AES-GCM roundtrip, tampered cipher fails
```
