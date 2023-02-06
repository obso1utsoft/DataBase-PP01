using DataGrid.Connection;
using DataGrid.Pages;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Логика взаимодействия для AddUsers.xaml
    /// </summary>
    public partial class AddUsers : Window
    {
        MySqlConnection con = ConnectionToDb.con;
        MySqlCommand cmd;
        MySqlDataReader dr;
        public AddUsers()
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
            MainWindow.usersDataBase.DataBase();
            this.Close();
        }
        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                cmd = new MySqlCommand();
                cmd.Connection = con;
                con.Open();

                cmd.CommandText = "SELECT * FROM users WHERE username='" + textBoxUsernameAdd.Text + "'";
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    MessageBox.Show("Пользователь с таким логином уже существует!", Title = "Ошибка");
                    return;
                }
                dr.Close();

                if (ComboBoxPermissionAdd.Text == "Админ")
                    cmd.CommandText = "INSERT INTO users ( id, permission, username, password ) VALUES ( NULL, 'admin', @username, @password )";
                else
                    cmd.CommandText = "INSERT INTO users ( id, permission, username, password ) VALUES ( NULL, 'member', @username, @password )";
                cmd.Parameters.AddWithValue("@username", textBoxUsernameAdd.Text);
                cmd.Parameters.AddWithValue("@password", textBoxPasswordAdd.Text);
                cmd.ExecuteNonQuery();
                con.Close();

                MessageBox.Show("Пользователь добавлен", Title = "Успех");
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
                AddUser_Click(sender, e);
        }
    }
}
