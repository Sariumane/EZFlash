using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace EZFlash.Views
{
    /// <summary>
    /// Interaction logic for CardView.xaml
    /// </summary>
    public partial class CardView : UserControl
    {
        public CardView()
        {
            InitializeComponent();
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
