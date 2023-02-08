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

namespace Kanban
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class Kirjautumissivu : Window
    {
        private string login_error = "Kirjautuminen ei onnistunut, koska käyttäjätunnus ja salasana eivät täsmää.";

        private SolidColorBrush error = new SolidColorBrush(Colors.Red);
        private SolidColorBrush normal = new SolidColorBrush(Colors.Black);

        List<User> users = new List<User>();
        public Kirjautumissivu()
        {
            InitializeComponent();
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
            MainWindow paaohjelma = new MainWindow();
            paaohjelma.Show();
            Close();
            // Käyttäjä ja salasana täsmäävät
            //ALLA OLEVA TOIMIVA KOODI ÄLÄ POISTA
            /*
            foreach (User use in users)
            {
                if (use.Get_Username() == nimi && use.Get_Password() == pass)
                {
                    Reset_Input();
                    MainWindow paaohjelma = new MainWindow();
                    paaohjelma.Show();
                }
                // Käyttäjä tai salasana eivät täsmää
                else
                {
                    Reset_Input();
                    login_error_text.Foreground = error;
                    login_error_text.Text = login_error;
                }
            }*/
        }

        // "Lisää uusi" painikkeen toiminnallisuudet
        private void Signup_Button_Click(object sender, RoutedEventArgs e)
        {
            //Siirrytään käyttäjänluonti ikkunaan sulkematta kirjautumissivua
            Kayttajanluonti kayttajanluonti = new Kayttajanluonti();
            kayttajanluonti.Show();

            //TOIMIVA KOODI MAHDOLLISESTI KÄYTTÄJÄNLUONTIIN?
            /*
            string nimi = Signer.Text;
            string pass = signerpass.Password;
            bool user_found = false;

            // Lisätessä uutta käyttäjää käyttäjä löytyy jo
            foreach (User use in users)
            {
                if (nimi == use.Get_Username())
                {
                    user_found = true;
                }
            }
            if (user_found) 
            { 
                Reset_Input();
                login_error_text.Foreground = error;
                login_error_text.Text = user_found_error;
            }
            // Luodaan uusi käyttäjä
            else
            {
                //Käyttäjätunnus ja salasana eivät tyhjiä -> Hyväksytään uusi käyttäjä
                if (nimi != string.Empty && pass != string.Empty)
                {
                    users.Add(new User(nimi, pass));
                    login_error_text.Foreground = normal;
                    login_error_text.Text = user_added;
                    Reset_Input();
                }
                //Käyttäjätunnus tai salasana tyhjiä -> palautetaan virheilmoitus
                else if (nimi == string.Empty || pass == string.Empty) 
                {
                    login_error_text.Foreground = error;
                    login_error_text.Text = invalid_signup;
                    Reset_Input();
                }
            }
            */
        }
    }
}
