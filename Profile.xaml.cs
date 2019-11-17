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
    /// Logika interakcji dla klasy Profile.xaml
    /// </summary>
    public partial class Profile : Window
    {
        SQLiteConnection con = null;
        public Profile()
        {
            SetConnection();
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) //Tutaj nic nic robi
        {
            this.FillDataGrid();
            MessageBox.Show("Załadowano");
            //this.ComboBox_SelectionChanged();
        }

        private void SetConnection()
        {
            String dir = Directory.GetCurrentDirectory(); //pobieram ścieżkę katalogu
            DirectoryInfo info = new DirectoryInfo(dir);  //pobieram informacje o katalogu, w którym jest aplikacja

            FileInfo[] fileList = info.GetFiles(); //pobieram informacje o wszystkich plikach w katalogu (nazwa i rozszerzenie)
            String searchDb = info.Name + ".db";
            String databaseName = searchDb;
            String connectionString = "Data Source=./" + databaseName + ";version=3;datetimeformat=CurrentCulture;";
            con = new SQLiteConnection(connectionString);          
        }
        //Funkcja robiąca select na bazie
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

        private void MenuNewProf_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuExit_Click(object sender, RoutedEventArgs e)
        {
                con.Close();
                Application.Current.Shutdown();
        }

        private void MenuInfoInfo_Click(object sender, RoutedEventArgs e)
        {
            Common.InfoBox();
        }

        private void FillDataGrid() //pokazuje wszystko, bez filtracji
        {
            con.Open();
            string squlitequery = "SELECT Nazwa AS [Nazwa profilu], Typ AS [Typ profilu], Wspol_pocz AS [Współrzędne początku], " +
                "Wspol_konca AS [Współrzędne końca], Wspol_opisowe_pocz AS [Współrzędne opisowe początku], " +
                "Wspol_opisowe_konca AS [Współrzętne opisowe końca], Dl_prof AS [Długość profilu(m)], Liczba_tras AS [Liczba tras], " +
                "Odl_m_trasami AS [Odległość między trasami(m)], Okno_czasowe AS [Okno czasowe(ns)], L_probek AS [Liczba próbek], " +
                "Czest_prob AS [Częstotliwość próbkowania(ns)], Skladanie AS [Składanie], Czestotliwosc || ' ' || Konstrukcja AS [Antena], Uwagi " +
                "FROM Anteny INNER JOIN Dane ON dane.id_ant = anteny.id_ant INNER JOIN Typ_prof ON typ_prof = id_typ";
            SQLiteDataReader dr = SelectFromDB(squlitequery);
            //DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Load(dr);
            GPRDataGrid.ItemsSource = dt.DefaultView;
            dr.Close();
            con.Close();
        }
    }
}
