using DataGrid.Connection;
using Google.Protobuf.WellKnownTypes;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Windows.Graphics;
using Windows.Networking;

namespace DataGrid.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddMembers.xaml
    /// </summary>
    public partial class AddMembers : Window
    {
        MySqlConnection con = ConnectionToDb.con;
        MySqlCommand cmd;
        MySqlDataReader dr;
        public AddMembers()
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
            MainWindow.membersDateBase.DataBase();
            this.Close();
        }

        private void AddNewMember_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime dateValue = DateTime.Parse(textBoxBirthYearAdd.Text);
                var sqlDateFormat = dateValue.ToString("yyyy.MM.dd");

                cmd = new MySqlCommand();
                cmd.Connection = con;
                con.Open();
                cmd.CommandText = "INSERT INTO members ( id, full_name, class, birth_year ) VALUES ( NULL, @full_name, @class, @birth_year )";
                cmd.Parameters.AddWithValue("@full_name", textBoxNameAdd.Text);
                cmd.Parameters.AddWithValue("@class", textBoxClassAdd.Text);
                cmd.Parameters.AddWithValue("@birth_year", sqlDateFormat);
                cmd.ExecuteNonQuery();

                cmd.CommandText = "SELECT * FROM members WHERE full_name='" + textBoxNameAdd.Text + "'";
                dr = cmd.ExecuteReader();
                dr.Read();
                int full_name = (int)dr["id"];
                dr.Close();

                cmd.CommandText = "INSERT INTO hobbies( id, hobbies, id_add_information, id_full_name ) VALUES ( NULL, @hobbies, 1, @id_full_name )";
                cmd.Parameters.AddWithValue("@hobbies", textBoxHobbiesAdd.Text);
                cmd.Parameters.AddWithValue("@id_full_name", full_name);
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Участник и хобби добавлены", Title = "Успех");
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

        private void Border_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                AddNewMember_Click(sender, e);
        }
    }
}
