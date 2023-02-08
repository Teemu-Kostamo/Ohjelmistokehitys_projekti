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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Kanban
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int testi = 0;
        public MainWindow()
        {
            InitializeComponent();
        }
        
        private void btnLuoUusiKayttaja_Click(object sender, RoutedEventArgs e)
        {
            testi = 1;
            /*
            NewUser newUser = new NewUser();
            newUser.Owner = this;
            newUser.ShowDialog();
            */
        }

        private void btnLuoUusiTaski_Click(object sender, RoutedEventArgs e)
        {
            testi = 2;
            /*
            NewTask newTask = new NewTask();
            newTask.Owner = this;
            newTask.ShowDialog();
            */
        }
    }
}
