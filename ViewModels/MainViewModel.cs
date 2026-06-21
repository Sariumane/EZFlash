
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
    private string _newDeckName;
    private string _newDeckHint = "Type in a Name";

    public ViewModelBase CurrentViewModel
    {
        get => _currentViewModel;
        set
        {
            _currentViewModel = value;
            OnPropertyChanged();
        }
    }

    public string NewDeckName {
        get => _newDeckName;
        set
        {
            _newDeckName = value;
            OnPropertyChanged(nameof(CanCreateDeck));
        }  
    }
    public bool CanCreateDeck => NewDeckName != _newDeckHint;

    public ICommand ShowHomeViewCommand { get; }
    public ICommand ShowLearnScheduledViewCommand { get; }
    public ICommand ShowCardManagementViewCommand { get; }
    public ICommand SaveNewDeckCommand { get; }

    public MainViewModel(DeckStore deckStore)
    {
        _deckStore = deckStore;
        _homeViewModel = new (_deckStore.Inventory.Decks);
        _learnScheduledViewModel = new ();
        _cardManagementViewModel = new();

        NewDeckName = _newDeckHint;
        CurrentViewModel = _homeViewModel;

        ShowHomeViewCommand = new RelayCommand(ShowHomeView);
        ShowLearnScheduledViewCommand = new RelayCommand(ShowLearnScheduledView);
        ShowCardManagementViewCommand = new RelayCommand(ShowCardManagementView);
        SaveNewDeckCommand = new RelayCommand(SaveNewDeck);
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
            _cardManagementViewModel.CurrentDeck = currentDeck;
            CurrentViewModel = _cardManagementViewModel;
        }
    }

    private void SaveNewDeck()
    {
        _deckStore.Inventory.AddDeck(NewDeckName);
        NewDeckName = _newDeckHint;
        OnPropertyChanged(nameof(NewDeckName));
    }
}
