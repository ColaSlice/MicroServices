using Microsoft.Data.Sqlite;

namespace MessageService.Database
{
    public class MessageDatabaseHandle
    {
        private const string sourceString = @"Data Source=:memory:";
        private SqliteConnection connection;

        public MessageDatabaseHandle()
        {
            connection = new SqliteConnection(sourceString);
            connection.Open();
            SqliteCommand cmd = new SqliteCommand("DROP TABLE IF EXISTS messages", connection);
            cmd.ExecuteNonQuery();
            cmd.CommandText = String.Format(@"CREATE TABLE messages(id INTEGER PRIMARY KEY NOT NULL, message TEXT NOT NULL)", typeof(int).ToString(), typeof(string).ToString());
            cmd.ExecuteNonQuery();
        }

        ~MessageDatabaseHandle()
        {
            connection.Close();
        }
    }
}
