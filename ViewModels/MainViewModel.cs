
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
    public ICommand ShowLearnFreeViewCommand { get; }
    public ICommand ShowCardManagementViewCommand { get; }

    public MainViewModel(DeckStore deckStore)
    {
        _deckStore = deckStore;
        _homeViewModel = new (_deckStore.Inventory.Decks, 
            ShowLearnScheduledView, ShowLearnFreeView, ShowCardManagementView, 
            DeleteDeck, SaveNewDeck, SaveExistingDeck);
        _learnScheduledViewModel = new ();
        _cardManagementViewModel = new(SaveExistingDeck, ShowHomeView);

        
        CurrentViewModel = _homeViewModel;

        ShowHomeViewCommand = new RelayCommand(ShowHomeView);
        ShowLearnScheduledViewCommand = new RelayCommand(ShowLearnScheduledView);
        ShowCardManagementViewCommand = new RelayCommand(ShowCardManagementView);
    }


    private void ShowLearnScheduledView()
    {
        CurrentViewModel = _learnScheduledViewModel;

    }

    private void ShowLearnFreeView()
    {
        CurrentViewModel = _learnScheduledViewModel;

    }

    private void ShowHomeView()
    {
        CurrentViewModel = _homeViewModel;
    }

    private void ShowCardManagementView()
    {
        Deck? currentDeck = _homeViewModel.SelectedDeck;
        if(currentDeck != null)
        {
            _cardManagementViewModel.CurrentDeck = currentDeck;
            _cardManagementViewModel.InitCardView();
            CurrentViewModel = _cardManagementViewModel;
        }
    }

    private void SaveNewDeck(string newDeckName)
    {
        Deck deck = _deckStore.Inventory.AddDeck(newDeckName);
        _deckStore.SaveDeckToDisk(deck);
    }

    private void SaveExistingDeck(Deck deck) => _deckStore.SaveDeckToDisk(deck);

    private void DeleteDeck()
    {
        if (_homeViewModel.SelectedDeck == null)
            return;
        _deckStore.DeleteDeckFromDisk(_homeViewModel.SelectedDeck);
        _homeViewModel.SelectedDeck = null;
    }
}
