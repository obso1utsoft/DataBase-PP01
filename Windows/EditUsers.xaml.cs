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
    /// Логика взаимодействия для EditUsers.xaml
    /// </summary>
    public partial class EditUsers : Window
    {
        MySqlConnection con = ConnectionToDb.con;
        MySqlCommand cmd;
        public EditUsers()
        {
            InitializeComponent();

            TextBlockPermission.Text = UsersDataBase.permission;
            textBoxUsernameEdit.Text = UsersDataBase.username;
            textBoxPasswordEdit.Text = UsersDataBase.password;
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
        private void EditUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                cmd = new MySqlCommand();
                cmd.Connection = con;
                con.Open();
                if (ComboBoxPermissionEdit.Text == "Админ" || TextBlockPermission.Text == "Админ" && ComboBoxPermissionEdit.Text != "Гость")
                    cmd.CommandText = "UPDATE users SET permission = 'admin', username = @username, password = @password" +
                                      " WHERE id = @id";
                else
                    cmd.CommandText = "UPDATE users SET permission = 'member', username = @username, password = @password" +
                                      " WHERE id = @id";
                cmd.Parameters.AddWithValue("@id", UsersDataBase.id);
                cmd.Parameters.AddWithValue("@username", textBoxUsernameEdit.Text);
                cmd.Parameters.AddWithValue("@password", textBoxPasswordEdit.Text);
                cmd.ExecuteNonQuery();
                con.Close();

                MessageBox.Show("Пользоваетль изменен", Title = "Успех");
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
                EditUser_Click(sender, e);
        }
    }
}
