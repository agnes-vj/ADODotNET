using ADO.NET;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;

var designers = Utils.DeserializeFromFile<List<Designer>>("./Resources/Designers.json");
var games = Utils.DeserializeFromFile<List<Game>>("./Resources/Games.json");

Utils.DropAndCreateDatabase();

String status;

DBService dbService = new(Utils.CONNECTION_STRING);

status = dbService.DropAndCreateTableDesigners();
Console.WriteLine("Create table Designers ...." + status);

status = dbService.DropAndCreateTableGames();
Console.WriteLine("Create table Designers ...." + status);

string firstName = "Elizabeth";
string lastName = "Hargrave";

status = dbService.insertSingleRecordToDesigners(firstName, lastName);
Console.WriteLine("Insert To table Designers ...." + status);



status = dbService.insertSingleRecordToGames("Wingspan", "EngineBuilding", 3500, 1);
Console.WriteLine("Insert To table Games ...." + status);

status = dbService.insertSingleRecordToGames("Mariposas", "Set Collection", 3000, 1);
Console.WriteLine("Insert To table Games ...." + status);

status = dbService.deleteFromGames("Mariposas");
Console.WriteLine("Delete from table Games ...." + status);

Console.WriteLine("Inserting Set of data to Designers table");
status = dbService.InsertDesignerRecords(designers);
Console.WriteLine("Status : " + status);

Console.WriteLine("Inserting Set of data to Games table");
status = dbService.InsertGamesRecords(games);
Console.WriteLine("Status : " + status);

Console.WriteLine("       Table -  Designers       ");
dbService.DisplayDesignerRecords();
Console.WriteLine("       Table  - Games       ");

dbService.DisplayGameRecords();


