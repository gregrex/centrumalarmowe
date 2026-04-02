# Wymagania gameplayowe dla AI botów

## Cel
Bot ma przejąć brakującą rolę tak, aby sesja była grywalna i wiarygodna.

## Minimalne zachowania
- odbiór i klasyfikacja prostych zgłoszeń,
- wysyłanie najbardziej oczywistych jednostek,
- reagowanie na krytyczne alerty,
- pingowanie gracza przy niepewności,
- oddawanie kontroli po powrocie człowieka.

## Poziomy botów
- Basic: bezpieczne, proste decyzje, mało optymalizacji.
- Standard: sensowna praca w większości przypadków.
- Advanced: lepsza predykcja, reagowanie na łańcuchy zdarzeń.

## Ograniczenia
Bot nie może:
- spamować decyzjami,
- zmieniać priorytetów bez uzasadnienia,
- psuć strategicznych decyzji gracza,
- ukrywać swojej niepewności.

## Interfejs wobec gracza
Bot musi pokazywać:
- co zrobił,
- dlaczego,
- gdzie jest niepewny,
- kiedy prosi o decyzję człowieka.
