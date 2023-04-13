using LiteDB;
using DatabaseService.Enums;
using DatabaseService.Models;
using MessageService.Models;

namespace DatabaseService.Database
{
    public class DatabaseHandler : IDatabaseHandler
    {
        private LiteDatabase _liteDatabase;
        private const string _messages = @"Messages.db";
        private const string _logs = @"Logs.db";
        private const string _messagesCollection = @"messages";
        private const string _logsCollection = @"logs";
        
        public void Save(Enum type, MessageDto? messageDto, LogMessage? logMessage)
        {
            if (Equals(type, (Enum)Types.Log))
            {
                using (var _liteDatabase = new LiteDatabase(_logs))
                {
                    var collection = _liteDatabase.GetCollection<LogMessage>(_logsCollection);

                    collection.Insert(logMessage);

                    collection.EnsureIndex(x => x.Timestamp);
                    
                    Console.WriteLine("Done writing to Logs");
                }
            }

            if (Equals(type, (Enum)Types.Message))
            {
                using (var _liteDatabase = new LiteDatabase(_messages))
                {
                    var collection = _liteDatabase.GetCollection<MessageDto>(_messagesCollection);

                    collection.Insert(messageDto);

                    collection.EnsureIndex(x => x.Timestamp);
                    
                    Console.WriteLine("Done writing to Messages");
                }
            }
        }

        public List<LogMessage> ReadLogs(DateTime? timeStamp)
        { 
            using (var _liteDatabase = new LiteDatabase(_logs))
            {
                var collection = _liteDatabase.GetCollection<LogMessage>(_logsCollection);

                var logs = collection.Query().ToList();
                Console.WriteLine("Done reading Logs");
                return logs;
            }
        }

        public List<MessageDto> ReadMessage(DateTime? timeStamp, string toUser, string fromUser)
        {
            using (var _liteDatabase = new LiteDatabase(_messages))
            {
                var collection = _liteDatabase.GetCollection<MessageDto>(_messagesCollection);

                var messages = collection.Query()
                    .Where(x => x.ToUser == toUser)
                    .Where(x => x.User == fromUser)
                    .ToList();
                Console.WriteLine("Done reading to Messages");
                return messages;
            }
        }
    }
}