using MessageService.Models;
using Microsoft.AspNetCore.Mvc;

namespace MessageService.Message;

public interface IMessageHandler
{
    public Task<HttpResponseMessage> SendMessage(MessageDto messageDto);
    public Task<HttpResponseMessage> GetMessage(string toUser, string user);
}