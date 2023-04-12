using MessageService.Models;

namespace MessageService.Message;

public interface IMessageHandler
{
    public string SendMessage(MessageDto messageDto);
}