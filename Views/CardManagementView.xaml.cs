using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace EZFlash.Views
{
    /// <summary>
    /// Interaktionslogik für CardManagement.xaml
    /// </summary>
    public partial class CardManagementView : UserControl
    {
        public CardManagementView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //Dispatcher = Warteschlange des UI Threads
            Dispatcher.BeginInvoke(new Action(() =>
            {
                Focus();
                Keyboard.Focus(this);
            }));
        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (IsVisible)
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    Focus();
                    Keyboard.Focus(this);
                }), DispatcherPriority.Input);
            }
        }
    }
}
