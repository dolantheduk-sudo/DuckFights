namespace Drakefighting.Sim
{
    public static class StaminaModel
    {
        public static float DecayPerRound(int baseDrain, int styleMultiplierPercent)
            => baseDrain * (1f + styleMultiplierPercent / 100f);

        public static float LowStaminaPenalty(float staPercent)
            => staPercent < 0.35f ? 0.6f : 1f; // damage multiplier
    }
}
