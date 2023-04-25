using LoginService.Models;

namespace LoginService.Database
{
    public interface ILoginDatabaseHandler
    {
        public Task SaveUser(User user);
        public Task<bool> UserExists(string? email, string? username);
        public User ReadUser(UserDto request);
        public void Dispose();
    }
}