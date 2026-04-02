# Fail / Retry / Next Flow

## Flow
1. Misja kończy się sukcesem / partial / fail.
2. System pokazuje gating screen.
3. Gracz widzi:
   - wynik,
   - główne błędy,
   - cele ukończone / nieukończone,
   - reward state,
   - rekomendację: retry / next.
4. Jeśli fail:
   - pokaż retry hints,
   - pokaż 3 główne powody porażki,
   - pokaż uproszczony wariant szybkiego restartu.
5. Jeśli partial:
   - pokaż zachętę do retry po lepszy wynik lub next po progres.
6. Jeśli success:
   - pokaż reward reveal i next mission.

## Ekrany
- fail summary,
- retry preparation,
- success summary,
- partial branch,
- next mission gate.

## UX rule
Nie zmuszaj gracza do długiego klikania po porażce.
