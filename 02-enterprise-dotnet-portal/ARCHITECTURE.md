# Architecture — Enterprise Portal

```
┌─────────────┐   HTTP / JWT   ┌──────────────────────┐
│  MVC UI     │ ──────────────▶│  EnterprisePortal.Web│
│  (Razor)    │   API v1       │  Controllers (thin)  │
└─────────────┘                └──────────┬───────────┘
                                          │ CaseService
                               ┌──────────▼───────────┐
                               │  Application         │
                               │  DTOs, services,     │
                               │  pagination, Result  │
                               └──────────┬───────────┘
                                          │ interfaces
                     ┌───────────────────┼───────────────────┐
                     ▼                   ▼                   ▼
              Domain (rules)    Infrastructure (EF Core,    Security
              Case state        SQL Server, repos,         (AES-GCM,
              machine, notes    migrations, seed)          Data Protection)
```

## Decisions (ADRs, short form)
- **Clean Architecture, not a single project.** Controllers stay thin so
  business rules are testable without HTTP or SQL. The cost is more projects;
  the payoff is that a developer can test a rule in seconds.
- **State machine in the domain, not in the controller.** An illegal
  transition is an exception in `Case`, covered by `CaseDomainTests`, not a
  string comparison scattered across endpoints.
- **Soft delete + query filter.** Deleted cases disappear from every query
  by default; the audit trail is not a second system to remember.
- **RowVersion concurrency.** Two workers editing the same case get a
  concurrency error, not a silent overwrite.
- **JWT in dev, Entra ID in production.** The API trusts the bearer scheme,
  not the dev issuer, so swapping identity providers changes config, not code.
- **Strangler Fig for Web Forms.** The legacy page in `legacy/WebForms/`
  keeps working. New pages are MVC/API. A Web Forms page migrates only when
  it needs a change — see `docs/strangler.md`.

## Data flow (create a case)
UI/API → validation (DataAnnotations) → CaseService → domain Case.Submit()
→ encrypted notes → repository → UnitOfWork.SaveChanges → SQL Server.
Every step is logged with a request ID, never with the sensitive value.

## Non-functional
- Pagination capped at 100/page, search is parameterised (EF Core, never
  string-built SQL), security headers on every response, health check at
  `/health`, audit log: who / what / when / request ID.
- Production checklist in SECURITY.md: Key Vault secrets, Entra ID, TLS,
  backup/restore test, migration review before apply.
