using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Transactions;

namespace ADO.NET
{
 internal class DBService
 {
        string CONNECTION_STRING;
        public DBService(string connectionString)
        {
            CONNECTION_STRING = connectionString;
        }
   internal string DropAndCreateTableDesigners()
   {
            string status = "";
            string dropTableQuery = "DROP TABLE IF EXISTS Designers;";
            string createTableQuery = @"CREATE TABLE Designers (
                                                            Id INT IDENTITY(1,1) PRIMARY KEY,
                                                            FirstName VARCHAR(50), 
                                                            LastName VARCHAR(50)
                                                            );";
            try
            {
                using (SqlConnection connection = new SqlConnection(CONNECTION_STRING))
                {
                    connection.Open();
                    SqlCommand dropTable = new SqlCommand(dropTableQuery, connection);
                    SqlCommand createTable = new SqlCommand(createTableQuery, connection);
                    dropTable.ExecuteNonQuery();
                    createTable.ExecuteNonQuery();
                    status = "New Table Designers Created Successfully...";
                }
            }
            catch (Exception ex)
            {
                status = ex.Message;
            }
            return status;
   }
  internal string DropAndCreateTableGames()
  {
            string status = "";
            string dropTableQuery = "DROP TABLE IF EXISTS Games";
            string createTableQuery = @"CREATE TABLE Games (
                                                        Id INT IDENTITY(1,1) PRIMARY KEY,
                                                        GameName VARCHAR(50),
                                                        Genre VARCHAR(30), 
                                                        PriceInPence INT, 
                                                        DesignerID INT REFERENCES Designers(Id)
                                                       );";
            try
            {
                using (SqlConnection connection = new SqlConnection(CONNECTION_STRING))
                {
                    connection.Open();
                    SqlCommand dropTable = new SqlCommand(dropTableQuery, connection);
                    SqlCommand createTable = new SqlCommand(createTableQuery, connection);
                    dropTable.ExecuteNonQuery();
                    createTable.ExecuteNonQuery();
                    status = "New Table Games Created Successfully...";
                }
            }
            catch (Exception ex)
            {
                status = ex.Message;
            }

            return status;
  }
  internal string insertSingleRecordToDesigners(string firstName, string lastName)
  {
            string status = "";
            string checkQuery = "SELECT 1 FROM Designers WHERE FirstName = '" + firstName + "' AND LastName = '" + lastName + "'";

            string insertQuery = @"INSERT INTO Designers(FirstName, LastName) 
                                                    SELECT @FirstName, @LastName 
                                                    WHERE NOT EXISTS (" + checkQuery + ")";
            try
            {
                using (SqlConnection connection = new SqlConnection(CONNECTION_STRING))
                {
                    connection.Open();
                    SqlCommand insertCommand = new SqlCommand(insertQuery, connection);
                    insertCommand.Parameters.AddWithValue("@FirstName", firstName);
                    insertCommand.Parameters.AddWithValue("@LastName", lastName);

                    status += insertCommand.ExecuteNonQuery() + " Rows Inserted";
                }
            }
            catch (Exception ex)
            {
                status = ex.Message;
            }
        return status;
   }
  internal string insertSingleRecordToGames(string gameName, string genre, int priceInPence, int designerId)
  {
        string status = "";
        string checkQuery = "SELECT 1 FROM Games WHERE GameName = '@GameName'";

        string insertQuery = @"INSERT INTO Games(GameName, Genre, PriceInPence, DesignerID)
                                                    SELECT @GameName, @Genre, @PriceInPence, @DesignerId 
                                                    WHERE NOT EXISTS(" + checkQuery + ");";
        try
         {
          using (SqlConnection connection = new SqlConnection(CONNECTION_STRING))
          {
                connection.Open();
                SqlCommand insertCommand = new SqlCommand(insertQuery, connection);
                insertCommand.Parameters.AddWithValue("@GameName", gameName);
                insertCommand.Parameters.AddWithValue("@Genre", genre);
                insertCommand.Parameters.AddWithValue("@PriceInPence", priceInPence);
                insertCommand.Parameters.AddWithValue("@DesignerId", designerId);

                    status += insertCommand.ExecuteNonQuery() + " Rows Inserted";
          }
        }
        catch (Exception ex)
        {
            status = ex.Message;
        }
        return status;
  }
  internal string deleteFromGames(string gameName)
  {
      string status = "";
      string deleteQuery = "DELETE FROM Games WHERE GameName = @GameName;";
      
        try
        {
            using (SqlConnection connection = new SqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                SqlCommand deleteCommand = new SqlCommand(deleteQuery, connection);
                deleteCommand.Parameters.AddWithValue("@gameName", gameName);
                status += deleteCommand.ExecuteNonQuery() + " Rows Deleted";
            }
        }
        catch (Exception ex)
        {
            status = ex.Message;
        }
        return status;
   }
 internal void DisplayGameRecords()
{
    try
    {
        using (SqlConnection connection = new SqlConnection(CONNECTION_STRING))
        {
            connection.Open();
            SqlCommand command = new($"SELECT * FROM Games;", connection);
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
                        Console.Write($"{reader.GetValue(i),-20}");
                    }
                    Console.WriteLine();
                }
            }
        }
    }

    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
 }
 internal void DisplayDesignerRecords()
 {
        try
        {
            using (SqlConnection connection = new SqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                SqlCommand command = new($"SELECT * FROM Designers;", connection);
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
                            Console.Write($"{reader.GetValue(i),-20}");
                        }
                        Console.WriteLine();
                    }
                }
             }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

 internal string InsertDesignerRecords(List<Designer> designers)
    {
        string status = "";
        string insertQuery = @"INSERT INTO Designers 
                                        SELECT @FirstName, @LastName
                                        WHERE NOT EXISTS(SELECT 1 FROM Designers WHERE FirstName = @FirstName AND LastName=@LastName);";
        try
        {

            using (SqlConnection connection = new SqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                SqlCommand insertDesignerRows;
                int  insertCounter = 0;
                foreach (Designer designer in designers)
                {
                    insertDesignerRows = new SqlCommand(insertQuery, connection);
                    insertDesignerRows.Parameters.AddWithValue("@FirstName", designer.FirstName);
                    insertDesignerRows.Parameters.AddWithValue("@LastName", designer.LastName);
                    insertCounter += insertDesignerRows.ExecuteNonQuery();
                    status = insertCounter + " Rows inserted....";
                }
            }
        }
        catch (Exception ex)
        {
            status = status + ex.Message;
        }
        return status;
    }


 internal string InsertGamesRecords(List<Game> games)
 {
        string status = "";
        string insertQuery = @"INSERT INTO Games
                                SELECT @GameName, @Genre, @Price, @DesignerId
                                WHERE NOT EXISTS(SELECT 1 FROM Games WHERE GameName = @GameName);";
        try
        {

            using (SqlConnection connection = new SqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                SqlCommand insertDesignerRows;
                int  insertCounter = 0;
                foreach (Game game in games)
                {
                    insertDesignerRows = new SqlCommand(insertQuery, connection);
                    insertDesignerRows.Parameters.AddWithValue("@GameName", game.GameName);
                    insertDesignerRows.Parameters.AddWithValue("@Genre", game.Genre);
                    insertDesignerRows.Parameters.AddWithValue("@Price", game.Price);
                    insertDesignerRows.Parameters.AddWithValue("@DesignerId", game.DesignerId);
                    insertCounter += insertDesignerRows.ExecuteNonQuery();
                    status = insertCounter + " Rows inserted....";
                }
            }
        }
        catch (Exception ex)
        {
            status = ex.Message;
        }
        return status;
 }

 }
}