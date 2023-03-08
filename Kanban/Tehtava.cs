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
        public int UserId { get; set; }
    }
}
