# 112: Centrum Alarmowe — Agent Pack v26

To repo zawiera rozszerzony blueprint, prompty, content-driven dane oraz starter source pack dla mobilnej gry 2D `112: Centrum Alarmowe`.

## Co wnosi v26
- **pakiet pod pierwszy rzeczywisty Android build flow end-to-end**,
- **bugfix freeze checklist** i runbook stabilizacji,
- **pełniejsze demo operatora i dispatchera** na jednej misji showcase,
- **mocniejszy final polish pack** dla grafiki 2D, obiektów, scen, UI, audio i muzyki,
- nowe kontrakty DTO, serwis aplikacyjny, endpointy demo i kontrolery Unity,
- dalsze dane słownikowe, build docs i materiały dla Copilota i Claude.

## Najważniejsze pliki startowe
- `START_PROMPT_COPILOT.md`
- `START_PROMPT_CLAUDE.md`
- `docs/implementation/48_V26_REAL_ANDROID_BUILD_AND_BUGFIX_FREEZE_SCOPE.md`
- `docs/tests/21_V26_REAL_ANDROID_BUILD_ACCEPTANCE.md`
- `docs/build/20_REAL_ANDROID_BUILD_AND_BUGFIX_FREEZE.md`
- `tools/smoke-v26.sh`
- `tools/run-migrations.ps1`

## AdminWeb live dashboard
- AdminWeb odświeża dashboard z własnego endpointu `GET /api/admin/dashboard`.
- Endpoint pobiera publiczne `/health` oraz chronione `/api/sessions` i `/api/content/validate` po stronie serwera.
- Aby działał także przy `Security__RequireAuth=true`, AdminWeb musi mieć skonfigurowane:
  - `ApiAuth__Jwt__SigningKey`
  - `ApiAuth__Jwt__Issuer` (domyślnie `Alarm112.Api`)
  - `ApiAuth__Jwt__Audience` (domyślnie `Alarm112.Client`)

## Status
To nadal jest scaffold + dokumentacja + starter source pack. Część kodu to minimalne szkielety pod dalszą pracę agenta kodującego i dopięcia w prawdziwym środowisku buildowym.

## v26

Ta wersja dodaje realniejszy pakiet pod Android build, freeze błędów, showcase operator/dispatcher dla jednej misji oraz mocniejszy polish release/demo/playtest/liveops.
