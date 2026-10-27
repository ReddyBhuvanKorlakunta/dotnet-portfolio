# Portfolio — Enterprise .NET + Accessible Front-End

For vendor submission: code samples / portfolio required upfront, including
front-end and UI work. This repo is that submission pack — not a claim that
the work is learned on the job. Every demo below is runnable, and every
requirement in the role is mapped to a file you can open.

**Live demos:** open the `index.html` files in `01-` and `03-` directly, or
serve this folder (`python3 -m http.server`). The .NET portal in `02-` needs
the .NET 8 SDK on Windows / macOS / Linux (`dotnet run` from
`02-enterprise-dotnet-portal/src/EnterprisePortal`).

**LeetCode:** add your profile URL in `04-leetcode-csharp/README.md` and drop
your own C# solutions in there. Do not paste LeetCode solutions you did not
write.

## Requirement map — read this before submitting

| Role requirement | Where it is proved |
|---|---|
| .NET / C#, MVC + Web Forms | `02-` — MVC app + `legacy/WebForms/CaseDetail.aspx` kept working in the same environment |
| HTML5, CSS, Sass, JavaScript, responsive pages | `01-` and `03-` — Sass tokens in `scss/`, compiled CSS in `css/`, vanilla JS in `js/` |
| UX / UI best practice, design thinking, designer collaboration | `03-/docs/ux-notes.md`, `01-/docs/design-system-vs-bootstrap.md` |
| Bootstrap + Material + enterprise design system | `01-` — Bootstrap-style grid + Material-style elevation, rebuilt as design-system tokens, not as a raw framework theme |
| WCAG 2.1 AA, JAWS, axe | `01-/docs/accessibility-test-notes.md` — checklist, axe steps, keyboard/JAWS test script. **Formal WCAG training certificate is still a separate submission item — add the certificate file / credential link before you submit if you have one; this code alone does not replace it.** |
| Backend services, APIs, middleware, SQL Server | `02-` — `Api/CasesController.cs`, `Middleware/`, `Data/AppDbContext.cs`, `sql/` |
| Secure coding, encryption APIs, classified / sensitive data | `02-/Services/EncryptionService.cs` — AES-GCM + ASP.NET Data Protection, dummy data only, no real classified data, role-based access, audit logging |
| Azure DevOps source control, deployments, CI/CD | `02-/azure-pipelines.yml` — build, test, publish stages |
| Production troubleshooting | `02-/README.md` — runbook section: logs, health endpoint, rollback |
| Government / healthcare preference | All demos use a dummy public-services / case-management domain, US context, dollars, no real personal data |

## What to send the vendor

1. This repo link.
2. The two live demo links (deploy `01-` and `03-` to GitHub Pages / Netlify — both are static).
3. Your LeetCode profile link + `04-leetcode-csharp` (C# only).
4. Your formal WCAG training credential, if you hold one — flag it explicitly, because prior pools were rejected on exactly this point.
5. A one-line note per demo: what it does, what you built, and which requirement it proves. Do not send a zip of screenshots instead of code.

## Repos on GitHub

Live at https://github.com/ReddyBhuvanKorlakunta/dotnet-portfolio (public,
pushed 2026-10-27). A public repo is what a vendor can actually open — do
not submit a private repo link without inviting the reviewer.
