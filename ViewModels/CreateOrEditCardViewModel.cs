using System.Windows.Input;
using EZFlash.Commands;
using EZFlash.Models;

namespace EZFlash.ViewModels
{

    public class CreateOrEditCardViewModel : ViewModelBase
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

        public Card? CardInEdit;

        public ICommand SaveCommand { get; private set; } = new RelayCommand(() => { return; });
        public ICommand CancelCommand { get; }



        public CreateOrEditCardViewModel(Action cancel)
        {
            CancelCommand = new RelayCommand(cancel);
        }

        public void InitSaveCommand(Action save)
        {
            SaveCommand = new RelayCommand(() =>
            {
                if (CardInEdit != null)
                {
                    CardInEdit.Front = Question;
                    CardInEdit.Back = Answer;
                    save();
                }
            });
            OnPropertyChanged(nameof(SaveCommand));
        }
    }
}
