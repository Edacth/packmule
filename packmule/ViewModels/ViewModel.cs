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

        public ViewModel()
        {

            
        }

        private void CreatePackHub()
        {

        }
    }
}
