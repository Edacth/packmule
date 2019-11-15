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

        private string _substring;
        public string Subtitle { get => _substring; set => SetProperty(ref _substring, value); }

        private readonly DelegateCommand _changeNameCommand;
        public ICommand ChangeNameCommand => _changeNameCommand;

        public ViewModel()
        {
            _changeNameCommand = new DelegateCommand(OnChangeName);
        }

        private void OnChangeName(object commandParamter)
        {
            Title = "Walter";
        }
    }
}
