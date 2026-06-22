using System.IO;
using System.Text.Json;

namespace EZFlash.Models
{
    public class SettingsStore
    {
        private readonly JsonSerializerOptions _options = new();
        private readonly string _appDir;
        private readonly string _settingsPath;

        public SettingsStore()
        {
            _options.WriteIndented = true;

            _appDir = AppContext.BaseDirectory;
            _settingsPath = Path.Combine(_appDir, "settings.json");

            LoadSettingsFromDisk();
        }

        public void LoadSettingsFromDisk()
        {
            AppSettings? settings = ReadSettingsFromDisk();

            if (settings == null)
            {
                settings = new AppSettings();
                SaveSettingsToDisk(settings);
            }

            GlobalSettings.Apply(settings);
        }

        public void SaveSettingsToDisk()
        {
            AppSettings settings = GlobalSettings.ToAppSettings();
            SaveSettingsToDisk(settings);
        }

        private void SaveSettingsToDisk(AppSettings settings)
        {
            string serializedSettings = JsonSerializer.Serialize(settings, _options);
            File.WriteAllText(_settingsPath, serializedSettings);
        }

        private AppSettings? ReadSettingsFromDisk()
        {
            try
            {
                if (!File.Exists(_settingsPath))
                    return null;

                string serializedSettings = File.ReadAllText(_settingsPath);

                AppSettings? settings = JsonSerializer.Deserialize<AppSettings>(serializedSettings);

                return settings;
            }
            catch (IOException)
            {
                return null;
            }
            catch (UnauthorizedAccessException)
            {
                return null;
            }
            catch (JsonException)
            {
                return null;
            }
        }
    }
}