using LoggerMachine.Models;
using LiteDB;

namespace LoggerMachine.Services
{
    public class DatabaseHandler : IDatabaseHandler
    {
        private const string ConnectionString = @"Logs.db";
        private LiteDatabase _db;

        public DatabaseHandler()
        {
            _db = new LiteDatabase(ConnectionString);
        }
        
        public void Insert(string message, DateTime timestamp)
        {
            var collection = _db.GetCollection<Log>("logs");

            Log log = new Log {Message = message, Timestamp = timestamp};
            collection.Insert(log);
            collection.EnsureIndex(x => x.Timestamp);
        }

        public List<Log> Read()
        {
            var collection = _db.GetCollection<Log>("logs");
            var logs = collection.Query().ToList();
            return logs;
        }
    }
}