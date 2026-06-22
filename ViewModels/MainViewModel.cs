using System.Windows;
using System.Windows.Input;
using EZFlash.Commands;
using EZFlash.Models;

namespace EZFlash.ViewModels;

public class MainViewModel : ViewModelBase
{
    private ViewModelBase _currentViewModel;
    private HomeViewModel _homeViewModel;
    private CardManagementViewModel _cardManagementViewModel;
    private LearningViewModel _learningViewModel;
    private LearningResultViewModel _learningResultViewModel;

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
    public ICommand StartFreeLearningCommand { get; }
    public ICommand StartScheduledLearningCommand { get; }
    public ICommand ShowCardManagementViewCommand { get; }

    public MainViewModel(DeckStore deckStore)
    {
        _deckStore = deckStore;

        _homeViewModel = new HomeViewModel(
            _deckStore.Inventory.Decks,
            StartScheduledLearning,
            StartFreeLearning,
            ShowCardManagementView,
            DeleteDeck,
            SaveNewDeck,
            SaveExistingDeck
        );

        _cardManagementViewModel = new CardManagementViewModel(
            SaveExistingDeck,
            ShowHomeView
        );

        _learningViewModel = new LearningViewModel(SaveExistingDeck);

        _learningResultViewModel = new LearningResultViewModel(ShowHomeView);

        _learningViewModel.LearningFinished += OnLearningFinished;

        CurrentViewModel = _homeViewModel;

        ShowHomeViewCommand = new RelayCommand(ShowHomeView);
        StartFreeLearningCommand = new RelayCommand(StartFreeLearning);
        StartScheduledLearningCommand = new RelayCommand(StartScheduledLearning);
        ShowCardManagementViewCommand = new RelayCommand(ShowCardManagementView);
    }

    private void StartFreeLearning()
    {
        Deck? currentDeck = _homeViewModel.SelectedDeck;

        if (currentDeck == null)
            return;

        _learningViewModel.StartFreeLearning(currentDeck);
        CurrentViewModel = _learningViewModel;
    }

    private void StartScheduledLearning()
    {
        Deck? currentDeck = _homeViewModel.SelectedDeck;

        if (currentDeck == null)
            return;

        _learningViewModel.StartScheduledLearning(currentDeck);
        CurrentViewModel = _learningViewModel;
    }

    private void OnLearningFinished(LearningMode mode, IReadOnlyList<CardReviewResult> results)
    {
        _learningResultViewModel.Load(mode, results);
        CurrentViewModel = _learningResultViewModel;
    }

    private void ShowHomeView()
    {
        CurrentViewModel = _homeViewModel;
    }

    private void ShowCardManagementView()
    {
        Deck? currentDeck = _homeViewModel.SelectedDeck;

        if (currentDeck != null)
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

    private void SaveExistingDeck(Deck deck)
    {
        _deckStore.SaveDeckToDisk(deck);
    }

    private void DeleteDeck()
    {
        if (_homeViewModel.SelectedDeck == null)
            return;

        _deckStore.DeleteDeckFromDisk(_homeViewModel.SelectedDeck);
        _homeViewModel.SelectedDeck = null;
    }
}