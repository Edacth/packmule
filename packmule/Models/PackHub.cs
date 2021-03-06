﻿using System;
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
    public class PackHub : INotifyPropertyChanged
    {
        #region Title
        private string _title;
        public string Title { get => _title; set => SetProperty(ref _title, value); }
        #endregion
        #region Id
        private int _id;
        public int Id { get => _id; set => SetProperty(ref _id, value); }
        #endregion
        #region Position
        private Thickness _position;
        public Thickness Position { get => _position; set => SetProperty(ref _position, value); }
        #endregion
        #region BaseDirectory
        private string _baseDirectory;
        public string BaseDirectory
        {
            get => _baseDirectory;
            set
            {
                if (SetProperty(ref _baseDirectory, value))
                {
                    try
                    {
                        DirectoryInfo DirInfo = new DirectoryInfo(value);
                        if (DirInfo.Exists)
                        {
                            watcher.Path = value;

                            watcher.EnableRaisingEvents = true;
                        }
                        else
                        {
                            watcher.EnableRaisingEvents = false;
                        }
                        PopulateLists();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("The process failed: {0}", e.ToString());
                    }
                }
            }
        }
        #endregion
        #region BPEntries
        private ObservableCollection<PackInfo> _bpEntries = new ObservableCollection<PackInfo>();
        public ObservableCollection<PackInfo> BPEntries { get => _bpEntries; }
        #endregion
        #region RPEntries
        private ObservableCollection<PackInfo> _rpEntries = new ObservableCollection<PackInfo>();
        public ObservableCollection<PackInfo> RPEntries { get => _rpEntries; }
        #endregion
        #region WorldEntries
        private ObservableCollection<PackInfo> _worldEntries = new ObservableCollection<PackInfo>();
        public ObservableCollection<PackInfo> WorldEntries { get => _worldEntries; }
        #endregion
        #region SelectedPackType
        // Which tabControl item is selected. Behavior/Resource/Worlds
        private int _selectedPackType;
        public int SelectedPackType { get => _selectedPackType; set => SetProperty(ref _selectedPackType, value); }
        #endregion
        #region StructureType
        private int _structureType;
        public int StructureType { get => _structureType;
            set
            {
                if (SetProperty(ref _structureType, value))
                    PopulateLists();
            }
        }
        #endregion
        #region CopyTarget
        private int _copyTarget;
        public int CopyTarget { get => _copyTarget; set => SetProperty(ref _copyTarget, value); }
        #endregion
        #region BackupEnabled
        private bool _backupEnabled;
        public bool BackupEnabled { get => _backupEnabled; set => SetProperty(ref _backupEnabled, value); }
        #endregion
        #region BackupTarget
        private int _backupTarget;
        public int BackupTarget { get => _backupTarget; set => SetProperty(ref _backupTarget, value); }
        #endregion
        #region Initialized
        private bool _initialized = false;
        public bool Initialized { get => _initialized; set => SetProperty(ref _initialized, value); }
        #endregion
        FileSystemWatcher watcher = new FileSystemWatcher();
        

        // Partial Constructor
        public PackHub(int _Id, System.Windows.Thickness _Position, string _BaseDirectory) 
                       :this(_Id, _Position, "", 0, _BaseDirectory, 0, false, 0)
        {
        }

        // PackHubSerialize Constructor
        public PackHub(PackHubSerialize PHS):this(PHS.Id, PHS.Position, PHS.Title,
                       PHS.StructureType, PHS.BaseDirectory, PHS.CopyTarget,
                       PHS.BackupEnabled, PHS.BackupTarget)
        {
        }

        // Full Constructor
        public PackHub(int _Id, System.Windows.Thickness _Position, string _Title,
                       int _StructureType, string _BaseDirectory, int _CopyTarget,
                       bool _BackupEnabled, int _BackupTarget)
        {
            Position = _Position;
            Id = _Id;
            Title = (_Title == "") ? "Pack Hub: " + Id : _Title;
            StructureType = _StructureType;
            BaseDirectory = _BaseDirectory;
            CopyTarget = _CopyTarget;
            BackupEnabled = _BackupEnabled;
            BackupTarget = _BackupTarget;


            #region FileSystemWatcher
            // https://docs.microsoft.com/en-us/dotnet/api/system.io.filesystemwatcher?view=netframework-4.8

            // Watch for changes in LastAccess and LastWrite times, and
            // the renaming of files or directories.
            watcher.NotifyFilter = NotifyFilters.LastAccess
                                    | NotifyFilters.LastWrite
                                    | NotifyFilters.FileName
                                    | NotifyFilters.DirectoryName;

            watcher.Filter = "";

            // Add event handlers
            watcher.Changed += OnChanged;
            watcher.Created += OnChanged;
            watcher.Deleted += OnChanged;
            watcher.Renamed += OnChanged;
            
            #endregion
        }

        public void PopulateLists()
        {
            BPEntries.Clear();
            RPEntries.Clear();
            WorldEntries.Clear();
            DetectPacks(BPEntries, BaseDirectory + "\\" + ViewModels.ViewModel.StructurePaths[StructureType].BPPath);
            DetectPacks(RPEntries, BaseDirectory + "\\" + ViewModels.ViewModel.StructurePaths[StructureType].RPPath);
            DetectWorlds(WorldEntries, BaseDirectory + "\\" + ViewModels.ViewModel.StructurePaths[StructureType].WorldPath);
        }

        private void DetectPacks(ObservableCollection<PackInfo> entries, string directoryPath)
        {
            DirectoryInfo directory = new DirectoryInfo(directoryPath);
            try
            {
                // Get array of directories in the development folder
                DirectoryInfo[] packDirs = directory.GetDirectories();
                int itemIndex = 0;

                // Itterate through folders looking for packs
                foreach (DirectoryInfo item in packDirs)
                {
                    
                    FileInfo[] manifestFiles = item.GetFiles("manifest.json");
                    // End the iteration if no manifest is found
                    if (manifestFiles.Length == 0) { continue; }
                    string manifestText;
                    // Read manifest and deserialize it
                    using (StreamReader sr = manifestFiles[0].OpenText())
                    {
                        manifestText = sr.ReadToEnd();
                    }
                    PackInfo entry = Newtonsoft.Json.JsonConvert.DeserializeObject<PackInfo>(manifestText);
                    entry.Directory = item.FullName;
                    entry.Index = itemIndex;
                    entry.IconPath = item.FullName + "\\pack_icon.png";
                    entry.LoadIcon();

                    // If the pack utilizes lang files, grab the name/desc from the lang file
                    if (entry.header.name == "pack.name")
                    {
                        DirectoryInfo langDir = new DirectoryInfo(item.FullName + "\\texts");
                        FileInfo[] langFiles = langDir.GetFiles("en_US.lang");

                        // Make sure the lang file exists
                        if (langFiles.Length != 0)
                        {
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
                        
                    }

                    entries.Add(entry);
                    itemIndex++;
                }
            }
            catch (DirectoryNotFoundException e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
                return;
            }
        }

        private void DetectWorlds(ObservableCollection<PackInfo> entries, string directoryPath)
        {
            DirectoryInfo directory = new DirectoryInfo(directoryPath);
            try
            {
                // Get array of directories in the development folder
                DirectoryInfo[] packDirs = directory.GetDirectories();
                int itemIndex = 0;

                // Itterate through folders looking for worlds
                foreach (DirectoryInfo item in packDirs)
                {
                    // Read filename.txt
                    FileInfo[] manifestFiles = item.GetFiles("levelname.txt");
                    // End the iteration if no levelname is found
                    if (manifestFiles.Length == 0) { continue; }
                    string fileNameText;
                    using (StreamReader sr = manifestFiles[0].OpenText())
                    {
                        fileNameText = sr.ReadToEnd();
                    }
                    PackInfo entry = new PackInfo();
                    entry.header.name = fileNameText;
                    entry.Directory = item.FullName;
                    entry.Index = itemIndex;

                    entries.Add(entry);
                    itemIndex++;
                }
            }
            catch (DirectoryNotFoundException e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
                return;
            }
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

        // Called when the file watcher detects a change
        private void OnChanged(object source, FileSystemEventArgs e)
        {
            App.Current.Dispatcher.Invoke((Action)delegate
            {
                PopulateLists();
            });
            //Console.WriteLine($"File: {e.FullPath} {e.ChangeType}");
        }
        #endregion
    }
}
