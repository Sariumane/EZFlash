using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using EZFlash.Commands;
using EZFlash.Models;

namespace EZFlash.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{

    public MainViewModel()
    {

    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
