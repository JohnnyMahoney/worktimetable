using System;
using System.Collections.ObjectModel;
using System.Data;

namespace Desktop_App
{
    public class WorkVM
    {
        IDbConnection dbConnection;
        public ObservableCollection<WorkEntry> DisplayEntries { get; set; }
        public DateTime SelectedDate { get; set; } = DateTime.Now;


        public void GetConnection()
        {

        }
        public void LoadEntries()
        {

        }
        public void ChangeEntries()
        {

        }
        public void SaveEntries()
        {

        }
    }
}
