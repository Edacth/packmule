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
    public partial class PackHub : UserControl
    {
        public PackHub()
        {
            InitializeComponent();
            Title = "WHAT";
        }
        public string Title { get; set; }

        public string ID { get; set; }
    }
}
