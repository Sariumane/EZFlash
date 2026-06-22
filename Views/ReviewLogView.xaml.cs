using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using EZFlash.Models;
using EZFlash.ViewModels;

namespace EZFlash.Views
{
    /// <summary>
    /// Interaction logic for ReviewLogView.xaml
    /// </summary>
    public partial class ReviewLogView : UserControl
    {
        public ReviewLogView()
        {
            InitializeComponent();
        }

        //Double Click on List not working correctly :( -> Code behind for quick and dirty
        private void ReviewLogItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is not ListViewItem listViewItem)
                return;

            if (listViewItem.DataContext is not Review review)
                return;

            if (DataContext is not ReviewLogViewModel viewModel)
                return;

            viewModel.SelectedReview = review;

            if (viewModel.OpenSelectedReviewCommand.CanExecute(null))
                viewModel.OpenSelectedReviewCommand.Execute(null);
        }
    }
}
