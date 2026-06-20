using System.Windows;
using EZFlash.Models;
using EZFlash.ViewModels;
using EZFlash.Views;

namespace EZFlash;

public partial class App : Application
{
    private DeckStore? _deckStore;
    private MainWindow? _mainWindow;
    private MainViewModel? _mainViewModel;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        _deckStore = new DeckStore();
        _mainViewModel = new MainViewModel(_deckStore);
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
        }

        base.OnExit(e);
    }
}
