# Premium 2D visual target

## Cel
Gra ma wyglądać premium, ale nadal być tania i szybka w produkcji. Oznacza to:
- czytelne 2D top-down,
- mocny kontrast priorytetów,
- mało przypadkowych detali,
- spójny język ikon i alertów,
- warstwowe tło miasta z prostą animacją.

## Zasady jakości
1. Każdy ekran musi dawać odpowiedź na pytanie: co jest najpilniejsze teraz.
2. Każda rola ma własny kolor akcentu, ale wspólny język alertów.
3. Tła nie mogą walczyć z HUD-em.
4. Najpierw silhouette, potem detal.
5. Najpierw czytelność w telefonie 6 cali, potem efekty premium.

## Styl docelowy
- dark operations UI,
- półrealistyczna mapa miasta,
- lekko stylizowane ikony służb,
- miękkie neonowe akcenty alarmowe,
- subtelne glow, scanline i radarowe pulsy,
- brak tandetnego cyberpunku i brak plastikowego casual mobile.

## Warstwy grafiki
### Warstwa 1 — gameplay critical
- mapa miasta,
- ikony incydentów,
- status jednostek,
- alerty,
- CTA operacyjne.

### Warstwa 2 — orientacja i klimat
- district tint,
- drogi główne,
- najważniejsze budynki,
- lekki ruch miasta,
- pogoda i pora dnia.

### Warstwa 3 — polish
- flare świateł,
- pulsujące ramki,
- animacje przyjazdu,
- drobne VFX audio-reactive,
- ambientowe cienie.

## Quality gates dla assetów 2D
Asset jest akceptowany tylko, jeśli:
- jest czytelny w rozmiarze 64x64 i 96x96,
- ma rozpoznawalny stan normal/warning/critical,
- nie zawiera tekstu wygenerowanego przez AI,
- ma spójne obrysy i cienie,
- działa na tle ciemnym i jasnym alarmowym overlay.
