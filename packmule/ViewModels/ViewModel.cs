using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace packmule.ViewModels
{
    public class ViewModel : ViewModelBase
    {
        private string _title;
        public string Title { get => _title; set => SetProperty(ref _title, value); }

        #region DelegateCommands
        // Change title command
        private readonly DelegateCommand _changeTitleCommand;
        public ICommand ChangeTitleCommand => _changeTitleCommand;
        #endregion
        // I'm almost there 
        https://docs.microsoft.com/en-us/dotnet/framework/wpf/advanced/how-to-create-a-routedcommand
        https://www.codeproject.com/Articles/28093/Using-RoutedCommands-with-a-ViewModel-in-WPF
        // RoutedCommand Test
        public static RoutedCommand ChangeTitleCmd = new RoutedCommand();
        public static RoutedCommand CustomRoutedCommand = new RoutedCommand();

        public ViewModel()
        {
            #region DelegateCommands
            _changeTitleCommand = new DelegateCommand(ChangeTitle, CanChangeTitle);
            #endregion
        }


        private void ChangeTitle(object commandParamter)
        {
            Title = "Walter";
            _changeTitleCommand.InvokeCanExecuteChanged();
        }

        private bool CanChangeTitle(object commandParameter)
        {
            return Title != "Walter";
        }

        private void ChangeTitleCmdExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Title = "Walter";
            System.Windows.MessageBox.Show("in RChangeTitle \n Parameter: " + e.Parameter);
        }

        private void ChangeTitleCmdCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CreatePackHub()
        {

        }
    }
}
