using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace packmule
{
    public class DirectoryStructure
    {
        public string Name { get; set; }
        public string BPPath { get; set; }
        public string RPPath { get; set; }
        public string WorldPath { get; set; }

        public DirectoryStructure(string _Name, string _BPPath, string _RPPath, string _WorldPath)
        {
            Name = _Name;
            BPPath = _BPPath;
            RPPath = _RPPath;
            WorldPath = _WorldPath;
        }
    }

    
}
