using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace EZFlash.Views
{
    /// <summary>
    /// Interaktionslogik für CreateOrEditCardView.xaml
    /// </summary>
    public partial class CreateOrEditCardView : UserControl
    {
        public CreateOrEditCardView()
        {
            InitializeComponent();
        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (IsVisible)
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    QuestionTextBox.Focus();
                    Keyboard.Focus(QuestionTextBox);

                    QuestionTextBox.CaretIndex = QuestionTextBox.Text?.Length ?? 0;
                }), DispatcherPriority.ContextIdle);
            }
        }
    }


}
