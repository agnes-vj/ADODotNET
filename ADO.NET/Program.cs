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
    createTable.ExecuteNonQuery();
   

    dropTable = new SqlCommand("DROP TABLE IF EXISTS Games;", connection);
    createTable = new SqlCommand("CREATE TABLE Games (Id INT IDENTITY(1,1) PRIMARY KEY, GameName VARCHAR(50),Genre VARCHAR(30), PriceInPence INT, DesignerID INT REFERENCES Designers(Id));", connection);
    dropTable.ExecuteNonQuery();
    createTable.ExecuteNonQuery();

    string firstName = "Elizabeth";
    string lastName = "Hargrave";
    string checkQuery = "SELECT 1 FROM Designers WHERE FirstName = '" + firstName + "' AND LastName = '" + lastName + "'";
    Console.WriteLine(checkQuery);

    string insertQuery = "INSERT INTO Designers(FirstName, LastName) " +
                                            "SELECT @FirstName, @LastName " +
                                            "WHERE NOT EXISTS (" + checkQuery + ")";
    Console.WriteLine(insertQuery);
    SqlCommand insertCommand = new SqlCommand(insertQuery , connection);
    
    insertCommand.Parameters.AddWithValue("@FirstName", firstName);
    insertCommand.Parameters.AddWithValue("@LastName", lastName);

    int rowsAffected = insertCommand.ExecuteNonQuery();
    Console.WriteLine(rowsAffected + " Rows Affected");

}
