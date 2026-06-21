
using System.Collections.ObjectModel;
using System.Windows.Input;
using EZFlash.Commands;
using EZFlash.Models;
using EZFlash.Views;
using System.Windows;

namespace EZFlash.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        private Action<Deck> _saveExistingDeck;

        private string _newDeckName;
        public string NewDeckName
        {
            get => _newDeckName;
            set
            {
                _newDeckName = value;
                OnPropertyChanged(nameof(CanCreateDeck));
            }
        }

        private Deck? _selectedDeck;
        public Deck? SelectedDeck
        {
            get
            {
                if (_selectedDeck != null)
                    return _selectedDeck;
                else
                    return null;
            }
            set
            {
                _selectedDeck = value;
                OnPropertyChanged();
            }
        }

        public bool CanCreateDeck => NewDeckName != "";

        public ObservableCollection<Deck> Library { get; init; }
        public ICommand LearnScheduledCommand { get; }
        public ICommand LearnFreeCommand { get; }
        public ICommand EditDeckCommand { get; }
        public ICommand SaveNewDeckCommand { get; }
        public ICommand DeleteDeckCommand { get; }
        public ICommand RenameDeckCommand { get; }

        public HomeViewModel(ObservableCollection<Deck> library, 
            Action learnScheduled, Action learnFree, Action editDeck, 
            Action deleteDeck, Action<string> saveDeck, Action<Deck> saveExistingDeck)
        {
            Library = library;
            LearnScheduledCommand = new RelayCommand(learnScheduled);
            LearnFreeCommand = new RelayCommand(learnFree);
            EditDeckCommand = new RelayCommand(editDeck);
            RenameDeckCommand = new RelayCommand(RenameSelectedDeck);
            DeleteDeckCommand = new RelayCommand(deleteDeck);
            SaveNewDeckCommand = new RelayCommand(() => 
            {
                saveDeck(NewDeckName);
                NewDeckName = "";
                OnPropertyChanged(nameof(NewDeckName));
                    
            });

            _saveExistingDeck = saveExistingDeck;

            NewDeckName = "";
        }
       

        private void RenameSelectedDeck()
        {
            if (SelectedDeck == null)
                return;

            RenameDeckDialogViewModel dialogViewModel =
                new RenameDeckDialogViewModel(SelectedDeck.Name);

            RenameDeckDialog dialog = new RenameDeckDialog
            {
                DataContext = dialogViewModel,
                Owner = System.Windows.Application.Current.MainWindow
            };

            dialogViewModel.CloseRequested += result =>
            {
                dialog.DialogResult = result;
            };

            bool? dialogResult = dialog.ShowDialog();

            if (dialogResult != true)
                return;

            string newName = dialogViewModel.DeckName.Trim();

            if (string.IsNullOrWhiteSpace(newName))
                return;

            if (newName == SelectedDeck.Name)
                return;

            SelectedDeck.Name = newName;

            _saveExistingDeck(SelectedDeck);

            OnPropertyChanged(nameof(SelectedDeck));
        }
    }
}