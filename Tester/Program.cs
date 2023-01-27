// See https://aka.ms/new-console-template for more information
using SharpEnd.Server;
using Tester;

Server server = new();
server.Start(8080, 20);
server.AddRoute("/index", Controller.Index);
server.AddRoute("/other", Controller.OtherPage);
DB DB = new();
DB.Migrate();
DB.Seed();
Console.ReadKey();


