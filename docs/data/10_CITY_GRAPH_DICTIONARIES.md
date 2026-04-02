# City Graph and Dictionary Batch

## Core dictionary groups
- city.node.type
- city.connection.type
- city.zone.type
- dispatch.unit.type
- dispatch.unit.state
- incident.action.type
- incident.result.code
- timeline.event.type
- marker.icon.type
- route.warning.type

## Example node types
- station.police
- station.fire
- station.medical
- road.hub
- district.center
- hospital
- event.venue
- industrial.site
- residential.cluster

## Example unit states
- available
- enroute
- onscene
- returning
- unavailable
- blocked
- ai-controlled

## Example incident action ids
- action.dispatch.ambulance
- action.dispatch.police
- action.dispatch.fire
- action.request.backup
- action.monitor
- action.escalate
- action.flag.falsealarm

## Authoring rule
Every id must be stable, lowercase and dot-separated.
No display strings inside logic files.
Use localization keys for UI text.
