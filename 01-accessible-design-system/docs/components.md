# Component library — usage rules

Every component ships with: markup, Sass, JS behaviour, keyboard pattern,
assistive-technology expectations, and a do / don't.

| Component | Keyboard | Screen reader |
|---|---|---|
| Tabs | Arrow keys move, Tab enters panel | role=tablist/tab/tabpanel, aria-selected |
| Dialog | Escape closes, focus trapped, focus returns | Native <dialog>, aria-labelledby |
| Sortable table | Header buttons are focusable | aria-sort on the sorted column |
| Alert | — | role=alert, text + colour, never colour alone |
| Form | Tab order = visual order | label for, aria-describedby, role=alert on errors |

## Versioning
Tokens are versioned (tokens.json is the contract). A breaking token change
is a major version, a new component is minor, a bug fix is patch. Consumers
pin a version — they do not copy-paste CSS from this repo into production.
