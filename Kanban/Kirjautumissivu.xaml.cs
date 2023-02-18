using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using User_Class;
using SQLite;

namespace Kanban
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class Kirjautumissivu : Window
    {
        public static string kayttaja = "";
        private string login_error = "Kirjautuminen ei onnistunut, koska käyttäjätunnus ja salasana eivät täsmää.";

        private SolidColorBrush error = new SolidColorBrush(Colors.Red);
        private SolidColorBrush normal = new SolidColorBrush(Colors.Black);

        public static List<CreateNewUser> users = new List<CreateNewUser>();
        public static CreateNewUser activeUser = null;
        public Kirjautumissivu()
        {
            InitializeComponent();
            Debug.WriteLine("TESTITESTITESTI FOLDERPATH: "+App.Users_databasePath);

            ReadDataBase();
        }
        //Tietokannan haku funktio
        void ReadDataBase()
        {
            using (SQLiteConnection conn = new SQLiteConnection(App.Users_databasePath))
            {
                conn.CreateTable<CreateNewUser>();
                users = (conn.Table<CreateNewUser>().ToList()).OrderBy(u => u.Id).ToList();
                Debug.WriteLine("DATABASE");
                if (users == null)
                {
                    Debug.WriteLine("Tyhjä");
                }
                else
                {
                    foreach (CreateNewUser user in users)
                    {
                        Debug.WriteLine(user.Username);
                    }
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

            ReadDataBase();

            foreach (CreateNewUser user in users)
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
            kayttajanluonti.Show();
        }
    }
}
