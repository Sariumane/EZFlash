

namespace EZFlash.ViewModels
{
    public class CardViewModel : ViewModelBase
    {
        private string _question = "";
        public string Question
        {
            get => _question;
            set
            {
                _question = value;
                OnPropertyChanged();
            }
        }

        private string _answer = "";
        public string Answer
        {
            get => _answer;
            set
            {
                _answer = value;
                OnPropertyChanged();
            }
        }
    }
}
