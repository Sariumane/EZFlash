
using System.Collections.ObjectModel;

namespace EZFlash.Models
{
    public class DeckCollection
    {
        public ObservableCollection<Deck> Decks { get; set; } = new();
        public Deck? SelectedDeck { get; set; }

        public Deck AddDeck(string title)
        {
            Deck deck = new(title);
            Decks.Add(deck);
            return deck;
        }
        public void AddDeck(Deck deck) => Decks.Add(deck);

        public void RemoveDeck(Deck deck)
        {
            if (deck != null)
                Decks.Remove(deck);
        }
    }
}
