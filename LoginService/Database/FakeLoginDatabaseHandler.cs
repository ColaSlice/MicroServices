using LoginService.Models;

namespace LoginService.Database
{
    public class FakeLoginDatabaseHandler : ILoginDatabaseHandler
    {
        private readonly List<User> _users = new List<User>();
        public User ReadUser(UserDto request)
        {
            return _users.FirstOrDefault(x => x.Username == request.Username)!;
        }

        public void SaveUser(User user)
        {
            _users.Add(user);
        }

        public bool UserExists(User user)
        {
            return _users.Contains(user);
        }
    }
}
