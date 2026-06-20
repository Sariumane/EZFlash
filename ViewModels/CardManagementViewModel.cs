using System.Windows.Input;
using EZFlash.Models;
using EZFlash.Commands;

namespace EZFlash.ViewModels
{
    public class CardManagementViewModel : ViewModelBase
    {
        private ViewModelBase? _currentEditorViewModel;

        public Deck CurrentDeck { get; set; }
        public Card? CurrentCard { get; private set; }
        public int CurrentCardIndex { get; private set; }
        public string CurrentDeckName => CurrentDeck.Name;

        public string CardPositionText =>
            CurrentDeck.Cards.Count == 0
                ? "Keine Karten"
                : $"{CurrentCardIndex + 1} / {CurrentDeck.Cards.Count}";


        public ViewModelBase? CurrentEditorViewModel
        {
            get => _currentEditorViewModel;
            set
            {
                _currentEditorViewModel = value;
                OnPropertyChanged();
            }
        }

        public CardManagementViewModel(){
            CreateNewCardCommand = new RelayCommand(CreateNewCard);
        }

        public ICommand CreateNewCardCommand { get; }

        private void CreateNewCard()
        {
            //CurrentEditorViewModel = new CreateOrEditCardViewModel();
        }
    }
}
