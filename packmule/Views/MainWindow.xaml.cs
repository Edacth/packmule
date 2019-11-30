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
        public static RoutedCommand PHSetPositionCmd = new RoutedCommand();
        public static RoutedCommand PHTranslateCmd = new RoutedCommand();
        public static RoutedCommand SetSelectedPHCmd = new RoutedCommand();

        private Point dragStartPoint;

        public MainWindow()
        {
            // Set the data context to the viewmodel
            DataContext = viewModel;

            // Create command bindings
            CommandBinding changeTitleCmdBinding = new CommandBinding(ChangeTitleCmd, ChangeTitleCmdExecuted, ChangeTitleCmdCanExecute);
            CommandBinding createPackHubCmdBinding = new CommandBinding(CreatePackHubCmd, CreatePackHubCmdExecuted, CreatePackHubCanExecute);
            CommandBinding PHSetPositionCmdBinding = new CommandBinding(PHSetPositionCmd, PHSetPositionCmdExecuted, PHSetPositionCanExecute);
            CommandBinding PHTranslateCmdBinding = new CommandBinding(PHTranslateCmd, PHTranslateCmdExecuted, PHTranslateCmdCanExecute);
            CommandBinding SetSelectedPHCmdBinding = new CommandBinding(SetSelectedPHCmd, SetSelectedPHCmdExecuted, SetSelectedPHCmdCanExecute);
            
            // Attach CommandBindings to root window
            this.CommandBindings.Add(changeTitleCmdBinding);
            this.CommandBindings.Add(createPackHubCmdBinding);
            this.CommandBindings.Add(PHSetPositionCmdBinding);
            this.CommandBindings.Add(PHTranslateCmdBinding);
            this.CommandBindings.Add(SetSelectedPHCmdBinding);
            //viewModel.OnPropertyChanged(nameof(viewModel.Title));

            InitializeComponent();
            Title = "Test";
            viewModel.Title = "Mark";

            // https://stackoverflow.com/questions/3067617/raising-an-event-on-parent-window-from-a-user-control-in-net-c-sharp
            AddHandler(PackHubUC.MouseDragEvent,
                new RoutedEventHandler(PackHubUC_MouseDragEventHandlerMethod));
            AddHandler(PackHubUC.SetDragStartPointEvent,
                new RoutedEventHandler(PackHubUC_SetDragStartPoint));
            //  HERE
        }

        private void PackHubUC_SetDragStartPoint(object sender, RoutedEventArgs e)
        {
            MouseButtonEventArgs mouseArgs = e.OriginalSource as MouseButtonEventArgs;
            dragStartPoint = mouseArgs.GetPosition(mouseArgs.Source as IInputElement);
            //Console.WriteLine(dragStartPoint);
        }

        private void PackHubUC_MouseDragEventHandlerMethod(object sender, RoutedEventArgs e)
        {
            MouseEventArgs mouseArgs = e.OriginalSource as MouseEventArgs;
            IInputElement source = e.Source as IInputElement;
            if (mouseArgs.LeftButton == MouseButtonState.Pressed)
            {
                Point position = Mouse.GetPosition(source);
                if (Math.Abs(position.X - dragStartPoint.X) > SystemParameters.MinimumHorizontalDragDistance ||
                    Math.Abs(position.Y - dragStartPoint.Y) > SystemParameters.MinimumVerticalDragDistance)
                {
                    viewModel.PHSetPosition(viewModel.SelectedPH, 
                        new Thickness(position.X - dragStartPoint.X, position.Y - dragStartPoint.Y, 0, 0));
                    Console.WriteLine(position.X + " - " + dragStartPoint.X + " = " + (position.X - dragStartPoint.X));
                    //Console.WriteLine(position.X - dragStartPoint.X + " " + (position.Y - dragStartPoint.Y));
                }

            }
        }

        #region ChangeTitleCmd
        private void ChangeTitleCmdExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            viewModel.Title = "Walter";
        }

        private void ChangeTitleCmdCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = viewModel.Title == "Walter" ? false : true;
        }
        #endregion
        #region CreatePackHubCmd
        private void CreatePackHubCmdExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            viewModel.CreatePackHub();
        }

        private void CreatePackHubCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = viewModel.PackHubs.Count < 4;
        }
        #endregion
        #region PHSetPositionCmd 
        // PHSetPosition
        private void PHSetPositionCmdExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Parameter != null)
            {
                try
                {
                    int id = (int)(e.Parameter as int?);
                    //viewModel.PHSetPosition(id, new Thickness(100, 100, 0 ,0));
                    viewModel.PHSetPosition(id, new Thickness(100, 100, 0, 0));
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void PHSetPositionCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        #endregion
        #region PHTranslateCmd
        // PHTranslate
        private void PHTranslateCmdExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Parameter != null)
            {
                try
                {
                    //int id = (int)(e.Parameter as int?);
                    string direction = (e.Parameter as string).ToLower();
                    switch (direction)
                    {
                        case "up":
                            viewModel.PHTranslate(viewModel.SelectedPH, new Thickness(0, -100, 0, 0));
                            break;
                        case "down":
                            viewModel.PHTranslate(viewModel.SelectedPH, new Thickness(0, 100, 0, 0));
                            break;
                        case "left":
                            viewModel.PHTranslate(viewModel.SelectedPH, new Thickness(100, 0, 0, 0));
                            break;
                        case "right":
                            viewModel.PHTranslate(viewModel.SelectedPH, new Thickness(-100, 0, 0, 0));
                            break;
                        default:
                            break;
                    }
                    

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void PHTranslateCmdCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        #endregion
        #region SetSelectedPHCmd
        // SetSelectedPH
        private void SetSelectedPHCmdExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Parameter != null)
            {
                try
                {
                    int id = (int)(e.Parameter as int?);
                    viewModel.SelectedPH = id;

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void SetSelectedPHCmdCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        #endregion
    }
}
