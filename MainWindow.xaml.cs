using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.Configuration;
using System.Data;
using System.Data.SQLite;

namespace bazagpr
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SQLiteConnection con = null;
        public MainWindow()
        {
            SetConnection();
            InitializeComponent();
        }

        private void SetConnection()
        {
            String connectionString = ConfigurationManager.ConnectionStrings["bazagpr.Properties.Settings.ConnectionStringBazaGPR"].ConnectionString;
            con = new SQLiteConnection(connectionString);
            try
            {
                con.Open();
            }
            catch (Exception exp)
            {
                MessageBox.Show("Błąd połączenia!!! " + exp.Message);
                Application.Current.Shutdown();
            }
        }
        //Window_Loaded pokazuje komunikat, że aplikacja została wczytana poprawnie i zaczytuje dane do grida.
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Loaded");
            this.FillDataGrid();
            //this.ComboBox_SelectionChanged();
        }

        //Window_Closed tylko pokazuje komunikat po zamknięciu aplikacji, że została zamknięta.
        /*
        private void Window_Closed(object sender, EventArgs e)
        {
            MessageBox.Show("Closed");
        }
        */
        private void FillDataGrid() //pokazuje wszystko, bez filtracji
        {
            SQLiteCommand cmd = con.CreateCommand();
            cmd.CommandText = "select id_prof, Typ_prof, Nazwa, RD3 from Dane"; 
            cmd.CommandType = CommandType.Text;
            SQLiteDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            GPRDataGrid.ItemsSource = dt.DefaultView;
            dr.Close();
        }

        private void Refresh_btn_Click(object sender, RoutedEventArgs e) //Odśwież
        {
            this.FillDataGrid();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SQLiteCommand cmd = con.CreateCommand();
            cmd.CommandText = "select Typ from Typ_prof";
            cmd.CommandType = CommandType.Text;
            SQLiteDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                tp_cmbbx.Items.Add(dr[0]);
            }
        }



        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e) //wrażliwość na zmiany na filtrach
        {
            DataGrid dg = sender as DataGrid;
            DataRowView dr = dg.SelectedItem as DataRowView;
            if (dr != null)
            {
                nazwa_txtbx.Text = dr["Nazwa"].ToString();
                tp_cmbbx.Text = dr["Typ"].ToString();
                data_date_picker.SelectedDate = DateTime.Parse(dr["Data"].ToString());
            }
        }

        private void ResetForm()
        {
            nazwa_txtbx.Text = "";
            tp_cmbbx.Text = "";
            data_date_picker.SelectedDate = null;
        }

        private void Reset_Button_Click(object sender, RoutedEventArgs e)
        {
            this.ResetForm();
        }

        //Otworzeie okna z profilami
        private void OpenProfile_btn_Click(object sender, RoutedEventArgs e)
        {
            Profile to = new Profile();
            to.ShowDialog();
        }
        //Otworzenie okna z projektami
        private void OpenProj_btn_Click(object sender, RoutedEventArgs e)
        {
            OpenProjekt to = new OpenProjekt();
            to.ShowDialog();
        }


        /*private void FillTpComboBox()
        {
            //string SQLLite = "select Typ from Typ_prof";
            SQLiteCommand cmd = con.CreateCommand();
            cmd.CommandText = "select Typ from Typ_prof";
            cmd.CommandType = CommandType.Text;
            SQLiteDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                tp_cmbbx.Items.Add(dr[0]);
            }
           
            DataTable dt = new DataTable();
            dt.Load(dr);
            GPRDataGrid.ItemsSource = dt.DefaultView;
            dr.Close();
            string[] typy = new string[] { "Typical", "Compact", "Custom" };
            ComboBox1.Items.AddRange(installs);
        }*/
    }
}
