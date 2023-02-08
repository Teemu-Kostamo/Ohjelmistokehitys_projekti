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

namespace Käyttäjäsivu
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        private string login_error = "Kirjautuminen ei onnistunut, koska käyttäjätunnus ja salasana eivät täsmää.";
        private string user_found_error = "Käyttäjä löytyy jo.";
        private string user_added = "Käyttäjä lisätty.";
        private string login_success = "Tervetuloa!";
        private string invalid_signup = "Käyttäjätunnus tai salasana ei voi olla tyhjä";

        private SolidColorBrush error = new SolidColorBrush(Colors.Red);
        private SolidColorBrush normal = new SolidColorBrush(Colors.Black);

        List<User> users = new List<User>();
        public MainWindow()
        {
            InitializeComponent();
        }
        // Nollataan käyttäjäsyöte kentät
        private void Reset_Input()
        {
            Signer.Text= string.Empty;
            signerpass.Password= string.Empty;
        }

        // "Kirjaudu sisään" painikkeen toiminnallisuudet
        private void Login_Button_Click(object sender, RoutedEventArgs e)
        {
            string nimi = Signer.Text;
            string pass = signerpass.Password;
            // Käyttäjä ja salasana täsmäävät
            foreach (User use in users)
            {
                if (use.Get_Username()== nimi && use.Get_Password() == pass) 
                {
                    Reset_Input();
                    login_error_text.Foreground = normal;
                    login_error_text.Text = login_success;
                }
                // Käyttäjä tai salasana eivät täsmää
                else
                {
                    Reset_Input();
                    login_error_text.Foreground = error;
                    login_error_text.Text = login_error;
                }
            }
        }

        // "Lisää uusi" painikkeen toiminnallisuudet
        private void Signup_Button_Click(object sender, RoutedEventArgs e)
        {
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
        }
    }
}
