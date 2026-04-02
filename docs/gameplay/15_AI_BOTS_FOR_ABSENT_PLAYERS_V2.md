# AI Bots for Absent Players — V2

## Design target
Bots exist to keep the match alive when:
- a role slot is empty,
- a player disconnects,
- a player reconnects late,
- a tutorial or solo session needs support.

## Bot priorities
1. Preserve continuity.
2. Avoid catastrophic inaction.
3. Prefer safe, understandable choices.
4. Leave room for human override.
5. Record actions in timeline.

## Handoff rules
- bot takes control after short grace period,
- bot marks actions with AI tag,
- reconnecting player sees a reclaim banner,
- human can reclaim instantly when safe,
- critical in-flight dispatch is not interrupted mid-command.

## Quality rules
- bots should use dictionaries and reference data, not hardcoded magic,
- bots should respect role-specific limits,
- bots should not spam actions,
- bots should produce legible logs for debugging.

## V6 target behaviors
- operator bot handles routine calls,
- dispatcher bot sends nearest matching unit,
- coordinator bot flags overload clusters,
- crisis bot recommends reserve actions.

## Future direction
- confidence scores,
- adaptive difficulty,
- player-style assist profiles,
- training mode ghosts.
