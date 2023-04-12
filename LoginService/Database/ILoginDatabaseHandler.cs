using LoginService.Models;

namespace LoginService.Database
{
    public interface ILoginDatabaseHandler
    {
        public void SaveUser(User user);
        public bool UserExists(User user);
        public bool UsernameExists(MessageDto messageDto);
        public User ReadUser(UserDto request);

    }
}