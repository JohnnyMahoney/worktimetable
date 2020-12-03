using Dapper;
using Microsoft.Win32;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Desktop_App
{
    public class WorkVM : INotifyPropertyChanged
    {
        private SQLiteConnection dbConnection;
        private DateTime selectedDate = DateTime.Now;
        private DateTime workStart;
        private DateTime workEnd;
        private TimeSpan breakTime;
        private string comment;
        private ObservableCollection<WorkEntry> displayEntries = new();

        public event PropertyChangedEventHandler PropertyChanged;
        protected void Raise([CallerMemberName] string propName = null)
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
                Raise();
            }
        }
        public DateTime SelectedDate
        {
            get { return selectedDate; }
            set
            {
                if (selectedDate == value) return;
                selectedDate = value;
                Raise();
                LoadEntries();
            }
        }
        public DateTime WorkStart
        {
            get { return workStart; }
            set
            {
                if (workStart == value) return;
                workStart = value;
                Raise();
            }
        }
        public DateTime WorkEnd
        {
            get { return workEnd; }
            set
            {
                if (workEnd == value) return;
                workEnd = value;
                Raise();
            }
        }
        public TimeSpan BreakTime
        {
            get { return breakTime; }
            set
            {
                if (breakTime == value) return;
                breakTime = value;
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
        public WorkEntry SelectedEntry { get; set; }

        public string HoursThisMonth
        {
            get
            {
                TimeSpan sum = new TimeSpan();
                foreach (var entry in DisplayEntries)
                {
                    sum += entry.Worked;
                }

                return sum.ToString();
            }
        }




        public void GetConnection()
        {
            var opf = new OpenFileDialog
            {
                InitialDirectory = Environment.CurrentDirectory,
                Filter = "Database Files (*.db)|*.db"
            };

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
            DisplayEntries.Clear();
            string queryString = $"SELECT * FROM history WHERE begin BETWEEN datetime('{SelectedDate:s}','start of month') AND datetime('{SelectedDate:s}','start of month','+1 month','-1 day')";
            dbConnection?.Open();
            using (var comm = new SQLiteCommand(queryString, dbConnection))
            {
                using var read = comm.ExecuteReader();
                while (read.Read())
                {
                    WorkEntry _ = new WorkEntry
                    {
                        Begin = read.GetDateTime(0),
                        End = read.GetDateTime(1),
                        ID = read.GetInt32(4)
                    };

                    string breakTime = read.GetString(2);
                    _.Break = new TimeSpan(int.Parse(breakTime.Split(":")[0]), int.Parse(breakTime.Split(":")[1]), int.Parse(breakTime.Split(":")[2]));
                    _.Worked = _.End - _.Begin - _.Break;
                    _.Comment = read.GetString(3);
                    DisplayEntries.Add(_);
                }
            }
            dbConnection.Close();
            Raise(nameof(HoursThisMonth));
        }

        public void ChangeEntries()
        {
        }

        public void DeleteEntries()
        {
            string insertString = $"DELETE FROM history WHERE id = {SelectedEntry?.ID}";
            dbConnection?.Execute(insertString);
            LoadEntries();
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
