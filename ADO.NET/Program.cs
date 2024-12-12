using ADO.NET;
using Microsoft.Data.SqlClient;

var designers = Utils.DeserializeFromFile<List<Designer>>("./Resources/Designers.json");
var games = Utils.DeserializeFromFile<List<Game>>("./Resources/Games.json");

Utils.DropAndCreateDatabase();

using (SqlConnection connection = new SqlConnection(Utils.CONNECTION_STRING))
{
    connection.Open();
    SqlCommand dropTable = new SqlCommand("DROP TABLE IF EXISTS Designers;", connection);
    SqlCommand createTable = new SqlCommand("CREATE TABLE Designers (Id INT IDENTITY(1,1) PRIMARY KEY, FirstName VARCHAR(50), LastName VARCHAR(50));", connection);
    dropTable.ExecuteNonQuery();
    var result = createTable.ExecuteNonQuery();
    Console.WriteLine(result + "rows affected");
}
