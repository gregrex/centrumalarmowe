# Fail / Retry / Next Branching Rules

## Branches
- success -> reward -> next
- partial -> reward_small -> retry_or_next
- fail -> retry_hint -> retry_or_home

## Retry logic
- zachowaj briefing shortcut,
- pokaż 3 główne błędy,
- pokaż proponowaną zmianę decyzji,
- nie resetuj kosmetyki i tożsamości gracza.

## Fail states
- objective critical fail,
- city pressure collapse,
- response timeout,
- chain escalation unresolved.
