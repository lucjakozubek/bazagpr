using System;
using System.IO;
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
        /// <summary>
        /// Sprawdza, czy istnieje baza danyc i łączy z nią, lub tworzy nową. 
        /// Zawsze wyświetli komunikat, że stworzył bazę, albo że połączył z istiejcą i ich nazwy.
        /// </summary>
        private void SetConnection() 
        {
            String dir = Directory.GetCurrentDirectory(); //pobieram ścieżkę katalogu
            DirectoryInfo info = new DirectoryInfo(dir);  //pobieram informacje o katalogu, w którym jest aplikacja

            FileInfo[] fileList = info.GetFiles(); //pobieram informacje o wszystkich plikach w katalogu (nazwa i rozszerzenie)
            bool isDbExist = false; //flaga
            String searchDb = info.Name + ".db";

            foreach (FileInfo file in fileList) //dla każdej inforamcji o pliku
            {
                ///sprawdza, czy isnieje plik o rozszerzeniu .db i nazwie jak nazwa katalogu 
                ///(jeśli baza ma inną nazwę niż folder, to jej nie bierzemy, pomijamy ją)
                if (file.Name == searchDb) 
                {
                    isDbExist = true;
                }
            }

            if (!isDbExist) //jeśli baza danych nie istnieje
            {
                SQLiteConnection.CreateFile(searchDb);
                string sql = "";
                try
                {   /// Otwieram plik z DDL i zapisuję w strigu
                    ///Plik się nazywa "init.sql", jest dołączony do aplikacji i zawiera DDL bazy (i podstawowych danych np. województw).
                    using (StreamReader sr = new StreamReader("init.sql"))
                    {
                        // Czytam plik
                        String line = sr.ReadToEnd();
                        sql += line;
                    }
                }
                catch (IOException e) //Jeśli nie uda mu się odczytać, to wyskakuje błąd.
                {
                    MessageBox.Show("Błąd w odczycie pliku SQL");
                }
                //A tutaj tworzy połączenie do bazy danych, którą stowrzył.
                String databaseName = searchDb;
                String connectionString = "Data Source=./" + databaseName;
                con = new SQLiteConnection(connectionString);
                con.Open();

                SQLiteCommand cmd = con.CreateCommand();
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                String messegeinfo = "Stworzono bazę " + databaseName + " dla projektu " + info.Name + ".";
                MessageBox.Show(messegeinfo, "Informacje o bazie", MessageBoxButton.OK, MessageBoxImage.Information);
                con.Close();
            } 
            //Jeśli baza danych istnieje w katalogu, to tworzy do niej połączenie
            else
            {
                String databaseName = searchDb;
                String connectionString = "Data Source=./" + databaseName;
                con = new SQLiteConnection(connectionString);
                String messegeinfo = "Połączono z istniejącą bazą " + databaseName + " w projekcie " + info.Name + ".";
                MessageBox.Show(messegeinfo, "Informacje o bazie", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        //Window_Loaded pokazuje komunikat, że aplikacja została wczytana poprawnie i zaczytuje dane do grida.
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Loaded");
            //this.FillDataGrid();
            this.AddForms();
            //this.ComboBox_SelectionChanged();
        }

        //Window_Closed tylko pokazuje komunikat po zamknięciu aplikacji, że została zamknięta.
        /*
        private void Window_Closed(object sender, EventArgs e)
        {
            MessageBox.Show("Closed");
        }
        */
        /*private void FillDataGrid() //pokazuje wszystko, bez filtracji
        {
            con.Open();
            SQLiteCommand cmd = con.CreateCommand();
            cmd.CommandText = "select id_prof, Typ_prof, Nazwa, RD3 from Dane"; 
            cmd.CommandType = CommandType.Text;
            SQLiteDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            GPRDataGrid.ItemsSource = dt.DefaultView;
            dr.Close();
            con.Close();
        }*/
        /// <summary>
        /// AddForms wyświetla dane w formularzu i blokuje komórki do edycji.
        /// Dane pochodzą z tabeli Projekt.
        /// </summary>
        private void AddForms ()
        {
            con.Open();
            string sqlite = "select Nazwa_proj, Data, Adres, Opis_miejsca, " +
                "Miejscowosc, Wojewodztwo, Prowadzacy, Pogoda, War_geol, Zleceniodawca, Uwagi " +
                "from Projekt INNER JOIN Wojewodztwo ON Projekt.Id_woj = Wojewodztwo.Id_woj";
            SQLiteCommand cmd = con.CreateCommand();
            cmd.CommandText = sqlite;
            cmd.CommandType = CommandType.Text;
            SQLiteDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                nazwa_txtbx.Text = dr["Nazwa_proj"].ToString();
                data_date_picker.Text = dr["Data"].ToString();
                woj_cmbbx.Text = dr["Wojewodztwo"].ToString();
                miejscowosc_txtbx.Text = dr["Miejscowosc"].ToString();
                adres_txtbx.Text = dr["Adres"].ToString();
                opis_txtbx.Text = dr["Opis_miejsca"].ToString();
                prowadzacy_txtbx.Text = dr["Prowadzacy"].ToString();
                pogoda_txtbx.Text = dr["Pogoda"].ToString();
                geol_txtbx.Text = dr["War_geol"].ToString();
                zlec_txtbx.Text = dr["Zleceniodawca"].ToString();
                uwagi_txtbx.Text = dr["Uwagi"].ToString();
            }
            nazwa_txtbx.IsEnabled = false;
            data_date_picker.IsEnabled = false;
            woj_cmbbx.IsEnabled = false;
            miejscowosc_txtbx.IsEnabled = false;
            adres_txtbx.IsEnabled = false;
            opis_txtbx.IsEnabled = false;
            prowadzacy_txtbx.IsEnabled = false;
            pogoda_txtbx.IsEnabled = false;
            geol_txtbx.IsEnabled = false;
            zlec_txtbx.IsEnabled = false;
            uwagi_txtbx.IsEnabled = false;
            con.Close();
        }

        /*private void Refresh_btn_Click(object sender, RoutedEventArgs e) //Odśwież
        {
            this.FillDataGrid();
        }*/

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SQLiteCommand cmd = con.CreateCommand();
            cmd.CommandText = "select Typ from Typ_prof";
            cmd.CommandType = CommandType.Text;
            SQLiteDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                woj_cmbbx.Items.Add(dr[0]);
            }
        }



        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e) //wrażliwość na zmiany na filtrach
        {
            DataGrid dg = sender as DataGrid;
            DataRowView dr = dg.SelectedItem as DataRowView;
            if (dr != null)
            {
                nazwa_txtbx.Text = dr["Nazwa"].ToString();
                woj_cmbbx.Text = dr["Wojewodztwo"].ToString();
                data_date_picker.SelectedDate = DateTime.Parse(dr["Data"].ToString());
            }
        }

        private void ResetForm()
        {
            nazwa_txtbx.Text = "";
            woj_cmbbx.Text = "";
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

        private void MenuExit_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.DialogResult result = new System.Windows.Forms.DialogResult();
            result = System.Windows.Forms.MessageBox.Show("Czy na pewno chcesz zamknąć aplikację?", "Zamykanie", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Exclamation);
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                con.Close();
                Application.Current.Shutdown();
            }
        }

        private void mod_btn_Click(object sender, RoutedEventArgs e)
        {
            nazwa_txtbx.IsEnabled = true;
            data_date_picker.IsEnabled = true;
            woj_cmbbx.IsEnabled = true;
            miejscowosc_txtbx.IsEnabled = true;
            adres_txtbx.IsEnabled = true;
            opis_txtbx.IsEnabled = true;
            prowadzacy_txtbx.IsEnabled = true;
            pogoda_txtbx.IsEnabled = true;
            geol_txtbx.IsEnabled = true;
            zlec_txtbx.IsEnabled = true;
            uwagi_txtbx.IsEnabled = true;
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
