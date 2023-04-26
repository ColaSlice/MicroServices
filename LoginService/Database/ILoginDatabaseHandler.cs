using LoginService.Models;

namespace LoginService.Database
{
    public interface ILoginDatabaseHandler
    {
        public ValueTask SaveUser(User user);
        public Task<bool> UserExists(string email);
        public User ReadUser(UserDto request);
        public void Dispose();
    }
}