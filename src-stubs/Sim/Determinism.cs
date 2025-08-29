using System;

namespace Drakefighting.Sim
{
    // Wrap System.Random so we can swap later if needed
    public sealed class Rng
    {
        private readonly Random _r;
        public int Seed { get; }
        public Rng(int seed) { Seed = seed; _r = new Random(seed); }

        public float NextFloat() => (float)_r.NextDouble();                // [0,1)
        public int Range(int minIncl, int maxExcl) => _r.Next(minIncl, maxExcl);
        public bool Roll(float probability) => NextFloat() < probability;  // 0..1
    }

    public static class TickTime
    {
        // Fight resolves in discrete rounds/ticks; renderer will map ticks to anims.
        public const int TicksPerRound = 1;
        public static int RoundsForMode(MatchMode mode) => mode switch
        {
            MatchMode.OneVOne => 12,
            MatchMode.TagTeam2v2 => 16,
            MatchMode.ThreeVThree => 18,
            MatchMode.FreeForAll => 14,
            MatchMode.Gladiator => 14,
            MatchMode.Dodgeball5v5 => 12,
            _ => 12
        };
    }
}
