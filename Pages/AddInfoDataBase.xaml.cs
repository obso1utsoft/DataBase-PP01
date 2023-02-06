using DataGrid.Connection;
using DataGrid.Windows;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
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
using System.Windows.Threading;

namespace DataGrid.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddInfoDataBase.xaml
    /// </summary>
    public partial class AddInfoDataBase : Page
    {
        private PagingCollectionView _cview;

        MySqlConnection con = ConnectionToDb.con;
        MySqlCommand cmd;
        MySqlDataReader dr;

        public static int id;
        public static string add_information;
        public static string specialization;
        public AddInfoDataBase()
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
                cmd.CommandText = "SELECT id_add_info, add_information, specialization" +
                                  " FROM additional_information ORDER BY id_add_info";
                dr = cmd.ExecuteReader();
                dr.Read();
                List<object> list = new List<object> {new
                {
                    Number = dr["id_add_info"],
                    Add_information = dr["add_information"],
                    Specialization = dr["specialization"]
                }};
                while (dr.Read())
                {
                    list.Add(new
                    {
                        Number = dr["id_add_info"],
                        Add_information = dr["add_information"],
                        Specialization = dr["specialization"]
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
                cmd.CommandText = "SELECT COUNT(*) FROM additional_information";
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
        private void AddAddInfo_Click(object sender, RoutedEventArgs e)
        {
            var addAddInformation = new AddAddInformation();
            addAddInformation.ShowDialog();
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
                MessageBox.Show("Вы не можете удалить первую запись!", Title="Ошибка");
                return;
            }
            try
            {
                cmd = new MySqlCommand();
                cmd.Connection = con;
                con.Open();
                cmd.CommandText = "UPDATE hobbies SET id_add_information = 1 WHERE id_add_information = @num;" +
                                  "DELETE FROM additional_information WHERE id_add_info = @num;" +
                                  "UPDATE additional_information SET id_add_info = id_add_info-1  WHERE id_add_info>@num;" +
                                  "ALTER TABLE additional_information AUTO_INCREMENT =1;";
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
                cmd.CommandText = "SELECT id_add_info, add_information, specialization " +
                                  "FROM additional_information WHERE id_add_info=@id";
                cmd.Parameters.AddWithValue("@id", row);
                dr = cmd.ExecuteReader();
                dr.Read();
                id = (int)dr["id_add_info"];
                add_information = (string)dr["add_information"];
                specialization = (string)dr["specialization"];
                con.Close();
                EditAddInformation editAddInformation = new EditAddInformation();
                editAddInformation.ShowDialog();
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
                cmd.CommandText = "SELECT id_add_info, add_information, specialization" +
                                  " FROM additional_information" +
                                  " WHERE add_information LIKE '%" + textBoxFilter.Text + "%'" +
                                  " OR specialization LIKE '%" + textBoxFilter.Text + "%'" +
                                  " OR id_add_info LIKE '%" + textBoxFilter.Text + "%' ORDER BY id_add_info";
                dr = cmd.ExecuteReader();
                dr.Read();
                List<object> list = new List<object> {new
                {
                    Number = dr["id_add_info"],
                    Add_information = dr["add_information"],
                    Specialization = dr["specialization"]
                }};
                while (dr.Read())
                {
                    list.Add(new
                    {
                        Number = dr["id_add_info"],
                        Add_information = dr["add_information"],
                        Specialization = dr["specialization"]
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
            dataGrid.Columns[3].Visibility = Visibility.Collapsed;
            addAddInfo.Visibility = Visibility.Collapsed;
        }
        public void UnHideColumn()
        {
            dataGrid.Columns[3].Visibility = Visibility.Visible;
            addAddInfo.Visibility = Visibility.Visible;
        }
    }
}
