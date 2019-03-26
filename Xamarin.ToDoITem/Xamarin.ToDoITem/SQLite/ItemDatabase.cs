using SQLite;

namespace MyCrossFitApp.SQLite
{
    public class ItemDatabase
    {
        readonly SQLiteAsyncConnection database;

        public ItemDatabase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<Task>().Wait();
        }

        public SQLiteAsyncConnection Database
        {
            get { return database; }
        }
    }
}