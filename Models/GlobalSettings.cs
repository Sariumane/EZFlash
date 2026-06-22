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

        public static float AgainMultiplier { get; set; } = 0.5f;
        public static float HardMultiplier { get; set; } = 1f;
        public static float GoodMultiplier { get; set; } = 1.2f;
        public static float EasyMultiplier { get; set; } = 1.5f;

        public static float StreakWeight { get; set; } = 0.1f;

        public static AppSettings ToAppSettings()
        {
            return new AppSettings
            {
                BaseUnit = BaseUnit,

                AgainStartFactor = AgainStartFactor,
                HardStartFactor = HardStartFactor,
                GoodStartFactor = GoodStartFactor,
                EasyStartFactor = EasyStartFactor,

                AgainMultiplier = AgainMultiplier,
                HardMultiplier = HardMultiplier,
                GoodMultiplier = GoodMultiplier,
                EasyMultiplier = EasyMultiplier,

                StreakWeight = StreakWeight
            };
        }

        public static void Apply(AppSettings settings)
        {
            BaseUnit = settings.BaseUnit;

            AgainStartFactor = settings.AgainStartFactor;
            HardStartFactor = settings.HardStartFactor;
            GoodStartFactor = settings.GoodStartFactor;
            EasyStartFactor = settings.EasyStartFactor;

            AgainMultiplier = settings.AgainMultiplier;
            HardMultiplier = settings.HardMultiplier;
            GoodMultiplier = settings.GoodMultiplier;
            EasyMultiplier = settings.EasyMultiplier;

            StreakWeight = settings.StreakWeight;
        }

        public static void ResetDefaults()
        {
            Apply(new AppSettings());
        }
    }
}