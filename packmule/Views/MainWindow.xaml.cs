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

        public static RoutedCommand ChangeTitleCmd = new RoutedCommand();
        public static RoutedCommand CreatePackHubCmd = new RoutedCommand();
        public static RoutedCommand MoveCmd = new RoutedCommand();


        public MainWindow()
        {
            // Set the data context to the viewmodel
            DataContext = viewModel;

            // Create command bindings
            CommandBinding changeTitleCmdBinding = new CommandBinding(ChangeTitleCmd, ChangeTitleCmdExecuted, ChangeTitleCmdCanExecute);
            CommandBinding createPackHubCmdBinding = new CommandBinding(CreatePackHubCmd, CreatePackHubExecuted, CreatePackHubCanExecute);
            CommandBinding moveCmdBinding = new CommandBinding(MoveCmd, SetPHPositionExecuted, SetPHPositionCanExecute);
            
            // Attach CommandBindings to root window
            this.CommandBindings.Add(changeTitleCmdBinding);
            this.CommandBindings.Add(createPackHubCmdBinding);
            this.CommandBindings.Add(moveCmdBinding);
            //viewModel.OnPropertyChanged(nameof(viewModel.Title));

            InitializeComponent();
            Title = "Test";

            viewModel.Title = "Mark";
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

        private void SetPHPositionExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Parameter != null)
            {
                try
                {
                    int id = (int)(e.Parameter as int?);
                    //viewModel.PHSetPosition(id, new Thickness(100, 100, 0 ,0));
                    viewModel.PHTranslate(id, new Thickness(100, 100, 0, 0));
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void SetPHPositionCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
    }
}
