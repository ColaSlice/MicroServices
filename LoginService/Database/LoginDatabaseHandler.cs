using Microsoft.Data.Sqlite;
using LoginService.Models;

namespace LoginService.Database
{
    public class LoginDatabaseHandler : ILoginDatabaseHandler, IDisposable
    {
        private const string DataSourceString = @"Data Source=TestLoginDatabase.db;Cache=Shared";
        private SqliteConnection _connection;
        private SqliteCommand _cmd;
        private static User? _user;

        public LoginDatabaseHandler()
        {
            _user = new User();
            _connection = new SqliteConnection(DataSourceString);
            _connection.Open();
            
            var sqlSetting = _connection.CreateCommand();

            sqlSetting.CommandText = @"PRAGMA synchronous = 0;";
            sqlSetting.Prepare();
            sqlSetting.ExecuteNonQuery();
            sqlSetting.Dispose();
            
            sqlSetting = _connection.CreateCommand();
            sqlSetting.CommandText = @"PRAGMA temp_store = 2;";
            sqlSetting.Prepare();
            sqlSetting.ExecuteNonQuery();
            sqlSetting.Dispose();
            
            sqlSetting.CommandText = @"VACUUM;";
            sqlSetting.Prepare();
            sqlSetting.ExecuteNonQuery();
            sqlSetting.Dispose();
            
            _cmd = new("CREATE TABLE IF NOT EXISTS Login (id INTEGER NOT NULL UNIQUE, username	TEXT NOT NULL, passwordhash	TEXT NOT NULL, email TEXT NOT NULL, license	TEXT NOT NULL, PRIMARY KEY(id AUTOINCREMENT));", _connection);
            _cmd.Prepare();
            _cmd.ExecuteNonQuery();
        }

        public async Task SaveUser(User user)
        {
            _cmd = new SqliteCommand("INSERT INTO login (username, passwordhash, email, license) VALUES (@username, @passwordhash, @email, @license)", _connection);
            _cmd.Parameters.AddWithValue("@username", user.Username);
            _cmd.Parameters.AddWithValue("@passwordhash", user.PasswordHash);
            _cmd.Parameters.AddWithValue("@email", user.Email);
            _cmd.Parameters.AddWithValue("@license", user.License);
            _cmd.Prepare();
            _cmd.ExecuteNonQuery();
        }

        public async Task<bool> UserExists(string? email, string? username)
        {
            if (email != null)
            {
                _cmd = new SqliteCommand("SELECT email FROM login WHERE email=@email", _connection);
                _cmd.Parameters.AddWithValue("@email", email);
            }

            if (username != null)
            {
                _cmd = new SqliteCommand("SELECT username FROM login WHERE username=@username", _connection);
                _cmd.Parameters.AddWithValue("@username", username);
            }
            SqliteDataReader reader = _cmd.ExecuteReader();
            if (reader.HasRows)
            {
                _cmd.Dispose();
                reader.Dispose();
                return true;
            }
            
            _cmd.Dispose();
            return false;
        }

        public User ReadUser(UserDto request)
        {
            _cmd = new SqliteCommand("SELECT id,username,passwordhash,email,license FROM login WHERE email=@email", _connection);
            _cmd.Parameters.AddWithValue("@email", request.Email);
            using (SqliteDataReader reader = _cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    _user.Username = reader["username"].ToString()!;
                    _user.PasswordHash = reader["passwordhash"].ToString()!;
                    _user.Email = reader["email"].ToString()!;
                    _user.License = reader["license"].ToString()!;
                }
            }
            _cmd.Dispose();
            return _user!;
        }

        public void Dispose()
        {
            _connection.Close();
            _connection.Dispose();
            _user!.Dispose();
        }
    }
}