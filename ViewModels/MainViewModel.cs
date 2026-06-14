using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using WpfMvvmMinimal.Commands;
using WpfMvvmMinimal.Models;

namespace WpfMvvmMinimal.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    private readonly Person _person = new();
    private string _greeting = "Bitte gib deinen Namen ein.";

    public MainViewModel()
    {
        ShowGreetingCommand = new RelayCommand(ShowGreeting);
    }

    public string Name
    {
        get => _person.Name;
        set
        {
            if (_person.Name == value)
            {
                return;
            }

            _person.Name = value;
            OnPropertyChanged();
        }
    }

    public string Greeting
    {
        get => _greeting;
        private set
        {
            if (_greeting == value)
            {
                return;
            }

            _greeting = value;
            OnPropertyChanged();
        }
    }

    public ICommand ShowGreetingCommand { get; }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void ShowGreeting()
    {
        Greeting = string.IsNullOrWhiteSpace(Name)
            ? "Bitte gib zuerst einen Namen ein."
            : $"Hallo, {Name.Trim()}!";
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
