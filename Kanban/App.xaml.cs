using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Kanban
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static string databaseName = "Users.db";
        static string folderPath = System.IO.Path.GetFullPath(@"..\..\..\");
        public static string databasePath = System.IO.Path.Combine(folderPath, databaseName);
    }
}
