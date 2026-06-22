using EZFlash.Commands;
using EZFlash.Models;

namespace EZFlash.ViewModels
{
    public class LearningResultViewModel : ViewModelBase
    {
        private readonly Action _showHomeView;

        public RelayCommand BackHomeCommand { get; }

        public LearningMode Mode { get; private set; }

        public int TotalCards { get; private set; }
        public int AgainCount { get; private set; }
        public int HardCount { get; private set; }
        public int GoodCount { get; private set; }
        public int EasyCount { get; private set; }

        public double Score { get; private set; }

        public string Title =>
            Mode == LearningMode.Free
                ? "Free Learning abgeschlossen"
                : "Scheduled Learning abgeschlossen";

        public string Subtitle =>
            Mode == LearningMode.Free
                ? "Hier ist deine Auswertung für das gesamte Deck."
                : "Hier ist deine Auswertung für die fälligen Karten.";

        public string ScoreText => $"{Score:0}%";

        public LearningResultViewModel(Action showHomeView)
        {
            _showHomeView = showHomeView;
            BackHomeCommand = new RelayCommand(_showHomeView);
        }

        public void Load(LearningMode mode, IReadOnlyList<CardReviewResult> results)
        {
            Mode = mode;

            TotalCards = results.Count;

            AgainCount = results.Count(result => result.Rating == CardRating.Again);
            HardCount = results.Count(result => result.Rating == CardRating.Hard);
            GoodCount = results.Count(result => result.Rating == CardRating.Good);
            EasyCount = results.Count(result => result.Rating == CardRating.Easy);

            Score = CalculateScore();

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