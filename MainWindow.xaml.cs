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
using System.Globalization;

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

                    //!!!!
                    //tu trzeba poprawić kodowanie - Decoder?
                    using (StreamReader sr = new StreamReader("init.sql", Encoding.UTF8))
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
                String connectionString = "Data Source=./" + databaseName + ";version=3;datetimeformat=CurrentCulture;";
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
                String connectionString = "Data Source=./" + databaseName + ";version=3;datetimeformat=CurrentCulture;";
                con = new SQLiteConnection(connectionString);
                String messegeinfo = "Połączono z istniejącą bazą " + databaseName + " w projekcie " + info.Name + ".";
                MessageBox.Show(messegeinfo, "Informacje o bazie", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        //Window_Loaded pokazuje komunikat, że aplikacja została wczytana poprawnie i zaczytuje dane do grida.
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.ComboBox_Fill();
            this.AddForms();
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
        /// SelectFromDB to funckja do robienia selectu projektów, który się powtarza
        /// </summary>
        private SQLiteDataReader SelectFromDB(string sqlitequery)
        {
            SQLiteCommand cmd = con.CreateCommand();
            cmd.CommandText = sqlitequery;
            cmd.CommandType = CommandType.Text;
            SQLiteDataReader dr = cmd.ExecuteReader();
            return dr;
        }

        //funkcja wywołująca zapytanie sqlitowe
        private void ExecuteQuery(string txtQuery)
        {
            SQLiteCommand cmd = con.CreateCommand();
            cmd.CommandText = txtQuery;
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
        }
        /// <summary>
        /// AddForms wyświetla dane w formularzu i blokuje komórki do edycji.
        /// Dane pochodzą z tabeli Projekt.
        /// </summary>

        private void AddForms()
        {
            con.Open();
            SQLiteDataReader dr = SelectFromDB("select Nazwa_proj, Data, Adres, Opis_miejsca, " +
                "Miejscowosc, Wojewodztwo, Prowadzacy, Pogoda, War_geol, Zleceniodawca, Uwagi " +
                "from Projekt INNER JOIN Wojewodztwo ON Projekt.Id_woj = Wojewodztwo.Id_woj");
            if (dr.Read())
            {
                nazwa_txtbx.Text = dr["Nazwa_proj"].ToString();
                data_date_picker.Text = dr["Data"].ToString();
                woj_cmbbx.SelectedItem = dr["Wojewodztwo"].ToString();
                miejscowosc_txtbx.Text = dr["Miejscowosc"].ToString();
                adres_txtbx.Text = dr["Adres"].ToString();
                opis_txtbx.Text = dr["Opis_miejsca"].ToString();
                prowadzacy_txtbx.Text = dr["Prowadzacy"].ToString();
                pogoda_txtbx.Text = dr["Pogoda"].ToString();
                geol_txtbx.Text = dr["War_geol"].ToString();
                zlec_txtbx.Text = dr["Zleceniodawca"].ToString();
                uwagi_txtbx.Text = dr["Uwagi"].ToString();
            }
            else //Jeśli nie ma jeszcze żadych danych, to nie wpisuje nic, tylko proponowaną nazwę projektu (taką, jak nazwa katalogu)
            {
                String dir = Directory.GetCurrentDirectory(); //pobieram ścieżkę katalogu
                DirectoryInfo info = new DirectoryInfo(dir);  //pobieram informacje o katalogu, w którym jest aplikacja
                String nazwainfo = info.Name;
                nazwa_txtbx.Text = nazwainfo;
            }
            dr.Close();
            con.Close();
        }

        /*private void Refresh_btn_Click(object sender, RoutedEventArgs e) //Odśwież
        {
            this.FillDataGrid();
        }*/

        /// <summary>
        /// ComboBoxFill wypełnia ComboBox danymi z tabeli województwa
        /// </summary>
        private void ComboBox_Fill()
        {
            
            con.Open();
            /*SQLiteCommand cmd = con.CreateCommand();
            cmd.CommandText = "select id_woj, Wojewodztwo from Wojewodztwo";
            cmd.CommandType = CommandType.Text;*/
            SQLiteDataReader dr = SelectFromDB("select id_woj, Wojewodztwo from Wojewodztwo");//cmd.ExecuteReader();
            while (dr.Read())
            {
                woj_cmbbx.Items.Add((dr["Wojewodztwo"]));
            }
            dr.Close();
            con.Close();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /*
            con.Open();
            SQLiteCommand cmd = con.CreateCommand();
            cmd.CommandText = "select id_woj, Wojewodztwo from Wojewodztwo";
            cmd.CommandType = CommandType.Text;
            SQLiteDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                woj_cmbbx.Items.Add(dr["Wojewodztwo"]);
            }
            dr.Close();
            con.Close();*/
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

        //Otworzenie okna z profilami
        private void OpenProfile_btn_Click(object sender, RoutedEventArgs e)
        {
            Profile to = new Profile();
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

        private void fot_btn_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void mapy_btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void addproj_btn_Click(object sender, RoutedEventArgs e)
        {
            con.Open();
            SQLiteDataReader dr = this.SelectFromDB("select Nazwa_proj, Data, Adres, Opis_miejsca, " +
                "Miejscowosc, Wojewodztwo, Prowadzacy, Pogoda, War_geol, Zleceniodawca, Uwagi " +
                "FROM Projekt LEFT OUTER JOIN Wojewodztwo ON Projekt.Id_woj = Wojewodztwo.Id_woj");

            /*
            string sqlitequery = "select Nazwa_proj, Data, Adres, Opis_miejsca, " +
                "Miejscowosc, Wojewodztwo, Prowadzacy, Pogoda, War_geol, Zleceniodawca, Uwagi " +
                "from Projekt INNER JOIN Wojewodztwo ON Projekt.Id_woj = Wojewodztwo.Id_woj";
            SQLiteCommand cmd = con.CreateCommand();
            cmd.CommandText = sqlitequery;
            cmd.CommandType = CommandType.Text;
            SQLiteDataReader dr = cmd.ExecuteReader();
            */
            if (dr.Read())
            {
                DateTime date = DateTime.Parse(data_date_picker.SelectedDate.ToString());
                string sqlite = "UPDATE Projekt " +
                "SET Nazwa_proj = '" + nazwa_txtbx.Text + "'" +
                ",Data = '" + date.ToString("dd-MM-yyyy") + "', Adres = '"+adres_txtbx.Text+"'," +
                "Opis_miejsca = '"+opis_txtbx.Text+"', Miejscowosc = '"+miejscowosc_txtbx.Text+"', Id_woj = '"+(woj_cmbbx.SelectedIndex + 1)+"'," +
                " Prowadzacy = '"+prowadzacy_txtbx.Text+"', Pogoda = '"+pogoda_txtbx.Text+"', War_geol = '"+geol_txtbx.Text+"'," +
                "Zleceniodawca = '"+zlec_txtbx.Text+"', Uwagi = '"+uwagi_txtbx.Text+"'" +
                "WHERE id_proj = 1";
                dr.Close();
                ExecuteQuery(sqlite);
                MessageBox.Show("Zaktualizowano atrybuty projektu.", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                DateTime date = new DateTime();
                if (data_date_picker.SelectedDate != null)
                {
                    date = DateTime.Parse(data_date_picker.SelectedDate.ToString());
                }
                else
                {
                    date = DateTime.Now;
                }
                dr.Close();
                string sqlite = "INSERT INTO Projekt (Nazwa_proj, Data, Adres, Opis_miejsca, Miejscowosc, Id_woj, Prowadzacy, Pogoda, War_geol, " +
                    "Zleceniodawca, Uwagi) " +
                    "VALUES ('" + nazwa_txtbx.Text + "','" + date.ToString("dd-MM-yyyy") + "','" + adres_txtbx.Text + "','" + opis_txtbx.Text + "'," +
                    "'" + miejscowosc_txtbx.Text + "','" + (woj_cmbbx.SelectedIndex + 1) + "','" + prowadzacy_txtbx.Text + "'," +
                    "'" + pogoda_txtbx.Text + "','" + geol_txtbx.Text + "','" + zlec_txtbx.Text + "','" + uwagi_txtbx.Text + "')";

                ExecuteQuery(sqlite);
                MessageBox.Show("Zapisano atrybuty projektu.", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            /*
            //string Nazwa = nazwa_txtbx.Text;
            con.Open();             
            SQLiteCommand cmd = con.CreateCommand();           
            cmd.Parameters.AddWithValue("@Nazwatxt", nazwa_txtbx.Text);
            string sqlite = "UPDATE Projekt" +
                "SET Nazwa = @Nazwatxt" +
                "WHERE id_proj = 1";

            cmd.CommandText = sqlite;
            cmd.CommandType = CommandType.Text;

            //SQLiteDataReader dr = cmd.ExecuteReader();
            cmd.ExecuteNonQuery();
            */
            con.Close();
            this.AddForms();
        }
            

        private void MenuOpenProf_Click(object sender, RoutedEventArgs e)
        {
            Profile prof = new Profile();
            prof.ShowDialog();
        }
        private void MenuNewProf_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuInfoInfo_Click(object sender, RoutedEventArgs e)
        {
            
            MessageBox.Show("Program BazaGPR został stworzony w 2019 roku jako składowa pracy magisterkiej. Służy do archiwizacji " +
                "i porządkowania pomiarów georadarowych oraz informacji o nich. Dane są przechowywane w bazie SQLite. \n \n --- \n \n " +
                "Autor: inż. Łucja Kozubek \n \n --- \n \n Autor ikon (Icons made by): Smashicons \n https://www.flaticon.com/authors/smashicons", "O programie");
        }


    }
}
