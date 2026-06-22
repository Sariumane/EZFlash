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

    private Review? _currentReviewResult;

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

        _learningResultViewModel = new LearningResultViewModel(
             ShowHomeView,
             ShowReviewLogView,
             ShowOlderReviewResult,
             ShowNewerReviewResult
         );

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
        _currentReviewResult = null;

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

        if (_currentReviewResult != null)
            _reviewLogViewModel.SelectedReview = _currentReviewResult;

        CurrentViewModel = _reviewLogViewModel;
    }

    private void ShowReviewResult(Review review)
    {
        _currentReviewResult = review;

        _learningResultViewModel.LoadFromReview(review);
        UpdateReviewResultNavigationState();

        CurrentViewModel = _learningResultViewModel;
    }

    private void ShowOlderReviewResult()
    {
        int currentIndex = GetCurrentReviewIndex();

        if (currentIndex < 0)
            return;

        int olderIndex = currentIndex + 1;

        if (olderIndex >= _reviewLogViewModel.Reviews.Count)
            return;

        ShowReviewResult(_reviewLogViewModel.Reviews[olderIndex]);
    }

    private void ShowNewerReviewResult()
    {
        int currentIndex = GetCurrentReviewIndex();

        if (currentIndex <= 0)
            return;

        int newerIndex = currentIndex - 1;

        ShowReviewResult(_reviewLogViewModel.Reviews[newerIndex]);
    }

    private int GetCurrentReviewIndex()
    {
        if (_currentReviewResult == null)
            return -1;

        return _reviewLogViewModel.Reviews.IndexOf(_currentReviewResult);
    }

    private void UpdateReviewResultNavigationState()
    {
        int currentIndex = GetCurrentReviewIndex();
        int totalReviews = _reviewLogViewModel.Reviews.Count;

        bool canShowOlder =
            currentIndex >= 0 &&
            currentIndex < totalReviews - 1;

        bool canShowNewer =
            currentIndex > 0;

        int currentPosition =
            currentIndex >= 0
                ? currentIndex + 1
                : 0;

        _learningResultViewModel.SetReviewNavigationState(
            canShowOlder,
            canShowNewer,
            currentPosition,
            totalReviews
        );
    }

    private void SaveNewDeck(string newDeckName)
    {
        string trimmedName = newDeckName.Trim();

        if (string.IsNullOrWhiteSpace(trimmedName))
            return;

        bool deckAlreadyExists = _deckStore.Inventory.Decks.Any(deck =>
            string.Equals(
                deck.Name.Trim(),
                trimmedName,
                StringComparison.CurrentCultureIgnoreCase));

        if (deckAlreadyExists)
            return;

        Deck deck = _deckStore.Inventory.AddDeck(trimmedName);
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