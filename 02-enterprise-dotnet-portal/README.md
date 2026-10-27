# Enterprise Portal — ASP.NET Core MVC + Web Forms (legacy) + API

Dummy public-services case-management domain. No real personal, classified
or health data anywhere in this repo — seed data is invented.

## Run it (needs .NET 8 SDK — not installed on the scaffold VM)
```
cd src/EnterprisePortal
dotnet run
# then open https://localhost:5001 — MVC home page
# API: GET /api/cases  (requires the demo role header — see CasesController)
```

## What each part proves
- **MVC:** `Controllers/HomeController.cs` + `Views/` — Razor, layout, validation.
- **Web Forms where it remains:** `legacy/WebForms/CaseDetail.aspx` + `.aspx.cs`
  — the same case read path, kept working, with a migration note. Interview
  point: Web Forms is page lifecycle + ViewState + server controls; MVC is
  routing + controllers + separation of concerns. You do not rewrite a working
  Web Forms page to MVC unless there is a reason — you wrap it, then migrate
  page by page.
- **API + middleware:** `Api/CasesController.cs`, `Middleware/` — security
  headers, audit logging (who, what, when — never the sensitive value itself).
- **SQL Server:** `sql/001_schema.sql`, `Data/AppDbContext.cs` — EF Core,
  parameterised queries only, no string-built SQL.
- **Encryption:** `Services/EncryptionService.cs` — AES-GCM for fields at rest
  in code, ASP.NET Data Protection for tokens/cookies. Keys come from
  configuration / Azure Key Vault in production — never committed. The demo
  key in code is a dev-only placeholder and is labelled as such.
- **CI/CD:** `azure-pipelines.yml` — restore, build, test, publish, deploy
  stages. Branch protection + PR review is the source-control practice.

## Production troubleshooting runbook
1. Health: `GET /health` — if it fails, check app logs before restarting.
2. Logs: correlate by request ID (audit middleware adds it to every log line).
3. Bad deploy: roll back to the previous Azure DevOps release, do not hot-patch
   production. 4. Database: check the migration applied, then the connection
   string / firewall — never edit production data by hand without a ticket.

## Security notes (state requirements, in general terms)
Role-based access on every controller, least privilege on the SQL login,
sensitive fields encrypted, audit trail retained, dummy data in every demo
and screenshot. Handling of real classified data follows the agency's own
handling rules — this repo does not demonstrate that with real data, and
must never be used to carry it.
