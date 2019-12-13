using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace packmule.Models
{
    public class PackHubSerialize
    {
        public int Id { get; set; }
        public Thickness Position { get; set; }
        public string Title { get; set; }
        public int StructureType { get; set; }
        public string BaseDirectory { get; set; }
        public int CopyTarget { get; set; }
        public bool BackupEnabled { get; set; }
        public int BackupTarget { get; set; }

        public PackHubSerialize()
        {
        }

        public PackHubSerialize(PackHub packHub)
        {
            Id = packHub.Id;
            Position = packHub.Position;
            Title = packHub.Title;
            StructureType = packHub.StructureType;
            BaseDirectory = packHub.BaseDirectory;
            CopyTarget = packHub.CopyTarget;
            BackupEnabled = packHub.BackupEnabled;
            BackupTarget = packHub.BackupTarget;
        }
    }
}
