using System;
using System.Windows.Input;
using EZFlash.Commands;

namespace EZFlash.ViewModels
{
    public class RenameDeckDialogViewModel : ViewModelBase
    {
        private string _deckName;

        public string DeckName
        {
            get => _deckName;
            set
            {
                _deckName = value;
                OnPropertyChanged(nameof(DeckName));

                if (SaveCommand is RelayCommand command)
                    command.RaiseCanExecuteChanged();
            }
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public event Action<bool>? CloseRequested;

        public RenameDeckDialogViewModel(string currentDeckName)
        {
            _deckName = currentDeckName;

            SaveCommand = new RelayCommand(Save, CanSave);
            CancelCommand = new RelayCommand(Cancel);
        }

        private bool CanSave()
        {
            return !string.IsNullOrWhiteSpace(DeckName);
        }

        private void Save()
        {
            DeckName = DeckName.Trim();
            CloseRequested?.Invoke(true);
        }

        private void Cancel()
        {
            CloseRequested?.Invoke(false);
        }
    }
}