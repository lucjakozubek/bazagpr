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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.ProjFillDataGrid();
            //this.ComboBox_SelectionChanged();
        }

        private void ProjFillDataGrid() //pokazuje wszystko, bez filtracji
        {
            SQLiteCommand cmd = con.CreateCommand();
            cmd.CommandText = "select Nazwa_proj AS [Nazwa projektu], Wojewodztwo AS [Województwo], Miejscowosc AS [Miejscowość], Adres, Opis_miejsca AS [Opis miejsca], Prowadzacy AS [Prowadzący], Pogoda, War_geol AS [Warunki geologiczne], Zleceniodawca, Uwagi from Projekt INNER JOIN Wojewodztwo ON wojewodztwo.id_woj=projekt.id_woj"; //ma problem z pokazaniem daty
            cmd.CommandType = CommandType.Text;
            SQLiteDataReader dr = cmd.ExecuteReader();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Load(dr);
            ProjGPRDataGrid.ItemsSource = dt.DefaultView;
            dr.Close();
        }

        private void Refreshproj_btn_Click(object sender, RoutedEventArgs e)
        {
            this.ProjFillDataGrid();
        }

        private void Resetproj_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
