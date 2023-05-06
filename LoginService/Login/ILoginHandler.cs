using LoginService.Models;
using Microsoft.AspNetCore.Mvc;

namespace LoginService.Login
{
    public interface ILoginHandler
    {
        public Task<User?> Register(UserDto request);
        public Task<User> Login(UserDto request);
        public Task<bool> ValidateUser(MessageDto messageDto);
        public string CreateToken(User user);
    }
}