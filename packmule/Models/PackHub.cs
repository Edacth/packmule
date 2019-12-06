using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.IO;
using Newtonsoft;

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
        private List<PackInfo> _bpEntries = new List<PackInfo>();
        public List<PackInfo> BPEntries { get => _bpEntries; }
        string defaultDirectory;
        

        public PackHub(int _ID):this(_ID, new Thickness(0, 0, 0, 0))
        {
        }

        public PackHub(int _ID, System.Windows.Thickness _Position)
        {
            Title = "Test Title";
            Position = _Position;
            ID = _ID;

            defaultDirectory = @"C:\Users\s189062\Desktop\packmule\testEnvironment";
            BaseDirectory = defaultDirectory == null ? System.IO.Directory.GetCurrentDirectory() : defaultDirectory;
            
            DetectPacks(BaseDirectory + "\\" + ViewModels.ViewModel.comMojang.BPPath);

            //Entries.Add(new PackInfo("Name1", "Desc1"));
            //Entries.Add(new PackInfo("Name2", "Desc2"));
        }

        private void PopulateLists()
        {
            throw new NotImplementedException();
        }

        private void DetectPacks(string directoryPath)
        {
            DirectoryInfo directory = new DirectoryInfo(directoryPath);
            try
            {
                DirectoryInfo[] dirs = directory.GetDirectories();

                foreach (DirectoryInfo item in dirs)
                {
                    FileInfo[] manifestFiles = item.GetFiles("manifest.json");
                    string text;
                    using (StreamReader sr = manifestFiles[0].OpenText())
                    {
                        text = sr.ReadToEnd();
                    }
                    PackInfo entry = Newtonsoft.Json.JsonConvert.DeserializeObject<PackInfo>(text);
                    BPEntries.Add(entry);
                    // TODO: If name/desc = "pack.name/desc" find the lang file and get the name/desc from there
                }
            }
            catch (DirectoryNotFoundException e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
                return;
            }
        }

        private void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            throw new NotImplementedException();
            //https://docs.microsoft.com/en-us/dotnet/api/system.io.directoryinfo?view=netframework-4.8
            if (source.FullName.ToLower() == target.FullName.ToLower())
            {
                return;
            }

            // Check if the target directory exists, if not, create it.
            if (Directory.Exists(target.FullName) == false)
            {
                Directory.CreateDirectory(target.FullName);
            }

            // Copy each file into it's new directory.
            foreach (FileInfo fi in source.GetFiles())
            {
                Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
                fi.CopyTo(Path.Combine(target.ToString(), fi.Name), true);
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }

        private void DeletePack(string deletionPath)
        {
            throw new NotImplementedException();
        }

        #region Property Changed Event
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
        #endregion
    }
}
