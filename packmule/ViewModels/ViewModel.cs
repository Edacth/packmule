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

        private bool ignoreTabChange = false;

        public ViewModel()
        {
            // TODO: Have this read from a config file and populate StructurePaths that way
            StructurePaths.Add(new DirectoryStructure("ComMojang", "development_behavior_packs", "development_resource_packs", "minecraftWorlds"));
            StructurePaths.Add(new DirectoryStructure("Short Hand", "behavior", "resource", "worlds"));

            // Read preferences file
            LoadSettings();

            // Read layoutfile
            if (LoadLayoutOnStart)
            {
                LoadLayout();
            }           
        }

        public void CreatePackHub()
        {
            PackHubs.Add(new PackHub(PackHubs.Count, new Thickness(450 * PackHubs.Count, 0, 0, 0), DefaultDirectory));
        }

        public void DeletePackHub(int id)
        {
            if (id > PackHubs.Count - 1) { MessageBox.Show("Tried to delete a packhub that does not exist. Id: " + id); return; }
            
            // Remove hub from list
            PackHubs.RemoveAt(id);

            for (int i = 0; i < PackHubs.Count; i++)
            {
                if (PackHubs[i].Id > id)
                {
                    PackHubs[i].Id--;
                }
            }

            // Wpf's Comboboxes act exactly the way I want them to when something is 
            // removed so no additional logic is needed. Nice!
         }

        public void CopyPack(int sourceId, int sourcePackType, int sourcePackIndex, int copyTargetId)
        {
            bool fileLockDetected = false;
            if (copyTargetId == -1) { return; }
            try
            {
                DirectoryInfo source = new DirectoryInfo(@"C:\");
                DirectoryInfo target = new DirectoryInfo(@"C:\");
                // This is hardcoded for behavior/resource/worlds. Might make this modular in the future.
                switch (sourcePackType)
                {
                    case 0:
                        source = new DirectoryInfo(PackHubs[sourceId].BPEntries[sourcePackIndex].Directory);
                        break;
                    case 1:
                        source = new DirectoryInfo(PackHubs[sourceId].RPEntries[sourcePackIndex].Directory);
                        break;
                    case 2:
                        source = new DirectoryInfo(PackHubs[sourceId].WorldEntries[sourcePackIndex].Directory);
                        break;
                    default:
                        break;
                }

                if (RecursiveIsFileLocked(source))
                {
                    fileLockDetected = true;
                }

                // Create target DirectoryInfo by appending baseDirectory and the correct structurePath
                string[] stringSeparators = new string[] { "\\" };
                string[] separatedPath = source.FullName.Split(stringSeparators, StringSplitOptions.None);
                string packName = separatedPath[separatedPath.Length - 1];
                string structurePath = "";
                switch (sourcePackType)
                {
                    case 0:
                        structurePath = StructurePaths[PackHubs[copyTargetId].StructureType].BPPath;
                        target = new DirectoryInfo(PackHubs[copyTargetId].BaseDirectory + "\\" + structurePath + "\\" + packName);
                        break;
                    case 1:
                        structurePath = StructurePaths[PackHubs[copyTargetId].StructureType].RPPath;
                        target = new DirectoryInfo(PackHubs[copyTargetId].BaseDirectory + "\\" + structurePath + "\\" + packName);
                        break;
                    case 2:
                        structurePath = StructurePaths[PackHubs[copyTargetId].StructureType].WorldPath;
                        target = new DirectoryInfo(PackHubs[copyTargetId].BaseDirectory + "\\" + structurePath + "\\" + packName);
                        break;
                    default:
                        break;
                }

                if (RecursiveIsFileLocked(target))
                {
                    fileLockDetected = true;
                }

                if (!fileLockDetected && target.Exists)
                {
                    // If backups are enabled on the target, create one.
                    if (PackHubs[copyTargetId].BackupEnabled && PackHubs[copyTargetId].BackupTarget != -1)
                    {
                        int backupTargetId = PackHubs[copyTargetId].BackupTarget;
                        DirectoryInfo backup = new DirectoryInfo(@"C:\");

                        switch (sourcePackType)
                        {
                            case 0:
                                structurePath = StructurePaths[PackHubs[backupTargetId].StructureType].BPPath;
                                backup = new DirectoryInfo(PackHubs[backupTargetId].BaseDirectory + "\\" + structurePath + "\\" + packName);
                                break;
                            case 1:
                                structurePath = StructurePaths[PackHubs[backupTargetId].StructureType].RPPath;
                                backup = new DirectoryInfo(PackHubs[backupTargetId].BaseDirectory + "\\" + structurePath + "\\" + packName);
                                break;
                            case 2:
                                structurePath = StructurePaths[PackHubs[backupTargetId].StructureType].WorldPath;
                                backup = new DirectoryInfo(PackHubs[backupTargetId].BaseDirectory + "\\" + structurePath + "\\" + packName);
                                break;
                            default:
                                break;
                        }

                        if (RecursiveIsFileLocked(backup) == false)
                        {
                            if (target.Exists && target.GetFiles().Length > 0)
                            {
                                if (backup.Exists) { SyncronousDelete(backup); }
                                Console.WriteLine(target.GetFiles().Length);
                                RecursiveCopy(target, backup);
                                PackHubs[backupTargetId].PopulateLists();
                            }
                        }
                        
                    }
                    SyncronousDelete(target);
                }
                if (fileLockDetected == false)
                {
                    RecursiveCopy(source, target);
                    PackHubs[copyTargetId].PopulateLists();
                }
                else
                {
                    showFileLockMessageBox(false, new Exception(""));
                }
            }
            catch (IOException e)
            {
                showFileLockMessageBox(true, e);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        public void DeletePack(int id, int packIndex)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure?", "Deletion Confirmation", MessageBoxButton.YesNo);
            if (messageBoxResult != MessageBoxResult.Yes) { return; }


            DirectoryInfo directory = new DirectoryInfo(@"C:\");
            // This is hardcoded for behavior/resource/worlds. Might make this modular in the future.
            switch (PackHubs[id].SelectedPackType)
            {
                case 0:
                    directory = new DirectoryInfo(PackHubs[id].BPEntries[packIndex].Directory);
                    //directory.Delete(true);
                    SyncronousDelete(directory);
                    break;
                case 1:
                    directory = new DirectoryInfo(PackHubs[id].RPEntries[packIndex].Directory);
                    //directory.Delete(true);
                    SyncronousDelete(directory);
                    break;
                case 2:
                    directory = new DirectoryInfo(PackHubs[id].WorldEntries[packIndex].Directory);
                    //directory.Delete(true);
                    SyncronousDelete(directory);
                    break;
                default:
                    return;
            }
            PackHubs[id].PopulateLists();
        }

        private void SyncronousDelete(DirectoryInfo directory)
        {
            //FileInfo file = new FileInfo(directory.FullName);
            //bool test = IsFileLocked(file);
            directory.Delete(true);
            
            for (int i = 0; i < 1000; i++)
            {
                if (!Directory.Exists(directory.FullName))
                {
                    //Console.WriteLine("PASSED AT " + i);
                    //Console.WriteLine(directory.FullName);
                    return;
                }
            }
            //Console.WriteLine("EXCEEDED");
            directory.Refresh();
        }

        public void ChainChangePackType(int id, int packType)
        {
            // This is to prevent the tab change event from triggering 
            // when this function changes the selectedIndex/SelectedPackType
            if (ignoreTabChange) { return; }
            ignoreTabChange = true;

            List<int> iterationList = new List<int>();
            List<int> nextIterationList = new List<int>();
            List<int> exemptionList = new List<int>();

            // Add Initial pack to process list
            iterationList.Add(id);

            // While iteration list is not empty 
            while (iterationList.Count > 0)
            {

                // Iterate through iterationList and add the targeted hubs to the next iteration list.
                for (int i = 0; i < iterationList.Count; i++)
                {
                    // Set selectedpacktype for all hubs in iteration list
                    PackHubs[iterationList[i]].SelectedPackType = packType;

                    // Add procesed packs to an exemption list 
                    exemptionList.Add(iterationList[i]);

                    // Also make sure targets are not on the exemptionList
                    bool shouldAddCopyTarget = true;
                    bool shouldAddBackupTarget = true;
                    for (int j = 0; j < exemptionList.Count; j++)
                    {
                        if (PackHubs[iterationList[i]].CopyTarget == exemptionList[j])
                        {
                            shouldAddCopyTarget = false;
                        }

                        if (!PackHubs[iterationList[i]].BackupEnabled || PackHubs[iterationList[i]].BackupTarget == exemptionList[j])
                        {
                            shouldAddBackupTarget = false;
                        }

                        if (!shouldAddCopyTarget && !shouldAddBackupTarget)
                        {
                            break;
                        }
                    }
                    if (shouldAddCopyTarget)
                    {
                        nextIterationList.Add(PackHubs[iterationList[i]].CopyTarget);
                    }
                    if (shouldAddBackupTarget)
                    {
                        nextIterationList.Add(PackHubs[iterationList[i]].BackupTarget);
                    }

                }
                // Make iterationList equal to nextIterationList and then clear nextIterationList
                iterationList.Clear();
                for (int i = 0; i < nextIterationList.Count; i++)
                {
                    iterationList.Add(nextIterationList[i]);
                }
                nextIterationList.Clear();
            }

            ignoreTabChange = false;
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
                Console.WriteLine(target.FullName + " DOES NOT EXIST");
                Directory.CreateDirectory(target.FullName);
            }

            // Copy each file into it's new directory.
            foreach (FileInfo fi in source.GetFiles())
            {
                try
                {
                    //Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
                    //String temp = Path.Combine(target.ToString(), fi.Name);
                    if (!Directory.Exists(target.FullName))
                    {
                        Console.WriteLine("BAD");
                    }
                    fi.CopyTo(Path.Combine(target.ToString(), fi.Name), true);
                }
                catch (System.IO.DirectoryNotFoundException e)
                {
                    Console.Write(e.ToString());
                }
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                RecursiveCopy(diSourceSubDir, nextTargetSubDir);
            }
        }

        private bool RecursiveIsFileLocked(DirectoryInfo directory)
        {
            // If the directory does not exist, it cannot be locked
            directory.Refresh();
            if (!directory.Exists)
            {
                return false;
            }
            //DirectoryInfo test = new DirectoryInfo(directory.FullName);

            // Check files in directory
            foreach (FileInfo fi in directory.GetFiles())
            {
                if (IsFileLocked(fi))
                {
                    return true;
                }
            }

            foreach (DirectoryInfo subDir in directory.GetDirectories())
            {
                RecursiveIsFileLocked(subDir);
            }
            return false;
        }

        private bool IsFileLocked(FileInfo file)
        {
            //https://stackoverflow.com/questions/876473/is-there-a-way-to-check-if-a-file-is-in-use
            try
            {
                using (FileStream stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    stream.Close();
                }
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            catch (System.UnauthorizedAccessException e)
            {
                return true;
            }

            //file is not locked
            return false;
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
                MessageBoxResult messageBox = MessageBox.Show("Packmule failed to load settings file. It will continue with default settings. This is normal if you have not saved any settings.", "Settings Failure", MessageBoxButton.OK);
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
                MessageBoxResult messageBox = MessageBox.Show("Packmule failed to load layout file. It will continue with an empty layout. This is normal if you have not saved a layout.", "Settings Failure", MessageBoxButton.OK);

            }
        }

        private void showFileLockMessageBox(bool hasExceptionData, Exception e)
        {
            String message = "Packmule has detected file lock in the operation directories and aborted the operation. Please check other programs and try again.";
            if (hasExceptionData)
            {
                message = message + " Here is the exception.\n\n" + e.ToString();
            }
            MessageBox.Show(message);
        }
    }
}
