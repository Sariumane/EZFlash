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

            SaveSettingsCommand = new RelayCommand(SaveSettings, CanSaveSettings);
            ResetSettingsCommand = new RelayCommand(ResetSettings);

            LoadFromGlobalSettings();
        }

        public float BaseUnit
        {
            get => _baseUnit;
            set
            {
                _baseUnit = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(BaseIntervalPreview));
                SaveSettingsCommand.RaiseCanExecuteChanged();
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
                SaveSettingsCommand.RaiseCanExecuteChanged();
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
                SaveSettingsCommand.RaiseCanExecuteChanged();
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
                SaveSettingsCommand.RaiseCanExecuteChanged();
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
                SaveSettingsCommand.RaiseCanExecuteChanged();
            }
        }

        public float AgainMultiplier
        {
            get => _againMultiplier;
            set
            {
                _againMultiplier = value;
                OnPropertyChanged();
                SaveSettingsCommand.RaiseCanExecuteChanged();
            }
        }

        public float HardMultiplier
        {
            get => _hardMultiplier;
            set
            {
                _hardMultiplier = value;
                OnPropertyChanged();
                SaveSettingsCommand.RaiseCanExecuteChanged();
            }
        }

        public float GoodMultiplier
        {
            get => _goodMultiplier;
            set
            {
                _goodMultiplier = value;
                OnPropertyChanged();
                SaveSettingsCommand.RaiseCanExecuteChanged();
            }
        }

        public float EasyMultiplier
        {
            get => _easyMultiplier;
            set
            {
                _easyMultiplier = value;
                OnPropertyChanged();
                SaveSettingsCommand.RaiseCanExecuteChanged();
            }
        }

        public float StreakWeight
        {
            get => _streakWeight;
            set
            {
                _streakWeight = value;
                OnPropertyChanged();
                SaveSettingsCommand.RaiseCanExecuteChanged();
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
            if (!CanSaveSettings())
                return;

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

        private bool CanSaveSettings()
        {
            return !CurrentSettingsContainNegativeValues();
        }

        private bool CurrentSettingsContainNegativeValues()
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
