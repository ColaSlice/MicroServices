using LoginService.Models;

namespace LoginService.Login
{
    public interface ILoginHandler
    {
        public Task<User> Register(UserDto request);
        //private Task<string> HashPassword(string password);
        public User Login(UserDto request);
        public Task<bool> ValidateUser(MessageDto messageDto);
        public string CreateToken(User user);
    }
}