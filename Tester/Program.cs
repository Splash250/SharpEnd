// See https://aka.ms/new-console-template for more information
using SharpEnd.Model;
using SharpEnd.MySQL;
using SharpEnd.Server;
using Tester;

Server server = new();
server.Start(8080, 20);
server.AddRoute("/index", Controller.Index);
server.AddRoute("/other", Controller.OtherPage);
server.AddRoute("/db", Controller.DatabasePage);

DB DB = new();
Test test = new Test(DB.Connection);

foreach (dynamic item in test.All())
{
    Console.WriteLine(item.testName);
}

foreach (dynamic item in test.All())
{
    Console.WriteLine(item.testReturnNumber);
}

Console.ReadKey();


