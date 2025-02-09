using Npgsql;

namespace MyCinemaDiary.Domain.Data
{
    public class DBContext
    {
        public DBContext()
        {
            // Initialize the database
            Initialize();
        }
        public void Initialize()
        {
            var host = "192.168.1.131:5432";
            var username = "obscure";
            var password = "secure";
            var database = "UCD";

            var connString = $"Host={host};Username={username};Password={password};Database={database}";

            var conn = new NpgsqlConnection(connString);
            conn.Open();
        }
    }
}
