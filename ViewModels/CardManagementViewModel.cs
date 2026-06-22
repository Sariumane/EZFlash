using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using EZFlash.Models;
using EZFlash.Commands;

namespace EZFlash.ViewModels
{
    public class CardManagementViewModel : ViewModelBase
    {
        private ViewModelBase? _currentEditorViewModel;
        private CardViewModel _cardViewModel;
        private CreateOrEditCardViewModel _createOrEditCardViewModel;

        private Card _cardInEdit = new("", "");
        private Deck _currentDeck;

        private Action? _exit;

        private readonly DispatcherTimer _reviewTimer;
        private bool _isReviewTimerEnabled;
        private string _reviewTimerText = "";

        public Deck CurrentDeck
        {
            get => _currentDeck;
            set
            {
                _currentDeck = value;

                CurrentCardIndex = 0;
                CurrentCard = CurrentDeck?.TotalCount > 0
                    ? CurrentDeck[0]
                    : null;

                UpdateCardView();
                UpdateReviewTimerText();

                OnPropertyChanged();
                OnPropertyChanged(nameof(CurrentDeckName));
                OnPropertyChanged(nameof(CurrentCard));
                OnPropertyChanged(nameof(CardPositionText));
            }
        }

        public Card? CurrentCard { get; private set; }
        public int CurrentCardIndex { get; private set; }
        public string CurrentDeckName => CurrentDeck.Name;

        public string CardPositionText =>
            CurrentDeck.TotalCount == 0
                ? "No cards"
                : $"{CurrentCardIndex + 1} / {CurrentDeck.TotalCount}";

        public bool IsReviewTimerEnabled
        {
            get => _isReviewTimerEnabled;
            private set
            {
                _isReviewTimerEnabled = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ReviewTimerVisibility));
                OnPropertyChanged(nameof(ReviewTimerButtonText));
            }
        }

        public string ReviewTimerText
        {
            get => _reviewTimerText;
            private set
            {
                _reviewTimerText = value;
                OnPropertyChanged();
            }
        }

        public Visibility ReviewTimerVisibility =>
            IsReviewTimerEnabled
                ? Visibility.Visible
                : Visibility.Collapsed;

        public string ReviewTimerButtonText =>
            IsReviewTimerEnabled
                ? "Hide Review Timer"
                : "Show Review Timer";

        public ViewModelBase? CurrentEditorViewModel
        {
            get => _currentEditorViewModel;
            set
            {
                _currentEditorViewModel = value;
                OnPropertyChanged();
            }
        }

        private Action<Deck> SaveDeck;

        public ICommand CreateNewCardCommand { get; }
        public ICommand EditCurrentCardCommand { get; }
        public ICommand DeleteCurrentCardCommand { get; }
        public ICommand NextCardCommand { get; }
        public ICommand PreviousCardCommand { get; }
        public ICommand AbortCommand { get; }
        public ICommand ToggleReviewTimerCommand { get; }

        public CardManagementViewModel(Action<Deck> saveDeck, Action exit)
        {
            _exit = exit;
            SaveDeck = saveDeck;

            _reviewTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };

            _reviewTimer.Tick += (_, _) => UpdateReviewTimerText();

            CreateNewCardCommand = new RelayCommand(ShowCreateNewCardView);
            EditCurrentCardCommand = new RelayCommand(ShowEditCardView);
            NextCardCommand = new RelayCommand(NextCard);
            PreviousCardCommand = new RelayCommand(PreviousCard);
            DeleteCurrentCardCommand = new RelayCommand(DeleteCurrentCard);
            ToggleReviewTimerCommand = new RelayCommand(ToggleReviewTimer);

            _createOrEditCardViewModel = new(AbortEdit);

            AbortCommand = new RelayCommand(() =>
            {
                if (_currentEditorViewModel == _cardViewModel)
                {
                    _reviewTimer.Stop();
                    IsReviewTimerEnabled = false;

                    _exit();
                    return;
                }

                AbortEdit();
            });

            _cardViewModel = new();
            _cardViewModel.Question = CurrentCard?.Front ?? "";
            _cardViewModel.Answer = CurrentCard?.Back ?? "";
            _currentEditorViewModel = _cardViewModel;
        }

        public void InitCardView()
        {
            _currentEditorViewModel = _cardViewModel;
            UpdateCardView();
            UpdateReviewTimerText();
            OnPropertyChanged(nameof(CurrentEditorViewModel));
        }

        private void ToggleReviewTimer()
        {
            IsReviewTimerEnabled = !IsReviewTimerEnabled;

            if (IsReviewTimerEnabled)
            {
                UpdateReviewTimerText();
                _reviewTimer.Start();
            }
            else
            {
                _reviewTimer.Stop();
            }
        }

        private void UpdateReviewTimerText()
        {
            if (CurrentCard == null)
            {
                ReviewTimerText = "No card selected";
                return;
            }

            if (CurrentCard.NextReviewDate == null)
            {
                ReviewTimerText = "Due now";
                CurrentDeck.NotifyDueCountChanged();
                return;
            }

            TimeSpan remaining = CurrentCard.NextReviewDate.Value - DateTime.UtcNow;

            if (remaining <= TimeSpan.Zero)
            {
                ReviewTimerText = "Due now";
                CurrentDeck.NotifyDueCountChanged();
                return;
            }

            if (remaining.TotalDays >= 1)
            {
                ReviewTimerText =
                    $"Next review in {(int)remaining.TotalDays}d {remaining.Hours:00}:{remaining.Minutes:00}:{remaining.Seconds:00}";
            }
            else
            {
                ReviewTimerText =
                    $"Next review in {remaining.Hours:00}:{remaining.Minutes:00}:{remaining.Seconds:00}";
            }
        }

        private void UpdateCardView()
        {
            _cardViewModel.Question = CurrentCard?.Front ?? "";
            _cardViewModel.Answer = CurrentCard?.Back ?? "";
        }

        private void ShowCreateNewCardView()
        {
            CurrentEditorViewModel = _createOrEditCardViewModel;

            _cardInEdit = new();
            _createOrEditCardViewModel.CardInEdit = _cardInEdit;
            _createOrEditCardViewModel.Question = _cardInEdit.Front;
            _createOrEditCardViewModel.Answer = _cardInEdit.Back;
            _createOrEditCardViewModel.InitSaveCommand(SaveNewCard);
        }

        private void ShowEditCardView()
        {
            if (CurrentCard != null)
            {
                CurrentEditorViewModel = _createOrEditCardViewModel;

                _createOrEditCardViewModel.CardInEdit = CurrentCard;
                _createOrEditCardViewModel.Question = CurrentCard.Front;
                _createOrEditCardViewModel.Answer = CurrentCard.Back;
                _cardInEdit = CurrentCard;
                _createOrEditCardViewModel.InitSaveCommand(SaveExistingCard);
            }
        }

        private void AbortEdit()
        {
            if (CurrentDeck.TotalCount == 0)
            {
                CurrentCard = null;
            }
            else
            {
                CurrentCard = CurrentDeck[CurrentCardIndex];
            }

            UpdateCardView();
            UpdateReviewTimerText();

            CurrentEditorViewModel = _cardViewModel;

            OnPropertyChanged(nameof(CurrentCard));
            OnPropertyChanged(nameof(CardPositionText));
        }

        private void SaveExistingCard()
        {
            SaveDeck(CurrentDeck);

            UpdateCardView();
            UpdateReviewTimerText();

            CurrentEditorViewModel = _cardViewModel;
        }

        private void SaveNewCard()
        {
            CurrentDeck.AddCard(_cardInEdit);
            SaveDeck(CurrentDeck);

            CurrentCardIndex = CurrentDeck.TotalCount - 1;
            CurrentCard = CurrentDeck[CurrentCardIndex];

            _cardInEdit = new();
            _createOrEditCardViewModel.CardInEdit = _cardInEdit;
            _createOrEditCardViewModel.Question = "";
            _createOrEditCardViewModel.Answer = "";

            UpdateReviewTimerText();

            OnPropertyChanged(nameof(CurrentCard));
            OnPropertyChanged(nameof(CardPositionText));
        }

        private void DeleteCurrentCard()
        {
            if (CurrentCard == null)
                return;

            CurrentDeck.RemoveCard(CurrentCard);
            SaveDeck(CurrentDeck);

            if (CurrentDeck.TotalCount == 0)
            {
                CurrentCardIndex = 0;
                CurrentCard = null;
            }
            else
            {
                if (CurrentCardIndex >= CurrentDeck.TotalCount)
                    CurrentCardIndex = CurrentDeck.TotalCount - 1;

                CurrentCard = CurrentDeck[CurrentCardIndex];
            }

            UpdateCardView();
            UpdateReviewTimerText();

            OnPropertyChanged(nameof(CurrentCard));
            OnPropertyChanged(nameof(CardPositionText));
        }

        private void NextCard()
        {
            if (CurrentDeck.TotalCount == 0)
            {
                CurrentCardIndex = 0;
                CurrentCard = null;
            }
            else
            {
                if (CurrentCardIndex < CurrentDeck.TotalCount - 1)
                    CurrentCardIndex++;

                CurrentCard = CurrentDeck[CurrentCardIndex];
            }

            UpdateCardView();
            UpdateReviewTimerText();

            OnPropertyChanged(nameof(CurrentCard));
            OnPropertyChanged(nameof(CardPositionText));
        }

        private void PreviousCard()
        {
            if (CurrentDeck.TotalCount == 0)
            {
                CurrentCardIndex = 0;
                CurrentCard = null;
            }
            else
            {
                if (CurrentCardIndex > 0)
                    CurrentCardIndex--;

                CurrentCard = CurrentDeck[CurrentCardIndex];
            }

            UpdateCardView();
            UpdateReviewTimerText();

            OnPropertyChanged(nameof(CurrentCard));
            OnPropertyChanged(nameof(CardPositionText));
        }
    }
}