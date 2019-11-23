﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.ComponentModel;
using System.Collections.ObjectModel;
using packmule.Models;

using Thickness = System.Windows.Thickness;

namespace packmule.ViewModels
{
    public class ViewModel : ViewModelBase
    {

        private string _title;
        public string Title { get => _title; set => SetProperty(ref _title, value); }

        ObservableCollection<PackHub> _packHubs = new ObservableCollection<PackHub>();
        public ObservableCollection<PackHub> PackHubs { get => _packHubs; }

        public int PackHubsCount { get => _packHubs.Count; }
        public ViewModel()
        {

        }

        public void CreatePackHub()
        {
            PackHubs.Add(new PackHub(PackHubs.Count, new Thickness(450 * PackHubs.Count, 0, 0, 0)));
        }

        public void PHSetPosition(int id, Thickness newPosition)
        {

            PackHubs[id].Position = newPosition;
            Console.WriteLine(id);
        }

        public void PHTranslate(int id, Thickness translationAmount)
        {
            Thickness currentPos = PackHubs[id].Position;
            Thickness newPos = new Thickness(currentPos.Left + translationAmount.Left,
                                             currentPos.Top + translationAmount.Top,
                                             currentPos.Right + translationAmount.Right,
                                             currentPos.Bottom + translationAmount.Bottom);
            PackHubs[id].Position = newPos;
        }
    }
}
