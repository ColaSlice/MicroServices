using LoginService.Models;

namespace LoginService.Database
{
    public interface ILoginDatabaseHandler
    {
        public void SaveUser(User user);
        public bool UserExists(User user);
        public User ReadUser(UserDto request);

    }
}