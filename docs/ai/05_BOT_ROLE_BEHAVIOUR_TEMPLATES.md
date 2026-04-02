# Bot role behaviour templates

## Cel
Bot ma zastąpić brakującego gracza w sposób wiarygodny, przewidywalny i tani implementacyjnie.

## Profile botów
### conservative
- wolniej reaguje,
- rzadko ryzykuje,
- preferuje procedurę.

### balanced
- domyślny profil do większości sesji.

### aggressive
- szybciej dispatchuje,
- lepiej działa w chaosie,
- częściej popełnia kosztowne skróty.

### tutorial
- czytelne zachowanie,
- wspiera gracza,
- nie destabilizuje sesji.

## Zachowania per rola
### Operator bot
- czyta priorytet z prostych reguł,
- nie prowadzi pełnej analizy NLP w MVP,
- używa szablonów pytań i decyzji.

### Dispatcher bot
- dobiera najbliższą sensowną jednostkę,
- bierze pod uwagę zajętość i SLA,
- komunikuje brak zasobów.

### Coordinator bot
- pilnuje kolejki incydentów,
- ustawia pingi i eskalacje,
- zgłasza potrzebę wsparcia.

### Crisis bot
- aktywuje mutatory kryzysowe,
- zarządza globalnymi alertami,
- ogranicza chaos, gdy za dużo graczy jest offline.
