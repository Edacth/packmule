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

namespace packmule
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            ViewModels.ViewModel viewModel = new ViewModels.ViewModel();
            viewModel.Title = "Kevin";
            viewModel.Subtitle = "the lad";
            DataContext = viewModel;

            InitializeComponent();

            viewModel.Title = "Mark";
            viewModel.Subtitle = "the man";
            //viewModel.OnPropertyChanged(nameof(viewModel.Title));

            //List<PackHub> items = new List<PackHub>();
            //items.Add(new PackHub() { Title = "1" });
            //items.Add(new PackHub() { Title = "2", Margin = new Thickness(200, 200, 0 , 0) });
            //items.Add(new PackHub() { Title = "3" });

            //icList.ItemsSource = items;
        }
    }
}
