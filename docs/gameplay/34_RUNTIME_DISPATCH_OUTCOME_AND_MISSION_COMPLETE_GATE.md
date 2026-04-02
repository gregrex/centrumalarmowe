# RUNTIME DISPATCH OUTCOME AND MISSION COMPLETE GATE

## Dispatch outcome model
Każdy dispatch kończy się jednym z outcome:
- success
- delayed
- blocked
- rerouted
- failed_no_unit

Outcome wpływa na:
- objective progress,
- city stability,
- score,
- event feed,
- audio state.

## Mission complete gate
Warunki success:
- minimum 2 primary objectives completed,
- city stability >= 40,
- nie ma objective critical failed.

Warunki fail:
- city stability < 20,
- objective critical failed,
- czas skończony przy zbyt niskim score.
