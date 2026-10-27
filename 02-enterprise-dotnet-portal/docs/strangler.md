# Web Forms → MVC migration (Strangler Fig)

1. Keep the Web Forms page working exactly as-is (`legacy/WebForms/`).
2. Build the new path in MVC/API behind the same data (CaseService / SQL).
3. Route new traffic to the new path; keep the old path for its existing users.
4. Migrate a page only when it needs a functional change — never a big-bang rewrite.
5. Delete the Web Forms page only after its traffic is zero and its tests pass on the new path.

Interview point: Web Forms = page lifecycle + ViewState + server controls.
MVC = routing + controllers + separation of concerns. Knowing both, and
knowing when *not* to rewrite, is the enterprise skill — not just knowing MVC.
