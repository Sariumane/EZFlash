using System.Windows.Input;
using EZFlash.Commands;

namespace EZFlash.ViewModels
{
    public enum CardEditorMode
    {
        Create,
        Edit
    }

    public class CreateOrEditCardViewModel : ViewModelBase
    {
        private string _question = "";
        private string _answer = "";

        private readonly CardEditorMode _mode;
        private readonly Action<string, string> _saveAction;
        private readonly Action _closeAction;

        public string Title =>
            _mode == CardEditorMode.Create
                ? "Neue Karte erstellen"
                : "Karte bearbeiten";

        public string SaveButtonText =>
            _mode == CardEditorMode.Create
                ? "Speichern und nächste"
                : "Änderungen speichern";

        public string Question
        {
            get => _question;
            set
            {
                if (_question == value)
                    return;

                _question = value;
                OnPropertyChanged();
            }
        }

        public string Answer
        {
            get => _answer;
            set
            {
                if (_answer == value)
                    return;

                _answer = value;
                OnPropertyChanged();
            }
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public CreateOrEditCardViewModel(
            CardEditorMode mode,
            Action<string, string> saveAction,
            Action closeAction,
            string initialQuestion = "",
            string initialAnswer = "")
        {
            _mode = mode;
            _saveAction = saveAction;
            _closeAction = closeAction;

            _question = initialQuestion;
            _answer = initialAnswer;

            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
        }

        private void Save()
        {
            if (string.IsNullOrWhiteSpace(Question) ||
                string.IsNullOrWhiteSpace(Answer))
            {
                return;
            }

            _saveAction(Question, Answer);

            if (_mode == CardEditorMode.Create)
            {
                Question = "";
                Answer = "";
            }
            else
            {
                _closeAction();
            }
        }

        private void Cancel()
        {
            _closeAction();
        }
    }

}
