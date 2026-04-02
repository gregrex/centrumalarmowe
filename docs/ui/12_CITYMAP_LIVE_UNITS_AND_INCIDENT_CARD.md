# City Map, Live Units, Incident Card — Mobile 2D HUD

## Screen goal
Provide one glance understanding of:
- where incidents are,
- what units are available,
- what action is required now,
- what will happen if the player dispatches.

## Layout
### Top bar
- session state,
- pressure score,
- active incidents count,
- connected role count,
- bot assist badge.

### Main area
- 2D city map with district boundaries,
- markers for incidents,
- markers for active units,
- route lines only for the currently selected item,
- optional heat overlay for overload.

### Right or bottom panel
Depends on device width.

#### Default portrait mobile
Use a bottom sheet:
- incident summary,
- severity,
- timer,
- recommended unit types,
- main dispatch CTA,
- alternate CTAs,
- escalation warning.

#### Larger phones / tablets
Use split layout:
- map left,
- incident card right,
- unit list below right.

### Bottom action rail
- dispatch,
- assign backup,
- mark false alarm,
- request police support,
- request fire support,
- request medical support,
- open timeline,
- pause.

## Incident card fields
- incident id,
- title,
- category,
- severity,
- zone,
- caller confidence,
- elapsed time,
- required units,
- available nearby units,
- risk notes,
- recommended action,
- action cooldowns,
- latest timeline items.

## Live unit list fields
- unit callsign,
- service type,
- current state,
- current zone,
- ETA,
- fatigue or busy tag if used later,
- route blocked warning,
- bot-controlled note if applicable.

## Readability rules
- never show more than one fully expanded card in portrait,
- marker icons must remain readable at 32–40 px,
- dispatch CTA must be reachable with one thumb,
- route lines must not overload the screen,
- use category icons before labels,
- use concise labels and strong badges.

## States
- loading,
- no network,
- no incidents,
- no units available,
- incident escalated,
- dispatch sent,
- dispatch failed,
- bot took over,
- player rejoined and reclaimed role.

## Edge cases
- player taps a marker while a dispatch request is pending,
- selected incident closes before dispatch completes,
- unit becomes unavailable mid-flow,
- human player reconnects while bot is acting,
- multiple incidents compete for one unit.

## MVP V6
- tap marker -> open card,
- choose unit -> send dispatch,
- receive simple result,
- update timeline,
- show report timeline later.

## Production direction
- richer route previews,
- layered map filters,
- predicted overload,
- smarter recommendations,
- collaborative pings and shared annotations.
