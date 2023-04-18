using MicroServiceProxy.Models;
using Microsoft.AspNetCore.Mvc;

namespace MicroServiceProxy.DatabaseProxy;

public interface IDatabaseProxyHandler
{
    public Task<HttpResponseMessage> GetLogs();
    public Task<HttpResponseMessage> GetMessages(string toUser);
    public Task<HttpResponseMessage> SaveLog(LogMessage logMessage);
    public Task<HttpResponseMessage> SaveMessage(MessageDto messageDto);
    public Task<bool> GetStatus();
}