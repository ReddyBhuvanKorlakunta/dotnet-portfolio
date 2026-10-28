# Case Portal UI — responsive front-end

## Run
```
python3 -m http.server 8002
# open http://localhost:8002 — cases load from data/cases.json by default
# with the API running (see 02-), set: window.PORTAL_API_BASE = 'http://localhost:5000'
# in the browser console before load, or serve behind the same origin in production.
```

## How to verify it
- Filter + search actually filter (js/main.js, js/api.js) — not hard-coded cards.
- Mobile menu, step form, and dashboard all work at 360px and 1440px.
- Errors are announced, focus moves, status is text + colour.
- No token in localStorage — see the note in js/api.js.
