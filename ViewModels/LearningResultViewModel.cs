using EZFlash.Commands;
using EZFlash.Models;

namespace EZFlash.ViewModels
{
    public class LearningResultViewModel : ViewModelBase
    {
        private readonly Action _showHomeView;

        private bool _isLoadedFromReviewLog;
        private DateTime? _reviewedAt;
        private string _reviewDeckName = "";

        public RelayCommand BackHomeCommand { get; }

        public LearningMode Mode { get; private set; }

        public int TotalCards { get; private set; }
        public int AgainCount { get; private set; }
        public int HardCount { get; private set; }
        public int GoodCount { get; private set; }
        public int EasyCount { get; private set; }

        public double Score { get; private set; }

        public string Title
        {
            get
            {
                if (_isLoadedFromReviewLog)
                    return "Review-Auswertung";

                return Mode == LearningMode.Free
                    ? "Free Learning abgeschlossen"
                    : "Scheduled Learning abgeschlossen";
            }
        }

        public string Subtitle
        {
            get
            {
                if (_isLoadedFromReviewLog && _reviewedAt != null)
                    return $"{_reviewDeckName} · {_reviewedAt.Value:dd.MM.yyyy HH:mm}";

                return Mode == LearningMode.Free
                    ? "Hier ist deine Auswertung für das gesamte Deck."
                    : "Hier ist deine Auswertung für die fälligen Karten.";
            }
        }

        public string ScoreText => $"{Score:0}%";

        public LearningResultViewModel(Action showHomeView)
        {
            _showHomeView = showHomeView;
            BackHomeCommand = new RelayCommand(_showHomeView);
        }

        public void Load(LearningMode mode, IReadOnlyList<CardReviewResult> results)
        {
            _isLoadedFromReviewLog = false;
            _reviewedAt = null;
            _reviewDeckName = "";

            Mode = mode;

            TotalCards = results.Count;

            AgainCount = results.Count(result => result.Rating == CardRating.Again);
            HardCount = results.Count(result => result.Rating == CardRating.Hard);
            GoodCount = results.Count(result => result.Rating == CardRating.Good);
            EasyCount = results.Count(result => result.Rating == CardRating.Easy);

            Score = CalculateScore();

            NotifyAllResultPropertiesChanged();
        }

        public void LoadFromReview(Review review)
        {
            _isLoadedFromReviewLog = true;
            _reviewedAt = review.ReviewedAt;
            _reviewDeckName = review.DeckName;

            Mode = review.Mode;

            TotalCards = review.TotalCards;

            AgainCount = review.AgainCount;
            HardCount = review.HardCount;
            GoodCount = review.GoodCount;
            EasyCount = review.EasyCount;

            Score = CalculateScore();

            NotifyAllResultPropertiesChanged();
        }

        private void NotifyAllResultPropertiesChanged()
        {
            OnPropertyChanged(nameof(Mode));
            OnPropertyChanged(nameof(Title));
            OnPropertyChanged(nameof(Subtitle));

            OnPropertyChanged(nameof(TotalCards));
            OnPropertyChanged(nameof(AgainCount));
            OnPropertyChanged(nameof(HardCount));
            OnPropertyChanged(nameof(GoodCount));
            OnPropertyChanged(nameof(EasyCount));

            OnPropertyChanged(nameof(Score));
            OnPropertyChanged(nameof(ScoreText));
        }

        private double CalculateScore()
        {
            if (TotalCards == 0)
                return 0;

            int points =
                AgainCount * 0 +
                HardCount * 1 +
                GoodCount * 2 +
                EasyCount * 3;

            int maxPoints = TotalCards * 3;

            return (double)points / maxPoints * 100;
        }
    }
}