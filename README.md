# Enterprise Case Management System

A .NET 8 case management system with an accessible design system and a
responsive portal UI. Sample data only — no real personal, health or
classified data anywhere in this repo.

## Systems in this repo

### 1. Enterprise Portal — `02-enterprise-dotnet-portal/`
Clean Architecture .NET 8.

- **Domain:** `Case` aggregate with a guarded state machine
  (Draft → Submitted → InReview → Approved/Rejected → Closed). Illegal
  transitions throw, and are covered by unit tests.
- **Application:** `CaseService` — pagination (capped at 100), filtering,
  sorting, `Result<T>` errors instead of exceptions for expected failures.
  Controllers are thin; UI and API share the same service.
- **Infrastructure:** EF Core + SQL Server, repository + unit of work,
  soft-delete query filter, `RowVersion` concurrency, hand-reviewed initial
  migration, sample seed data only.
- **Web + API:** MVC UI plus versioned `GET/POST/PATCH /api/v1/cases`,
  JWT bearer + roles (`Admin`, `CaseWorker`), security headers, request-ID
  logging, `/health`. Dev JWT accounts only — production uses Entra ID
  (see `02-enterprise-dotnet-portal/ARCHITECTURE.md`).
- **Security:** AES-GCM for sensitive fields, Data Protection for tokens,
  secrets via Key Vault in production, dev keys labelled dev-only and
  refused at startup in Production.
- **Tests:** xUnit — domain transitions, service rules, encryption
  roundtrip + tamper failure. Run: `dotnet test src/EnterprisePortal.Tests`
- **Run:** `docker compose up --build` in `02-enterprise-dotnet-portal/`,
  or `dotnet run -- seed` in `src/EnterprisePortal.Web`.
- **Legacy:** working Web Forms page in `legacy/WebForms/` with a Strangler
  Fig migration plan in `docs/strangler.md` — keep it working, migrate
  page by page, never a big-bang rewrite.
- **CI/CD:** Azure DevOps pipeline — build, test with coverage, publish,
  deploy to staging.

### 2. Design System — `01-accessible-design-system/`
A buildable component library.

- Tokens as contract: `tokens.json` + `scss/_tokens.scss`, one source of truth.
- Components: tabs (ARIA pattern, arrow keys), native `<dialog>`
  (focus trapped, Escape closes), sortable table (`aria-sort`), alerts
  (text + colour, never colour alone), labelled forms with announced errors.
- Checks: `npm run build` (Sass), `npm run check:a11y` (static lint —
  axe DevTools + the JAWS script in `docs/accessibility-test-notes.md`
  are still required, and the remediation log must be filled with a dated run).
- Background: `docs/design-system-vs-bootstrap.md` — framework vs.
  component library vs. design system, and how to roll one out.

### 3. Portal UI — `03-responsive-frontend/`
Responsive HTML5 / Sass / JavaScript UI that fetches the API
(`js/api.js`, `GET /api/v1/cases`) and falls back to `data/cases.json`
when the API is not running, so the data flow is visible either way.
Filter/search, dashboard, two-step form with draft recovery, mobile menu
with `aria-expanded`. No token in `localStorage` — see the note in `js/api.js`.

## Components

| System | Code |
|---|---|
| .NET, MVC + Web Forms | 02: Clean Architecture solution + legacy Web Forms page |
| HTML5, CSS, Sass, JS, responsive UI | 01 + 03: Sass builds, vanilla JS, mobile-first, 360px–1440px |
| UX / UI, design thinking | 03 `docs/ux-notes.md`, 01 component usage rules |
| Bootstrap + Material + design system | 01: grid + elevation inside a token-owned system |
| WCAG 2.1 AA, JAWS, axe | 01: components + test notes + lint |
| APIs, middleware, SQL Server | 02: API v1, middleware, EF Core, migrations |
| Secure coding, encryption, sensitive data | 02: AES-GCM, roles, audit, Key Vault note, sample data only |
| Azure DevOps, CI/CD, deployments | 02 `azure-pipelines.yml`, Docker |
| Production troubleshooting | 02 `docs/runbook.md`, health + request IDs |

## Deployment

Static UI (01 and 03) via GitHub Pages: repo Settings → Pages →
Deploy from a branch → main / root. The landing page at
`https://ReddyBhuvanKorlakunta.github.io/dotnet-portfolio/` links to both.
The .NET API is not hosted on Pages (Pages is static only) — run it with
Docker or `dotnet run` as above.
