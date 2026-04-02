# Bot assist and takeover playbook

## Cel
Boty mają pomagać, a nie psuć sesję. Gracz ma czuć wsparcie, nie utratę kontroli.

## Poziomy wsparcia
### Assist
- bot tylko sugeruje,
- podświetla rekomendowane akcje,
- nie wykonuje ich bez zgody.

### Soft Takeover
- bot obsługuje drobne, bezpieczne akcje,
- gracz może w każdej chwili przerwać.

### Hard Takeover
- brak gracza lub utrata połączenia,
- bot przejmuje slot,
- loguje wszystkie decyzje.

## Zasady dla botów
- nigdy nie robić zbędnego chaosu,
- preferować bezpieczne decyzje,
- ujawniać powód decyzji skrótowym logiem,
- oddać sterowanie człowiekowi bez tarcia.

## Log decyzji bota
Każda akcja bota powinna mieć:
- timestamp,
- role,
- reason code,
- target entity,
- confidence bucket.

## Quality gate
Jeśli agent rozwija bota, musi dostarczyć:
- reguły fallback,
- listę akcji dozwolonych,
- listę akcji zablokowanych,
- testy takeover i return-to-player.
