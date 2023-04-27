using LiteDB;
using DatabaseService.Enums;
using DatabaseService.Models;
using MessageService.Models;

namespace DatabaseService.Database
{
    public class DatabaseHandler : IDatabaseHandler
    {
        private LiteDatabase _liteDatabase;
        private const string Messages = @"Messages.db";
        private const string Logs = @"Logs.db";
        private const string MessagesCollection = @"messages";
        private const string LogsCollection = @"logs";

        public void Save(Enum type, MessageDto? messageDto, LogMessage? logMessage)
        {
            if (Equals(type, (Enum)Types.Log))
            {
                using (var liteDatabase = new LiteDatabase(Logs))
                {
                    var collection = liteDatabase.GetCollection<LogMessage>(LogsCollection);

                    collection.Insert(logMessage);

                    collection.EnsureIndex(x => x.Timestamp);
                }
            }

            if (Equals(type, (Enum)Types.Message))
            {
                using (var liteDatabase = new LiteDatabase(Messages))
                {
                    var collection = liteDatabase.GetCollection<MessageDto>(MessagesCollection);

                    collection.Insert(messageDto);

                    collection.EnsureIndex(x => x.Timestamp);
                }
            }
        }

        public List<LogMessage> ReadLogs(DateTime? timeStamp)
        { 
            using (var liteDatabase = new LiteDatabase(Logs))
            {
                var collection = liteDatabase.GetCollection<LogMessage>(LogsCollection);

                var logs = collection.Query().ToList();
                return logs;
            }
        }

        public List<MessageDto> ReadMessage(DateTime? timeStamp, string toUser)
        {
            using (var liteDatabase = new LiteDatabase(Messages))
            {
                var collection = liteDatabase.GetCollection<MessageDto>(MessagesCollection);

                var messages = collection.Query()
                    .Where(x => x.ToUser == toUser)
                    .ToList();
                for (int i = 0; i < messages.Count; i++)
                {
                    Console.WriteLine(messages[i].Message);
                }
                return messages;
            }
        }
    }
}