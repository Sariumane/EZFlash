
using System.Collections.ObjectModel;
using EZFlash.Models;

namespace EZFlash.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        private Deck? _selectedDeck;

        public ObservableCollection<Deck> Library { get; init; }

        public HomeViewModel(ObservableCollection<Deck> library)
        {
            Library = library;
        }

        public Deck? SelectedDeck { 
            get {
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
    }
}