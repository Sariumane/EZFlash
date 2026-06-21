

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using EZFlash.ViewModels;

namespace EZFlash.Models
{
    public class Deck : ViewModelBase
    {
        public Guid Id { get; init; } = Guid.NewGuid();

        private string _name;
        public string Name {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            } 
        }
        public int TotalCount => Cards.Count;
        public int DueCount => Cards.Count(card => card.NextReviewDate <= DateTime.UtcNow);

        public int NewCardLimit { get; set; } = 20;

        public ObservableCollection<Card> Cards { get; set; } = new();
        public ObservableCollection<Review> ReviewLog { get; set; } = new();




        public Deck(string name) => this.Name = name;



        public Card this[int index]
        {
            get => Cards[index];
            set => Cards[index] = value;
        }


        public void AddCard(string front, string back) => Cards.Add(new(front, back));

        public void AddCard(Card card) => Cards.Add(card);

        public void RemoveCard(Card card) => Cards.Remove(card);

        public ObservableCollection<Card> GetDueCards(DateTime UtcNow)
        {
            return new ObservableCollection<Card>(
                Cards
                .Where(card => card.NextReviewDate <= DateTime.UtcNow)
                .OrderBy(card => card.NextReviewDate));
        }
    }
}