# ADO.NET

This repo is where you will be working on todays sprint. To get started you will first need to run the code contained in `.\db\Setup.sql` to get you started with a local database.

To run this file, open up a terminal and navigate to the root of the solution.

Run:

```
sqlcmd -S <server_name> -U <sql_username> -P <sql_password> -i .\db\Setup.sql 
```

Replace the `server_name`, `sql_username` and `sql_password` with your info.

To ensure this has worked correctly, open `SSMS` (SQL Server Management Studio) and navigate to your 'Databases'. 

You should see the new database `MyBoardGameCafe` on the list with two created tables: `Designers` and `Games`.