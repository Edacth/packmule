using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace packmule.Models
{
    public class PackInfo
    {
        public string Directory { get; set; }
        public int Index { get; set; }
        public string IconPath { get; set; }
        public ImageSource IconSource { get; set; }

        public int format_version { get; set; }
        public Header header { get; set; }
        public List<Module> modules { get; set; }
        public List<Dependency> dependencies { get; set; }

        public PackInfo()
        {
            header = new Header();
        }

        public void LoadIcon()
        {
            // TODO: Loads pack icon
            try
            {
                BitmapImage icon = new BitmapImage();
                icon.BeginInit();
                icon.CacheOption = BitmapCacheOption.OnLoad;
                icon.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                icon.UriSource = new Uri(IconPath);
                icon.EndInit();

                IconSource = icon;
            }
            catch
            {
                BitmapSource icon = BitmapImage.Create(
                2,
                2,
                96,
                96,
                PixelFormats.Indexed1,
                new BitmapPalette(new List<System.Windows.Media.Color> { Colors.Transparent }),
                new byte[] { 0, 0, 0, 0 },
                1);

                IconSource = icon;
            }
        }
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
