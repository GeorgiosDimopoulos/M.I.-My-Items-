using SQLite;

namespace MyCrossFitApp
{
    public class Task
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }
        public int Type { get; set; }
        public string Text { get; set; }
    }
}