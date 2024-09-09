using ADO.NET;

var designers = Utils.DeserializeFromFile<List<Designer>>("./Resources/Designers.json");
var games = Utils.DeserializeFromFile<List<Game>>("./Resources/Games.json");

string connectionString = "Server=<server_name>;Database=MyBoardGameCafe;User Id=<user_id>;Password=<password>;Trust Server Certificate=True";