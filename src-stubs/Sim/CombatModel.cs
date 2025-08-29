using Drakefighting.Domain;

namespace Drakefighting.Sim
{
    public struct FighterState
    {
        public DrakeLoadout Loadout;
        public float HpPct;        // 0..1
        public float StaminaPct;   // 0..1
        public Rng Rng;

        public FighterState(DrakeLoadout d, Rng rng)
        {
            Loadout = d;
            HpPct = 1f;
            StaminaPct = 1f;
            Rng = rng;
        }
    }

    public static class CombatModel
    {
        public static float AttackCadence(DrakeStats s)
            => 1f + (s.SPD - 50) / 100f; // SPD 50 => 1.0, 80 => 1.3

        public static float HitChance(DrakeStats atk, DrakeStats def)
        {
            var baseHit = 0.78f;
            var smtDelta = (atk.SMT - def.SMT) / 300f; // -0.33..+0.33
            return Clamp01(baseHit + smtDelta);
        }

        public static float DodgeChance(DrakeStats def) => Clamp01(0.05f + def.SMT / 500f); // 5–25%

        public static float CritChance(DrakeStats atk) => Clamp01(0.05f + atk.Luck / 400f); // 5–30%

        public static float RollDamage(DrakeStats atk, float staPct, Rng rng)
        {
            var pow = atk.POW / 100f;                 // 0..1
            var baseDmg = 0.12f + pow * 0.38f;        // ~12–50% per solid hit (tune)
            var variance = 0.9f + rng.NextFloat() * 0.2f;
            return baseDmg * StaminaModel.LowStaminaDamageMult(staPct) * variance;
        }

        public static float Clamp01(float v) => v < 0 ? 0 : (v > 1 ? 1 : v);
    }
}
