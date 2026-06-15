
using System.Collections.ObjectModel;
using EZFlash.Models;

namespace EZFlash.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        public ObservableCollection<Deck> Library { get; } = new();

        public HomeViewModel()
        {
            Library.Add(new Deck("Deck1"));
            Library.Add(new Deck("Deck2"));
            Library.Add(new Deck("Deck3"));
            Library.Add(new Deck("Deck4"));
        }
    }
}