using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;

namespace FinalTask.Class
{
    class ApplicationViewModel : INotifyPropertyChanged
    {
        public static Class.ApplicationViewModel appViewModel = new ApplicationViewModel();

        private BusFile activeBusFile;
        public ObservableCollection<BusFile> BusFiles { get; set; }

        public ApplicationViewModel()
        {
            BusFiles = new ObservableCollection<BusFile>();
        }

        public BusFile ActiveBusFile
        {
            get { return activeBusFile; }
            set
            {
                activeBusFile = value;
                OnPropertyChanged("ActiveBusFile");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
