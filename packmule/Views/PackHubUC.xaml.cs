﻿using System;
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

        public static readonly RoutedEvent ChainChangePackTypeEvent =
            EventManager.RegisterRoutedEvent("ChainChangePackType", RoutingStrategy.Bubble,
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

        public event RoutedEventHandler ChainChangePackType
        {
            add { AddHandler(ChainChangePackTypeEvent, value);  }
            remove { RemoveHandler(ChainChangePackTypeEvent, value); }
        }

        private void OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            RaiseEvent( new DragEventArgs(SetDragStartPointEvent, (DataContext as packmule.Models.PackHub).Id, e));
            //Console.WriteLine("LeftButtonDown");
        }

        private void OnPreviewMouseMove(object sender, MouseEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(MouseDragEvent, e));
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((DataContext as packmule.Models.PackHub).Initialized)
            {
                RaiseEvent(new SelectionEventArgs(ChainChangePackTypeEvent, (DataContext as packmule.Models.PackHub).Id, (sender as TabControl).SelectedIndex));
            }
            else
            {
                (DataContext as packmule.Models.PackHub).Initialized = true;
            }
        }
    }

    // Creating my own args because I need to pass along addtional data
    public class DragEventArgs : RoutedEventArgs
    {
        private readonly int _id;
        private readonly MouseButtonEventArgs _mouseArgs;

        public int Id
        {
            get { return _id; }
        }

        public MouseButtonEventArgs MouseArgs
        {
            get { return _mouseArgs; }
        }

        public DragEventArgs(RoutedEvent routedEvent, int _id, MouseButtonEventArgs _mouseArgs) : base(routedEvent)
        {
            this._id = _id;
            this._mouseArgs = _mouseArgs;
        }
    }

    public class SelectionEventArgs : RoutedEventArgs
    {
        private readonly int _id;
        private readonly int _index;

        public int Id
        {
            get { return _id; }
        }

        public int Index
        {
            get { return _index; }
        }

        public SelectionEventArgs(RoutedEvent routedEvent, int _id, int _index) : base(routedEvent)
        {
            this._id = _id;
            this._index = _index;
        }
    }
}