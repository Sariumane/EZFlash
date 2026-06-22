using System.Collections.ObjectModel;
using System.Text.Json.Serialization;
using EZFlash.ViewModels;

namespace EZFlash.Models
{
    public class Deck : ViewModelBase
    {
        public Guid Id { get; init; } = Guid.NewGuid();

        private string _name;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        [JsonIgnore]
        public int TotalCount => Cards.Count;

        [JsonIgnore]
        public int DueCount => Cards.Count(card => card.IsDue);

        public int NewCardLimit { get; set; } = 20;

        public ObservableCollection<Card> Cards { get; set; } = new();

        public ObservableCollection<Review> ReviewLog { get; set; } = new();

        public Deck(string name) => Name = name;

        public Card this[int index]
        {
            get => Cards[index];
            set
            {
                Cards[index] = value;
                NotifyCardStatsChanged();
            }
        }

        public void AddCard(string front, string back)
        {
            Cards.Add(new Card(front, back));
            NotifyCardStatsChanged();
        }

        public void AddCard(Card card)
        {
            Cards.Add(card);
            NotifyCardStatsChanged();
        }

        public void RemoveCard(Card card)
        {
            Cards.Remove(card);
            NotifyCardStatsChanged();
        }

        public ObservableCollection<Card> GetDueCards(DateTime utcNow)
        {
            return new ObservableCollection<Card>(
                Cards
                    .Where(card => card.IsDue)
                    .OrderBy(card => card.NextReviewDate));
        }

        public void NotifyDueCountChanged()
        {
            OnPropertyChanged(nameof(DueCount));
        }

        public void NotifyCardStatsChanged()
        {
            OnPropertyChanged(nameof(TotalCount));
            OnPropertyChanged(nameof(DueCount));
        }
    }
}