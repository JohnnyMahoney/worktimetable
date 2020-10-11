using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace Desktop_App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        WorkVM m_viewModel = new WorkVM();
        public MainWindow()
        {
            InitializeComponent();

            m_viewModel.GetConnection();
            m_viewModel.LoadEntries();


        }

        public void WireUp()
        {
            cal.SelectedDate = m_viewModel.SelectedDate;
        }

        private void txtBox_Stunde_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            TextBox t1 = (TextBox)sender;
            Regex regex2 = new Regex("^([0-2]?[0-9])$");
            if (t1.Text.Length == 1)
            {
                string time = string.Format("{0}{1}", t1.Text, e.Text);
                e.Handled = !regex2.IsMatch(time);
                return;
            }
            if (t1.Text.Length > 1)
            {
                e.Handled = true;
                return;
            }
            e.Handled = regex.IsMatch(e.Text);
        }

        private void txtBox_Minute_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            TextBox t1 = (TextBox)sender;
            Regex regex2 = new Regex("^([0-5]?[0-9])$");
            if (t1.Text.Length == 1)
            {
                string time = string.Format("{0}{1}", t1.Text, e.Text);
                e.Handled = !regex2.IsMatch(time);
                return;
            }
            if (t1.Text.Length > 1)
            {
                e.Handled = true;
                return;
            }
            e.Handled = regex.IsMatch(e.Text);
        }

        private void cal_DisplayDateChanged(object sender, CalendarDateChangedEventArgs e)
        {
            m_viewModel.LoadEntries();
        }

        private void btn_SaveTime_Click(object sender, RoutedEventArgs e)
        {
            m_viewModel.SaveEntries();
        }

        private void MenuItem_OpenDB_Click(object sender, RoutedEventArgs e)
        {
            m_viewModel.GetConnection();
        }
    }
}
