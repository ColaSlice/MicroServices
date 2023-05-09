using LoginService.Models;

namespace LoginService.Database
{
    public interface ILoginDatabaseHandler
    {
        public Task<bool> SaveUser(User user);
        public Task<bool> UserExists(string email);
        public Task<User?> ReadUser(UserDto request);
        public void Dispose();
    }
}