using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Data;

namespace Desktop_App
{
    public class WorkEntry : INotifyPropertyChanged
    {
        private DateTime begin;
        private DateTime end;
        private TimeSpan @break;
        private string comment;
        private TimeSpan worked;

        public DateTime Begin
        {
            get { return begin; }
            set
            {
                if (begin == value) return;
                begin = value;
                Raise();
            }
        }
        public DateTime End
        {
            get { return end; }
            set
            {
                if (end == value) return;
                end = value;
                Raise();
            }
        }
        public TimeSpan Break
        {
            get { return @break; }
            set
            {
                if (@break == value) return;
                @break = value;
                Raise();
            }
        }
        public string Comment
        {
            get { return comment; }
            set
            {
                if (comment == value) return;
                comment = value;
                Raise();
            }
        }
        public TimeSpan Worked
        {
            get { return worked; }
            set
            {
                if (worked == value) return;
                worked = value;
                Raise();
            }
        }

        public int ID { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void Raise([CallerMemberName] string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
