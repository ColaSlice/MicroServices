using LoginService.Models;

namespace LoginService.Login
{
    public interface ILoginHandler
    {
        public Task<User> Register(UserDto request);
        public User Login(UserDto request);
        public bool ValidateUser(MessageDto messageDto);
        public string CreateToken(User user);
    }
}