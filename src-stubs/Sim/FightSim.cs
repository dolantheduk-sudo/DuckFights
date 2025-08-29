using System.Collections.Generic;
using Drakefighting.Domain;

namespace Drakefighting.Sim
{
    /// <summary>
    /// Orchestrates a fight simulation and emits a deterministic replay log
    /// that your renderer can play back later. This version implements 1v1.
    /// Other modes can be wired in via a switch on MatchMode.
    /// </summary>
    public class FightSim
    {
        public SimResult Run(MatchSpec spec)
        {
            switch (spec.Mode)
            {
                case MatchMode.OneVOne:
                    return RunOneVOne(spec);

                // You can route additional modes here later:
                // case MatchMode.TagTeam2v2: return ModeCoordinator.RunTagTeam(spec);
                // case MatchMode.ThreeVThree: return ModeCoordinator.RunThreeVThreeSeq(spec);
                // case MatchMode.FreeForAll: return ModeCoordinator.RunFreeForAll(spec);
                // case MatchMode.Gladiator: return ModeCoordinator.RunGladiator(spec);
                // case MatchMode.Dodgeball5v5: return ModeCoordinator.RunDodgeball(spec);
                default:
                    // Fallback to 1v1 for now to keep things runnable during stub phase
                    return RunOneVOne(spec);
            }
        }

        // -----------------------------
        // 1v1 core implementation
        // -----------------------------
        private SimResult RunOneVOne(MatchSpec spec)
        {
            // Determinism: derive two RNGs from the same base seed
            var rngA = new Rng(spec.Seed);
            var rngB = new Rng(spec.Seed ^ 0x5F3759DF); // simple divergent seed

            var a = new FighterState(spec.TeamA.Ducks[0], rngA);
            var b = new FighterState(spec.TeamB.Ducks[0], rngB);

            var events = new List<SimEvent>(256);
            var maxRounds = TickTime.RoundsForMode(spec.Mode);

            for (int round = 1; round <= maxRounds; round++)
            {
                // Log a tick event so the renderer can pace animations
                events.Add(new SimEvent(
                    round, 0,
                    a.Loadout.Id.Value, b.Loadout.Id.Value,
                    EventType.Tick,
                    a.HpPct, a.StaminaPct, b.HpPct, b.StaminaPct));

                // A attacks B (if alive)
                if (a.HpPct > 0f && b.HpPct > 0f)
                    ResolveAttack(round, 0, ref a, ref b, events);

                // B attacks A (if alive)
                if (b.HpPct > 0f && a.HpPct > 0f)
                    ResolveAttack(round, 0, ref b, ref a, events);

                // Stamina drain (end of round)
                a.StaminaPct = System.MathF.Max(0f,
                    a.StaminaPct - StaminaModel.DrainPerTick(a.Loadout.Stats.SMT, a.Loadout.Style));
                b.StaminaPct = System.MathF.Max(0f,
                    b.StaminaPct - StaminaModel.DrainPerTick(b.Loadout.Stats.SMT, b.Loadout.Style));

                // Early exit on KO
                if (a.HpPct <= 0f || b.HpPct <= 0f)
                    break;
            }

            // Decide winner (KO first; otherwise by remaining HP)
            string winnerTeamId;
            if (a.HpPct <= 0f && b.HpPct <= 0f)         winnerTeamId = "DRAW"; // edge case; handle as you like
            else if (a.HpPct <= 0f)                     winnerTeamId = spec.TeamB.Id.Value;
            else if (b.HpPct <= 0f)                     winnerTeamId = spec.TeamA.Id.Value;
            else                                        winnerTeamId = a.HpPct >= b.HpPct ? spec.TeamA.Id.Value : spec.TeamB.Id.Value;

            return new SimResult(spec, events, winnerTeamId, /*RoundsElapsed*/ events.Count);
        }

        private static void ResolveAttack(
            int round, int tick,
            ref FighterState attacker, ref FighterState defender,
            List<SimEvent> log)
        {
            // Attack cadence becomes a probability per round (clamped)
            // SPD 50 => cadence 1.0 => ~50% chance to swing
            var cadence = CombatModel.AttackCadence(attacker.Loadout.Stats);
            var swingChance = CombatModel.Clamp01(0.5f * cadence);
            if (!attacker.Rng.Roll(swingChance))
                return;

            // Defender dodge roll first
            var dodgeChance = CombatModel.DodgeChance(defender.Loadout.Stats);
            if (defender.Rng.Roll(dodgeChance))
            {
                log.Add(new SimEvent(round, tick,
                    attacker.Loadout.Id.Value, defender.Loadout.Id.Value,
                    EventType.Dodge,
                    attacker.HpPct, attacker.StaminaPct, defender.HpPct, defender.StaminaPct));
                return;
            }

            // Hit chance
            var hitChance = CombatModel.HitChance(attacker.Loadout.Stats, defender.Loadout.Stats);
            if (!attacker.Rng.Roll(hitChance))
            {
                log.Add(new SimEvent(round, tick,
                    attacker.Loadout.Id.Value, defender.Loadout.Id.Value,
                    EventType.Miss,
                    attacker.HpPct, attacker.StaminaPct, defender.HpPct, defender.StaminaPct));
                return;
            }

            // Damage + crit
            var dmg = CombatModel.RollDamage(attacker.Loadout.Stats, attacker.StaminaPct, attacker.Rng);
            var isCrit = attacker.Rng.Roll(CombatModel.CritChance(attacker.Loadout.Stats));
            if (isCrit) dmg *= 1.5f;

            defender.HpPct = System.MathF.Max(0f, defender.HpPct - dmg);

            log.Add(new SimEvent(round, tick,
                attacker.Loadout.Id.Value, defender.Loadout.Id.Value,
                isCrit ? EventType.Crit : EventType.Hit,
                attacker.HpPct, attacker.StaminaPct, defender.HpPct, defender.StaminaPct));

            if (defender.HpPct <= 0f)
            {
                log.Add(new SimEvent(round, tick,
                    attacker.Loadout.Id.Value, defender.Loadout.Id.Value,
                    EventType.KO,
                    attacker.HpPct, attacker.StaminaPct, defender.HpPct, defender.StaminaPct));
            }
        }
    }
}
