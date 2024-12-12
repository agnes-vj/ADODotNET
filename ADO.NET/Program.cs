using ADO.NET;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;

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

        SqlCommand insertCommand = new SqlCommand(insertQuery, connection, transaction);

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



        //SqlCommand getAllDesignersCommand = new("SELECT * FROM Designers;", connection);
        //using (SqlDataReader reader = getAllDesignersCommand.ExecuteReader())
        //{
        //    while (reader.Read())
        //    {
        //        Console.Write(reader.GetValue(0) + "   ");
        //        Console.Write(reader.GetValue(1) + "   ");
        //        Console.Write(reader.GetValue(2) + "   ");
        //    }
        //}
        InsertDesignerRecords(designers, connection, transaction);
       InsertGamesRecords(games, connection, transaction);
        //DisplayDesignerRecords( connection, transaction);
        //DisplayGameRecords( connection, transaction);

        SqlCommand command = new("SELECT d.FirstName + ' ' + d.LastName as Designer, g.* FROM Games g JOIN Designers d ON g.DesignerId = d.Id", connection, transaction);
        using (SqlDataReader reader = command.ExecuteReader())
        {    
            for (int i = 0; i < reader.FieldCount; i++)
            {
                Console.Write($"{reader.GetName(i),-20}");
            }
            Console.WriteLine();
            Console.WriteLine(new string('-', reader.FieldCount * 20));

            while (reader.Read())
            {

                for (int i = 0; i < reader.FieldCount; i++)
                { 
                    Console.Write($"{reader.GetValue(i), -20}");
                }
                Console.WriteLine();

            }
        }
            transaction.Commit();
    }
    catch(Exception ex)
    {
        transaction.Rollback();
        Console.WriteLine(ex.Message);
    }

}

static void DisplayGameRecords(SqlConnection connection, SqlTransaction transaction)
{
    SqlCommand command = new($"SELECT * FROM Games;", connection, transaction);


    using (SqlDataReader reader = command.ExecuteReader())
    {
        while (reader.Read())
        {

            for (int i = 0; i < reader.FieldCount; i++)
            {

                Console.Write(reader.GetValue(i) + "   ");
            }
            Console.WriteLine();

        }
    }
}
static void DisplayDesignerRecords(SqlConnection connection, SqlTransaction transaction)
{
    SqlCommand command = new($"SELECT * FROM Designers;", connection, transaction);


    using (SqlDataReader reader = command.ExecuteReader())
    {
        while (reader.Read())
        {

            for (int i = 0; i < reader.FieldCount; i++)
            {

                Console.Write(reader.GetValue(i) + "   ");
            }
            Console.WriteLine();

        }
    }
}
static void InsertDesignerRecords(List<Designer> designers, SqlConnection connection, SqlTransaction transaction)
{
    SqlCommand insertDesignerRows;
    foreach (Designer designer in designers)
    {
        insertDesignerRows = new SqlCommand("INSERT INTO Designers " +
         "SELECT @FirstName, @LastName" +
         " WHERE NOT EXISTS(SELECT 1 FROM Designers WHERE FirstName = @FirstName AND LastName=@LastName);", connection, transaction);
        insertDesignerRows.Parameters.AddWithValue("@FirstName", designer.FirstName);
        insertDesignerRows.Parameters.AddWithValue("@LastName", designer.LastName);
        insertDesignerRows.ExecuteNonQuery();
    }

}

static void InsertGamesRecords(List<Game> games, SqlConnection connection, SqlTransaction transaction)
{
    SqlCommand insertDesignerRows;
    foreach (Game game in games)
    {
        insertDesignerRows = new SqlCommand("INSERT INTO Games " +
         "SELECT @GameName, @Genre, @Price, @DesignerId" +
         " WHERE NOT EXISTS(SELECT 1 FROM Games WHERE GameName = @GameName);", connection, transaction);
        insertDesignerRows.Parameters.AddWithValue("@GameName", game.GameName);
        insertDesignerRows.Parameters.AddWithValue("@Genre", game.Genre);
        insertDesignerRows.Parameters.AddWithValue("@Price", game.Price);
        insertDesignerRows.Parameters.AddWithValue("@DesignerId", game.DesignerId);
        insertDesignerRows.ExecuteNonQuery();
    }

}
