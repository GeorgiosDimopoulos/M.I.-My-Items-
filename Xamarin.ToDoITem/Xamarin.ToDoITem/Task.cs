 using System;
using SQLite;

namespace MyItems
{
    public class Task
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }
        public string Price{ get; set; }
        public string Details{ get; set; }
        public int Type { get; set; }
        public DateTime Date{ get; set; }
        public string Text { get; set; }
        public int Priority { get; set; }
    }
}