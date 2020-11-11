using Dapper;
using Microsoft.Win32;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Windows;

namespace Desktop_App
{
    public class WorkVM : INotifyPropertyChanged
    {
        private IDbConnection dbConnection;
        private DateTime selectedDate = DateTime.Now;
        private DateTime workStart;
        private DateTime workEnd;
        private TimeSpan breakTime;
        private string comment;
        private ObservableCollection<WorkEntry> displayEntries = new ObservableCollection<WorkEntry>();

        public event PropertyChangedEventHandler PropertyChanged;
        protected void Raise(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }


        public WorkVM()
        {
            var path = Environment.CurrentDirectory;
            dbConnection = new SQLiteConnection($"Data Source={path}\\workhistory.db;Version=3;");
            LoadEntries();
        }

        public ObservableCollection<WorkEntry> DisplayEntries
        {
            get { return displayEntries; }
            set
            {
                if (displayEntries == value) return;
                displayEntries = value;
                Raise("DisplayEntries");
            }
        }
        public DateTime SelectedDate
        {
            get { return selectedDate; }
            set
            {
                if (selectedDate == value) return;
                selectedDate = value;
                Raise("SelectedDate");
            }
        }
        public DateTime WorkStart
        {
            get { return workStart; }
            set
            {
                if (workStart == value) return;
                workStart = value;
                Raise("WorkStart");
            }
        }
        public DateTime WorkEnd
        {
            get { return workEnd; }
            set
            {
                if (workEnd == value) return;
                workEnd = value;
                Raise("WorkEnd");
            }
        }
        public TimeSpan BreakTime
        {
            get { return breakTime; }
            set
            {
                if (breakTime == value) return;
                breakTime = value;
                Raise("BreakTime");
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



        public void GetConnection()
        {
            var opf = new OpenFileDialog();
            opf.InitialDirectory = Environment.CurrentDirectory;
            opf.Filter = "Database Files (*.db)|*.db";
            if (opf.ShowDialog() == true)
            {
                string connectionString = $"Data Source={opf.FileName};Version=3;";
                dbConnection = new SQLiteConnection(connectionString);
                LoadEntries();
            }
            else
            {
                MessageBox.Show("Fehler beim Öffnen der Datei.", "Error");
            }
        }


        public void LoadEntries()
        {
            string queryString = $"SELECT * FROM history WHERE begin BETWEEN date('{SelectedDate:s}','start of month') AND date('{SelectedDate:s}','start of month','+1 month','-1 day')";
            (dbConnection?.Query<WorkEntry>(queryString) ?? new List<WorkEntry>()).ToList().ForEach(x=> DisplayEntries.Add(x));
        }
        public void ChangeEntries()
        {

        }
        public void SaveEntries(DateTime start, DateTime end, TimeSpan breakTime)
        {
            WorkStart = start;
            WorkEnd = end;
            BreakTime = breakTime;

            string insertString = $"REPLACE INTO history (begin, end, break, comment) VALUES (datetime('{WorkStart:s}'),datetime('{WorkEnd:s}'),time('{BreakTime:c}'),'{Comment}')";
            dbConnection?.Execute(insertString);
            LoadEntries();

        }


    }
}
