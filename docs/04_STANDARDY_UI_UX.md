# 04 — Standardy UI/UX: 112 Centrum Alarmowe

> Design system projektu. Obowiązuje dla Web (AdminWeb) i Mobile (Unity 2D).

---

## Filozofia designu

1. **Silhouette-first** — od czerni do koloru. Tło zawsze dark, akcenty neonowe.
2. **Dark operations UI** — projekt inspirowany centrum dowodzenia / dyspozytorni.
3. **Mobile-first** — UI czytelne w pionie na 360px, touch targets ≥ 44px.
4. **Information hierarchy** — 3 poziomy ważności: primary / secondary / tertiary.
5. **Status-driven** — kolor zawsze niesie znaczenie (ok=zielony, warn=żółty, err=czerwony).

---

## Design Tokens (CSS Variables)

Zdefiniowane w `assets/ui/design-tokens.css`. Obowiązkowe w całym projekcie web.

### Kolory

```css
/* Tła */
--bg-base:      #080c12    /* Główne tło strony */
--bg-surface:   #0f1420    /* Karty, sekcje */
--bg-elevated:  #161c2a    /* Modalne, dropdowny */
--bg-sidebar:   #0b0f1a    /* Sidebar nawigacyjny */

/* Akcent główny */
--accent:       #ff4b1f    /* CTA, alerty, brand 112 */
--accent-dim:   #cc3c19
--accent-glow:  rgba(255,75,31,0.35)

/* Role (4 kolory ról) */
--role-op:      #38bdf8    /* CallOperator — niebieski */
--role-dis:     #fb923c    /* Dispatcher — bursztyn */
--role-coord:   #4ade80    /* OperationsCoordinator — zielony */
--role-crisis:  #f87171    /* CrisisOfficer — czerwony */

/* Statusy */
--ok:           #22c55e
--warn:         #f59e0b
--err:          #ef4444

/* Tekst */
--text:         #d8e0ec    /* Główny tekst */
--text-dim:     #6b7a90    /* Pomocniczy tekst */

/* Granice */
--border:       rgba(255,255,255,0.07)
```

### Typografia

```css
font-family: 'Segoe UI', system-ui, Arial, sans-serif;

/* Skala */
--font-xs:    11px
--font-sm:    12px
--font-base:  14px
--font-md:    16px
--font-lg:    18px
--font-xl:    22px
--font-2xl:   28px
--font-display: 36px
```

### Spacing

```css
/* 4px grid */
--space-1:  4px
--space-2:  8px
--space-3:  12px
--space-4:  16px
--space-5:  20px
--space-6:  24px
--space-8:  32px
--space-10: 40px
--space-12: 48px
```

### Touch Targets

Minimalne wymiary: **44×44px** (Apple HIG standard).  
Preferowane: **48×48px** (Material 3).

---

## Komponenty web (AdminWeb)

### Przyciski

```
Variant         Tło                    Tekst          Użycie
──────────────────────────────────────────────────────────
primary         var(--accent)          #fff           CTA główne
secondary       var(--bg-elevated)     var(--text)    Działania drugorzędne
danger          var(--err)             #fff           Destruktywne akcje
ghost           transparent            var(--text-dim) Minimalne akcje
```

**Stany:** default → hover (brightness 1.1) → active (brightness 0.9) → disabled (opacity 0.4).

### Karty

```
.card {
  background:    var(--bg-surface);
  border:        1px solid var(--border);
  border-radius: 8px;
  padding:       16px;
}

.card--elevated {
  background:    var(--bg-elevated);
  box-shadow:    0 4px 24px rgba(0,0,0,0.4);
}
```

### Statusy / Badge

```
.badge--ok     { background: var(--ok-bg);   color: var(--ok);   }
.badge--warn   { background: var(--warn-bg); color: var(--warn); }
.badge--err    { background: var(--err-bg);  color: var(--err);  }
```

### Formularze

- Label zawsze nad polem, nie jako placeholder
- Placeholder: tekst pomocniczy, nie label
- Error message: czerwony, pod polem, z ikoną ⚠
- Success state: zielona ramka + ikona ✓

### Tabele

```
thead  { background: var(--bg-elevated); sticky top }
tr:hover { background: rgba(255,255,255,0.03) }
```

---

## Komponenty mobile (Unity 2D)

### HUD Layout

```
┌─────────────────────┐
│  [ROLA] [TIMER] [❤] │ ← Top bar (56dp)
├─────────────────────┤
│                     │
│    MAPA / WIDOK     │ ← Content area (flex)
│                     │
├─────────────────────┤
│  [A] [B] [C] [D]   │ ← Action bar (72dp)
│  [INFO]  [DISPATCH] │ ← Bottom panel (96dp)
└─────────────────────┘
```

### Kolory per rola (mobile HUD)

| Rola | Kolor akcentu | Kolor tła paska |
|------|--------------|----------------|
| CallOperator | `#38bdf8` (niebieski) | `#0c1a28` |
| Dispatcher | `#fb923c` (bursztyn) | `#281a0c` |
| OperationsCoordinator | `#4ade80` (zielony) | `#0c2818` |
| CrisisOfficer | `#f87171` (czerwony) | `#280c0c` |

### Stany ekranów

Każdy ekran musi mieć zaimplementowane:
- **Loading state** — skeleton / spinner
- **Empty state** — ilustracja + komunikat + CTA
- **Error state** — ikona błędu + opis + przycisk retry
- **Success state** — animacja + potwierdzenie

---

## Dostępność (WCAG 2.2 AA)

| Wymóg | Wartość | Status |
|-------|---------|--------|
| Kontrast tekst/tło | ≥ 4.5:1 (normalne), ≥ 3:1 (duże) | ✅ design tokens zaprojektowane |
| Touch target size | ≥ 44×44dp | ✅ standardowy wymóg |
| Focus visible | Widoczny indicator | ⚠ do weryfikacji w AdminWeb |
| Alt text | Ikony z aria-label | ⚠ do dodania |
| Keyboard nav | Pełna nawigacja klawiaturą | ⚠ do weryfikacji |
| Error messages | Powiązane z polem (aria-describedby) | ❌ do dodania |

---

## Zasady niedozwolone (UI)

- ❌ Nie używać kolorów ról poza kontekstem roli
- ❌ Nie używać czerwonego jako koloru dekoracyjnego (tylko error/crisis)
- ❌ Nie mieszać font-size poniżej --font-sm bez uzasadnienia
- ❌ Nie używać opacity <0.4 na elementach interaktywnych
- ❌ Nie używać background: white w dark-mode UI
- ❌ Nie umieszczać akcji destruktywnych bez potwierdzenia (dialog)
