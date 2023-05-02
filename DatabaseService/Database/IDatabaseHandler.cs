using DatabaseService.Models;
using MessageService.Models;

namespace DatabaseService.Database;

public interface IDatabaseHandler
{
    public void Save(Enum type, MessageDto? messageDto, LogDto? logMessage);
    public List<LogDto> ReadLogs(DateTime? timeStamp);
    public List<MessageDto> ReadMessage(DateTime? timeStamp, string toUser);
}