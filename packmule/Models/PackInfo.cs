using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace packmule.Models
{
    public class PackInfo
    {
        public int format_version { get; set; }
        public Header header { get; set; }
        public List<Module> modules { get; set; }
        public List<Dependency> dependencies { get; set; }
    }
    public class Header
    {
        public string description { get; set; }
        public string name { get; set; }
        public string uuid { get; set; }
        public List<int> version { get; set; }
        public List<int> min_engine_version { get; set; }
    }

    public class Module
    {
        public string type { get; set; }
        public string uuid { get; set; }
        public List<int> version { get; set; }
    }

    public class Dependency
    {
        public string uuid { get; set; }
        public List<int> version { get; set; }
    }

}
