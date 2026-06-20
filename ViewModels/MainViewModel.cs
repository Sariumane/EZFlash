
using System.Windows;
using System.Windows.Input;
using EZFlash.Commands;
using EZFlash.Models;

namespace EZFlash.ViewModels;

public class MainViewModel : ViewModelBase
{

    private ViewModelBase _currentViewModel;
    private HomeViewModel _homeViewModel;
    private LearnScheduledViewModel _learnScheduledViewModel;
    private CardManagementViewModel _cardManagementViewModel;
    private DeckStore _deckStore;

    public ViewModelBase CurrentViewModel
    {
        get => _currentViewModel;
        set
        {
            _currentViewModel = value;
            OnPropertyChanged();
        }
    }

    public ICommand ShowHomeViewCommand { get; }
    public ICommand ShowLearnScheduledViewCommand { get; }
    public ICommand ShowCardManagementViewCommand { get; }

    public MainViewModel(DeckStore deckStore)
    {
        _deckStore = deckStore;
        _homeViewModel = new (_deckStore.Inventory.Decks);
        _learnScheduledViewModel = new ();
        _cardManagementViewModel = new();

        CurrentViewModel = _homeViewModel;

        ShowHomeViewCommand = new RelayCommand(ShowHomeView);
        ShowLearnScheduledViewCommand = new RelayCommand(ShowLearnScheduledView);
        ShowCardManagementViewCommand = new RelayCommand(ShowCardManagementView);
    }


    //Dirty Functions
    private void ShowLearnScheduledView()
    {
        CurrentViewModel = _learnScheduledViewModel;

    }

    private void ShowHomeView()
    {
        CurrentViewModel = _homeViewModel;
    }

    private void ShowCardManagementView()
    {
        var currentDeck = _homeViewModel.SelectedDeck;
        if(currentDeck != null)
        {
            _cardManagementViewModel.CurrentDeck = _homeViewModel.SelectedDeck;
            CurrentViewModel = _cardManagementViewModel;
        }
    }
}
