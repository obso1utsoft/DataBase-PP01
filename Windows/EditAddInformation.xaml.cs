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
    /// Логика взаимодействия для EditAddInformation.xaml
    /// </summary>
    public partial class EditAddInformation : Window
    {
        MySqlConnection con = ConnectionToDb.con;
        MySqlCommand cmd;

        public EditAddInformation()
        {
            InitializeComponent();

            membersShow();
        }
        private void membersShow()
        {
            textBoxAddInformationEdit.Text = AddInfoDataBase.add_information;
            textBoxSpecializationEdit.Text = AddInfoDataBase.specialization;
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
            MainWindow.addInfoDataBase.DataBase();
            this.Close();
        }

        private void EditAddInfo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                cmd = new MySqlCommand();
                cmd.Connection = con;
                con.Open();
                cmd.CommandText = "UPDATE additional_information SET add_information = @add_information, specialization = @specialization" +
                                  " WHERE id_add_info = @id";
                cmd.Parameters.AddWithValue("@id", AddInfoDataBase.id);
                cmd.Parameters.AddWithValue("@add_information", textBoxAddInformationEdit.Text);
                cmd.Parameters.AddWithValue("@specialization", textBoxSpecializationEdit.Text);
                cmd.ExecuteNonQuery();


                MessageBox.Show("Клуб изменен", Title = "Успезх");
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
                EditAddInfo_Click(sender, e);
        }
    }
}
