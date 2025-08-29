namespace Drakefighting.Sim
{
    public static class StaminaModel
    {
        // How much stamina is drained in one “active” tick of combat
        public static float DrainPerTick(int smarts, Style style)
        {
            // Base drain ~5.5% per active tick; smarter ducks waste less
            var baseDrain = 0.055f;
            var styleMult = style switch
            {
                Style.Brawler   => 1.25f,
                Style.Tactician => 0.90f,
                Style.Counter   => 1.00f,
                Style.Wildcard  => 1.00f,
                _               => 1.00f
            };
            var smartsMult = 1.0f - (smarts - 50) / 300.0f; // SMT 80 ≈ -10%
            return (float)(baseDrain * styleMult * smartsMult);
        }

        // Damage multiplier penalty when tired
        public static float LowStaminaDamageMult(float staminaPct) => staminaPct < 0.35f ? 0.65f : 1f;
    }
}
