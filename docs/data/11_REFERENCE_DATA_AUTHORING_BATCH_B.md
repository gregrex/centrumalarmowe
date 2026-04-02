# Reference Data Authoring — Batch B

## Purpose
Expand the content-first architecture without making build complexity explode.

## Rule set
- keep ids stable,
- avoid duplicate labels,
- keep one source of truth per group,
- validate every new json file,
- separate gameplay ids from UI copy,
- use packs that can be loaded independently,
- always include a minimal demo pack.

## Batch B content
- city map nodes,
- city map connections,
- unit roster,
- incident actions,
- timeline sample items,
- report grade thresholds,
- audio priority maps,
- icon marker groups.

## Practical guidance for Copilot / Claude
When generating code:
- load content first,
- validate second,
- bind to UI third,
- never hardcode zone names in gameplay classes,
- never hardcode marker labels in scene scripts.
