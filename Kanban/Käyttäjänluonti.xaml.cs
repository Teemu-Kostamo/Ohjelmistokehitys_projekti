using SQLite;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Kanban
{
    /// <summary>
    /// Interaction logic for SubWindow.xaml
    /// </summary>
    public partial class Kayttajanluonti : Window
    {
        public Kayttajanluonti()
        {
            InitializeComponent();
        }

        private async void lastName_TextChanged(object sender, TextChangedEventArgs e)
        {
            await Task.Delay(4000);
            userName.Text = (firstName.Text.Substring(0, 3) + lastName.Text.Substring(0, 3)).ToLower();
        }
        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            User u = new User()
            {
                Username = userName.Text,
                Name = firstName.Text + " " + lastName.Text,
                Password = Convert.ToString(passWord.Password),
                Role = rolePosition.Text,
            };

            Close();

            using (SQLiteConnection connection = new SQLiteConnection(App.Users_databasePath))
            {
                connection.CreateTable<User>();
                connection.Insert(u);
            }
        }
    }
}
