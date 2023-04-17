using LoginService.Models;

namespace LoginService.Database
{
    public interface ILoginDatabaseHandler
    {
        public ValueTask SaveUser(User user);
        public ValueTask<bool> UserExists(User user);
        public bool UsernameExists(MessageDto messageDto);
        public User ReadUser(UserDto request);
        public void Dispose();
    }
}