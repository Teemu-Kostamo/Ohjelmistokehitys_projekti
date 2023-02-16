using SQLite;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for Tehtavanluonti.xaml
    /// </summary>
    public partial class Tehtavanluonti : Window
    {
        public Tehtavanluonti()
        {
            InitializeComponent();
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
                Tag = TehtavaTagi.Text,
                Description = TehtavaKuvaus.Text,
                DueDate = TehtavaMääräaika.Text,
                Status = TehtavaStatus.Text
            };

            using (SQLiteConnection connection = new SQLiteConnection(App.Tasks_databasePath))
            {
                connection.CreateTable<Tehtava>();
                connection.Insert(tehtava);
            }

            Close();
        }
    }
}
