using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace packmule.Models
{
    public class PackHub
    {
        private string _title;
        public string Title { get => _title; set => SetProperty(ref _title, value); }
        private int _id;
        public int ID { get => _id; set => SetProperty(ref _id, value); }
        private Vector2 _position;
        public Vector2 Position { get => _position; set => SetProperty(ref _position, value); }
        private string _baseDirectory;
        public string BaseDirectory { get => _baseDirectory; set => _baseDirectory = value; }
        private List<PackInfo> _entries;
        public List<PackInfo> Entries { get => _entries; }

        public PackHub(int _ID)
        {
            Title = "Test Title";
            ID = _ID;
        }

        public event PropertyChangedEventHandler PropertyChanged;

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
    }
}
