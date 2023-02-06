using DataGrid.Connection;
using DataGrid.Pages;
using MySql.Data.MySqlClient;
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
using System.Windows.Shapes;

namespace DataGrid.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddAddInformation.xaml
    /// </summary>
    public partial class AddAddInformation : Window
    {
        MySqlConnection con = ConnectionToDb.con;
        MySqlCommand cmd;
        public AddAddInformation()
        {
            InitializeComponent();
        }
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
        private void Cansel_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.addInfoDataBase.DataBase();
            this.Close();
        }

        private void AddAddInfo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                cmd = new MySqlCommand();
                cmd.Connection = con;
                con.Open();
                cmd.CommandText = "INSERT INTO additional_information ( id_add_info, add_information, specialization ) VALUES ( NULL, @add_information, @specialization ) ";
                cmd.Parameters.AddWithValue("@id", AddInfoDataBase.id);
                cmd.Parameters.AddWithValue("@add_information", textBoxAddInformationAdd.Text);
                cmd.Parameters.AddWithValue("@specialization", textBoxSpecializationAdd.Text);
                cmd.ExecuteNonQuery();
                con.Close();

                MessageBox.Show("Клуб добавлен", Title = "Успех");
            }
            catch (Exception ex)
            {
                con.Close();
                MessageBox.Show("Ошибка", Title = "Ошибка");
            }
            finally
            {
                cmd.Dispose();
                con.Dispose();
            }
        }

        private void Border_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                AddAddInfo_Click(sender, e);
        }
    }
}
