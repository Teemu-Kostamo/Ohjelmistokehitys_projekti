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
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //Populoidaan ikkunan pudotusvalikko käyttäjien nimillä
            foreach (CreateNewUser user in Kirjautumissivu.users)
            {
                kayttajat.Items.Add(user.Name);
            }
            //Populoidaan näkyvä nimi aktiiviseksi käyttäjäksi
            kayttajat.Text= Kirjautumissivu.activeUser.Name.ToString();
        }
        
        private void btnLuoUusiTaski_Click(object sender, RoutedEventArgs e)
        {
            Tehtavanluonti newTask = new Tehtavanluonti();
            newTask.ShowDialog();
        }
        private void menubtnKirjauduUlos(object sender, RoutedEventArgs e)
        {
            Kirjautumissivu kirjautumissivu = new Kirjautumissivu();
            Close();
            kirjautumissivu.ShowDialog();
        }
        private void menubtnLopeta(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void menubtnPoistaKayttaja(object sender, RoutedEventArgs e) 
        {
            Debug.WriteLine(null);
            //Käyttäjän poiston funktion placeholder,
            //Mahdollisuus poistaa aktiivinen käyttäjä
            //ilman tietokannan suoraa manipulaatiota
        }
    }
}
