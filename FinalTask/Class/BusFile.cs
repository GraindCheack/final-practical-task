using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;

namespace FinalTask.Class
{
    class BusFile : INotifyPropertyChanged
    {
        public static Class.BusFile activeBusFile = Class.ApplicationViewModel.appViewModel.ActiveBusFile;

        private string pathToFile;
        private string fileName;
        private int scrollPage = 1;
        private BusWay activeWay;
        public ObservableCollection<BusWay> BusWays { get; set; }

        [NonSerialized]
        private int number;

        private int amount;

        public BusFile()
        {
            BusWays = new ObservableCollection<BusWay>();
        }

        public BusFile(int number)
        {
            BusWays = new ObservableCollection<BusWay>();
            this.number = number;
            if (BusWays != null)
            {
                amount = BusWays.Count;
            }
        }

        public BusWay ActiveWay
        {
            get { return activeWay; }
            set
            {
                activeWay = value;
                OnPropertyChanged("ActiveWay");
            }
        }

        public string PathToFile
        {
            get { return pathToFile; }
            set
            {
                pathToFile = value;
                OnPropertyChanged("PathToFile");
            }
        }

        public string FileName
        {
            get { return fileName; }
            set
            {
                fileName = value;
                OnPropertyChanged("FileName");
            }
        }

        public int Number
        {
            get { return number; }
            set
            {
                number = value;
                OnPropertyChanged("Number");
            }
        }

        public int Amount
        {
            get { return amount; }
            set
            {
                amount = value;
                OnPropertyChanged("Amount");
            }
        }

        public int ScrollPage
        {
            get { return scrollPage; }
            set
            {
                scrollPage = value;
                OnPropertyChanged("ScrollPage");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
    }
}
