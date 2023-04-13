using MicroServiceProxy.Models;
using Microsoft.AspNetCore.Mvc;

namespace MicroServiceProxy.DatabaseProxy;

public interface IDatabaseProxy
{
    public Task<HttpResponseMessage> GetLogs();
    public Task<HttpResponseMessage> GetMessages(string toUser, string user);
    public Task<HttpResponseMessage> SaveLog(LogMessage logMessage);
    public Task<HttpResponseMessage> SaveMessage(MessageDto messageDto);
}