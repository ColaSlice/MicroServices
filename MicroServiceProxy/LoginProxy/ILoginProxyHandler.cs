using MicroServiceProxy.Models;


namespace MicroServiceProxy.LoginProxy
{
    public interface ILoginProxyHandler
    {
        public ValueTask<HttpResponseMessage> Register(UserDto userDto);
        public ValueTask<HttpResponseMessage> Login(UserDto userDto);
        public Task<HttpResponseMessage> ValidateUser(MessageDto messageDto);
        public ValueTask<bool> GetStatus();
    }
}
