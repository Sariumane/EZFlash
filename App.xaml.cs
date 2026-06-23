using System.Windows;
using EZFlash.Models;
using EZFlash.ViewModels;
using EZFlash.Views;

namespace EZFlash;

public partial class App : Application
{
    private DeckStore? _deckStore;
    private SettingsStore? _settingsStore;
    private MainWindow? _mainWindow;
    private MainViewModel? _mainViewModel;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        _deckStore = new DeckStore();
        _settingsStore = new SettingsStore();

        if (_settingsStore.UsedDefaultsBecauseSettingsWereInvalid)
        {
            MessageBox.Show(
                "The settings file contained invalid values and was reset to default settings.",
                "Invalid Settings",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
        }

        _mainViewModel = new MainViewModel(_deckStore, _settingsStore);
        _mainWindow = new MainWindow
        {
            DataContext = _mainViewModel
        };

        _mainWindow.Show();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        if (_deckStore != null)
        {
            _deckStore?.SaveAllDecksToDisk();
            _settingsStore?.SaveSettingsToDisk();
        }

        base.OnExit(e);
    }
}
