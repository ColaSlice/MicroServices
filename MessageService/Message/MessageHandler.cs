using MessageService.Models;
using Microsoft.AspNetCore.Mvc;

namespace MessageService.Message;

public class MessageHandler : IMessageHandler
{
    public string SendMessage(MessageDto messageDto)
    {
        return messageDto.Message + " abc";
    }

    public string GetMessage()
    {
        throw new NotImplementedException();
    }
}