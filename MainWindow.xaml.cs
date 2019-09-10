﻿using System;
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
        SQLiteConnection co = null;
        public MainWindow()
        {
            SetConnection();
            InitializeComponent();
        }

        private void SetConnection()
        {
            String connectionStrig = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
            con = new SQLiteConnection(connectionString:);
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
            MessageBox.Show("Loaded");
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            MessageBox.Show("Closed");
        }
    }
}
