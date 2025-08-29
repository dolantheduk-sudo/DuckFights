using Drakefighting.Domain;

namespace Drakefighting.Sim
{
    public static class ModeCoordinator
    {
        // Tag 2v2: maintain active + bench, tag when HP < threshold or on timer
        public static SimResult RunTagTeam(MatchSpec spec) { /* TODO */ return null!; }

        // 3v3 sequential: next drake steps in when previous is KO'd
        public static SimResult RunThreeVThreeSeq(MatchSpec spec) { /* TODO */ return null!; }

        // FFA: n fighters; random targeting each tick (weighted by SMT or proximity)
        public static SimResult RunFreeForAll(MatchSpec spec) { /* TODO */ return null!; }

        // Gladiator: Team A single fighter gets pre-buff effect
        public static SimResult RunGladiator(MatchSpec spec) { /* TODO */ return null!; }

        // Dodgeball: replace melee with projectile-only RNG table
        public static SimResult RunDodgeball(MatchSpec spec) { /* TODO */ return null!; }
    }
}
