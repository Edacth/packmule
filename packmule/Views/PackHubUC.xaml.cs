using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace packmule
{
    /// <summary>
    /// Interaction logic for PackHub.xaml
    /// </summary>
    public partial class PackHubUC : UserControl
    {
        public PackHubUC()
        {
            InitializeComponent();
            
        }

        // This shit is magic
        // https://stackoverflow.com/questions/3067617/raising-an-event-on-parent-window-from-a-user-control-in-net-c-sharp
        public static readonly RoutedEvent MouseDragEvent =
            EventManager.RegisterRoutedEvent("MouseDragEvent", RoutingStrategy.Bubble,
                typeof(RoutedEvent), typeof(PackHubUC));

        public static readonly RoutedEvent SetDragStartPointEvent =
            EventManager.RegisterRoutedEvent("SetDragStartPointEvent", RoutingStrategy.Bubble,
                typeof(RoutedEvent), typeof(PackHubUC));

        public event RoutedEventHandler MouseDrag
        {
            add { AddHandler(MouseDragEvent, value); }
            remove { RemoveHandler(MouseDragEvent, value); }
        }

        public event RoutedEventHandler SetDragStartPoint
        {
            add { AddHandler(SetDragStartPointEvent, value); }
            remove { RemoveHandler(SetDragStartPointEvent, value); }
        }

        private void OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(SetDragStartPointEvent, e));
        }

        private void OnPreviewMouseMove(object sender, MouseEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(MouseDragEvent, e));
        }
    }
}
