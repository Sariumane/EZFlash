using EZFlash.Commands;
using EZFlash.Models;

namespace EZFlash.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        public RelayCommand SaveSettingsCommand { get; }
        public RelayCommand ResetSettingsCommand { get; }

        private float _baseUnit;

        private float _againStartFactor;
        private float _hardStartFactor;
        private float _goodStartFactor;
        private float _easyStartFactor;

        private float _againMultiplier;
        private float _hardMultiplier;
        private float _goodMultiplier;
        private float _easyMultiplier;

        private float _streakWeight;

        private SettingsStore _settingsStore;

        public SettingsViewModel(SettingsStore settingsStore)
        {
            _settingsStore = settingsStore;

            LoadFromGlobalSettings();

            SaveSettingsCommand = new RelayCommand(SaveSettings);
            ResetSettingsCommand = new RelayCommand(ResetSettings);
        }

        public float BaseUnit
        {
            get => _baseUnit;
            set
            {
                _baseUnit = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(BaseIntervalPreview));
            }
        }

        public float AgainStartFactor
        {
            get => _againStartFactor;
            set
            {
                _againStartFactor = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(AgainStartPreview));
            }
        }

        public float HardStartFactor
        {
            get => _hardStartFactor;
            set
            {
                _hardStartFactor = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HardStartPreview));
            }
        }

        public float GoodStartFactor
        {
            get => _goodStartFactor;
            set
            {
                _goodStartFactor = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(GoodStartPreview));
            }
        }

        public float EasyStartFactor
        {
            get => _easyStartFactor;
            set
            {
                _easyStartFactor = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(EasyStartPreview));
            }
        }

        public float AgainMultiplier
        {
            get => _againMultiplier;
            set
            {
                _againMultiplier = value;
                OnPropertyChanged();
            }
        }

        public float HardMultiplier
        {
            get => _hardMultiplier;
            set
            {
                _hardMultiplier = value;
                OnPropertyChanged();
            }
        }

        public float GoodMultiplier
        {
            get => _goodMultiplier;
            set
            {
                _goodMultiplier = value;
                OnPropertyChanged();
            }
        }

        public float EasyMultiplier
        {
            get => _easyMultiplier;
            set
            {
                _easyMultiplier = value;
                OnPropertyChanged();
            }
        }

        public float StreakWeight
        {
            get => _streakWeight;
            set
            {
                _streakWeight = value;
                OnPropertyChanged();
            }
        }

        public string BaseIntervalPreview =>
            $"Base Interval: {BaseUnit:0.##}h";

        public string AgainStartPreview =>
            $"Again: {BaseUnit * AgainStartFactor:0.##}h";

        public string HardStartPreview =>
            $"Hard: {BaseUnit * HardStartFactor:0.##}h";

        public string GoodStartPreview =>
            $"Good: {BaseUnit * GoodStartFactor:0.##}h";

        public string EasyStartPreview =>
            $"Easy: {BaseUnit * EasyStartFactor:0.##}h";

        private void LoadFromGlobalSettings()
        {
            BaseUnit = GlobalSettings.BaseUnit;

            AgainStartFactor = GlobalSettings.AgainStartFactor;
            HardStartFactor = GlobalSettings.HardStartFactor;
            GoodStartFactor = GlobalSettings.GoodStartFactor;
            EasyStartFactor = GlobalSettings.EasyStartFactor;

            AgainMultiplier = GlobalSettings.AgainMultiplier;
            HardMultiplier = GlobalSettings.HardMultiplier;
            GoodMultiplier = GlobalSettings.GoodMultiplier;
            EasyMultiplier = GlobalSettings.EasyMultiplier;

            StreakWeight = GlobalSettings.StreakWeight;
        }

        private void SaveSettings()
        {
            GlobalSettings.BaseUnit = BaseUnit;

            GlobalSettings.AgainStartFactor = AgainStartFactor;
            GlobalSettings.HardStartFactor = HardStartFactor;
            GlobalSettings.GoodStartFactor = GoodStartFactor;
            GlobalSettings.EasyStartFactor = EasyStartFactor;

            GlobalSettings.AgainMultiplier = AgainMultiplier;
            GlobalSettings.HardMultiplier = HardMultiplier;
            GlobalSettings.GoodMultiplier = GoodMultiplier;
            GlobalSettings.EasyMultiplier = EasyMultiplier;

            GlobalSettings.StreakWeight = StreakWeight;

            _settingsStore.SaveSettingsToDisk();
        }

        private void ResetSettings()
        {
            GlobalSettings.ResetDefaults();
            _settingsStore.SaveSettingsToDisk();

            LoadFromGlobalSettings();
        }
    }
}