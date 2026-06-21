using System.Windows.Input;
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
                ? "Keine Karten"
                : $"{CurrentCardIndex + 1} / {CurrentDeck.TotalCount}";


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
        public ICommand NextCardCommand { get;  }
        public ICommand PreviousCardCommand {  get; }
        public ICommand AbortCommand { get; }
        


        public CardManagementViewModel(Action<Deck> saveDeck, Action exit){

            _exit = exit;
            SaveDeck = saveDeck;

            CreateNewCardCommand = new RelayCommand(ShowCreateNewCardView);
            EditCurrentCardCommand = new RelayCommand(ShowEditCardView);
            NextCardCommand = new RelayCommand(NextCard);
            PreviousCardCommand = new RelayCommand(PreviousCard);
            DeleteCurrentCardCommand = new RelayCommand(DeleteCurrentCard);

            _createOrEditCardViewModel = new(AbortEdit);

            AbortCommand = new RelayCommand(() =>
            {
                if(_currentEditorViewModel == _cardViewModel)
                {
                    _exit();
                    return;
                }
                AbortEdit();
            });

            _cardViewModel = new();
            _cardViewModel.Answer = CurrentCard?.Front ?? "";
            _cardViewModel.Question = CurrentCard?.Back ?? "";
            _currentEditorViewModel = _cardViewModel;

        }


        public void InitCardView()
        {
            _currentEditorViewModel = _cardViewModel;
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
            if(CurrentCard != null)
            {
                CurrentEditorViewModel = _createOrEditCardViewModel;

                _createOrEditCardViewModel.CardInEdit = CurrentCard;
                _createOrEditCardViewModel.Question = CurrentCard.Front;
                _createOrEditCardViewModel.Answer = CurrentCard.Back;
                _cardInEdit = CurrentCard;
                _createOrEditCardViewModel.InitSaveCommand(SaveExistingCard);
            }
        }

        private void AbortEdit() => CurrentEditorViewModel = _cardViewModel;

        private void SaveExistingCard()
        {
            SaveDeck(CurrentDeck);
            _cardViewModel.Question = CurrentCard?.Front ?? "";
            _cardViewModel.Answer = CurrentCard?.Back ?? "";
            CurrentEditorViewModel = _cardViewModel;
        }

        private void SaveNewCard()
        {
            CurrentDeck.AddCard(_cardInEdit);
            SaveDeck(CurrentDeck);
            _cardInEdit = new();
            _createOrEditCardViewModel.CardInEdit = _cardInEdit;
            _createOrEditCardViewModel.Question = "";
            _createOrEditCardViewModel.Answer = "";
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

                _cardViewModel.Question = "";
                _cardViewModel.Answer = "";
            }
            else
            {
                if (CurrentCardIndex >= CurrentDeck.TotalCount)
                    CurrentCardIndex = CurrentDeck.TotalCount - 1;

                CurrentCard = CurrentDeck[CurrentCardIndex];

                _cardViewModel.Question = CurrentCard.Front;
                _cardViewModel.Answer = CurrentCard.Back;
            }

            OnPropertyChanged(nameof(CardPositionText));
        }

        private void NextCard()
        {
            if((CurrentCardIndex+1)<CurrentDeck.TotalCount)
                CurrentCardIndex++;
            CurrentCard = CurrentDeck[CurrentCardIndex];
            _cardViewModel.Question = CurrentCard.Front;
            _cardViewModel.Answer = CurrentCard.Back;
            OnPropertyChanged(nameof(CardPositionText));
        }
        private void PreviousCard()
        {
            if (CurrentCardIndex > 0)
                CurrentCardIndex--;
            CurrentCard = CurrentDeck[CurrentCardIndex];
            _cardViewModel.Question = CurrentCard.Front;
            _cardViewModel.Answer = CurrentCard.Back;
            OnPropertyChanged(nameof(CardPositionText));
        }
    }
}
