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
        //Tietokantojen nimien määritys
        static string Users_databaseName = "Users.db";
        static string Tasks_databaseName = "Tasks.db";
        //Kansiorakenteen haku
        static string folderPath = System.IO.Path.GetFullPath(@"..\..\..\");
        //Tietokantojen osoitteet
        public static string Users_databasePath = System.IO.Path.Combine(folderPath, Users_databaseName);
        public static string Tasks_databasePath = System.IO.Path.Combine(folderPath, Tasks_databaseName);
    }
}
