using MicroServiceProxy.Models;

namespace MicroServiceProxy.MessageProxy;

public interface IMessageProxyHandler
{
    public Task<HttpResponseMessage> SendMessage(MessageDto messageDto);
}