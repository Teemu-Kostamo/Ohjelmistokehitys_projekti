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
using System.Collections;
using System.Drawing;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Globalization;
using Microsoft.VisualBasic;
using System.Xaml;

namespace Kanban
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {

        int SelectedUserID;
        public static string rivin_id = "";


        //SQL tietokantaosoitteet ja komennot

        string Tasks_db = "Data Source=Tasks.db";
        string Users_db = "Data Source=Users.db";

        string GetToDo = "Select * from Tehtava WHERE status like 'To-Do' AND UserId =";
        string GetWIP = "Select * from Tehtava WHERE status like 'WIP' AND UserId =";
        string GetTesting = "Select * from Tehtava WHERE status like 'Testing' AND UserId=";
        string GetDone = "Select * from Tehtava WHERE status like 'Done' AND UserId =";
        string DeleteUser = "Delete from CreateNewUser WHERE Id=";
        string UpdateId = "UPDATE Tehtava Set UserId =" + GetUserID("Tyhjä").ToString() + " WHERE UserId="+Kirjautumissivu.activeUser.Id.ToString();
        public MainWindow()
        {
            InitializeComponent();
            //Populoidaan ikkunan pudotusvalikko käyttäjien nimillä
            foreach (User user in Kirjautumissivu.users)
            {
                kayttajat.Items.Add(user.Name);
            }
            //Populoidaan näkyvä nimi aktiiviseksi käyttäjäksi
            kayttajat.Text = Kirjautumissivu.activeUser.Name.ToString();
            kirjautunut_kayttaja.Text = Kirjautumissivu.activeUser.Name.ToString();




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
            string messageBoxText = "Haluatko varmasti poistaa käyttäjän?";
            string caption = "Käyttäjän poisto";
            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Warning;
            MessageBoxResult result;

            result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
            if (result == MessageBoxResult.Yes)
            {
                SQL_Command(UpdateId,Tasks_db);
                MessageBox.Show("Käyttäjä poistettu tietokannasta");
                SQL_Command(DeleteUser + Kirjautumissivu.activeUser.Id, Users_db);
                Kirjautumissivu kirsiv = new Kirjautumissivu();
                kirsiv.Show();
                Close();

            }
        }
        private void menubtnLopeta(object sender, RoutedEventArgs e)
        {
            Close();
        }

        //Taustatoiminnallisuudet-------------------------------------------
        private void kayttajat_DropDownClosed(object sender, EventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            SelectedUserID = GetUserID(kayttajat.SelectedItem.ToString());
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
            User FoundUser = null;
            foreach (User user in Kirjautumissivu.users)
            {
                if (user.Name == name)
                {
                    FoundUser= user;
                }
            }
            return FoundUser.Id;
        }

        private void List_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var row = ItemsControl.ContainerFromElement((DataGrid)sender, e.OriginalSource as DependencyObject) as DataGridRow;
            DataGrid datagrid = sender as DataGrid;
            if (row == null) return;
            DataRowView datarowview = datagrid.SelectedItem as DataRowView;


            if (datarowview != null)
            {
                rivin_id = datarowview.Row["Id"].ToString();
            }
            Tehtävän_katselu KatseluSivu = new Tehtävän_katselu();
            KatseluSivu.ShowDialog();
        }

    }

}
