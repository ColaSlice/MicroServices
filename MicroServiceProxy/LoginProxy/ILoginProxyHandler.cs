using MicroServiceProxy.Models;


namespace MicroServiceProxy.LoginProxy
{
    public interface ILoginProxyHandler
    {
        public Task<HttpResponseMessage> Register(UserDto userDto);
        public Task<HttpResponseMessage> Login(UserDto userDto);
        public Task<HttpResponseMessage> ValidateUser(MessageDto messageDto);
        public Task<bool> GetStatus();
    }
}
