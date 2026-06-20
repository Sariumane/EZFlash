

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

        public void AddCard(string front, string back)
        {
            Cards.Add(new(front, back));
        }

        public void RemoveCard(Card card)
        {
            Cards.Remove(card);
        }

        public ObservableCollection<Card> GetDueCards(DateTime UtcNow)
        {
            return new ObservableCollection<Card>(
                Cards
                .Where(card => card.NextReviewDate <= DateTime.UtcNow)
                .OrderBy(card => card.NextReviewDate));
        }
    }
}