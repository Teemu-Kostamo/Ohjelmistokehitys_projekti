using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using SQLitePCL;

namespace Kanban
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        //Tietokantojen osoitteet
        public static string Users_databasePath = "Users.db";
        public static string Tasks_databasePath = "Tasks.db";
    }
}
