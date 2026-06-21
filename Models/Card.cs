

namespace EZFlash.Models
{
    public class Card
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

        public string Front { get; set; }
        public string Back { get; set; }

        public DateTime? ReviewedAt { get; private set; }
        public DateTime? NextReviewDate { get; private set; }
        public int Streak { get; private set; } = 0;
        public int ReviewCount { get; private set; } = 0;
        public float Interval { get; private set; }

        public CardRating LastRating { get; private set; }



        public Card(string front = "", string back = "")
        {
            this.Front = front;
            this.Back = back;
        }



        public void RateCard(CardRating rating)
        {
            ReviewedAt = DateTime.UtcNow;
            LastRating = rating;
            float secondsToBeAdded;

            UpdateStreak(rating);
            (Interval, secondsToBeAdded) = CalculateIntervalInSeconds(rating);

            NextReviewDate = ReviewedAt.Value.AddSeconds(secondsToBeAdded);
            ReviewCount++;
        }



        private (float nextInterval, float secondsToBeAdded) CalculateIntervalInSeconds(CardRating rating)
        {
            float currentInterval = Interval;
            float streakFactor = 1f;
            float secondsToBeAdded;
            
            if(ReviewCount == 0)
            {
                currentInterval = rating switch
                {
                    CardRating.Again => GlobalSettings.AgainStartInterval,
                    CardRating.Hard => GlobalSettings.HardStartInterval,
                    CardRating.Good => GlobalSettings.GoodStartInterval,
                    CardRating.Easy => GlobalSettings.EasyStartInterval,
                    _ => throw new ArgumentOutOfRangeException(nameof(rating), rating, null)
                };
                return (currentInterval, currentInterval);
            }

           
            if (rating == CardRating.Good || rating == CardRating.Easy)
                streakFactor = 1 + Streak * GlobalSettings.StreakWeight;
            

            float multiplier = rating switch
            {
                CardRating.Again => GlobalSettings.AgainMultiplier,
                CardRating.Hard => GlobalSettings.HardMultiplier,
                CardRating.Good => GlobalSettings.GoodMultiplier * streakFactor,
                CardRating.Easy => GlobalSettings.EasyMultiplier * streakFactor,
                _ => throw new ArgumentOutOfRangeException(nameof(rating), rating, null)
            };

            currentInterval *= multiplier;


            if(rating == CardRating.Again)
                secondsToBeAdded = GlobalSettings.AgainStartInterval;
            else
                secondsToBeAdded = currentInterval;


            return (currentInterval, secondsToBeAdded);
        }

        private void UpdateStreak(CardRating rating)
        {
            if (rating == CardRating.Good || rating == CardRating.Easy)
            {
                Streak++;
            }
            else
            {
                Streak = 0;
            }
        }

        public enum CardRating
        {
            Again = 0,
            Hard = 1,
            Good = 2,
            Easy = 3
        }
    }
}
