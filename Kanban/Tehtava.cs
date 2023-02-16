using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace Kanban
{
    public class Tehtava
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Tag { get; set; }
        public string DueDate { get; set; }

        public string Status { get; set; }

    }
}
