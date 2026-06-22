using System.Windows;
using System.Windows.Input;
using EZFlash.Commands;
using EZFlash.Models;

namespace EZFlash.ViewModels;

public class MainViewModel : ViewModelBase
{
    private ViewModelBase _currentViewModel;
    private readonly HomeViewModel _homeViewModel;
    private readonly CardManagementViewModel _cardManagementViewModel;
    private readonly LearningViewModel _learningViewModel;
    private readonly LearningResultViewModel _learningResultViewModel;
    private readonly SettingsViewModel _settingsViewModel;
    private readonly ReviewLogViewModel _reviewLogViewModel;

    private DeckStore _deckStore;
    private SettingsStore _settingsStore;

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
    public ICommand ShowSettingsViewCommand { get; }
    public ICommand ShowReviewLogViewCommand { get; }

    public MainViewModel(DeckStore deckStore, SettingsStore settingsStore)
    {
        _deckStore = deckStore;
        _settingsStore = settingsStore;

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

        _settingsViewModel = new SettingsViewModel(_settingsStore);

        _reviewLogViewModel = new ReviewLogViewModel(_deckStore, ShowReviewResult);

        CurrentViewModel = _homeViewModel;

        ShowHomeViewCommand = new RelayCommand(ShowHomeView);
        StartFreeLearningCommand = new RelayCommand(StartFreeLearning);
        StartScheduledLearningCommand = new RelayCommand(StartScheduledLearning);
        ShowCardManagementViewCommand = new RelayCommand(ShowCardManagementView);
        ShowSettingsViewCommand = new RelayCommand(ShowSettingsView);
        ShowReviewLogViewCommand = new RelayCommand(ShowReviewLogView);
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

    private void ShowSettingsView() => CurrentViewModel = _settingsViewModel;

    private void ShowReviewLogView()
    {
        _reviewLogViewModel.RefreshReviews();
        CurrentViewModel = _reviewLogViewModel;
    }

    private void ShowReviewResult(Review review)
    {
        _learningResultViewModel.LoadFromReview(review);
        CurrentViewModel = _learningResultViewModel;
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