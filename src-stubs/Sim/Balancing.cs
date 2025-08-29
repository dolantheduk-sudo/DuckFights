namespace Drakefighting.Sim
{
    public static class Balancing
    {
        public const float BaseHit = 0.78f;
        public const float CritMult = 1.5f;
        public const float BaseDamageMin = 0.12f;
        public const float BaseDamageMaxFromPOW = 0.38f;

        // Drain
        public const float BaseStaDrain = 0.055f;
        public const float BrawlerDrainMult = 1.25f;
        public const float TacticianDrainMult = 0.90f;
        public const float SmartsDrainFactor = 300f;

        // Stamina penalties
        public const float LowStaThreshold = 0.35f;
        public const float LowStaDamageMult = 0.65f;
    }
}
