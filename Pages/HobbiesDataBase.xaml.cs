using DataGrid.Connection;
using DataGrid.Windows;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Claims;
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
using System.Windows.Threading;

namespace DataGrid.Pages
{
    /// <summary>
    /// Логика взаимодействия для HobbiesDataBase.xaml
    /// </summary>
    public partial class HobbiesDataBase : Page
    {
        private PagingCollectionView _cview;

        MySqlConnection con = ConnectionToDb.con;
        MySqlCommand cmd;
        MySqlDataReader dr;

        public static int id;
        public static string birth_year;
        public static string hobbies;

        public HobbiesDataBase()
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
                cmd.CommandText = "SELECT hobbies.id, hobbies, add_information, birth_year " +
                                  "FROM members, hobbies, additional_information WHERE hobbies.id_full_name = members.id " +
                                  "AND hobbies.id_add_information = additional_information.id_add_info ORDER BY hobbies.id";
                dr = cmd.ExecuteReader();
                dr.Read();
                List<object> list = new List<object> {new
                {
                    Number = dr["id"],
                    Hobbies = dr["hobbies"],
                    Birth_year = Convert.ToString(dr["birth_year"]).Substring(0, 10),
                    Add_information = dr["add_information"]
                }};
                while (dr.Read())
                {
                    list.Add(new
                    {
                        Number = dr["id"],
                        Hobbies = dr["hobbies"],
                        Birth_year = Convert.ToString(dr["birth_year"]).Substring(0, 10),
                        Add_information = dr["add_information"]
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
                cmd.CommandText = "SELECT COUNT(*) FROM hobbies";
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
        private void AddAddInfo_Click(object sender, RoutedEventArgs e)
        {
            var addMembers = new AddMembers();
            addMembers.ShowDialog();
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

            try
            {
                cmd = new MySqlCommand();
                cmd.Connection = con;
                con.Open();
                cmd.CommandText = "DELETE FROM hobbies WHERE id_full_name = @num;" +
                                  "DELETE FROM members WHERE id = @num;" +
                                  "UPDATE hobbies SET id = id-1  WHERE id>@num;" +
                                  "UPDATE hobbies SET id_full_name = id_full_name-1 Where id>@num;" +
                                  "UPDATE members SET id = id-1 WHERE id>@num;" +
                                  "ALTER TABLE hobbies AUTO_INCREMENT =1;" +
                                  "ALTER TABLE members AUTO_INCREMENT =1";
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
                cmd.CommandText = "SELECT hobbies.id, hobbies " +
                                  "FROM members, hobbies WHERE members.id=@id AND hobbies.id_full_name=members.id";
                cmd.Parameters.AddWithValue("@id", row);
                dr = cmd.ExecuteReader();
                dr.Read();
                id = (int)dr["id"];
                hobbies = (string)dr["hobbies"];
                con.Close();
                EditHobbies editHobbies = new EditHobbies();
                editHobbies.ShowDialog();
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
                cmd.CommandText = "SELECT hobbies.id, hobbies, add_information, birth_year, id_add_information, additional_information.id_add_info" +
                                  " FROM members, hobbies, additional_information" +
                                  " WHERE hobbies.id_full_name = members.id AND hobbies.id_add_information = additional_information.id_add_info AND hobbies LIKE '%" + textBoxFilter.Text + "%'" +
                                  " OR hobbies.id_full_name = members.id AND hobbies.id_add_information = additional_information.id_add_info AND add_information LIKE '%" + textBoxFilter.Text + "%'" +
                                  " OR hobbies.id_full_name = members.id AND hobbies.id_add_information = additional_information.id_add_info AND birth_year LIKE '%" + textBoxFilter.Text + "%'" +
                                  " OR hobbies.id_full_name = members.id AND hobbies.id_add_information = additional_information.id_add_info AND hobbies.id LIKE '%" + textBoxFilter.Text + "%' ORDER BY hobbies.id";
                dr = cmd.ExecuteReader();
                dr.Read();
                List<object> list = new List<object> {new
                {
                    Number = dr["id"],
                    Hobbies = dr["hobbies"],
                    Birth_year = Convert.ToString(dr["birth_year"]).Substring(0, 10),
                    Add_information = dr["add_information"]
                }};
                while (dr.Read())
                {
                    list.Add(new
                    {
                        Number = dr["id"],
                        Hobbies = dr["hobbies"],
                        Birth_year = Convert.ToString(dr["birth_year"]).Substring(0, 10),
                        Add_information = dr["add_information"]
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
        public void HideColumn()
        {
            dataGrid.Columns[4].Visibility = Visibility.Collapsed;
        }
        public void UnHideColumn()
        {
            dataGrid.Columns[4].Visibility = Visibility.Visible;
        }
    }
}
