# Design system vs. component library vs. framework

**Framework — Bootstrap:** a grid, utilities and pre-styled components.
Fast to start, but without overrides every site looks like Bootstrap, and
upgrades can break custom overrides.

**Component library — Material:** ready components with behaviour (menus,
dialogs, elevation). Strong consistency, but the visual language is the
library author's, and accessibility behaviour is only as good as your implementation
of it.

**Design system:** tokens (colour, spacing, type, elevation) + components
built from those tokens + usage guidance + accessibility standards, owned
and versioned by the enterprise. It may *use* Bootstrap's grid and Material's
elevation under the hood — this demo does — but the product identity,
contrast rules, focus rules and component APIs are the enterprise's.

**How I would roll one out:** audit existing Bootstrap/Material usage,
extract tokens first, build the 10 most-used components to AA, document
do/don't usage with real examples, then migrate page by page — never a
big-bang rewrite, and never two visual languages live on the same page.
