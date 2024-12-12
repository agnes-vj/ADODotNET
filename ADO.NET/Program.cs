using ADO.NET;
using Microsoft.Data.SqlClient;

var designers = Utils.DeserializeFromFile<List<Designer>>("./Resources/Designers.json");
var games = Utils.DeserializeFromFile<List<Game>>("./Resources/Games.json");

Utils.DropAndCreateDatabase();

using (SqlConnection connection = new SqlConnection(Utils.CONNECTION_STRING))
{
    connection.Open();
    SqlTransaction transaction = connection.BeginTransaction();
    try
    {
    SqlCommand dropTable = new SqlCommand("DROP TABLE IF EXISTS Designers;", connection, transaction);
    SqlCommand createTable = new SqlCommand("CREATE TABLE Designers (Id INT IDENTITY(1,1) PRIMARY KEY, FirstName VARCHAR(50), LastName VARCHAR(50));", connection, transaction);
    dropTable.ExecuteNonQuery();
    createTable.ExecuteNonQuery();
   

    dropTable = new SqlCommand("DROP TABLE IF EXISTS Games;", connection, transaction);
    createTable = new SqlCommand("CREATE TABLE Games (Id INT IDENTITY(1,1) PRIMARY KEY, GameName VARCHAR(50),Genre VARCHAR(30), PriceInPence INT, DesignerID INT REFERENCES Designers(Id));", connection, transaction);
    dropTable.ExecuteNonQuery();
    createTable.ExecuteNonQuery();

    string firstName = "Elizabeth";
    string lastName = "Hargrave";
    string checkQuery = "SELECT 1 FROM Designers WHERE FirstName = '" + firstName + "' AND LastName = '" + lastName + "'";

    string insertQuery = "INSERT INTO Designers(FirstName, LastName) " +
                                            "SELECT @FirstName, @LastName " +
                                            "WHERE NOT EXISTS (" + checkQuery + ")";

    SqlCommand insertCommand = new SqlCommand(insertQuery , connection, transaction);
    
    insertCommand.Parameters.AddWithValue("@FirstName", firstName);
    insertCommand.Parameters.AddWithValue("@LastName", lastName);

    int rowsAffected = insertCommand.ExecuteNonQuery();

    SqlCommand insertGameCmd = new SqlCommand("INSERT INTO Games (GameName, Genre, PriceInPence, DesignerID)" +
                                                "SELECT 'Wingspan', 'EngineBuilding', 3500, 1" +
                                                "WHERE NOT EXISTS(" +
                                                "SELECT 1 " +
                                                "FROM Games " +
                                                "WHERE GameName = 'Wingspan');", connection, transaction);

    int rowsAffected2 = insertGameCmd.ExecuteNonQuery();



    SqlCommand insertGame2Cmd = new SqlCommand("INSERT INTO Games (GameName, Genre, PriceInPence, DesignerID)" +
                                               "SELECT 'Mariposas', 'Set Collection', 3000, 1" +
                                               "WHERE NOT EXISTS(" +
                                               "SELECT 1 " +
                                               "FROM Games " +
                                               "WHERE GameName = 'Mariposas');", connection, transaction);

    int rowsAffected3 = insertGame2Cmd.ExecuteNonQuery();
    Console.WriteLine(rowsAffected3 + "rows affected in Games table");


    SqlCommand deleteGame2Cmd = new SqlCommand("DELETE FROM Games WHERE GameName = 'Mariposas'", connection, transaction);
    int rowsDeleted = deleteGame2Cmd.ExecuteNonQuery();
        Console.WriteLine(rowsDeleted + "rows deleted");

        transaction.Commit();
    }
    catch(Exception ex)
    {
        transaction.Rollback();
        Console.WriteLine(ex.Message);
    }

}
