using System.Text.Json;

namespace ADO.NET
{
    internal class Utils
    {
        public const string MASTER_CONNECTION_STRING = "Server=localhost\\SQLEXPRESS;Database=master;User Id=<user_id>;Password=<password>;Trust Server Certificate=True";
        public const string CONNECTION_STRING = "Server=<server_name>;Database=MyBoardGameCafe;User Id=<user_id>;Password=<password>;Trust Server Certificate=True";
        public static void DropAndCreateDatabase()
        {
        using (SqlConnection conn = new SqlConnection(MASTER_CONNECTION_STRING))
        {
            conn.Open();
            SqlCommand dropDb = new SqlCommand("DROP DATABASE IF EXISTS MyBoardGameCafe", conn);
            SqlCommand createDb = new SqlCommand("CREATE DATABASE MyBoardGameCafe", conn);
            dropDb.ExecuteNonQuery();
            createDb.ExecuteNonQuery();
        }
        }
        public static T DeserializeFromFile<T>(string filePath)
        {
            var json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<T>(json);
        }
    }
}
