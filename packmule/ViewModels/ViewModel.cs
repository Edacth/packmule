using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.ComponentModel;
using System.Collections.ObjectModel;
using packmule.Models;
using System.IO;
using Newtonsoft;

using Thickness = System.Windows.Thickness;
using Newtonsoft.Json;

namespace packmule.ViewModels
{

    public class ViewModel : ViewModelBase
    {
        #region Title
        private string _title;
        public string Title { get => _title; set => SetProperty(ref _title, value); }
        #endregion
        #region PackHubs
        private ObservableCollection<PackHub> _packHubs = new ObservableCollection<PackHub>();
        public ObservableCollection<PackHub> PackHubs { get => _packHubs; }
        #endregion
        #region SelectedPH
        private int _selectedPH;
        public int SelectedPH { get => _selectedPH; set => SetProperty(ref _selectedPH, value); }
        #endregion
        #region DraggingEnabled
        private bool _draggingEnabled;
        public bool DraggingEnabled { get => _draggingEnabled; set => SetProperty(ref _draggingEnabled, value); }
        #endregion
        #region SaveLayoutOnClose
        private bool _saveLayoutOnClose;
        public bool SaveLayoutOnClose { get => _saveLayoutOnClose; set => SetProperty(ref _saveLayoutOnClose, value); }
        #endregion
        #region LoadLayoutOnStart
        private bool _loadLayoutOnStart;
        public bool LoadLayoutOnStart { get => _loadLayoutOnStart; set => SetProperty(ref _loadLayoutOnStart, value); }
        #endregion
        #region StructurePaths
        static ObservableCollection<DirectoryStructure> _structurePaths = new ObservableCollection<DirectoryStructure>();
        public static ObservableCollection<DirectoryStructure> StructurePaths { get => _structurePaths; }
        #endregion
        #region DefaultDirectory
        private string _defaultDirectory;
        public string DefaultDirectory { get => _defaultDirectory; set => SetProperty(ref _defaultDirectory, value); }
        #endregion

        public ViewModel()
        {
            // TODO: Have this read from a config file and populate StructurePaths that way
            StructurePaths.Add(new DirectoryStructure("ComMojang", "development_behavior_packs", "development_resource_packs", "minecraftWorlds"));
            StructurePaths.Add(new DirectoryStructure("Short Hand", "behavior", "resource", "worlds"));

            // TODO: Populate packhubs from file
            LoadSettings();

            if (LoadLayoutOnStart)
            {
                LoadLayout();
            }           
        }

        public void CreatePackHub()
        {
            PackHubs.Add(new PackHub(PackHubs.Count, new Thickness(450 * PackHubs.Count, 0, 0, 0), DefaultDirectory));
        }

        public void CopyPack(int sourceId, int packIndex)
        {
            try
            {
                DirectoryInfo source = new DirectoryInfo(@"C:\");
                DirectoryInfo target = new DirectoryInfo(@"C:\");
                int targetId = PackHubs[sourceId].CopyTarget;
                // This is hardcoded for behavior/resource/worlds. Might make this modular in the future.
                switch (PackHubs[sourceId].SelectedPackType)
                {
                    case 0:
                        source = new DirectoryInfo(PackHubs[sourceId].BPEntries[packIndex].Directory);

                        break;
                    case 1:
                        throw new NotImplementedException();
                        break;
                    case 2:
                        throw new NotImplementedException();
                        break;
                    default:
                        break;
                }

                // Create target DirectoryInfo by appending baseDirectory and the correct structurePath
                switch (PackHubs[targetId].SelectedPackType)
                {
                    case 0:
                        // TODO Finish creating target directory info
                        string[] stringSeparators = new string[] { "\\" };
                        string[] separatedPath = source.FullName.Split(stringSeparators, StringSplitOptions.None);
                        string packName = separatedPath[separatedPath.Length - 1];

                        string structurePath = StructurePaths[PackHubs[targetId].StructureType].BPPath;
                        target = new DirectoryInfo(PackHubs[targetId].BaseDirectory + "\\" + structurePath + "\\" + packName);
                        break;
                    case 1:
                        throw new NotImplementedException();
                        break;
                    case 2:
                        throw new NotImplementedException();
                        break;
                    default:
                        break;
                }

                RecursiveCopy(source, target);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void DeletePack(int id, int packIndex)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure?", "Deletion Confirmation", MessageBoxButton.YesNo);
            if (messageBoxResult != MessageBoxResult.Yes) { return; }

            try
            {
                DirectoryInfo directory;
                // This is hardcoded for behavior/resource/worlds. Might make this modular in the future.
                switch (PackHubs[id].SelectedPackType)
                {
                    case 0:
                        directory = new DirectoryInfo(PackHubs[id].BPEntries[packIndex].Directory);
                        directory.Delete(true);
                        break;
                    case 1:
                        directory = new DirectoryInfo(PackHubs[id].RPEntries[packIndex].Directory);
                        directory.Delete(true);
                        break;
                    case 2:
                        directory = new DirectoryInfo(PackHubs[id].WorldEntries[packIndex].Directory);
                        directory.Delete(true);
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void RecursiveCopy(DirectoryInfo source, DirectoryInfo target)
        {
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
                RecursiveCopy(diSourceSubDir, nextTargetSubDir);
            }
        }

        public void PHSetPosition(int id, Thickness newPosition)
        {
            PackHubs[id].Position = newPosition;
        }

        public void PHTranslate(int id, Thickness translationAmount)
        {
            if (id < 0 || id > PackHubs.Count - 1) { Console.WriteLine("PHTranslate: Id out of bounds"); return; }
            Thickness currentPos = PackHubs[id].Position;
            Thickness newPos = new Thickness(currentPos.Left + translationAmount.Left,
                                             currentPos.Top + translationAmount.Top,
                                             currentPos.Right + translationAmount.Right,
                                             currentPos.Bottom + translationAmount.Bottom);
            PackHubs[id].Position = newPos;
        }

        public void SaveSettings()
        {
            // TODO: Separate the settings and layout files
            // Encapsulate settings
            Settings settings = new Settings();
            settings.SaveLayoutOnClose = SaveLayoutOnClose;
            settings.LoadLayoutOnStart = LoadLayoutOnStart;
            settings.DefaultDirectory = DefaultDirectory;

            // Serialize
            string output = Newtonsoft.Json.JsonConvert.SerializeObject(settings);
            File.WriteAllText(System.IO.Path.GetFullPath(".") + "\\settings.txt", output);
        }

        public void SaveLayout()
        {
            // Encapsulate layout
            Layout layout = new Layout();
            layout.PHSerializes = new PackHubSerialize[PackHubs.Count];
            for (int i = 0; i < PackHubs.Count; i++)
            {
                PackHubSerialize serializeObj = new PackHubSerialize(PackHubs[i]);

                layout.PHSerializes[i] = serializeObj;
            }

            // Serialize
            string output = Newtonsoft.Json.JsonConvert.SerializeObject(layout);
            File.WriteAllText(System.IO.Path.GetFullPath(".") + "\\layout.txt", output);
        }

        private void LoadSettings()
        {
            try
            {
                // Read settings file
                FileInfo settingsFile = new FileInfo(System.IO.Path.GetFullPath(".") + "\\settings.txt");
                string settingsText;
                using (StreamReader sr = settingsFile.OpenText())
                {
                    settingsText = @sr.ReadToEnd();
                }

                // Deserialize
                Settings settings = Newtonsoft.Json.JsonConvert.DeserializeObject<Settings>(settingsText);

                // Apply settings
                SaveLayoutOnClose = settings.SaveLayoutOnClose;
                LoadLayoutOnStart = settings.LoadLayoutOnStart;
                DefaultDirectory = settings.DefaultDirectory;
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
                MessageBoxResult messageBox = MessageBox.Show("Packmule failed to load settings file. It will continue with default settings.", "Settings Failure", MessageBoxButton.OK);
                SaveLayoutOnClose = true;
                LoadLayoutOnStart = true;
                DefaultDirectory = DefaultDirectory = System.IO.Directory.GetCurrentDirectory();
            }
        }

        public void LoadLayout()
        {
            try
            {
                // Read layout file
                FileInfo layoutFile = new FileInfo(System.IO.Path.GetFullPath(".") + "\\layout.txt");
                string layoutText;
                using (StreamReader sr = layoutFile.OpenText())
                {
                    layoutText = @sr.ReadToEnd();
                }

                // Deserialize
                Layout layout = Newtonsoft.Json.JsonConvert.DeserializeObject<Layout>(layoutText);

                // Apply layout
                if (layout.PHSerializes.Length > 0)
                {
                    PackHubs.Clear();
                    for (int i = 0; i < layout.PHSerializes.Length; i++)
                    {
                        PackHubs.Add(new PackHub(layout.PHSerializes[i]));
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
                MessageBoxResult messageBox = MessageBox.Show("Packmule failed to load layout file. It will continue with an empty layout.", "Settings Failure", MessageBoxButton.OK);

            }
        }
    }
}
