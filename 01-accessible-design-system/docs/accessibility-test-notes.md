# Accessibility test notes — WCAG 2.1 AA

## What this library claims
- Semantic HTML5, visible focus, skip link, labelled forms, error
  announcement, table headers, colour + text status, contrast ≥ 4.5:1.
- Training records are kept outside this repo; this code is the
  implementation, not a training certificate.

## Automated test (axe)
1. Open `index.html` in Chrome/Edge.
2. Run axe DevTools (or `npx @axe-core/cli index.html` for a static check).
3. Expected result at scaffold time: 0 violations on this page.
   Re-run after every change and paste the dated result here — a stale
   "0 violations" claim from an old build is not evidence.

## Keyboard test (no mouse)
Tab through the page in order: skip link → nav → buttons → table → form.
Shift+Tab goes backwards. Enter/Space activate buttons. No keyboard trap.
Focus must always be visible (3px outline).

## Screen reader test (JAWS / NVDA script)
1. Start JAWS/NVDA, open the page, press H to move by headings — headings
   must read in logical order, with no skipped levels used for styling.
2. Tab to the form, leave name blank, submit — the error must be announced
   and focus must move to the name field.
3. Read the status badges — status must be understandable from text alone,
   not colour alone.
4. Read the table — headers must announce for each cell (scope="col").

## Remediation log
| Date | Issue found | Fix | Retest |
|---|---|---|---|
| (fill in) | e.g. low-contrast badge | Darkened text to #0e7a5f | axe + keyboard |
