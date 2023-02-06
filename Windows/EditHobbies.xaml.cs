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
    /// Логика взаимодействия для EditHobbies.xaml
    /// </summary>
    public partial class EditHobbies : Window
    {
        MySqlConnection con = ConnectionToDb.con;
        MySqlCommand cmd;
        MySqlDataAdapter da;
        MySqlDataReader dr;

        int id_add_info;

        public EditHobbies()
        {
            InitializeComponent();

            membersShow();

            ListCategories();
        }
        private void membersShow()
        {
            textBoxHobbiesEdit.Text = HobbiesDataBase.hobbies;
        }

        private void ListCategories()
        {
            try
            {
                cmd = new MySqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "SELECT id_add_info, add_information FROM additional_information";
                con.Open();
                da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                ComboBoxAddInfoEdit.ItemsSource = dt.DefaultView;
                ComboBoxAddInfoEdit.DisplayMemberPath = "add_information";
                ComboBoxAddInfoEdit.SelectedValuePath = "id_add_info";
                con.Close();

            }
            catch (Exception ex)
            {
                con.Close();
                MessageBox.Show("Произошла ошибка во время загрузки данных!", Title="Ошибка");
            }
            finally
            {
                da.Dispose();
                cmd.Dispose();
                con.Dispose();
            }

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
            MainWindow.hobbiesDataBase.DataBase();
            this.Close();
        }

        private void EditHobbies_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                cmd = new MySqlCommand();
                cmd.Connection = con;
                con.Open();
                if (ComboBoxAddInfoEdit.Text != "")
                {
                    cmd.CommandText = "SELECT id_add_info FROM additional_information WHERE add_information='" + ComboBoxAddInfoEdit.Text + "'";
                    dr = cmd.ExecuteReader();
                    dr.Read();
                    id_add_info = (int)dr["id_add_info"];
                    dr.Close();
                    cmd.CommandText = "UPDATE hobbies SET hobbies = @hobbies, id_add_information = @id_add_info" +
                                      " WHERE id_full_name = @id";
                    cmd.Parameters.AddWithValue("@id_add_info", id_add_info);
                }
                else
                {
                    cmd.CommandText = "UPDATE hobbies SET hobbies = @hobbies, id_add_information = 1" +
                                      " WHERE id_full_name = @id";
                }
                cmd.Parameters.AddWithValue("@id", HobbiesDataBase.id);
                cmd.Parameters.AddWithValue("@hobbies", textBoxHobbiesEdit.Text);
                cmd.ExecuteNonQuery();


                MessageBox.Show("Хобби изменено", Title = "Успех");
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
                EditHobbies_Click(sender, e);
        }
    }
}
