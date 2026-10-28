# UX / UI notes — what this page is trying to prove

## Layout
Mobile-first, single content order at every breakpoint. The hero collapses
from 2 columns to 1, cards from 3 to 1 — no content is hidden on mobile,
because hidden content is a usability and accessibility failure, not a
responsive strategy.

## Interaction patterns
- Mobile menu: a real `<button>` with `aria-expanded`, not a div with a click.
- Multi-step form: steps are an ordered list, current step uses
  `aria-current="step"`, back navigation is allowed, errors focus the field.
- Dashboard stats: values are text, updated with a status region, not a
  silent DOM swap a screen reader would miss.

## Designer collaboration
In a real handoff I would: confirm tokens (not pixel values) with the
designer, flag any colour pair below 4.5:1 before building, prototype the
form error state (not just the happy path), and agree the mobile breakpoint
from content — when the layout breaks — not from a device list.

## Design decisions
"Translate designs into production pages, and push back where a design
breaks accessibility or mobile usability — with a working alternative, not
just a no."
