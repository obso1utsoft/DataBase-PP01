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
    /// Логика взаимодействия для RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        MySqlConnection con = ConnectionToDb.con;
        MySqlCommand cmd;
        MySqlDataReader dr;

        public RegistrationWindow()
        {
            InitializeComponent();

            textBoxUserName.Focus();
        }
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
        private void CreateAcc_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                cmd = new MySqlCommand();
                con.Open();
                cmd.Connection = con;

                cmd.CommandText = "SELECT * FROM users WHERE username='" + textBoxUserName.Text + "'";
                dr = cmd.ExecuteReader();
                if(dr.Read())
                {
                    MessageBox.Show("Пользователь с таким логином существует", Title = "Ошибка");
                    return;
                }
                dr.Close();

                LoginWindow.loginUserName = textBoxUserName.Text;
                LoginWindow.loginPrefix = "Гость";

                cmd.CommandText = ("INSERT INTO users ( id, permission, username, password ) VALUES ( NULL, 'member', @username, @password )");
                cmd.Parameters.AddWithValue("@username", textBoxUserName.Text);
                cmd.Parameters.AddWithValue("@password", PasswordBoxPassword.Password);
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Аккаунт создан", Title = "Успех");
                MainWindow mainWindow = new MainWindow();
                this.Close();
                mainWindow.Show();
            }
            catch
            {
                MessageBox.Show("Ошибка", Title="Ошибка");
            }
            finally
            {
                cmd.Dispose();
                con.Dispose();
            }
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void SignIn_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            this.Close();
            loginWindow.Show();
        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            // Allow alphanumeric and space.
            if (e.Key >= Key.D0 && e.Key <= Key.D9 ||
                e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9 ||
                e.Key >= Key.A && e.Key <= Key.Z)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }

            // If tab is presses, then the focus must go to the
            // next control.
            if (e.Key == Key.Tab)
            {
                e.Handled = false;
            }

            if (e.Key == Key.Enter)
                CreateAcc_Click(sender, e);
        }
    }
}
