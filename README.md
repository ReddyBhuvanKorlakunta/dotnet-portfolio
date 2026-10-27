# Portfolio — Enterprise .NET Full-Stack

Live repo: https://github.com/ReddyBhuvanKorlakunta/dotnet-portfolio

This is a submission pack for a senior enterprise .NET / full-stack role.
It is built to be read the way a senior reviewer reads code: architecture
first, then a rule, then a test for that rule — not screenshots first.

## Honest verdict on the first version

The first scaffold in this repo would have been filtered for a senior
enterprise role. It showed breadth (MVC, Web Forms, Sass, WCAG notes) but
no depth: one project, no tests, no auth, no migrations, no deployment.
This rebuild replaces it. If a piece below is still thin, it is labelled
as thin — a reviewer trusts a labelled gap more than a hidden one.

## What is here, and what it proves

### 1. Enterprise Portal — `02-enterprise-dotnet-portal/`
Clean Architecture .NET 8, dummy public-services case management.

- **Domain:** `Case` aggregate with a guarded state machine
  (Draft → Submitted → InReview → Approved/Rejected → Closed). Illegal
  transitions throw, and are covered by unit tests.
- **Application:** `CaseService` — pagination (capped at 100), filtering,
  sorting, `Result<T>` errors instead of exceptions for expected failures.
  Controllers are thin; UI and API share the same service, so they cannot drift.
- **Infrastructure:** EF Core + SQL Server, repository + unit of work,
  soft-delete query filter, `RowVersion` concurrency, hand-reviewed initial
  migration, dummy seed only.
- **Web + API:** MVC UI plus versioned `GET/POST/PATCH /api/v1/cases`,
  JWT bearer + roles (`Admin`, `CaseWorker`), security headers, request-ID
  logging, `/health`. Dev JWT accounts only — production is Entra ID
  (see `ARCHITECTURE.md`).
- **Security:** AES-GCM for sensitive fields, Data Protection for tokens,
  secrets via Key Vault in production, dev keys labelled dev-only and
  refused at startup in Production. Dummy data only — never real personal,
  health or classified data.
- **Tests:** xUnit — domain transitions, service rules, encryption
  roundtrip + tamper failure. Run: `dotnet test src/EnterprisePortal.Tests`
- **Run:** `docker compose up --build` in `02-`, or `dotnet run -- seed`
  in `src/EnterprisePortal.Web`. Full runbook in `docs/runbook.md`.
- **Legacy:** working Web Forms page in `legacy/WebForms/` with a Strangler
  Fig migration plan in `docs/strangler.md` — keep it working, migrate
  page by page, never a big-bang rewrite.
- **CI/CD:** Azure DevOps pipeline — build, test with coverage, publish,
  deploy to staging. Static demos also deploy via GitHub Pages
  (`.github/workflows/pages.yml` — enable Pages → Source: GitHub Actions
  in repo settings, then the demos are live at
  `https://ReddyBhuvanKorlakunta.github.io/dotnet-portfolio/`).

### 2. Accessible Design System — `01-accessible-design-system/`
A buildable component library, not a theme.

- Tokens as contract: `tokens.json` + `scss/_tokens.scss`, one source of truth.
- Components that work: tabs (ARIA pattern, arrow keys), native `<dialog>`
  (focus trapped, Escape closes), sortable table (`aria-sort`), alerts
  (text + colour, never colour alone), labelled forms with announced errors.
- Checks: `npm run build` (Sass), `npm run check:a11y` (static lint —
  axe DevTools + the JAWS script in `docs/accessibility-test-notes.md`
  are still required, and the remediation log must be filled with a dated
  run before submission).
- Interview answer in writing: `docs/design-system-vs-bootstrap.md` —
  framework vs. component library vs. design system, and how to roll one out.

### 3. Case Portal UI — `03-responsive-frontend/`
Responsive HTML5 / Sass / JavaScript UI that fetches the real API
(`js/api.js`, `GET /api/v1/cases`) and falls back to `data/cases.json`
when the API is not running, so a reviewer sees the data flow either way.
Filter/search, dashboard, two-step form with draft recovery, mobile menu
with `aria-expanded`. No token in `localStorage` — see the note in `api.js`.

### 4. LeetCode — `04-leetcode-csharp/`
Folder + template for your own C# solutions. Add your profile URL there.
LeetCode proves coding speed; it does not replace 1–3 above, and this
repo does not pretend otherwise.

## Requirement map

| Requirement | Proof |
|---|---|
| .NET / C#, MVC + Web Forms | 02: Clean Architecture solution + legacy Web Forms page |
| HTML5, CSS, Sass, JS, responsive | 01 + 03: Sass builds, vanilla JS, mobile-first, 360px–1440px |
| UX / UI, design thinking | 03 `docs/ux-notes.md`, 01 component usage rules |
| Bootstrap + Material + design system | 01: grid + elevation inside a token-owned system |
| WCAG 2.1 AA, JAWS, axe | 01: components + test notes + lint. **Formal WCAG training certificate is still a separate item — add it before submitting; code alone does not replace it.** |
| APIs, middleware, SQL Server | 02: API v1, middleware, EF Core, migrations |
| Secure coding, encryption, sensitive data | 02: AES-GCM, roles, audit, Key Vault note, dummy data only |
| Azure DevOps, CI/CD, deployments | 02 `azure-pipelines.yml`, Docker, Pages workflow |
| Production troubleshooting | 02 `docs/runbook.md`, health + request IDs |
| Government / healthcare preference | Dummy public-services domain, US context, dollars |

## What to send a vendor

1. This repo link.
2. The live Pages links once enabled (above).
3. LeetCode profile link.
4. Formal WCAG credential, if held — flagged explicitly.
5. One line per project: what it does, what you built, which requirement it proves.

## Still labelled as gaps, not hidden

- .NET solution file: generate with `dotnet new sln` + `dotnet sln add`
  if your checkout lacks it — the projects and references are the source of truth.
- Live Pages URL: needs Pages enabled in repo settings (one click, above).
- WCAG certificate: separate credential, not in this repo unless you add it.
- LeetCode profile URL: add it in `04-leetcode-csharp/README.md`.
