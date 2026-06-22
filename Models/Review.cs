namespace EZFlash.Models
{
    public class Review
    {
        public Guid Id { get; init; } = Guid.NewGuid();

        public Guid DeckId { get; init; }
        public string DeckName { get; init; } = "";

        public DateTime ReviewedAt { get; init; } = DateTime.UtcNow;

        public LearningMode Mode { get; init; }

        public int TotalCards { get; init; }

        public int AgainCount { get; init; }
        public int HardCount { get; init; }
        public int GoodCount { get; init; }
        public int EasyCount { get; init; }

        public List<ReviewLogCardResult> Results { get; init; } = new();

        public static Review Create(
            Deck deck,
            LearningMode mode,
            IReadOnlyList<CardReviewResult> results)
        {
            return new Review
            {
                DeckId = deck.Id,
                DeckName = deck.Name,

                ReviewedAt = DateTime.UtcNow,

                Mode = mode,

                TotalCards = results.Count,

                AgainCount = results.Count(result => result.Rating == CardRating.Again),
                HardCount = results.Count(result => result.Rating == CardRating.Hard),
                GoodCount = results.Count(result => result.Rating == CardRating.Good),
                EasyCount = results.Count(result => result.Rating == CardRating.Easy),

                Results = results
                    .Select(result => new ReviewLogCardResult
                    {
                        CardId = result.Card.Id,
                        Front = result.Card.Front,
                        Back = result.Card.Back,
                        Rating = result.Rating
                    })
                    .ToList()
            };
        }
    }
}