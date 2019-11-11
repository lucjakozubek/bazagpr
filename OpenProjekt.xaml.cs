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
    /// Logika interakcji dla klasy OpenProjekt.xaml
    /// </summary>
    public partial class OpenProjekt : Window
    {
        SQLiteConnection con = null;
        public OpenProjekt()
        {
            SetConnection();
            InitializeComponent();
            this.ProjFillDataGrid(); //To od razu ładuje DataGrid
        }

        //Stworzenie połączenia z bazą
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

        private void Window_Loaded(object sender, RoutedEventArgs e) //Tutaj nic nic robi
        {
            this.ProjFillDataGrid();
            //this.ComboBox_SelectionChanged();
        }

        private void ProjFillDataGrid() //pokazuje wszystko, bez filtracji
        {
            SQLiteCommand cmd = con.CreateCommand();
            cmd.CommandText = "select Nazwa_proj AS [Nazwa projektu], cast(data as text) AS Data, Wojewodztwo AS [Województwo], Miejscowosc AS [Miejscowość], Adres, Opis_miejsca AS [Opis miejsca], Prowadzacy AS [Prowadzący], Pogoda, War_geol AS [Warunki geologiczne], Zleceniodawca, Uwagi from Projekt INNER JOIN Wojewodztwo ON wojewodztwo.id_woj=projekt.id_woj"; //ma problem z pokazaniem daty
            //Data jest wyżej zamieniona na tekst, bo inaczej nie chciał czytać
            cmd.CommandType = CommandType.Text;
            SQLiteDataReader dr = cmd.ExecuteReader();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Load(dr);
            ProjGPRDataGrid.ItemsSource = dt.DefaultView;
            dr.Close();
        }
        private void ResetForm()
        {
            nazwaproj_txtbx.Text = "";
            tpproj_cmbbx.Text = "";
            dataproj_date_picker.SelectedDate = null;
            //addproj_btn.IsEnabled = true;
            //modproj_btn.IsEnabled = false;
            //delproj_btn.IsEnabled = false;
        }

        private void Refreshproj_btn_Click(object sender, RoutedEventArgs e)
        {
            this.ProjFillDataGrid();
        }

        private void Resetproj_Button_Click(object sender, RoutedEventArgs e)
        {
            this.ResetForm();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Addproj_btn_Click(object sender, RoutedEventArgs e)
        {
            string sqlite = @"insert Projekt(Nazwa_proj, Data, Adres, Opis_miejsca, Miejscowosc, Id_woj, Prowadzacy, Pogoda, War_geol, Zleceniodawca, Uwagi) " +
                "VALUES(@nazwa, @data, @adres, @opis, @miejscowosc, @id_woj, @prowadzacy, @pogoda, @war_geol, @zleceiodawca, @uwagi)";
            SQLiteCommand cmd = con.CreateCommand();
            cmd.CommandText = sqlite;
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("nazwa", DbType.String, 100).Value = nazwaproj_txtbx.Text;
            //próbuję dodać

        }
    }
}
