# Enterprise Design System — component library

Buildable, not a mockup. This is the library a design-system team would own.

## Run
```
npm install
npm run build        # sass scss/main.scss -> css/main.css
npm run check:a11y   # static a11y lint (axe + JAWS still required, see docs/)
npm start            # serves this folder; open index.html, then components.html
```

## What to show a reviewer
1. `tokens.json` + `scss/_tokens.scss` — one source of truth, consumed by every component.
2. `components.html` — tabs, dialog, sortable table, alert, all keyboard-operable. Try it without a mouse.
3. `docs/design-system-vs-bootstrap.md` — the interview answer, in writing.
4. `docs/accessibility-test-notes.md` — axe steps + JAWS script + remediation log. Fill the log with a real dated run before submitting; an empty log is a red flag.

## What this is not
It is not a Bootstrap theme. Bootstrap's grid is a tool inside the system;
the tokens, components and accessibility rules are the system.
