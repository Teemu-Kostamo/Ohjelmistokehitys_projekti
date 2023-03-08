using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Kanban
{
    /// <summary>
    /// Interaction logic for Tehtävän_katselu.xaml
    /// </summary>
    public partial class Tehtävän_katselu : Window
    {
        string task_id = MainWindow.rivin_id;

        public static List<Tehtava> tasks = new List<Tehtava>();
        public static List<string> tags = new List<string>();

        string Tasks_db = "Data Source=Tasks.db";
        string Users_db = "Data Source=Users.db";

        string GetTaskId = "Select * from Tehtava WHERE Id=";
        public Tehtävän_katselu()
        {
            InitializeComponent();
            foreach (User user in Kirjautumissivu.users)
            {
                TehtavaTekija.Items.Add(user.Name);
            }
            ReadTaskDatabase();
            SQL_Command(GetTaskId + task_id, Tasks_db);
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
                adap.Update(dt);
                conn.Close();
                TehtavaNimi.Text = dt.Rows[0][1].ToString();
                TehtavaKuvaus.Text = dt.Rows[0][2].ToString();
                TehtavaTagi.Text = dt.Rows[0][3].ToString();
                TehtavaStatus.Text = dt.Rows[0][5].ToString();
                TehtavaMääräaika.Text = dt.Rows[0][4].ToString();
                TehtavaTekija.Text = Get_username(Int32.Parse(dt.Rows[0][6].ToString()));

            }
        }
        public string Get_username(int userid)
        {
            foreach (User user in Kirjautumissivu.users)
            {
                if (userid == user.Id)
                {
                    return user.Name;
                }
            }
            return null;
        }
        public string Get_userid(string username)
        {
            foreach (User user in Kirjautumissivu.users)
            {
                if (username == user.Name)
                {
                    return user.Id.ToString();
                }
            }
            return null;
        }

        public void btnPalaa_paaikkunaan(object sender, EventArgs e)
        {
            Close();
        }
        private void SaveEdits_Click(object sender, RoutedEventArgs e)
        {
            string UpdateTask = "UPDATE Tehtava Set Name = '" + TehtavaNimi.Text + "', Description ='" + TehtavaKuvaus.Text + "', Tag ='" + TehtavaTagi.Text + "',DueDate = '" + TehtavaMääräaika.Text + "',Status = '" + TehtavaStatus.Text + "',UserId = '" + Get_userid(TehtavaTekija.Text) + "' WHERE Id=" + task_id;
            using (System.Data.SQLite.SQLiteConnection conn = new System.Data.SQLite.SQLiteConnection(Tasks_db))
            {
                conn.Open();

                System.Data.SQLite.SQLiteCommand command = new System.Data.SQLite.SQLiteCommand(UpdateTask, conn);
                command.ExecuteNonQuery();
                conn.Close();
                Close();
            }
        }
        void ReadTaskDatabase()
        {
            using (SQLite.SQLiteConnection connection = new SQLite.SQLiteConnection(App.Tasks_databasePath))
            {
                connection.CreateTable<Tehtava>();
                tasks = (connection.Table<Tehtava>().ToList()).OrderBy(u => u.Id).ToList();
                foreach (Tehtava tehtava in tasks)
                {
                    if (tehtava.Tag != "")
                    {
                        tags.Add(tehtava.Tag);
                    }
                }
            }
            TehtavaTagi.ItemsSource = tags.Distinct();
        }
    }
}
