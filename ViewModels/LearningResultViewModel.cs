using System.Windows;
using EZFlash.Commands;
using EZFlash.Models;

namespace EZFlash.ViewModels
{
    public class LearningResultViewModel : ViewModelBase
    {
        private readonly Action _showHomeView;
        private readonly Action _showReviewLogView;
        private readonly Action _showOlderReview;
        private readonly Action _showNewerReview;

        private bool _isLoadedFromReviewLog;
        private DateTime? _reviewedAt;
        private string _reviewDeckName = "";

        public RelayCommand BackOverviewCommand { get; }
        public RelayCommand ShowOlderReviewCommand { get; }
        public RelayCommand ShowNewerReviewCommand { get; }

        public LearningMode Mode { get; private set; }

        public int TotalCards { get; private set; }
        public int AgainCount { get; private set; }
        public int HardCount { get; private set; }
        public int GoodCount { get; private set; }
        public int EasyCount { get; private set; }

        public double Score { get; private set; }

        public string ReviewPositionText { get; private set; } = "";

        public bool CanShowOlderReview { get; private set; }
        public bool CanShowNewerReview { get; private set; }

        public Visibility ReviewNavigationVisibility =>
            _isLoadedFromReviewLog
                ? Visibility.Visible
                : Visibility.Collapsed;

        public string Title
        {
            get
            {
                if (_isLoadedFromReviewLog)
                    return "Review Summary";

                return Mode == LearningMode.Free
                    ? "Free Learning Complete"
                    : "Scheduled Learning Complete";
            }
        }

        public string Subtitle
        {
            get
            {
                if (_isLoadedFromReviewLog && _reviewedAt != null)
                    return $"{_reviewDeckName} · {_reviewedAt.Value:dd.MM.yyyy HH:mm}";

                return Mode == LearningMode.Free
                    ? "Here is your summary for the entire deck."
                    : "Here is your summary for the cards that were due.";
            }
        }

        public string BackOverviewText =>
            _isLoadedFromReviewLog
                ? "Back to Stats"
                : "Back to Overview";

        public string ScoreText => $"{Score:0}%";

        public LearningResultViewModel(
            Action showHomeView,
            Action showReviewLogView,
            Action showOlderReview,
            Action showNewerReview)
        {
            _showHomeView = showHomeView;
            _showReviewLogView = showReviewLogView;
            _showOlderReview = showOlderReview;
            _showNewerReview = showNewerReview;

            BackOverviewCommand = new RelayCommand(GoBackToOverview);

            ShowOlderReviewCommand = new RelayCommand(
                _showOlderReview,
                () => _isLoadedFromReviewLog && CanShowOlderReview
            );

            ShowNewerReviewCommand = new RelayCommand(
                _showNewerReview,
                () => _isLoadedFromReviewLog && CanShowNewerReview
            );
        }

        public void Load(LearningMode mode, IReadOnlyList<CardReviewResult> results)
        {
            _isLoadedFromReviewLog = false;
            _reviewedAt = null;
            _reviewDeckName = "";

            CanShowOlderReview = false;
            CanShowNewerReview = false;

            Mode = mode;

            TotalCards = results.Count;

            AgainCount = results.Count(result => result.Rating == CardRating.Again);
            HardCount = results.Count(result => result.Rating == CardRating.Hard);
            GoodCount = results.Count(result => result.Rating == CardRating.Good);
            EasyCount = results.Count(result => result.Rating == CardRating.Easy);

            Score = CalculateScore();

            NotifyAllResultPropertiesChanged();
            NotifyNavigationPropertiesChanged();
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
            NotifyNavigationPropertiesChanged();
        }

        public void SetReviewNavigationState(
            bool canShowOlderReview,
            bool canShowNewerReview,
            int currentReviewPosition,
            int totalReviews)
        {
            CanShowOlderReview = canShowOlderReview;
            CanShowNewerReview = canShowNewerReview;

            ReviewPositionText =
                totalReviews > 0
                    ? $"{currentReviewPosition} / {totalReviews}"
                    : "";

            NotifyNavigationPropertiesChanged();
        }

        private void GoBackToOverview()
        {
            if (_isLoadedFromReviewLog)
                _showReviewLogView();
            else
                _showHomeView();
        }

        private void NotifyAllResultPropertiesChanged()
        {
            OnPropertyChanged(nameof(Mode));
            OnPropertyChanged(nameof(Title));
            OnPropertyChanged(nameof(Subtitle));
            OnPropertyChanged(nameof(BackOverviewText));

            OnPropertyChanged(nameof(TotalCards));
            OnPropertyChanged(nameof(AgainCount));
            OnPropertyChanged(nameof(HardCount));
            OnPropertyChanged(nameof(GoodCount));
            OnPropertyChanged(nameof(EasyCount));

            OnPropertyChanged(nameof(Score));
            OnPropertyChanged(nameof(ScoreText));
        }

        private void NotifyNavigationPropertiesChanged()
        {
            OnPropertyChanged(nameof(ReviewNavigationVisibility));
            OnPropertyChanged(nameof(CanShowOlderReview));
            OnPropertyChanged(nameof(CanShowNewerReview));
            OnPropertyChanged(nameof(ReviewPositionText));

            ShowOlderReviewCommand.RaiseCanExecuteChanged();
            ShowNewerReviewCommand.RaiseCanExecuteChanged();
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
