using DataGrid.Windows;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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
using DataGrid;
using System.Diagnostics;
using System.Data;
using System.Threading;
using DataGrid.Connection;

namespace DataGrid.Pages
{
    /// <summary>
    /// Логика взаимодействия для MembersDateBase.xaml
    /// </summary>
    public partial class MembersDateBase : Page
    {
        private PagingCollectionView _cview;

        MySqlConnection con = ConnectionToDb.con;
        MySqlCommand cmd;
        MySqlDataReader dr;

        public static int id;
        public static string full_name;
        public static string Class;
        public static string birth_year;
        public static string hobbies;

        public MembersDateBase()
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
                cmd.CommandText = "SELECT members.id, full_name, class, birth_year, additional_information.specialization " +
                                  "FROM members, hobbies, additional_information " +
                                  "WHERE hobbies.id_full_name = members.id " +
                                  "AND hobbies.id_add_information = additional_information.id_add_info ORDER BY id";
                dr = cmd.ExecuteReader();
                dr.Read();
                List<object> list = new List<object> {new
                {
                    Number = dr["id"],
                    Character = ((string)dr["full_name"]).Substring(0, 1),
                    BgColor = (Brush)converter.ConvertFromString("#1098AD"),
                    Name = dr["full_name"],
                    Class = dr["class"],
                    Birth_Year = Convert.ToString(dr["birth_year"]).Substring(0, 10),
                    Specialization = dr["specialization"]
                }};
                while (dr.Read())
                {
                    list.Add(new
                    {
                        Number = dr["id"],
                        Character = ((string)dr["full_name"]).Substring(0, 1),
                        BgColor = (Brush)converter.ConvertFromString("#1098AD"),
                        Name = dr["full_name"],
                        Class = dr["class"],
                        Birth_Year = Convert.ToString(dr["birth_year"]).Substring(0, 10),
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
                cmd.CommandText = "SELECT COUNT(*) FROM members";
                membersCountTextBlock.Text = Convert.ToString(cmd.ExecuteScalar());
                con.Close();
            }
            catch (Exception ex)
            {
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
        private void AddMember_Click(object sender, RoutedEventArgs e)
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
                MessageBox.Show("Запись удалена", Title = "Успех");
                DataBase();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка", Title="Ошибка");
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
                cmd.CommandText = "SELECT members.id, full_name, class, birth_year, hobbies.hobbies " +
                                  "FROM members, hobbies WHERE members.id=@id AND hobbies.id_full_name=members.id";
                cmd.Parameters.AddWithValue("@id", row);
                dr = cmd.ExecuteReader();
                dr.Read();
                id = (int)dr["id"];
                full_name = (string)dr["full_name"];
                Class = (string)dr["class"];
                birth_year = Convert.ToString(dr["birth_year"]).Substring(0, 10);
                hobbies = (string)dr["hobbies"];
                con.Close();
                EditMembers editMembers = new EditMembers();
                editMembers.ShowDialog();
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

        private void textBoxFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var converter = new BrushConverter();

                cmd = new MySqlCommand();
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = "SELECT members.id, full_name, class, birth_year, additional_information.specialization FROM members, hobbies, additional_information" +
                                  " WHERE hobbies.id_full_name = members.id AND hobbies.id_add_information = additional_information.id_add_info AND full_name LIKE '%" + textBoxFilter.Text + "%'" +
                                  " OR hobbies.id_full_name = members.id AND hobbies.id_add_information = additional_information.id_add_info AND class LIKE '%" + textBoxFilter.Text + "%'" +
                                  " OR hobbies.id_full_name = members.id AND hobbies.id_add_information = additional_information.id_add_info AND birth_year LIKE '%" + textBoxFilter.Text + "%'" +
                                  " OR hobbies.id_full_name = members.id AND hobbies.id_add_information = additional_information.id_add_info AND additional_information.specialization LIKE '%" + textBoxFilter.Text + "%'" +
                                  " OR hobbies.id_full_name = members.id AND hobbies.id_add_information = additional_information.id_add_info AND members.id LIKE '%" + textBoxFilter.Text + "%' ORDER BY id";
                dr = cmd.ExecuteReader();
                dr.Read();
                List<object> list = new List<object> {new
                {
                    Number = dr["id"],
                    Character = ((string)dr["full_name"]).Substring(0, 1),
                    BgColor = (Brush)converter.ConvertFromString("#1098AD"),
                    Name = dr["full_name"],
                    Class = dr["class"],
                    Birth_Year = Convert.ToString(dr["birth_year"]).Substring(0, 10),
                    Specialization = dr["specialization"]
                }};
                while (dr.Read())
                {
                    list.Add(new
                    {
                        Number = dr["id"],
                        Character = ((string)dr["full_name"]).Substring(0, 1),
                        BgColor = (Brush)converter.ConvertFromString("#1098AD"),
                        Name = dr["full_name"],
                        Class = dr["class"],
                        Birth_Year = Convert.ToString(dr["birth_year"]).Substring(0, 10),
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
            dataGrid.Columns[5].Visibility = Visibility.Collapsed;
            addMember.Visibility = Visibility.Collapsed;
        }
        public void UnHideColumn()
        {
            dataGrid.Columns[5].Visibility = Visibility.Visible;
            addMember.Visibility = Visibility.Visible;
        }
    }
    public class PagingCollectionView : CollectionView
    {
        private readonly IList _innerList;
        private readonly int _itemsPerPage;

        private int _currentPage = 1;

        public PagingCollectionView(IList innerList, int itemsPerPage)
            : base(innerList)
        {
            this._innerList = innerList;
            this._itemsPerPage = itemsPerPage;
        }

        public override int Count
        {
            get
            {
                if (this._currentPage < this.PageCount) // page 1..n-1
                {
                    return this._itemsPerPage;
                }
                else // page n
                {
                    var itemsLeft = this._innerList.Count % this._itemsPerPage;
                    if (0 == itemsLeft)
                    {
                        return this._itemsPerPage; // exactly itemsPerPage left
                    }
                    else
                    {
                        // return the remaining items
                        return itemsLeft;
                    }
                }
            }
        }

        public int CurrentPage
        {
            get { return this._currentPage; }
            set
            {
                this._currentPage = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("CurrentPage"));
            }
        }

        public int ItemsPerPage { get { return this._itemsPerPage; } }

        public int PageCount
        {
            get
            {
                return (this._innerList.Count + this._itemsPerPage - 1)
                    / this._itemsPerPage;
            }
        }

        private int EndIndex
        {
            get
            {
                var end = this._currentPage * this._itemsPerPage - 1;
                return (end > this._innerList.Count) ? this._innerList.Count : end;
            }
        }

        private int StartIndex
        {
            get { return (this._currentPage - 1) * this._itemsPerPage; }
        }

        public override object GetItemAt(int index)
        {
            var offset = index % (this._itemsPerPage);
            return this._innerList[this.StartIndex + offset];
        }

        
    }
}
