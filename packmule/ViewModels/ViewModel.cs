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

using Thickness = System.Windows.Thickness;

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
        #region StructurePaths
        static ObservableCollection<DirectoryStructure> _structurePaths = new ObservableCollection<DirectoryStructure>();
        public static ObservableCollection<DirectoryStructure> StructurePaths { get => _structurePaths; }
        #endregion

        public ViewModel()
        {
            // TODO: Have this read from a config file and populate StructurePaths that way
            StructurePaths.Add(new DirectoryStructure("ComMojang", "development_behavior_packs", "development_resource_packs", "minecraftWorlds"));
            StructurePaths.Add(new DirectoryStructure("Short Hand", "behavior", "resource", "worlds"));
        }

        public void CreatePackHub()
        {
            PackHubs.Add(new PackHub(PackHubs.Count, new Thickness(450 * PackHubs.Count, 0, 0, 0)));
        }

        public void CopyPack(int sourceId, int packIndex, int destinationId )
        {
            throw new NotImplementedException();
        }

        public void DeletePack(int id, int packType, int packIndex)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure?", "Deletion Confirmation", MessageBoxButton.YesNo);
            if (messageBoxResult != MessageBoxResult.Yes) { return; }
            DirectoryInfo directory = new DirectoryInfo(PackHubs[id].BPEntries[packIndex].Directory);
            directory.Delete(true);
        }

        private void RecursiveCopy(DirectoryInfo source, DirectoryInfo target)
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
    }
}
