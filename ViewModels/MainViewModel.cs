
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

    public MainViewModel()
    {
        CurrentViewModel = new HomeViewModel();
    }
}
