
using System.Windows;
using System.Windows.Input;
using EZFlash.Commands;

namespace EZFlash.ViewModels;

public class MainViewModel : ViewModelBase
{

    private ViewModelBase _currentViewModel;

    public ViewModelBase CurrentViewModel
    {
        get => _currentViewModel;
        set
        {
            _currentViewModel = value;
            OnPropertyChanged();
        }
    }

    public ICommand ShowLearnScheduledViewCommand { get; }
    public ICommand ShowHomeViewCommand { get; }

    public MainViewModel()
    {
        CurrentViewModel = new HomeViewModel();

        ShowLearnScheduledViewCommand = new RelayCommand(ShowLearnScheduledView);
        ShowHomeViewCommand = new RelayCommand(ShowHomeView);
    }


    //Dirty Functions
    private void ShowLearnScheduledView()
    {
        CurrentViewModel = new LearnScheduledViewModel();

    }

    private void ShowHomeView()
    {
        CurrentViewModel = new HomeViewModel();
    }
}
