# Civic Services case workspace
A runnable .NET 10 MVC and C# API portfolio application. Uses fictional community assistance cases. It is not an official government service or approved for real sensitive or classified data.

## Run
Install the current .NET 10 SDK and Node 22. From the repository root:
```bash
export Auth__Password="$(openssl rand -base64 24)"
export Encryption__Key="$(openssl rand -base64 32)"
export ASPNETCORE_ENVIRONMENT=Development
export DOTNET_USE_POLLING_FILE_WATCHER=1
dotnet run --project 02-enterprise-dotnet-portal/src/EnterprisePortal.Web -- --urls http://127.0.0.1:5080
```
Keep both values in your terminal session. Use the password in `Auth__Password` with `worker@example.test` or `auditor@example.test`. Open http://127.0.0.1:5080. Reuse the encryption key to reopen existing records; changing it makes those records unreadable. Do not commit keys. The process uses a 20-minute, non-sliding session.

## Verify
```bash
dotnet build 02-enterprise-dotnet-portal/src/EnterprisePortal.sln -p:UseSqlServer=true
dotnet run --project 02-enterprise-dotnet-portal/src/EnterprisePortal.Tests
cd 02-enterprise-dotnet-portal/frontend
npm ci
npm run build
npx playwright install chromium
export PORTAL_TEST_PASSWORD="$Auth__Password"
npm test
```
Run the server in a separate terminal with the same password and encryption key. Unit checks return a nonzero exit code on failure. Playwright checks workflows, API authentication, CSRF, auditor restrictions, focus, mobile overflow, and axe WCAG A/AA rules. Automated results do not establish full WCAG conformance.

## Demonstrate
Sign in as a worker, create a synthetic request, select priority, search, start review, reject without a reason to demonstrate validation, approve, and close. Sign in as an auditor to demonstrate read-only access. Open Design system to explain tokens, components, usage rules, and governance. Use the separate Windows Web Forms project for Page_Load, IsPostBack, view state, GridView and accessible postbacks.

## API
The same authenticated browser cookie applies to `/api/v1/cases`. GET supports status, type, search, page, pageSize and sort. POST creates cases; PATCH `/{id}/status` applies domain transitions. Writes require `X-CSRF-TOKEN` containing the request token from an authenticated rendered form, together with its antiforgery cookie. This is a same-origin browser API, not a public bearer-token service. No restricted note plaintext appears in responses.

## SQL Server demonstration
Build with `-p:UseSqlServer=true`, run `sql/001_schema.sql` on an isolated test database, set `Storage__Provider=SqlServer` and `ConnectionStrings__Default` with `Encrypt=True;TrustServerCertificate=False`, and run in Development. Use a trusted SQL Server certificate. Grant the application account only SELECT, INSERT and UPDATE on dbo.CaseStore. DDL runs under a separate deployment principal.
The low-volume demo stores one authenticated encrypted snapshot with an atomic serializable transaction. It deliberately serializes writes. It is not a scalable enterprise case store. A production design needs normalized case tables, row-level authorization, bounded database pagination, per-record concurrency and append-only audit storage. SQL runtime testing requires an available SQL Server instance; compilation alone does not validate connectivity or transaction behavior.

## Deployment boundary
Non-demo startup is deliberately blocked. Before an agency release, replace demo credentials with approved OIDC/Windows identity and MFA, scope access by case/agency, provide managed key rotation and recovery, durable protected Data Protection keys, a tamper-resistant audit sink, retention and disposal controls, operations monitoring, load testing, a reviewed threat model and the agency's actual classification policy. No compliance accreditation is claimed.
See ARCHITECTURE.md, SECURITY.md and docs/verification.md. Web Forms requires Windows/IIS and .NET Framework 4.8.
