
using System.Collections;
using System.Collections.ObjectModel;

namespace EZFlash.Models
{
    public class DeckCollection
    {
        public ObservableCollection<Deck> Decks { get; set; } = new();
        public Deck? SelectedDeck { get; set; }

        public void AddDeck(Deck deck) => Decks.Add(deck);

        public void RemoveSelectedDeck(Deck deck)
        {
            if (SelectedDeck != null)
                Decks.Remove(SelectedDeck);
        }
    }
}
