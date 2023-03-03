using SQLite;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
using System.Diagnostics;
using System.Windows.Markup;
using Kanban;

namespace Kanban
{
    /// <summary>
    /// Interaction logic for Tehtavanluonti.xaml
    /// </summary>
    public partial class Tehtavanluonti : Window
    {
        public static List<Tehtava> tasks = new List<Tehtava>();   
        public static List<string> tags = new List<string>();
        string GetUniqueTags = "SELECT DISTINCT Tag from Tehtava ORDER BY Tag";
        public Tehtavanluonti()
        {
            
            
            InitializeComponent();
            foreach (User user in Kirjautumissivu.users)
            {
                TehtavaTekija.Items.Add(user.Name);
            }
            ReadTaskDatabase();
            
        }

        private void Peruuta_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void OK_Button_Click(object sender, RoutedEventArgs e)
        {
            
            

            Tehtava tehtava = new Tehtava()
            {
                Name = TehtavaNimi.Text,
                //Tag = TehtavaTagi.Text,
                Description = TehtavaKuvaus.Text,
                DueDate = TehtavaMääräaika.Text,
                Status = TehtavaStatus.Text,
            };

            foreach (User user in Kirjautumissivu.users)
            {
                if (TehtavaTekija.SelectedValue == user.Name)
                {
                    tehtava.UserId = user.Id;
                }
            }

            if (UudenTaginLisays.Text != "")
            {
                TehtavaTagi.SelectedIndex = -1;
                tehtava.Tag = UudenTaginLisays.Text;
            }
            else
            {
                tehtava.Tag = TehtavaTagi.Text;
            }

            using (SQLite.SQLiteConnection connection = new SQLite.SQLiteConnection(App.Tasks_databasePath))
            {
                connection.CreateTable<Tehtava>();
                connection.Insert(tehtava);
            }


            
            Close();
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

