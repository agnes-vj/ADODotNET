using System.Text.Json;
using Microsoft.Data.SqlClient;

namespace ADO.NET
{
    internal class Utils
    {

        public const string MASTER_CONNECTION_STRING = "Server=.\\SQLEXPRESS;Database=master;User Id=sa;Password=Password@123;Trust Server Certificate=True";
        public const string CONNECTION_STRING = "Server=.\\SQLEXPRESS;Database=MyBoardGameCafe;User Id=sa;Password=Password@123;Trust Server Certificate=True";
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
