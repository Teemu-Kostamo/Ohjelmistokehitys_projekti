using SQLite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
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
using System.Collections;


namespace Kanban
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int SelectedUserID;


        //SQL tietokantaosoitteet ja komennot

        string Tasks_db = "Data Source=Tasks.db";
        string Users_db = "Data Source=Users.db";

        string GetToDo = "Select * from Tehtava WHERE status like 'To-Do' AND UserId =";
        string GetWIP = "Select * from Tehtava WHERE status like 'WIP' AND UserId =";
        string GetTesting = "Select * from Tehtava WHERE status like 'Testing' AND UserId=";
        string GetDone = "Select * from Tehtava WHERE status like 'Done' AND UserId =";
        string DeleteUser = "Delete from CreateNewUser WHERE Id=";
        public MainWindow()
        {
            InitializeComponent();
            //Populoidaan ikkunan pudotusvalikko käyttäjien nimillä
            foreach (CreateNewUser user in Kirjautumissivu.users)
            {
                kayttajat.Items.Add(user.Name);
            }
            //Populoidaan näkyvä nimi aktiiviseksi käyttäjäksi
            kayttajat.Text = Kirjautumissivu.activeUser.Name.ToString();
        }

        //Painikkeiden toiminnallisuudet--------------------------------------
        private void btnLuoUusiTaski_Click(object sender, RoutedEventArgs e)
        {
            Tehtavanluonti newTask = new Tehtavanluonti();
            newTask.ShowDialog();
        }

        //Menun toiminnallisuudet---------------------------------------------
        private void menubtnKirjauduUlos(object sender, RoutedEventArgs e)
        {
            Kirjautumissivu kirjautumissivu = new Kirjautumissivu();
            Close();
            kirjautumissivu.ShowDialog();
        }
        private void menubtnPoistaKayttaja(object sender, RoutedEventArgs e)
        {
            //Käyttäjän poiston funktio toimii
            //Täytyy vielä rakentaa poiston myötä
            //automaatio, että käyttäjän tekemättömät
            //taskit siirtyvät backlogiin kaikkien 
            //käyttöön

            //Lisäksi tarvitsee tehdä varmistus popup
            SQL_Command(DeleteUser + Kirjautumissivu.activeUser.Id, Users_db);
            Kirjautumissivu kirsiv = new Kirjautumissivu();
            kirsiv.Show();
            Close();
        }
        private void menubtnLopeta(object sender, RoutedEventArgs e)
        {
            Close();
        }

        //Taustatoiminnallisuudet-------------------------------------------
        private void kayttajat_DropDownClosed(object sender, EventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            SelectedUserID = GetUserID(cmb.SelectedItem.ToString());
            SQL_Command(GetToDo + SelectedUserID,Tasks_db);
            SQL_Command(GetWIP + SelectedUserID,Tasks_db);
            SQL_Command(GetTesting + SelectedUserID, Tasks_db);
            SQL_Command(GetDone+ SelectedUserID, Tasks_db);
        }
        public void SQL_Command(string comm, string db)
        {

            using (System.Data.SQLite.SQLiteConnection conn = new System.Data.SQLite.SQLiteConnection(db))
            {
                conn.Open();

                System.Data.SQLite.SQLiteCommand command = new System.Data.SQLite.SQLiteCommand(comm, conn);
                command.ExecuteNonQuery();

                SQLiteDataAdapter adap = new SQLiteDataAdapter(command);

                DataTable dt = new DataTable("Tasks");
                adap.Fill(dt);
                if (comm.Contains("To-Do"))
                {
                toDoList.ItemsSource = dt.DefaultView;
                }
                else if (comm.Contains("WIP"))
                {
                    wipList.ItemsSource = dt.DefaultView;
                }
                else if (comm.Contains("Testing"))
                {
                    testingList.ItemsSource = dt.DefaultView;
                }
                else if (comm.Contains("Done"))
                {
                    doneList.ItemsSource = dt.DefaultView;
                }
                adap.Update(dt);

                conn.Close();

            }
        }
        private static int GetUserID(string name)
        {
            CreateNewUser FoundUser = null;
            foreach (CreateNewUser user in Kirjautumissivu.users)
            {
                if (user.Name == name)
                {
                    FoundUser= user;
                }
            }
            return FoundUser.Id;
        }
    }
}
