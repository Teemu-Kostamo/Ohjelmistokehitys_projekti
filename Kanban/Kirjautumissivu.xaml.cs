using SQLite;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Kanban
{
    public partial class Kirjautumissivu : Window
    {
        private string login_error = "Kirjautuminen ei onnistunut, koska käyttäjätunnus ja salasana eivät täsmää.";

        public static List<User> users = new List<User>();
        public static User activeUser;

        string SQL_User = "Select Username and Password from CreateNewUser";
        public Kirjautumissivu()
        {
            InitializeComponent();
            ReadUserDataBase();
        }
        //Tietokannan haku ja määrittämättömän käyttäjän luonti tarvittaessa
        void ReadUserDataBase()
        {
            bool tyhja_tekija = false;
            using (SQLiteConnection conn = new SQLiteConnection(App.Users_databasePath))
            {
                conn.CreateTable<User>();
                users = (conn.Table<User>().ToList()).OrderBy(u => u.Id).ToList();
                foreach (User user in users)
                {
                    if (user.Name == "Määrittämättömät")
                    {
                        tyhja_tekija = true;
                        break;
                    }
                }
                if (tyhja_tekija != true)
                {
                    User tyhja = new User()
                    {
                        Name = "Määrittämättömät"

                    };
                    conn.Insert(tyhja);
                }
            }
        }
        // Nollataan käyttäjäsyöte kentät
        private void Reset_Input()
        {
            Signer.Text = string.Empty;
            signerpass.Password = string.Empty;
        }

        // "Kirjaudu sisään" painikkeen toiminnallisuudet
        private void Login_Button_Click(object sender, RoutedEventArgs e)
        {
            string nimi = Signer.Text;
            string pass = signerpass.Password;
            bool correct = false;

            ReadUserDataBase();

            foreach (User user in users)
            {
                if (user.Username == nimi && user.Password == pass)
                {
                    correct = true;
                    activeUser = user;
                    break;
                }
            }
            if (correct == true)
            {
                MainWindow paaohjelma = new MainWindow();
                paaohjelma.Show();
                Close();
            }
            else
            {
                login_error_text.Text = login_error;
                Reset_Input();
            }
        }

        // "Lisää uusi" painikkeen toiminnallisuudet
        private void Signup_Button_Click(object sender, RoutedEventArgs e)
        {
            //Siirrytään käyttäjänluonti ikkunaan sulkematta kirjautumissivua
            Kayttajanluonti kayttajanluonti = new Kayttajanluonti();
            kayttajanluonti.ShowDialog();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ReadUserDataBase();
            activeUser = users[0];
            MainWindow paaohjelma = new MainWindow();
            paaohjelma.Show();
            Close();
        }
    }
}
