# Verification record
Executed locally on October 28, 2026 using .NET SDK 10.0.403 and headless Chrome on macOS.

| Check | Result |
| --- | --- |
| .NET 10 build including conditional SQL Server adapter | Passed, zero warnings/errors |
| Domain, storage, validation and encryption assertions | 17 passed |
| NuGet transitive audit including SQL driver | No known vulnerable packages reported |
| npm audit | Zero vulnerabilities reported |
| Sass compilation | Passed |
| Playwright end-to-end suite | Passed |
| axe WCAG 2.1 A/AA rules on tested screens | Zero reported violations |
| Keyboard skip link and validation-summary focus | Passed automated browser assertions |
| 375px mobile overflow | Passed on workspace |
| Unauthenticated API | 401 |
| Missing CSRF token | 400 |
| Auditor create route | 403 |
| SQL Server live database | Pending Windows CI/local SQL instance |
| Web Forms build/runtime | Windows CI build configured; IIS runtime pending |
| JAWS | Not performed; Windows required |
| Full manual WCAG 2.1 AA assessment | Pending |
| Formal WCAG training | Not established by this project |

Automated accessibility scans cover sign-in, workspace, create, details, error state, design system and mobile workspace. They do not prove all success criteria. Repeat tests for changed screens and assistive-technology combinations.

## Manual acceptance protocol
Use Windows Chrome with JAWS. Record OS, browser, JAWS version, tester, date, route and expected/actual behavior. Navigate landmarks and headings; operate all forms using keyboard; verify input names, validation, error focus, table headers, status announcements, logical reading order and session expiration. Verify at 200% text zoom and 400% browser zoom, 320 CSS-pixel reflow, high contrast, reduced motion and text spacing overrides. Review all WCAG 2.1 A/AA criteria including those axe cannot automate. Test legacy postbacks independently in IIS. Log issues and retest each fix. Do not mark pending items complete without execution evidence.

## Training evidence
Complete formal accessibility training acceptable to the employer. Record provider, course, dates, certificate and practical exercises. This portfolio cannot create a training credential or substitute for real hands-on JAWS proficiency.
