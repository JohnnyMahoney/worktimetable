using System;
using System.ComponentModel;

namespace Desktop_App
{
    public class WorkEntry : INotifyPropertyChanged
    {
        private DateTime begin;
        private DateTime end;
        private TimeSpan @break;
        private string comment;

        public DateTime Begin
        {
            get { return begin; }
            set
            {
                if (begin == value) return;
                begin = value;
                Raise("Begin");
            }
        }
        public DateTime End
        {
            get { return end; }
            set
            {
                if (end == value) return;
                end = value;
                Raise("End");
            }
        }
        public TimeSpan Break
        {
            get { return @break; }
            set
            {
                if (@break == value) return;
                @break = value;
                Raise("Break");
            }
        }
        public string Comment
        {
            get { return comment; }
            set
            {
                if (comment == value) return;
                comment = value;
                Raise("Comment");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void Raise(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }


    }
}
