using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace packmule.ViewModels
{
    class ViewModel : ViewModelBase
    {
        private string _title;
        public string Title { get => _title; set => SetProperty(ref _title, value); }

        public ViewModel()
        {
            _changeTitleCommand = new DelegateCommand(OnChangeName, CanChangeName);
        }

        // Change title command
        private readonly DelegateCommand _changeTitleCommand;
        public ICommand ChangeTitleCommand => _changeTitleCommand;

        private void OnChangeName(object commandParamter)
        {
            Title = "Walter";
            _changeTitleCommand.InvokeCanExecuteChanged();
        }

        private bool CanChangeName(object commandParameter)
        {
            return Title != "Walter";
        }

        private void OnCreatePackHub()
        {

        }
    }
}
