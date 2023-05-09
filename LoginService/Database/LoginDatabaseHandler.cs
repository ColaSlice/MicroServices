using Microsoft.Data.Sqlite;
using LoginService.Models;

namespace LoginService.Database
{
    public class LoginDatabaseHandler : ILoginDatabaseHandler, IDisposable
    {
        private const string DataSourceString = @"Data Source=TestLoginDatabase";
        private readonly SqliteConnection _connection;
        private readonly User _user;

        public LoginDatabaseHandler()
        {
            _user = new User();
            _connection = new SqliteConnection(DataSourceString);
            _connection.Open();
            using (SqliteCommand cmd = new SqliteCommand("CREATE TABLE IF NOT EXISTS Login (id INTEGER NOT NULL UNIQUE, username TEXT NOT NULL, passwordhash TEXT NOT NULL, email TEXT NOT NULL, license TEXT NOT NULL, PRIMARY KEY(id AUTOINCREMENT));", _connection)) 
            {
                cmd.Prepare();
                cmd.ExecuteNonQuery(); 
            }
        }

        public async Task<bool> SaveUser(User user)
        {
            await using (SqliteCommand cmd = new SqliteCommand("INSERT INTO login (username, passwordhash, email, license) VALUES (@username, @passwordhash, @email, @license)", _connection))
            {
                try
                {
                    cmd.Parameters.AddWithValue("@username", user.Username);
                    cmd.Parameters.AddWithValue("@passwordhash", user.PasswordHash);
                    cmd.Parameters.AddWithValue("@email", user.Email);
                    cmd.Parameters.AddWithValue("@license", user.License);
                    cmd.Prepare();
                    cmd.ExecuteNonQuery();
                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }
            }
        }

        public async Task<bool> UserExists(string email)
        {
            await using (SqliteCommand cmd = new SqliteCommand("SELECT email FROM login WHERE email=@email", _connection))
            {
                cmd.Parameters.AddWithValue("@email", email);
                await using (SqliteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader.HasRows)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public async Task<User?> ReadUser(UserDto request)
        {
            await using (SqliteCommand cmd = new SqliteCommand("SELECT id,username,passwordhash,email,license FROM login WHERE email=@email", _connection))
            {
                cmd.Parameters.AddWithValue("@email", request.Email);
                await using (SqliteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        _user!.Username = reader["username"].ToString()!;
                        _user.PasswordHash = reader["passwordhash"].ToString()!;
                        _user.Email = reader["email"].ToString()!;
                        _user.License = reader["license"].ToString()!;
                    }
                }
            }
            return _user;
        }

        public void Dispose()
        {
            _connection.Close();
            _connection.Dispose();
            _user.Dispose();
        }
    }
}