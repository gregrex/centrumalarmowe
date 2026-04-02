# Android Preview Session Resilience

## Cel
Zapewnić, że preview build jednej misji zachowuje się stabilnie w trybach:
- offline solo,
- online coop,
- bot fill fallback,
- reconnect review path.

## Wymagania
- czasowe odtworzenie stanu rundy,
- bezpieczny fallback na boty,
- retry bez utraty podstawowych statystyk,
- capture mode bez wpływu na sesję.
