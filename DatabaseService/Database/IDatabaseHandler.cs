using DatabaseService.Models;
using MessageService.Models;

namespace DatabaseService.Database;

public interface IDatabaseHandler
{
    public void Save(Enum type, MessageDto? messageDto, LogMessage? logMessage);
    public List<LogMessage> ReadLogs(DateTime? timeStamp);
    public List<MessageDto> ReadMessage(DateTime? timeStamp, string toUser, string fromUser);
}