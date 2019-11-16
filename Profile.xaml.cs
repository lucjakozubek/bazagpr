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

        private void SetConnection()
        {
            String dir = Directory.GetCurrentDirectory(); //pobieram ścieżkę katalogu
            DirectoryInfo info = new DirectoryInfo(dir);  //pobieram informacje o katalogu, w którym jest aplikacja

            FileInfo[] fileList = info.GetFiles(); //pobieram informacje o wszystkich plikach w katalogu (nazwa i rozszerzenie)
            bool isDbExist = false; //flaga
            String searchDb = info.Name + ".db";
            String databaseName = searchDb;
            String connectionString = "Data Source=./" + databaseName + ";version=3;datetimeformat=CurrentCulture;";
            con = new SQLiteConnection(connectionString);          
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

            MessageBox.Show("Program BazaGPR został stworzony w 2019 roku jako składowa pracy magisterkiej. Służy do archiwizacji " +
                "i porządkowania pomiarów georadarowych oraz informacji o nich. Dane są przechowywane w bazie SQLite. \n \n --- \n \n " +
                "Autor: inż. Łucja Kozubek \n \n --- \n \n Autor ikon (Icons made by): Smashicons \n https://www.flaticon.com/authors/smashicons", "O programie");
        }
    }
}
