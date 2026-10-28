# Security boundaries
Synthetic data only. This repository is a portfolio demonstration, not an approved classified system. No state-specific authorization, FIPS validation, CJIS, HIPAA or FedRAMP certification is claimed.

Implemented controls: authenticated MVC and API routes, role-restricted writes, CSRF checks, local return URL validation, PBKDF2 password verification, short-lived HttpOnly SameSite cookies, login rate limiting, encoded Razor output, CSP, frame blocking, content-type protection, bounded input and AES-GCM authenticated encryption with random nonces. Base64 keys must decode to 32 random bytes. No secrets are committed. Health liveness reveals no database details. Restricted notes are omitted from DTOs. Both file and SQL snapshots are encrypted. Transition history lives inside the encrypted snapshot and is not a tamper-resistant external audit log.

Threats remaining: shared demo identities, no MFA/SSO, no record ownership isolation, no key versioning or automated rotation, no append-only audit sink, no operational alerting, single-snapshot scalability, session loss when host keys change and unverified agency requirements. Demo loopback HTTP must not be exposed to a network. Production startup is blocked. An agency must supply its actual data-classification, hosting, key-management, access, retention and incident-response requirements before production implementation.

Report vulnerabilities privately through the repository owner's contact channel. Do not include personal information or secrets in public issues.
