using System;

namespace Drakefighting.Sim
{
    public class FightSim
    {
        public int Seed { get; }
        private readonly Random _rng;

        public FightSim(int seed)
        {
            Seed = seed;
            _rng = new Random(seed);
        }

        // TODO: Inject teams, produce a log of rounds that the renderer can play back.
        // public SimResult Run(MatchSpec spec) { ... }
    }
}
