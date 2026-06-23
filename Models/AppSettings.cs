namespace EZFlash.Models
{
    public class AppSettings
    {
        public float BaseUnit { get; set; } = 1f;

        public float AgainStartFactor { get; set; } = 1f;
        public float HardStartFactor { get; set; } = 2.5f;
        public float GoodStartFactor { get; set; } = 8f;
        public float EasyStartFactor { get; set; } = 24f * 3f;

        public float AgainMultiplier { get; set; } = 0.5f;
        public float HardMultiplier { get; set; } = 1f;
        public float GoodMultiplier { get; set; } = 1.2f;
        public float EasyMultiplier { get; set; } = 1.5f;

        public float StreakWeight { get; set; } = 0.1f;

        public bool ContainsNegativeValues()
        {
            return BaseUnit < 0
                   || AgainStartFactor < 0
                   || HardStartFactor < 0
                   || GoodStartFactor < 0
                   || EasyStartFactor < 0
                   || AgainMultiplier < 0
                   || HardMultiplier < 0
                   || GoodMultiplier < 0
                   || EasyMultiplier < 0
                   || StreakWeight < 0;
        }
    }
}
