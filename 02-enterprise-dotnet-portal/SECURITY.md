# Security

- Secrets: never committed. Production secrets live in Azure Key Vault.
  `appsettings.Development.json` contains dev-only keys and is labelled as such.
- Sensitive fields: AES-GCM (confidentiality + integrity), never ECB, never
  plain text, never in logs. Audit logs record who/what/when, never the value.
- Auth: JWT bearer, role-based (`Admin`, `CaseWorker`). Anonymous API calls
  get 401, unauthorised roles get 403 — never data.
- Input: DataAnnotations validation, EF Core parameterisation, anti-forgery
  on MVC forms, CSP / nosniff / frame-deny headers.
- Classified / sensitive data: this repo contains dummy data only. Real
  handling follows the owning agency's rules, which are not demonstrated
  here with real data and must never be carried in a portfolio repo.
- Reporting: if you find a real vulnerability in this demo code, do not
  open a public issue with exploit details — contact the repo owner first.
