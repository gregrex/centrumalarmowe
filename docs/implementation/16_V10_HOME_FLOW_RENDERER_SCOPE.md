# V10 scope — Home flow, renderer, audio routing

## Cel
Doprowadzić menu i home hub do poziomu spójnego produktu mobilnego, który:
- wygląda premium,
- jest łatwy do zbudowania,
- jest sterowany danymi,
- prowadzi gracza do realnego gameplayu bez chaosu.

## Zakres
1. Home hub:
   - continue session,
   - quick play,
   - campaign,
   - coop,
   - profile,
   - settings,
   - daily challenge,
   - reward inbox.
2. Renderer sceny menu:
   - background layers,
   - weather overlays,
   - hero object slot,
   - subtle motion,
   - parallax,
   - scene variant switching.
3. Audio per screen:
   - home,
   - campaign,
   - coop lobby,
   - profile,
   - settings,
   - quick play entry.
4. Dane i słowniki:
   - home cards,
   - challenge cards,
   - settings bundle,
   - audio routing,
   - renderer layers.
5. Starter source:
   - DTO,
   - service,
   - endpoints,
   - Unity controllers.

## MVP v10
- home hub działa na JSON,
- można pobrać home bundle z API,
- renderer menu umie wczytać warstwy i hero object slot,
- audio router przełącza stan między ekranami,
- settings bundle ma audio/accessibility/controls.
