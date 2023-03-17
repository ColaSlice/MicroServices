using LoginService.Models;

namespace LoginService.Login
{
    public interface ILoginHandler
    {
        public User Register(UserDto request);
        public User Login(UserDto request);
        public string CreateToken(User user);
    }
}