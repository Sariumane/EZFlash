using EZFlash.Models;

namespace EZFlash.Models
{
    public class LearningSession
    {
        public LearningMode Mode { get; }
        public List<Card> Cards { get; }
        public List<CardReviewResult> Results { get; } = new();

        public int CurrentCardIndex { get; private set; } = 0;

        public Card? CurrentCard =>
            CurrentCardIndex >= 0 && CurrentCardIndex < Cards.Count
                ? Cards[CurrentCardIndex]
                : null;

        public bool IsFinished => CurrentCardIndex >= Cards.Count;

        public LearningSession(LearningMode mode, IEnumerable<Card> cards)
        {
            Mode = mode;
            Cards = cards.ToList();
        }

        public void AddResult(Card card, CardRating rating)
        {
            Results.Add(new CardReviewResult(card, rating));
        }

        public void MoveNext()
        {
            CurrentCardIndex++;
        }
    }
}