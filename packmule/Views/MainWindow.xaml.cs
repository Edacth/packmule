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
        ViewModels.ViewModel viewModel = new ViewModels.ViewModel();

        public static RoutedCommand CustomRoutedCommand = new RoutedCommand();
        public static RoutedCommand ChangeTitleCmd = new RoutedCommand();


        public MainWindow()
        {

            // attach CommandBinding to root window
            CommandBinding customCommandBinding = new CommandBinding(CustomRoutedCommand, ExecutedCustomCommand, CanExecuteCustomCommand);
            this.CommandBindings.Add(customCommandBinding);
            CommandBinding ChangeTitleCmdBinding = new CommandBinding(ChangeTitleCmd, ChangeTitleCmdExecuted, ChangeTitleCmdCanExecute);
            this.CommandBindings.Add(ChangeTitleCmdBinding);


            viewModel.Title = "Kevin";
            DataContext = viewModel;

            InitializeComponent();

            viewModel.Title = "Mark";


            //viewModel.OnPropertyChanged(nameof(viewModel.Title));

            //
            //items.Add(new PackHub() { Title = "1" });
            //items.Add(new PackHub() { Title = "2", Margin = new Thickness(200, 200, 0 , 0) });
            //items.Add(new PackHub() { Title = "3" });

            //icList.ItemsSource = items;
        }

        private void ExecutedCustomCommand(object sender, ExecutedRoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("Custom Command Executed");
        }

        private void CanExecuteCustomCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void ChangeTitleCmdExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            viewModel.Title = "Walter";
        }

        private void ChangeTitleCmdCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = viewModel.Title == "Walter" ? false : true;
            //e.CanExecute = true;
        }
    }
}
