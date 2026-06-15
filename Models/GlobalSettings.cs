

namespace EZFlash.Models
{
    static internal class GlobalSettings
    {
        //Die Basiseinheit für die Intervalle in Sekunden
        public static float BaseUnit { get; set; } = 1f; 
        public static float BaseInterval { get; set; } = BaseUnit*3600f;

        public static float AgainStartInterval { get; set; } = BaseInterval * 1f;
        public static float HardStartInterval { get; set; } = BaseInterval * 2.5f;
        public static float GoodStartInterval { get; set; } = BaseInterval * 8f;
        public static float EasyStartInterval { get; set; } = BaseInterval * 24f * 3f;

        public static float AgainMultiplier { get; set; } = 1f;
        public static float HardMultiplier { get; set; } = 0.5f;
        public static float GoodMultiplier { get; set; } = 1.2f;
        public static float EasyMultiplier { get; set; } = 1.5f;
        public static float StreakWeight { get; set; } = 0.1f;
    }
}
