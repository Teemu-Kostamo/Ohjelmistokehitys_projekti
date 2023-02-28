using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Data;
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
using System.Windows.Shapes;

namespace Kanban
{
    /// <summary>
    /// Interaction logic for Tehtävän_katselu.xaml
    /// </summary>
    public partial class Tehtävän_katselu : Window
    {
        string testi = MainWindow.rivin_id;

        string Tasks_db = "Data Source=Tasks.db";
        string Users_db = "Data Source=Users.db";

        string GetTaskId = "Select * from Tehtava WHERE Id=";
        public Tehtävän_katselu()
        {
            InitializeComponent();
            SQL_Command(GetTaskId + testi, Tasks_db);
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
                TehtavaTagi.Text= dt.Rows[0][3].ToString();
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
        public void btnPalaa_paaikkunaan(object sender, EventArgs e)
        {
            Close();
        }
    }
}
