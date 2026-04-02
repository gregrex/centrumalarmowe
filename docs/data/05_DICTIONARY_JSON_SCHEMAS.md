# Dictionary JSON schemas

## Cel
Zamknąć chaos danych słownikowych. Każdy plik JSON, który zasila klienta i backend, ma mieć jawny schemat.

## Obszary obowiązkowo schematyzowane
- reference data,
- units,
- incidents,
- statuses,
- texts,
- icon catalog,
- atlas manifest,
- bot profiles,
- session templates.

## Minimalne zasady
- pole `id` jest obowiązkowe,
- pole `version` jest obowiązkowe dla top-level content bundles,
- każde pole enum-like ma opisane dopuszczalne wartości,
- tablice nie mogą zawierać duplikatów ID,
- klient i backend walidują ten sam model.

## Quality gate
Nowy bundle contentu nie przechodzi dalej, jeśli:
- nie ma schematu,
- nie przechodzi walidacji,
- nie ma testowego przykładu,
- nie ma ownera i changelogu.
