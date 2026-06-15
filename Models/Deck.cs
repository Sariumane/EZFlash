

using System.Collections.ObjectModel;

namespace EZFlash.Models
{
    public class Deck
    {
        public Guid Id { get; init; } = Guid.NewGuid();

        public string Name { get; set; }
        public int TotalCount => Cards.Count;
        public int DueCount => Cards.Count(card => card.NextReviewDate <= DateTime.UtcNow);

        public int NewCardLimit { get; set; } = 20;

        public ObservableCollection<Card> Cards { get; set; } = new();
        public ObservableCollection<Review> ReviewLog { get; set; } = new();

        public Deck(string name) => this.Name = name;

        public void AddCard(Card card)
        {
            Cards.Add(card);
        }

        public void RemoveCard(Card card)
        {
            Cards.Remove(card);
        }

        public IEnumerable<Card> GetDueCards(DateTime UtcNow)
        {
            return Cards
                .Where(card => card.NextReviewDate <= DateTime.UtcNow)
                .OrderBy(card => card.NextReviewDate);
        }
    }
}