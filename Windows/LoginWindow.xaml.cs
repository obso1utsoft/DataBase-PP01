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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DataGrid.Windows
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    
    public partial class LoginWindow : Window
    {
        public static string loginUserName;
        public static string loginPrefix;
        MySqlConnection con = ConnectionToDb.con;
        MySqlCommand cmd;
        MySqlDataReader dr;
        public LoginWindow()
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
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                loginUserName = textBoxUserName.Text;

                cmd = new MySqlCommand();
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = "SELECT * FROM users WHERE username='" + textBoxUserName.Text + "' AND password='" + PasswordBoxPassword.Password + "'";
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    if ((string)dr["permission"] == "admin")
                    {
                        loginPrefix = "Админ";
                        MessageBox.Show("Добро пожаловать\nВы зашли как админ", Title = "Успех");
                    }
                    else
                    {
                        loginPrefix = "Гость";
                        MessageBox.Show("Добро пожаловать", Title = "Успех");
                    }
                    dr.Close();
                    con.Close();
                    MainWindow mainWindow = new MainWindow();
                    this.Close();
                    mainWindow.Show();
                }
                else
                {
                    MessageBox.Show("Неудалось зайти\nПожалуйста, проверьте логин или пароль", Title = "Ошибка");
                }
                dr.Close();
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Неверные данные подключения к базе данных", Title="Ошибка");
            }
            finally
            {
                cmd.Dispose();
                con.Dispose();
            }
        }

        private void CreateAcc_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            RegistrationWindow registrationWindow = new RegistrationWindow();
            this.Close();
            registrationWindow.Show();
            
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
                Login_Click(sender, e);
        }
    }
}
