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
    /// Логика взаимодействия для EditMembers.xaml
    /// </summary>
    public partial class EditMembers : Window
    {
        MySqlConnection con = ConnectionToDb.con;
        MySqlCommand cmd;
        public EditMembers()
        {
            InitializeComponent();

            membersShow();
        }
        private void membersShow()
        {
            textBoxNameEdit.Text = MembersDateBase.full_name;
            textBoxClassEdit.Text = MembersDateBase.Class;
            textBoxBirthYearEdit.Text = MembersDateBase.birth_year;
            textBoxHobbiesEdit.Text = MembersDateBase.hobbies;
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

        private void EditNewMember_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime dateValue = DateTime.Parse(textBoxBirthYearEdit.Text);
                var sqlDateFormat = dateValue.ToString("yyyy.MM.dd");

                cmd = new MySqlCommand();
                cmd.Connection = con;
                con.Open();
                cmd.CommandText = "UPDATE members SET full_name = @full_name," +
                                  "class = @class, birth_year = @birth_year" +
                                  " WHERE id = @id;" +
                                  " UPDATE hobbies SET hobbies = @hobbies" +
                                  " WHERE id_full_name = @id";
                cmd.Parameters.AddWithValue("@id", MembersDateBase.id);
                cmd.Parameters.AddWithValue("@full_name", textBoxNameEdit.Text);
                cmd.Parameters.AddWithValue("@class", textBoxClassEdit.Text);
                cmd.Parameters.AddWithValue("@birth_year", sqlDateFormat);
                cmd.Parameters.AddWithValue("@hobbies", textBoxHobbiesEdit.Text);
                cmd.ExecuteNonQuery();


                MessageBox.Show("Участник изменен", Title = "Успех");
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
                EditNewMember_Click(sender, e);
        }
    }
}
