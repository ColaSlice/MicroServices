using LoginService.Models;

namespace LoginService.Database
{
    public class FakeLoginDatabaseHandler : ILoginDatabaseHandler
    {
        private readonly List<User> _users = new List<User>();
        public bool UsernameExists(MessageDto messageDto)
        {
            throw new NotImplementedException();
        }

        public User ReadUser(UserDto request)
        {
            return _users.FirstOrDefault(x => x.Username == request.Username)!;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public async ValueTask SaveUser(User user)
        {
            _users.Add(user);
        }

        public async ValueTask<bool> UserExists(User user)
        {
            return _users.Contains(user);
        }
    }
}
