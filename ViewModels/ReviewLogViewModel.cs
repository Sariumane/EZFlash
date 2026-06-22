using System.Collections.ObjectModel;
using System.Windows.Input;
using EZFlash.Commands;
using EZFlash.Models;

namespace EZFlash.ViewModels
{
    public class ReviewLogViewModel : ViewModelBase
    {
        private readonly DeckStore _deckStore;
        private readonly Action<Review> _openReview;

        private Review? _selectedReview;

        public ObservableCollection<Review> Reviews { get; } = new();

        public ICommand OpenSelectedReviewCommand { get; }

        public Review? SelectedReview
        {
            get => _selectedReview;
            set
            {
                if (_selectedReview == value)
                    return;

                _selectedReview = value;
                OnPropertyChanged();
            }
        }

        public ReviewLogViewModel(DeckStore deckStore, Action<Review> openReview)
        {
            _deckStore = deckStore;
            _openReview = openReview;

            OpenSelectedReviewCommand = new RelayCommand(OpenSelectedReview);

            RefreshReviews();
        }

        private void OpenSelectedReview()
        {
            if (SelectedReview == null)
                return;

            _openReview(SelectedReview);
        }

        public void RefreshReviews()
        {
            Reviews.Clear();

            IEnumerable<Review> allReviews = _deckStore.Inventory.Decks
                .SelectMany(deck => deck.ReviewLog)
                .OrderByDescending(review => review.ReviewedAt);

            foreach (Review review in allReviews)
                Reviews.Add(review);

            OnPropertyChanged(nameof(HasReviews));
            OnPropertyChanged(nameof(EmptyStateVisibility));
            OnPropertyChanged(nameof(ReviewListVisibility));
        }

        public bool HasReviews => Reviews.Count > 0;

        public System.Windows.Visibility EmptyStateVisibility =>
            HasReviews
                ? System.Windows.Visibility.Collapsed
                : System.Windows.Visibility.Visible;

        public System.Windows.Visibility ReviewListVisibility =>
            HasReviews
                ? System.Windows.Visibility.Visible
                : System.Windows.Visibility.Collapsed;
    }
}