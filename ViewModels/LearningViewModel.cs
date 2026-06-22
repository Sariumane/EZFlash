using System.Windows;
using EZFlash.Commands;
using EZFlash.Models;

namespace EZFlash.ViewModels
{
    public class LearningViewModel : ViewModelBase
    {
        private Deck? _currentDeck;
        private LearningSession? _session;
        private bool _isAnswerVisible;

        private readonly Action<Deck> _saveDeck;

        public event Action<LearningMode, IReadOnlyList<CardReviewResult>>? LearningFinished;

        public RelayCommand ShowAnswerCommand { get; }
        public RelayCommand RateAgainCommand { get; }
        public RelayCommand RateHardCommand { get; }
        public RelayCommand RateGoodCommand { get; }
        public RelayCommand RateEasyCommand { get; }

        public LearningViewModel(Action<Deck> saveDeck)
        {
            _saveDeck = saveDeck;

            ShowAnswerCommand = new RelayCommand(
                ShowAnswer,
                () => HasCards && !IsAnswerVisible
            );

            RateAgainCommand = new RelayCommand(
                () => RateCurrentCard(CardRating.Again),
                () => HasCards && IsAnswerVisible
            );

            RateHardCommand = new RelayCommand(
                () => RateCurrentCard(CardRating.Hard),
                () => HasCards && IsAnswerVisible
            );

            RateGoodCommand = new RelayCommand(
                () => RateCurrentCard(CardRating.Good),
                () => HasCards && IsAnswerVisible
            );

            RateEasyCommand = new RelayCommand(
                () => RateCurrentCard(CardRating.Easy),
                () => HasCards && IsAnswerVisible
            );
        }

        public Card? CurrentCard => _session?.CurrentCard;

        public string Question => CurrentCard?.Front ?? "";
        public string Answer => CurrentCard?.Back ?? "";

        public bool IsAnswerVisible
        {
            get => _isAnswerVisible;
            private set
            {
                if (_isAnswerVisible == value)
                    return;

                _isAnswerVisible = value;

                OnPropertyChanged(nameof(IsAnswerVisible));
                OnPropertyChanged(nameof(AnswerVisibility));
                OnPropertyChanged(nameof(ShowAnswerButtonVisibility));
                OnPropertyChanged(nameof(RatingButtonsVisibility));

                RefreshCommandStates();
            }
        }

        public bool HasCards =>
            _session != null &&
            _session.Cards.Count > 0 &&
            CurrentCard != null;

        public LearningMode CurrentMode =>
            _session?.Mode ?? LearningMode.Free;

        public string Title =>
            CurrentMode == LearningMode.Free
                ? "Free Learning"
                : "Scheduled Learning";

        public string Subtitle =>
            CurrentMode == LearningMode.Free
                ? "Review the entire deck once."
                : "Study only cards that are due for review.";

        public string ProgressText
        {
            get
            {
                if (_session == null || _session.Cards.Count == 0)
                    return "0 / 0";

                int current = Math.Min(_session.CurrentCardIndex + 1, _session.Cards.Count);
                int total = _session.Cards.Count;

                return $"{current} / {total}";
            }
        }

        public string EmptyStateText =>
            CurrentMode == LearningMode.Scheduled
                ? "No cards are currently due."
                : "This deck contains no cards.";

        public Visibility AnswerVisibility =>
            IsAnswerVisible ? Visibility.Visible : Visibility.Collapsed;

        public Visibility ShowAnswerButtonVisibility =>
            !IsAnswerVisible && HasCards ? Visibility.Visible : Visibility.Collapsed;

        public Visibility RatingButtonsVisibility =>
            IsAnswerVisible && HasCards ? Visibility.Visible : Visibility.Collapsed;

        public Visibility CardVisibility =>
            HasCards ? Visibility.Visible : Visibility.Collapsed;

        public Visibility EmptyStateVisibility =>
            HasCards ? Visibility.Collapsed : Visibility.Visible;

        public void StartFreeLearning(Deck deck)
        {
            _currentDeck = deck;

            _session = new LearningSession(
                LearningMode.Free,
                deck.Cards
            );

            RefreshView();
        }

        public void StartScheduledLearning(Deck deck)
        {
            _currentDeck = deck;

            List<Card> dueCards = deck.Cards
                .Where(card => card.IsDue)
                .ToList();

            _session = new LearningSession(
                LearningMode.Scheduled,
                dueCards
            );

            RefreshView();
        }

        private void ShowAnswer()
        {
            IsAnswerVisible = true;
        }

        private void RateCurrentCard(CardRating rating)
        {
            if (_session == null || _currentDeck == null || CurrentCard == null)
                return;

            Card ratedCard = CurrentCard;

            _session.AddResult(ratedCard, rating);

            if (_session.Mode == LearningMode.Scheduled)
            {
                ratedCard.RateCard(rating);
                _currentDeck.NotifyDueCountChanged();
            }

            _session.MoveNext();

            if (_session.IsFinished)
            {
                FinishLearningSession();
                return;
            }

            RefreshView();
        }

        private void FinishLearningSession()
        {
            if (_session == null || _currentDeck == null)
                return;

            if (_session.Results.Count > 0)
            {
                Review review = Review.Create(
                    _currentDeck,
                    _session.Mode,
                    _session.Results
                );

                _currentDeck.AddReview(review);

                if (_session.Mode == LearningMode.Scheduled)
                    _currentDeck.NotifyDueCountChanged();

                _saveDeck(_currentDeck);
            }

            LearningFinished?.Invoke(_session.Mode, _session.Results);
        }

        private void RefreshView()
        {
            IsAnswerVisible = false;

            OnPropertyChanged(nameof(CurrentCard));
            OnPropertyChanged(nameof(Question));
            OnPropertyChanged(nameof(Answer));

            OnPropertyChanged(nameof(HasCards));
            OnPropertyChanged(nameof(CurrentMode));
            OnPropertyChanged(nameof(Title));
            OnPropertyChanged(nameof(Subtitle));
            OnPropertyChanged(nameof(ProgressText));
            OnPropertyChanged(nameof(EmptyStateText));

            OnPropertyChanged(nameof(CardVisibility));
            OnPropertyChanged(nameof(EmptyStateVisibility));
            OnPropertyChanged(nameof(AnswerVisibility));
            OnPropertyChanged(nameof(ShowAnswerButtonVisibility));
            OnPropertyChanged(nameof(RatingButtonsVisibility));

            RefreshCommandStates();
        }

        private void RefreshCommandStates()
        {
            ShowAnswerCommand.RaiseCanExecuteChanged();

            RateAgainCommand.RaiseCanExecuteChanged();
            RateHardCommand.RaiseCanExecuteChanged();
            RateGoodCommand.RaiseCanExecuteChanged();
            RateEasyCommand.RaiseCanExecuteChanged();
        }
    }
}
