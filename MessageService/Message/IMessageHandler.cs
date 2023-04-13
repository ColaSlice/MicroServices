using MessageService.Models;
using Microsoft.AspNetCore.Mvc;

namespace MessageService.Message;

public interface IMessageHandler
{
    public string SendMessage(MessageDto messageDto);
    public string GetMessage();
}