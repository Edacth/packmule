using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace packmule.ViewModels
{
    public class ViewModel : ViewModelBase
    {

        private string _title;
        public string Title { get => _title; set => SetProperty(ref _title, value); }

        ObservableCollection<PackHub> _packHubs = new ObservableCollection<PackHub>();
        public ObservableCollection<PackHub> PackHubs { get => _packHubs; }

        public string PackHubsCount { get => _packHubs.Count.ToString(); }
        public ViewModel()
        {

            PackHubs.Add(new PackHub() { Title = "1" });
            //PackHubs.Add(new PackHub() { Title = "2", Margin = new System.Windows.Thickness(200, 200, 0, 0) });
        }

        public void CreatePackHub()
        {
            //PackHubs.Add(new PackHub() { Title = (PackHubs.Count + 1).ToString()}); 
            PackHubs.Add(new PackHub() { Title = "2", Margin = new System.Windows.Thickness(200, 200, 0, 0) });
            OnPropertyChanged(nameof(PackHubsCount));
        }
    }
}
