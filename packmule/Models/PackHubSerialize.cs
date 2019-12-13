using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace packmule.Models
{
    class PackHubSerialize
    {
        public int Id { get; set; }
        public Thickness Position { get; set; }
        public string Title { get; set; }
        public int StructureType { get; set; }
        public string BaseDirectory { get; set; }
        public int CopyTarget { get; set; }
        public bool BackupEnabled { get; set; }
        public int BackupTarget { get; set; }

    }
}
