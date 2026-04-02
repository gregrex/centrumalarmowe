# Route preview i shared actions

## Route preview
Ma odpowiedzieć na jedno pytanie: czy wysłanie tej jednostki tu i teraz ma sens.

### Pokazuje
- ETA,
- dystans,
- węzły trasy,
- typ drogi,
- ostrzeżenia: korek, blokada, objazd,
- spodziewany wpływ na presję,
- alternatywę jeśli dostępna.

## Shared actions
To akcje wymagające więcej niż jednej roli.

### Przykłady
- eskalacja do trybu kryzysowego,
- przekierowanie sił z innego incydentu,
- potwierdzenie priorytetu mass casualty.

### Flow
1. jedna rola inicjuje akcję,
2. system tworzy request,
3. wymagane role dostają alert,
4. rola lub bot wysyła ACK,
5. po uzyskaniu quorum akcja jest wykonywana albo wygasa.
