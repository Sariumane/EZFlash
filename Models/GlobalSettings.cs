namespace EZFlash.Models
{
    static internal class GlobalSettings
    {
        // Basiseinheit in Stunden
        public static float BaseUnit { get; set; } = 1f;

        // Basiseinheit in Sekunden
        public static float BaseInterval => BaseUnit * 3600f;

        public static float AgainStartFactor { get; set; } = 1f;
        public static float HardStartFactor { get; set; } = 2.5f;
        public static float GoodStartFactor { get; set; } = 8f;
        public static float EasyStartFactor { get; set; } = 24f * 3f;

        public static float AgainStartInterval => BaseInterval * AgainStartFactor;
        public static float HardStartInterval => BaseInterval * HardStartFactor;
        public static float GoodStartInterval => BaseInterval * GoodStartFactor;
        public static float EasyStartInterval => BaseInterval * EasyStartFactor;

        public static float AgainMultiplier { get; set; } = 1f;
        public static float HardMultiplier { get; set; } = 0.5f;
        public static float GoodMultiplier { get; set; } = 1.2f;
        public static float EasyMultiplier { get; set; } = 1.5f;

        public static float StreakWeight { get; set; } = 0.1f;

        public static void ResetDefaults()
        {
            BaseUnit = 1f;

            AgainStartFactor = 1f;
            HardStartFactor = 2.5f;
            GoodStartFactor = 8f;
            EasyStartFactor = 24f * 3f;

            AgainMultiplier = 1f;
            HardMultiplier = 0.5f;
            GoodMultiplier = 1.2f;
            EasyMultiplier = 1.5f;

            StreakWeight = 0.1f;
        }
    }
}