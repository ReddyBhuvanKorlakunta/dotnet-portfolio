# Architecture and code map
| Area | Responsibility |
| --- | --- |
| EnterprisePortal.Domain | Case state machine, priority and record rules |
| EnterprisePortal.Application | Shared orchestration, validation, DTOs and repository contracts |
| EnterprisePortal.Infrastructure | AES-256-GCM, durable demo snapshots, parameterized SQL adapter and transactions |
| EnterprisePortal.Web | Cookie authentication, role checks, CSRF, MVC, API, rate limits and HTTP headers |
| EnterprisePortal.Tests | Executable assertions without a test-framework dependency |
| frontend | Sass foundations, framework adapter source, Playwright and axe |
| legacy/WebForms | Independent Windows .NET Framework host |

Controllers and API share the same service and state machine. Only submitted cases enter review. Only cases in review can be approved or rejected; rejection requires a reason. Closed cases do not reopen. Persistence replays recorded transitions to reconstruct the aggregate. A process-level gate holds the demo storage snapshot for the scoped repository lifetime; SQL adds a serializable transaction and locking across processes. Do not scale the file mode to multiple processes. Repository reads hold the gate until the request ends.

The snapshot design makes an easily runnable demonstration with no infrastructure required. It does not claim enterprise-scale query performance. Reflection restores database identity and creation time; a production ORM or dedicated persistence constructor should replace this demo mapping. There is no implemented tenant ownership model. All signed-in demo staff can read all synthetic cases.

## Design decisions
Native form controls, clear labels, one primary action, explicit status text and a focusable error summary support predictable workflows. HTML5 supplies landmarks and semantic tables. Sass tokens unify visual behavior. Bootstrap/Material adapters show how framework appearance can map to approved foundations; framework imports are not themselves a governed design system. Component acceptance includes keyboard behavior, errors, contrast, reflow, content guidance and ownership. Designer collaboration and user research in this project are proposed practices, not claimed past research.

## Sources
- https://learn.microsoft.com/aspnet/core/security/
- https://learn.microsoft.com/aspnet/web-forms/overview/security/
- https://www.w3.org/TR/WCAG21/
- https://cheatsheetseries.owasp.org/cheatsheets/DotNet_Security_Cheat_Sheet.html
