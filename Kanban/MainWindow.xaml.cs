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
        public static List<Tehtava> tasks = new List<Tehtava>();
        int SelectedUserID;
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
            Debug.WriteLine(null);
            //Käyttäjän poiston funktion placeholder,
            //Mahdollisuus poistaa aktiivinen käyttäjä
            //ilman tietokannan suoraa manipulaatiota
        }
        private void menubtnLopeta(object sender, RoutedEventArgs e)
        {
            Close();
        }

        //Taustatoiminnallisuudet-------------------------------------------
        private void kayttajat_DropDownClosed(object sender, EventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            //testinimi.Text = cmb.SelectedItem.ToString();
            SelectedUserID = GetUserID(cmb.SelectedItem.ToString());
        }

        void ReadTaskDataBase()
        {
            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.Users_databasePath))
            {
                conn.CreateTable<Tehtava>();
                tasks = (conn.Table<Tehtava>().ToList()).OrderBy(u => u.Id).ToList();
                Debug.WriteLine("TASK-DATABASE");
                if (tasks == null)
                {
                    Debug.WriteLine("Tyhjä");
                }
                else
                {
                    foreach (Tehtava task in tasks)
                    {
                        Debug.WriteLine(task.Id);
                    }
                }
            }
        }

        public void getToDo()
        {

            using (System.Data.SQLite.SQLiteConnection conn = new System.Data.SQLite.SQLiteConnection("Data Source=C:\\Users\\warde\\source\\repos\\Ohjelmistokehitys_projekti4\\Kanban\\Tasks.db;"))
            {
                conn.Open();

                System.Data.SQLite.SQLiteCommand command = new System.Data.SQLite.SQLiteCommand("Select * from Tehtava WHERE status like 'To-Do' AND UserID =" + SelectedUserID + "", conn);
                command.ExecuteNonQuery();

                SQLiteDataAdapter adap = new SQLiteDataAdapter(command);

                DataTable dt = new DataTable("Tasks");
                adap.Fill(dt);

                toDoList.ItemsSource = dt.DefaultView;
                adap.Update(dt);

                conn.Close();

            }
        }
        public void getWIP()
        {

            using (System.Data.SQLite.SQLiteConnection conn = new System.Data.SQLite.SQLiteConnection("Data Source=C:\\jostain\\syystä\\koko\\polku\\Ohjelmistokehitys_projekti3\\Kanban\\Kanban\\Tasks.db;"))
            {
                conn.Open();

                System.Data.SQLite.SQLiteCommand command = new System.Data.SQLite.SQLiteCommand("Select * from Tehtava WHERE status like 'WIP'", conn);
                command.ExecuteNonQuery();

                SQLiteDataAdapter adap = new SQLiteDataAdapter(command);

                DataTable dt = new DataTable("Tasks");
                adap.Fill(dt);

                wipList.ItemsSource = dt.DefaultView;
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

        //vaan tilapäisiä nappuloita joilla päivitetään
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            getToDo();
        }
        private void Button_Click2(object sender, RoutedEventArgs e)
        {
            getWIP();
        }
    }
}
