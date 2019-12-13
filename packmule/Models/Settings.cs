using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace packmule.Models
{
    class Settings
    {
        public bool SaveLayoutOnClose { get; set; }
        public bool LoadLayoutOnStart { get; set; }
        public string DefaultDirectory { get; set; }
        public PackHubSerialize[] PHSerializes { get; set; }
    }
}
