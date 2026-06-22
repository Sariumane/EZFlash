namespace EZFlash.Models
{
    public class ReviewLogCardResult
    {
        public Guid CardId { get; init; }

        public string Front { get; init; } = "";
        public string Back { get; init; } = "";

        public CardRating Rating { get; init; }
    }
}