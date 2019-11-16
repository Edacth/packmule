using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace packmule.Models
{
    class PackHub
    {
        string _baseDirectory;
        public string BaseDirectory { get; set; }
        List<PackInfo> entries;

    }

    class PackInfo
    {
        public string Directory { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
