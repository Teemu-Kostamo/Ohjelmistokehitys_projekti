using System;
using System.Data;
using System.Data.SQLite;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

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
        string DeleteUser = "Delete from User WHERE Id=";
        string UpdateId = "UPDATE Tehtava Set UserId =" + GetUserID("Tyhjä").ToString() + " WHERE UserId=" + Kirjautumissivu.activeUser.Id.ToString();
        string SetToDo = "UPDATE Tehtava Set Status = 'To-Do' WHERE Id=" + rivin_id;
        string SetWIP = "UPDATE Tehtava Set Status = 'WIP' WHERE Id=" + rivin_id;
        string SetTesting = "UPDATE Tehtava Set Status = 'Testing' WHERE Id=" + rivin_id;
        string SetDone = "UPDATE Tehtava Set Status = 'Done' WHERE Id=" + rivin_id;
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
            SQL_Command(GetToDo + SelectedUserID, Tasks_db);
            SQL_Command(GetWIP + SelectedUserID, Tasks_db);
            SQL_Command(GetTesting + SelectedUserID, Tasks_db);
            SQL_Command(GetDone + SelectedUserID, Tasks_db);
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            SQL_Command(GetToDo + SelectedUserID, Tasks_db);
            SQL_Command(GetWIP + SelectedUserID, Tasks_db);
            SQL_Command(GetTesting + SelectedUserID, Tasks_db);
            SQL_Command(GetDone + SelectedUserID, Tasks_db);
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
                SQL_Command(UpdateId, Tasks_db);
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
            SQL_Command(GetToDo + SelectedUserID, Tasks_db);
            SQL_Command(GetWIP + SelectedUserID, Tasks_db);
            SQL_Command(GetTesting + SelectedUserID, Tasks_db);
            SQL_Command(GetDone + SelectedUserID, Tasks_db);

        }
        public void SQL_Command(string comm, string db)
        {
            //Tarkistetaan jos DB on olemassa ja luodaan tarvittaessa.
            if (!System.IO.File.Exists(@"Tasks.db"))
            {
                System.Data.SQLite.SQLiteConnection.CreateFile(@"Tasks.db");

                using (var sqlite2 = new System.Data.SQLite.SQLiteConnection(@"Data Source=Tasks.db"))
                {
                    sqlite2.Open();
                    string sql = "CREATE TABLE \"Tehtava\" (\"Id\" integer NOT NULL,\"Name\" varchar,\"Description\" varchar,\"Tag\" varchar,\"DueDate\" varchar, \"Status\" varchar,\"UserId\" INTEGER, PRIMARY KEY (\"Id\" AUTOINCREMENT))";
                    System.Data.SQLite.SQLiteCommand command = new System.Data.SQLite.SQLiteCommand(sql, sqlite2);
                    command.ExecuteNonQuery();
                }
            }
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
            User FoundUser = new User();
            foreach (User user in Kirjautumissivu.users)
            {
                if (user.Name == name)
                {
                    FoundUser = user;
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

        //Rivin valinta
        private DataRowView FindDataRowViewFor(TextBlock textCell)
        {
            var asGridRow = textCell.BindingGroup.Owner as DataGridRow;
            if (asGridRow != null)
            {
                return asGridRow.DataContext as DataRowView;

            }
            return null;


        }

        //Dragin aloitukset

        private void toDoList_MouseMove(object sender, MouseEventArgs e)
        {
            TextBlock valittuRivi = sender as TextBlock;
            if (valittuRivi != null && e.LeftButton == MouseButtonState.Pressed)
            {
                DataRowView draggedRow = FindDataRowViewFor(valittuRivi);
                rivin_id = draggedRow.Row["Id"].ToString();
                DragDrop.DoDragDrop(toDoList, draggedRow, DragDropEffects.Move);

            }
        }

        private void wipList_MouseMove(object sender, MouseEventArgs e)
        {
            TextBlock valittuRivi = sender as TextBlock;
            if (valittuRivi != null && e.LeftButton == MouseButtonState.Pressed)
            {
                DataRowView draggedRow = FindDataRowViewFor(valittuRivi);
                rivin_id = draggedRow.Row["Id"].ToString();
                DragDrop.DoDragDrop(wipList, draggedRow, DragDropEffects.Move);

            }
        }

        private void testingList_MouseMove(object sender, MouseEventArgs e)
        {
            TextBlock valittuRivi = sender as TextBlock;
            if (valittuRivi != null && e.LeftButton == MouseButtonState.Pressed)
            {
                DataRowView draggedRow = FindDataRowViewFor(valittuRivi);
                rivin_id = draggedRow.Row["Id"].ToString();
                DragDrop.DoDragDrop(testingList, draggedRow, DragDropEffects.Move);

            }
        }

        private void doneList_MouseMove(object sender, MouseEventArgs e)
        {
            TextBlock valittuRivi = sender as TextBlock;
            if (valittuRivi != null && e.LeftButton == MouseButtonState.Pressed)
            {
                DataRowView draggedRow = FindDataRowViewFor(valittuRivi);
                rivin_id = draggedRow.Row["Id"].ToString();
                DragDrop.DoDragDrop(doneList, draggedRow, DragDropEffects.Move);

            }
        }

        // Dropit

        private void HandleDropWIP(Object sender, DragEventArgs e)
        {

            SQL_Command(SetWIP + rivin_id, Tasks_db);
            SQL_Command(GetToDo + SelectedUserID, Tasks_db);
            SQL_Command(GetWIP + SelectedUserID, Tasks_db);
            SQL_Command(GetTesting + SelectedUserID, Tasks_db);
            SQL_Command(GetDone + SelectedUserID, Tasks_db);

        }
        private void HandleDropToDo(Object sender, DragEventArgs e)
        {

            SQL_Command(SetToDo + rivin_id, Tasks_db);
            SQL_Command(GetToDo + SelectedUserID, Tasks_db); ;
            SQL_Command(GetWIP + SelectedUserID, Tasks_db);
            SQL_Command(GetTesting + SelectedUserID, Tasks_db);
            SQL_Command(GetDone + SelectedUserID, Tasks_db);

        }
        private void HandleDropTesting(Object sender, DragEventArgs e)
        {

            SQL_Command(SetTesting + rivin_id, Tasks_db);
            SQL_Command(GetToDo + SelectedUserID, Tasks_db);
            SQL_Command(GetWIP + SelectedUserID, Tasks_db);
            SQL_Command(GetTesting + SelectedUserID, Tasks_db);
            SQL_Command(GetDone + SelectedUserID, Tasks_db);
        }
        private void HandleDropDone(Object sender, DragEventArgs e)
        {

            SQL_Command(SetDone + rivin_id, Tasks_db);
            SQL_Command(GetToDo + SelectedUserID, Tasks_db);
            SQL_Command(GetWIP + SelectedUserID, Tasks_db);
            SQL_Command(GetTesting + SelectedUserID, Tasks_db);
            SQL_Command(GetDone + SelectedUserID, Tasks_db);

        }






    }

    public class DateTimeToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var pvm = value.ToString();
            DateTime myDate;
            DateTime.TryParse(pvm, out myDate);
            return myDate < DateTime.Now;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
