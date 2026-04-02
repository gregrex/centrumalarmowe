# Online / Offline Entry and Role Reservation

## Stany rezerwacji roli
- free,
- preferred,
- reserved,
- locked_for_start,
- bot_reserved,
- reconnect_hold.

## Flows
### Offline
Brak serwera sesji. Reservation state jest lokalny.

### Online
1. host tworzy sesję,
2. role mogą być preferowane,
3. po ready-check role wchodzą w locked_for_start,
4. brakujące role przechodzą na bot_reserved.

### Rejoin
- rejoin ma pierwszeństwo przed botem,
- bot schodzi do assist mode,
- po pełnym sync człowiek odzyskuje rolę.
