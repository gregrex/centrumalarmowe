
# First 3 incident types end-to-end

## 1. Medical critical
### Przykład
Nieprzytomny kierowca autobusu.

### Flow
1. Operator odbiera zgłoszenie.
2. Operator waliduje adres i stan.
3. Dispatcher wysyła karetkę.
4. Coordinator podnosi priorytet skrzyżowania.
5. Crisis Officer może odciążyć ruchem inne sektory.
6. Incydent przechodzi do `Resolved` albo `Failed`.

## 2. Fire medium
### Przykład
Pożar kuchni w mieszkaniu.

### Flow
1. Operator potwierdza ogień i liczbę osób.
2. Dispatcher wysyła straż.
3. W razie potrzeby dodaje policję do ewakuacji strefy.
4. Jeśli opóźnienie rośnie, incydent eskaluje.

## 3. Violence / domestic disorder
### Przykład
Awantura domowa, możliwe obrażenia.

### Flow
1. Operator rozróżnia zagrożenie natychmiastowe od hałasu.
2. Dispatcher wysyła patrol.
3. Przy wzroście ryzyka dochodzi medyczne wsparcie.
4. Coordinator pilnuje, by lokalne przeciążenie nie zablokowało innych zgłoszeń.

## Dlaczego te 3 typy
- są różne operacyjnie,
- uczą współpracy ról,
- są czytelne w 2D mobile,
- dobrze pokazują wartość BOT fallback.
