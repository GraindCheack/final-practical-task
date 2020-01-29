using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FinalTask.Class
{
    class BusWay : INotifyPropertyChanged
    {
        private string number;
        private string type;
        private string wayStart;
        private string wayStop;
        private DateTime dateDeparture = DateTime.Now;
        private DateTime dateArrival = DateTime.Now;

        [NonSerialized]
        private DateTime dateCreated = DateTime.Now;

        public BusWay() { }

        public BusWay(string number, string type, string wayStop, DateTime dateDep, DateTime dateArriv) 
        {
            this.number = number;
            this.type = type;
            this.wayStop = wayStop;
            this.dateDeparture = dateDep;
            this.dateArrival = dateArriv;
        }

        public string Number
        {
            get { return number; }
            set
            {
                number = value;
                OnPropertyChanged("Number");
            }
        }

        public string Type
        {
            get { return type; }
            set
            {
                type = value;
                OnPropertyChanged("Type");
            }
        }

        public string WayStart
        {
            get { return wayStart; }
            set
            {
                wayStart = value;
                OnPropertyChanged("WayStart");
            }
        }

        public string WayStop
        {
            get { return wayStop; }
            set
            {
                wayStop = value;
                OnPropertyChanged("WayStop");
            }
        }
        public string DateDeparture
        {
            get { return dateDeparture.ToString("dd:MM:yyyy"); }
            set
            {
                dateDeparture = DateTime.Parse(value.Replace(":", "."));
                OnPropertyChanged("DateDeparture");
            }
        }

        public string DateArrival
        {
            get { return dateArrival.ToString("dd:MM:yyyy"); }
            set
            {
                dateArrival = DateTime.Parse(value.Replace(":", ".")); 
                OnPropertyChanged("DateArrival");
            }
        }

        public DateTime DateDepartureSpec
        {
            get { return dateDeparture; }
            set
            {
                dateDeparture = DateTime.Parse(value.ToString("dd.MM.yyyy ") + TimeDeparture); ;
                OnPropertyChanged("DateDepartureSpec");
                OnPropertyChanged("DateDeparture");
            }
        }

        public DateTime DateArrivalSpec
        {
            get { return dateArrival; }
            set
            {
                dateArrival = DateTime.Parse(value.ToString("dd.MM.yyyy ") + TimeArrival);
                OnPropertyChanged("DateArrivalSpec");
                OnPropertyChanged("DateArrival");
            }
        }

        public string TimeDeparture
        {
            get { return dateDeparture.ToString("hh:mm tt", System.Globalization.CultureInfo.InvariantCulture); }
            set
            {
                dateDeparture = DateTime.Parse(dateDeparture.Day + "." + dateDeparture.Month + "." + dateDeparture.Year + " " + value);
                OnPropertyChanged("TimeDeparture");
            }
        }

        public string TimeArrival
        {
            get { return dateArrival.ToString("hh:mm tt", System.Globalization.CultureInfo.InvariantCulture); }
            set
            {
                dateArrival = DateTime.Parse(dateArrival.Day + "." + dateArrival.Month + "." + dateArrival.Year + " " + value);
                OnPropertyChanged("TimeArrival");
            }
        }

        public string DateCreated
        {
            get { return dateCreated.ToString("dd.MM.yyyy hh:mm"); }
            set
            {
                dateCreated = DateTime.Parse(value);
                OnPropertyChanged("DateCreated");
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
