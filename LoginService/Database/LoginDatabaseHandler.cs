using Microsoft.Data.Sqlite;
using LoginService.Models;

namespace LoginService.Database
{
    public class LoginDatabaseHandler : ILoginDatabaseHandler
    {
        private const string DataSourceString = @"Data Source=TestLoginDatabase";
        private readonly SqliteConnection _connection;
        private User _user;

        public LoginDatabaseHandler()
        {
            _user = new User();
            _connection = new SqliteConnection(DataSourceString);
            _connection.Open();
            SqliteCommand _cmd = new SqliteCommand("CREATE TABLE IF NOT EXISTS Login (id INTEGER NOT NULL UNIQUE, username	TEXT NOT NULL, passwordhash	TEXT NOT NULL, email TEXT NOT NULL, license	TEXT NOT NULL, PRIMARY KEY(id AUTOINCREMENT));", _connection);
            _cmd.Prepare();
            _cmd.ExecuteNonQuery();
        }

        public void SaveUser(User user)
        {
            using (SqliteCommand _cmd = new SqliteCommand("INSERT INTO login (username, passwordhash, email, license) VALUES (@username, @passwordhash, @email, @license)", _connection))
            {
                _cmd.Parameters.AddWithValue("@username", user.Username);
                _cmd.Parameters.AddWithValue("@passwordhash", user.PasswordHash);
                _cmd.Parameters.AddWithValue("@email", user.Email);
                _cmd.Parameters.AddWithValue("@license", user.License);
                _cmd.Prepare();
                _cmd.ExecuteNonQuery();
                _connection.Close();
                _connection.Dispose();
            }
        }

        public bool UserExists(User user)
        {
            using (SqliteCommand _cmd = new SqliteCommand("SELECT email FROM login WHERE email=@email", _connection))
            {
                _cmd.Parameters.AddWithValue("@email", user.Email);
                using (SqliteDataReader reader = _cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        _connection.Close();
                        _connection.Dispose();
                        _cmd.Dispose();
                        return true;
                    }
                }
            }
            return false;
        }

        public User ReadUser(UserDto request)
        {
            using (SqliteCommand _cmd = new SqliteCommand("SELECT id,username,passwordhash,email,license FROM login WHERE email=@email", _connection))
            {
                _cmd.Parameters.AddWithValue("@email", request.Email);
                using (SqliteDataReader reader = _cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader.GetInt32(0)} {reader["email"]}");
                        _user.Username = reader["username"].ToString()!;
                        _user.PasswordHash = reader["passwordhash"].ToString()!;
                        _user.Email = reader["email"].ToString()!;
                        _user.License = reader["license"].ToString()!;
                    }
                }
                _cmd.Dispose();
                return _user;
            }
        }
    }
}