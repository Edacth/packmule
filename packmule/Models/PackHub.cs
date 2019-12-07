using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.IO;
using Newtonsoft;
using System.Collections.ObjectModel;

using Thickness = System.Windows.Thickness;

namespace packmule.Models
{
    public enum StructureTypes
    {
        ComMojang = 0, // development_behavior_packs, development_resource_packs, minecraftWorlds
        ShortHand = 1, // BPs, RPs, worlds
        SolvedStructure = 2 // Might depricate this one. I don't know if it has a place in a world with many packHubs.
    }

    public class PackHub : INotifyPropertyChanged
    {
        private string _title;
        public string Title { get => _title; set => SetProperty(ref _title, value); }
        private int _id;
        public int ID { get => _id; set => SetProperty(ref _id, value); }
        private Thickness _position;
        public Thickness Position { get => _position; set => SetProperty(ref _position, value); } 
        private string _baseDirectory;
        public string BaseDirectory
        {
            get => _baseDirectory;
            set
            {
                if (SetProperty(ref _baseDirectory, value))
                    PopulateLists();
            }
        }
        private ObservableCollection<PackInfo> _bpEntries = new ObservableCollection<PackInfo>();
        public ObservableCollection<PackInfo> BPEntries { get => _bpEntries; }
        private string defaultDirectory;
        private StructureTypes _structureType;
        public StructureTypes StructureType { get => _structureType; set => SetProperty(ref _structureType, value); }
        static Dictionary<StructureTypes, DirectoryStructure> structurePaths = new Dictionary<StructureTypes, DirectoryStructure>();
        // TODO: Put these DirectoryStructure into an ObservableCollections so I can bind the
        // file structure combo box itemSource to it.
        ObservableCollection<DirectoryStructure> _directoryStructures = new ObservableCollection<DirectoryStructure>();
        ObservableCollection<DirectoryStructure> DirectoryStructures { get => _directoryStructures; set => SetProperty(ref _directoryStructures, value); }
        public static DirectoryStructure comMojang = new DirectoryStructure("development_behavior_packs", "development_resouce_packs", "minecraftWorlds");
        public static DirectoryStructure shortHand = new DirectoryStructure("behavior", "resource", "worlds");


        public PackHub(int _ID):this(_ID, new Thickness(0, 0, 0, 0))
        {
        }

        public PackHub(int _ID, System.Windows.Thickness _Position)
        {
            Title = "Test Title";
            Position = _Position;
            ID = _ID;

            // TODO: Figure out why this doesn't update the binding.
            DirectoryStructures.Add(new DirectoryStructure("development_behavior_packs", "development_resouce_packs", "minecraftWorlds"));

            structurePaths.Add(StructureTypes.ComMojang, comMojang);
            structurePaths.Add(StructureTypes.ShortHand, shortHand);

            defaultDirectory = @"C:\Users\s189062\Desktop\packmule\testEnvironment";
            BaseDirectory = defaultDirectory == null ? System.IO.Directory.GetCurrentDirectory() : defaultDirectory;

        }

        private void PopulateLists()
        {
            // TODO: Add support for different directory structures. 
            BPEntries.Clear();
            DetectPacks(BPEntries, BaseDirectory + "\\" + structurePaths[StructureType].BPPath);
        }

        private void DetectPacks(ObservableCollection<PackInfo> entries, string directoryPath)
        {
            DirectoryInfo directory = new DirectoryInfo(directoryPath);
            try
            {
                // Get array of directories in the development folder
                DirectoryInfo[] packDirs = directory.GetDirectories();

                // Itterate through folders looking for packs
                foreach (DirectoryInfo item in packDirs)
                {
                    // Read manifest and deserialize it
                    FileInfo[] manifestFiles = item.GetFiles("manifest.json");
                    string manifestText;
                    using (StreamReader sr = manifestFiles[0].OpenText())
                    {
                        manifestText = sr.ReadToEnd();
                    }
                    PackInfo entry = Newtonsoft.Json.JsonConvert.DeserializeObject<PackInfo>(manifestText);

                    if (entry.header.name == "pack.name")
                    {
                        DirectoryInfo langDir = new DirectoryInfo(item.FullName + "\\texts");
                        FileInfo[] langFiles = langDir.GetFiles("en_US.lang");

                        string[] langLine;
                        using (StreamReader sr = langFiles[0].OpenText())
                        {
                            string[] stringSeparators = new string[] { "=" };
                            while (sr.EndOfStream == false)
                            {
                                langLine = sr.ReadLine().Split(stringSeparators, StringSplitOptions.None);
                                if (langLine[0] == "pack.name" && langLine.Length > 1)
                                {
                                    entry.header.name = langLine[1];
                                }
                                if (langLine[0] == "pack.description" && langLine.Length > 1)
                                {
                                    entry.header.description = langLine[1];
                                }
                            }
                        }
                    }

                    entries.Add(entry);
                    
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
