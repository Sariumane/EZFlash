namespace EZFlash.Models
{
    static internal class GlobalSettings
    {
        private static readonly AppSettings Defaults = new();

        // Basiseinheit in Stunden
        public static float BaseUnit { get; set; } = Defaults.BaseUnit;

        // Basiseinheit in Sekunden
        public static float BaseInterval => BaseUnit * 3600f;

        public static float AgainStartFactor { get; set; } = Defaults.AgainStartFactor;
        public static float HardStartFactor { get; set; } = Defaults.HardStartFactor;
        public static float GoodStartFactor { get; set; } = Defaults.GoodStartFactor;
        public static float EasyStartFactor { get; set; } = Defaults.EasyStartFactor;

        public static float AgainStartInterval => BaseInterval * AgainStartFactor;
        public static float HardStartInterval => BaseInterval * HardStartFactor;
        public static float GoodStartInterval => BaseInterval * GoodStartFactor;
        public static float EasyStartInterval => BaseInterval * EasyStartFactor;

        public static float AgainMultiplier { get; set; } = Defaults.AgainMultiplier;
        public static float HardMultiplier { get; set; } = Defaults.HardMultiplier;
        public static float GoodMultiplier { get; set; } = Defaults.GoodMultiplier;
        public static float EasyMultiplier { get; set; } = Defaults.EasyMultiplier;

        public static float StreakWeight { get; set; } = Defaults.StreakWeight;

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
