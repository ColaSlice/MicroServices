using MessageService.Models;

namespace MessageService.Message;

public class MessageHandler : IMessageHandler
{
    public string SendMessage(MessageDto messageDto)
    {
        return messageDto.Message + " abc";
    }
}