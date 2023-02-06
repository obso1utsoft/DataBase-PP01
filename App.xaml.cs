using DataGrid.Windows;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace DataGrid
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string connectToDB;
        protected override void OnStartup(StartupEventArgs e)
        {
            if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + "ConnectToDB.config"))
            {
                using (StreamWriter sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "ConnectToDB.config"))
                {
                    sw.WriteLine("Server=127.0.0.1;Database=school3;user=root;charset=utf8");
                }
            }
            using (StreamReader sr = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "ConnectToDB.config"))
            {
                connectToDB = sr.ReadLine();
            }

            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
        }
    }

}
