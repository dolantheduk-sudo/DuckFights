using System.Collections.Generic;
using Drakefighting.Domain;

namespace Drakefighting.Sim
{
    public struct FighterState
    {
        public DrakeLoadout Loadout;
        public float HpPct;        // 0..1
        public float StaminaPct;   // 0..1
        public Rng Rng;
        public List<StatusEffect> ActiveEffects;
        public List<StatusEffect> PendingEffects;

        public FighterState(DrakeLoadout d, Rng rng)
        {
            Loadout = d;
            HpPct = 1f;
            StaminaPct = 1f;
            Rng = rng;
            ActiveEffects = new();
            PendingEffects = new();
        }
    }

    public static class DamageModel
    {
        public static float AttackCadence(DrakeStats s)
        {
            // Higher SPD = more frequent attacks; baseline 1.0
            return 1f + (s.SPD - 50) / 100f; // SPD 50 => 1.0, 80 => 1.3
        }

        public static float StaminaDrainPerTick(DrakeStats s, Style style)
        {
            var baseDrain = 0.055f; // 5.5% per “active” tick baseline
            var styleMult = style switch
            {
                Style.Brawler => 1.25f,
                Style.Tactician => 0.9f,
                Style.Counter => 1.0f,
                Style.Wildcard => 1.0f,
                _ => 1.0f
            };
            return (float)(baseDrain * styleMult * (1.0 - (s.SMT - 50) / 300.0)); // smarter = slightly less drain
        }

        public static float LowStaminaDmgMult(float staPct) => staPct < 0.35f ? 0.65f : 1f;

        public static float HitChance(DrakeStats atk, DrakeStats def)
        {
            var baseHit = 0.78f;
            var smtDelta = (atk.SMT - def.SMT) / 300f; // -0.33..+0.33
            return Clamp01(baseHit + smtDelta);
        }

        public static float DodgeChance(DrakeStats def)
            => Clamp01(0.05f + def.SMT / 500f); // 5%..25%

        public static float CritChance(DrakeStats atk)
            => Clamp01(0.05f + atk.Luck / 400f); // 5%..30%

        public static float RollDamage(DrakeStats atk, float staPct, Rng rng)
        {
            var pow = atk.POW / 100f;                 // 0..1
            var baseDmg = 0.12f + pow * 0.38f;        // ~12%..50% per solid hit (tune)
            var variance = 0.9f + rng.NextFloat() * 0.2f;
            return baseDmg * LowStaminaDmgMult(staPct) * variance;
        }

        public static float Clamp01(float v) => v < 0 ? 0 : (v > 1 ? 1 : v);
    }
}
