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
        public static RoutedCommand CreatePackHubCmd = new RoutedCommand();


        public MainWindow()
        {
            // Set the data context to the viewmodel
            DataContext = viewModel;

            // Create command bindings
            CommandBinding customCommandBinding = new CommandBinding(CustomRoutedCommand, ExecutedCustomCommand, CanExecuteCustomCommand);
            CommandBinding ChangeTitleCmdBinding = new CommandBinding(ChangeTitleCmd, ChangeTitleCmdExecuted, ChangeTitleCmdCanExecute);
            CommandBinding createPackHubCmdBinding = new CommandBinding(CreatePackHubCmd, CreatePackHubExecuted, CreatePackHubCanExecute);
            
            // Attach CommandBindings to root window
            this.CommandBindings.Add(customCommandBinding);
            this.CommandBindings.Add(ChangeTitleCmdBinding);
            this.CommandBindings.Add(createPackHubCmdBinding);
            //viewModel.OnPropertyChanged(nameof(viewModel.Title));

            InitializeComponent();
            Title = "Test";

            viewModel.Title = "Mark";

            Binding binding1 = new Binding();
            binding1.Source = viewModel.PackHubs;
            //icList.SetBinding(icList.ItemsSource, binding1);
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
        }

        private void CreatePackHubExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            viewModel.CreatePackHub();
        }

        private void CreatePackHubCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = viewModel.PackHubs.Count < 4;
        }
    }
}
