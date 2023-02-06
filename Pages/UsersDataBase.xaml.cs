using DataGrid.Connection;
using DataGrid.Windows;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DataGrid.Pages
{
    /// <summary>
    /// Логика взаимодействия для UsersDataBase.xaml
    /// </summary>
    public partial class UsersDataBase : Page
    {
        private PagingCollectionView _cview;

        MySqlConnection con = ConnectionToDb.con;
        MySqlCommand cmd;
        MySqlDataReader dr;

        public static int id;
        public static string permission;
        public static string username;
        public static string password;
        public UsersDataBase()
        {
            InitializeComponent();
        }
        public void DataBase()
        {
            try
            {
                var converter = new BrushConverter();
                cmd = new MySqlCommand();
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = "SELECT id, permission, username, password" +
                                  " FROM users ORDER BY id";
                dr = cmd.ExecuteReader();
                dr.Read();
                List<object> list = new List<object> {new
                {
                    Number = dr["id"],
                    Permission = dr["permission"],
                    Username = dr["username"],
                    Password = dr["password"]
                }};
                while (dr.Read())
                {
                    list.Add(new
                    {
                        Number = dr["id"],
                        Permission = dr["permission"],
                        Username = dr["username"],
                        Password = dr["password"]
                    });
                }
                this._cview = new PagingCollectionView(
                list,
                MainWindow.records
                );
                dr.Close();
                dataGrid.ItemsSource = this._cview;
                currentPage.Content = _cview.CurrentPage;
                textBoxFilter.Text = "";
                cmd.CommandText = "SELECT COUNT(*) FROM users";
                membersCountTextBlock.Text = Convert.ToString(cmd.ExecuteScalar());
                con.Close();
            }
            catch (Exception ex)
            {
                con.Close();
                currentPage.Content = "1";
                dataGrid.ItemsSource = null;
                MessageBox.Show("База данных не имеет записей!", Title = "Ошибка");
            }
            finally
            {
                cmd.Dispose();
                con.Dispose();
            }
        }
        private void OnNextClicked(object sender, RoutedEventArgs e)
        {
            if (_cview != null)
            {
                if (_cview.CurrentPage < _cview.PageCount)
                {
                    _cview.CurrentPage += 1;
                }
                currentPage.Content = _cview.CurrentPage;
                _cview.Refresh();
            }
            else
            {
                dataGrid.ItemsSource = null;
                MessageBox.Show("База данных не имеет записей!", Title = "Ошибка");
            }

        }

        private void OnPreviousClicked(object sender, RoutedEventArgs e)
        {
            if (_cview != null)
            {
                if (_cview.CurrentPage > 1)
                {
                    _cview.CurrentPage -= 1;
                }
                currentPage.Content = _cview.CurrentPage;
                _cview.Refresh();
            }
            else
            {
                dataGrid.ItemsSource = null;
                MessageBox.Show("База данных не имеет записей!", Title = "Ошибка");
            }
        }
        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            var addUsers = new AddUsers();
            addUsers.ShowDialog();
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            DataBase();
        }
        private void gridRemoveButton_Click(object sender, RoutedEventArgs e)
        {
            int row;
            dynamic row_list = dataGrid.SelectedItem;
            row = row_list.Number;

            if (row == 1)
            {
                MessageBox.Show("Вы не можете удалить первую запись!", Title = "Ошибка");
                return;
            }
            try
            {
                cmd = new MySqlCommand();
                cmd.Connection = con;
                con.Open();
                cmd.CommandText = "DELETE FROM users WHERE id = @num;" +
                                  "UPDATE users SET id = id-1  WHERE id>@num;" +
                                  "ALTER TABLE users AUTO_INCREMENT =1;";
                cmd.Parameters.AddWithValue("@num", row);
                cmd.ExecuteNonQuery();

                con.Close();
                MessageBox.Show("Запись удалена!", Title = "Успех");
                DataBase();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка", Title = "Ошибка");
            }
            finally
            {
                cmd.Dispose();
                con.Dispose();
            }
        }
        private void gridEditButton_Click(object sender, RoutedEventArgs e)
        {
            int row;
            dynamic row_list = dataGrid.SelectedItem;
            row = row_list.Number;

            try
            {
                cmd = new MySqlCommand();
                cmd.Connection = con;
                con.Open();
                cmd.CommandText = "SELECT id, permission, username, password " +
                                  "FROM users WHERE id=@id";
                cmd.Parameters.AddWithValue("@id", row);
                dr = cmd.ExecuteReader();
                dr.Read();
                id = (int)dr["id"];
                permission = (string)dr["permission"];
                username = (string)dr["username"];
                password = (string)dr["password"];
                con.Close();
                EditUsers editUsers = new EditUsers();
                editUsers.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка", Title = "Ошибка");
            }

        }

        private void textBoxFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var converter = new BrushConverter();

                cmd = new MySqlCommand();
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = "SELECT id, permission, username, password" +
                                  " FROM users" +
                                  " WHERE username LIKE '%" + textBoxFilter.Text + "%'" +
                                  " OR permission LIKE '%" + textBoxFilter.Text + "%'" +
                                  " OR password LIKE '%" + textBoxFilter.Text + "%'" +
                                  " OR id LIKE '%" + textBoxFilter.Text + "%' ORDER BY id";
                dr = cmd.ExecuteReader();
                dr.Read();
                List<object> list = new List<object> {new
                {
                    Number = dr["id"],
                    Permission = dr["permission"],
                    Username = dr["username"],
                    Password = dr["password"]
                }};
                while (dr.Read())
                {
                    list.Add(new
                    {
                        Number = dr["id"],
                        Permission = dr["permission"],
                        Username = dr["username"],
                        Password = dr["password"]
                    });
                }
                this._cview = new PagingCollectionView(
                list,
                MainWindow.records
                );
                con.Close();
                dataGrid.ItemsSource = this._cview;
                currentPage.Content = _cview.CurrentPage;
            }
            catch (Exception ex)
            {
                con.Close();
                currentPage.Content = "1";
                MessageBox.Show("Такой записи не существует!", Title = "Ошибка");
                textBoxFilter.Text = textBoxFilter.Text.Remove(textBoxFilter.Text.Length - 1);
                textBoxFilter.SelectionStart = textBoxFilter.Text.Length;
            }
            finally
            {
                cmd.Dispose();
                con.Dispose();
            }
        }
    }
}
