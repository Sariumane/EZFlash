namespace EZFlash.Models
{
    public class CardReviewResult
    {
        public Card Card { get; }
        public CardRating Rating { get; }

        public CardReviewResult(Card card, CardRating rating)
        {
            Card = card;
            Rating = rating;
        }
    }
}