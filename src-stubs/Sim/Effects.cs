using System.Collections.Generic;

namespace Drakefighting.Sim
{
    public enum EffectType { Buff, Debuff, Stun, DodgeUp, CritUp, BreadHeal, GladiatorBuff }
    public record StatusEffect(EffectType Type, int TicksRemaining, float Magnitude);

    public interface ITrait
    {
        string Id { get; }
        // Called before each tick to optionally modify attacker/defender state.
        void OnPreTick(ref FighterState self, ref FighterState enemy);
        // Optional: post-hit hooks, dodge hooks, etc.
    }

    public sealed class BreadHealTrait : ITrait
    {
        public string Id => "BreadHeal";
        public void OnPreTick(ref FighterState self, ref FighterState enemy)
        {
            if (self.Rng.Roll(0.05f) && self.StaminaPct < 0.6f)
                self.PendingEffects.Add(new StatusEffect(EffectType.BreadHeal, 1, 0.08f)); // +8% STA once
        }
    }

    // TODO: Add WebStunTrait, ShowoffCritTrait, etc.
}
