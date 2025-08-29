# Stats & Formulas (First Pass)

## Base Stats
- SPD (Speed): attack cadence
- STA (Stamina): endurance; low STA reduces DMG and cadence
- POW (Power): damage per hit
- SMT (Smarts): dodge/counter efficiency and stamina usage
- Mood: aggression profile (affects STA curve / crit risk)
- Luck: crit and event rolls

## Combat Tick Model
- Discrete ticks (e.g., 10–20 rounds). Each round:
  - Attack chance per duck = f(SPD, current STA)
  - Hit chance = base ± SMT differential
  - Dodge chance = base + SMT * k1
  - Damage = POW * rand(0.9–1.1) * StaminaMultiplier
  - Crit = Luck * k2 → Damage * CritMult
  - Stamina decay = BaseDrain * ActivityMult (style-dependent)
  - Low STA penalty: If STA < T, Damage *= 0.6; SPD cadence penalty +15%

## Styles (examples)
- Brawler: +15% early Damage, +25% Stamina drain
- Tactician: -10% Damage, +15% Dodge, -10% Stamina drain
- Counter: +20% Damage if opponent STA < 40%
- Wildcard: +10% Crit, +10% self-miss chance

*(Tune constants k1, k2, CritMult, thresholds T via playtests.)*
