// See https://aka.ms/new-console-template for more information
using SharpEnd.Model;
using SharpEnd.MySQL;
using SharpEnd.Server;
using Tester;

Server server = new();
server.Start(8080, 20);
server.AddRoute("/index", Controller.Index);
server.AddRoute("/other", Controller.OtherPage);
DB DB = new();
DB.Migrate();
DB.Seed();

Model model = new(DB.Connection, "testTable");
MySqlQuery query = new MySqlQuery();
model.SelectItems(DB.Connection,
                  query.Select("*").From("testTable"));

foreach (dynamic row in model.rows)
{
    Console.WriteLine(row.testName);
}
Console.ReadKey();


