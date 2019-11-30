using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using Thickness = System.Windows.Thickness;

namespace packmule.Models
{
    public class PackHub : INotifyPropertyChanged
    {
        private string _title;
        public string Title { get => _title; set => SetProperty(ref _title, value); }
        private int _id;
        public int ID { get => _id; set => SetProperty(ref _id, value); }
        private Thickness _position;
        public Thickness Position { get => _position; set => SetProperty(ref _position, value); } 
        private string _baseDirectory;
        public string BaseDirectory { get => _baseDirectory; set => SetProperty(ref _baseDirectory, value); }
        private List<PackInfo> _entries = new List<PackInfo>();
        public List<PackInfo> Entries { get => _entries; }

        public PackHub(int _ID):this(_ID, new Thickness(0, 0, 0, 0))
        {   
        }

        public PackHub(int _ID, System.Windows.Thickness _Position)
        {
            Title = "Test Title";
            Position = _Position;
            ID = _ID;
            BaseDirectory = System.IO.Directory.GetCurrentDirectory();

            Entries.Add(new PackInfo("Name1", "Desc1"));
            Entries.Add(new PackInfo("Name2", "Desc2"));
        }

        // Property Changed Event
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected bool SetProperty<T>(ref T field, T newValue, [CallerMemberName]string propertyName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(field, newValue))
            {
                field = newValue;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                return true;
            }
            return false;
        }
    }

    public class PackInfo
    {
        public string Directory { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public PackInfo(string _Name, string _Description)
        {
            Name = _Name;
            Description = _Description;
        }
    }
}
