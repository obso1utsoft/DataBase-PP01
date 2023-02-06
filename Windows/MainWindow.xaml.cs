
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.ComponentModel;
using MySql.Data.MySqlClient;
using System.Windows.Input;
using System.Windows.Data;
using System.Windows.Media;
using System;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Data;
using DataGrid.Windows;
using DataGrid.Pages;
using System.Windows.Navigation;
using static System.Net.Mime.MediaTypeNames;
using Windows.System;
using System.Windows.Media.Imaging;

namespace DataGrid
{
    public partial class MainWindow : Window
    {
        public static int recordsunM = 8;
        public static int recordsIsM = 13;
        public static int records;
        public static int sp;
        public static MembersDateBase membersDateBase = new();
        public static HobbiesDataBase hobbiesDataBase = new();
        public static AddInfoDataBase addInfoDataBase = new();
        public static UsersDataBase usersDataBase = new();
        public MainWindow()
        {
            InitializeComponent();

            ImageSourceConverter imageSourceConverter = new ImageSourceConverter();

            records = recordsunM;

            membersDateBase.DataBase();
            MainFrame.Navigate(membersDateBase);

            MembersButtonMenu.IsChecked = true;
            MembersButtonMenu.IsEnabled = false;

            nameUser.Text = LoginWindow.loginUserName;
            prefix.Text = LoginWindow.loginPrefix;

            if (prefix.Text == "Гость")
            {
                string packUri = "pack://application:,,,/DataGrid;component/Images/user.png";
                imageBrush.ImageSource = new ImageSourceConverter().ConvertFromString(packUri) as ImageSource;
                UsersButtonMenu.Visibility = Visibility.Hidden;
                membersDateBase.HideColumn();
                hobbiesDataBase.HideColumn();
                addInfoDataBase.HideColumn();
            }
            else
            {
                string packUri = "pack://application:,,,/DataGrid;component/Images/admin.png";
                imageBrush.ImageSource = new ImageSourceConverter().ConvertFromString(packUri) as ImageSource;
                UsersButtonMenu.Visibility = Visibility.Visible;
                membersDateBase.UnHideColumn();
                hobbiesDataBase.UnHideColumn();
                addInfoDataBase.UnHideColumn();
            }
        }
        void HandleNavigating(Object sender, NavigatingCancelEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.Forward) { e.Cancel = true; }
            if (e.NavigationMode == NavigationMode.Back) { e.Cancel = true; }
        }

        private bool IsMaximize = false;
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (IsMaximize)
                {
                    this.WindowState = WindowState.Normal;
                    this.Width = 1080;
                    this.Height = 720;

                    IsMaximize = false;

                    records = recordsunM;
                }
                else
                {
                    this.WindowState = WindowState.Maximized;

                    IsMaximize = true;

                    records = recordsIsM;
                }
                if (MembersButtonMenu.IsEnabled == false)
                    membersDateBase.DataBase();
                else if (HobbiesButtonMenu.IsEnabled == false)
                    hobbiesDataBase.DataBase();
                else if (AddInfoButtonMenu.IsEnabled == false)
                    addInfoDataBase.DataBase();
                else if (UsersButtonMenu.IsEnabled == false)
                    usersDataBase.DataBase();

            }
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            this.Close();
            loginWindow.Show();
        }

        private void MembersButtonMenu_Click(object sender, RoutedEventArgs e)
        {
            membersDateBase.DataBase();

            MainFrame.Navigate(membersDateBase);

            MembersButtonMenu.IsChecked = true;
            HobbiesButtonMenu.IsChecked = false;
            AddInfoButtonMenu.IsChecked = false;
            UsersButtonMenu.IsChecked = false;
            MembersButtonMenu.IsEnabled = false;
            HobbiesButtonMenu.IsEnabled = true;
            AddInfoButtonMenu.IsEnabled = true;
            UsersButtonMenu.IsEnabled = true;
        }

        private void HobbiesButtonMenu_Click(object sender, RoutedEventArgs e)
        {
            hobbiesDataBase.DataBase();

            MainFrame.Navigate(hobbiesDataBase);

            MembersButtonMenu.IsChecked = false;
            HobbiesButtonMenu.IsChecked = true;
            AddInfoButtonMenu.IsChecked = false;
            UsersButtonMenu.IsChecked = false;
            MembersButtonMenu.IsEnabled = true;
            HobbiesButtonMenu.IsEnabled = false;
            AddInfoButtonMenu.IsEnabled = true;
            UsersButtonMenu.IsEnabled = true;
        }

        private void AddInfoButtonMenu_Click(object sender, RoutedEventArgs e)
        {


            addInfoDataBase.DataBase();

            MainFrame.Navigate(addInfoDataBase);

            MembersButtonMenu.IsChecked = false;
            HobbiesButtonMenu.IsChecked = false;
            AddInfoButtonMenu.IsChecked = true;
            UsersButtonMenu.IsChecked = false;
            MembersButtonMenu.IsEnabled = true;
            HobbiesButtonMenu.IsEnabled = true;
            AddInfoButtonMenu.IsEnabled = false;
            UsersButtonMenu.IsEnabled = true;
        }

        private void UsersButtonMenu_Click(object sender, RoutedEventArgs e)
        {
            usersDataBase.DataBase();

            MainFrame.Navigate(usersDataBase);

            MembersButtonMenu.IsChecked = false;
            HobbiesButtonMenu.IsChecked = false;
            AddInfoButtonMenu.IsChecked = false;
            UsersButtonMenu.IsChecked = true;
            MembersButtonMenu.IsEnabled = true;
            HobbiesButtonMenu.IsEnabled = true;
            AddInfoButtonMenu.IsEnabled = true;
            UsersButtonMenu.IsEnabled = false;
        }
    }
}