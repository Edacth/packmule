using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace packmule
{
    public class DirectoryStructure
    {
        public string BPPath { get; set; }
        public string RPPath { get; set; }
        public string worldPath { get; set; }

        public DirectoryStructure(string _BPPath, string _RPPath, string _worldPath)
        {
            BPPath = _BPPath;
            RPPath = _RPPath;
            worldPath = _worldPath;
        }
    }

    
}
