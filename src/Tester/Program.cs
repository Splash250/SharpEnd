using SharpEnd;
using SharpEnd.Packet;
using Tester;

SharpEndWebServer app = new SharpEndWebServer("../../../../html");

app.Route(RequestMethod.POST, "/index", Controller.Index);
app.Route(RequestMethod.GET, "/index", Controller.Index);

app.Route(RequestMethod.GET, "/other", Controller.OtherPage);
app.Route(RequestMethod.GET, "/db", Controller.DatabasePage);

app.Start(8080, 20);
DB DB = new();
DB.Migrate();
DB.Seed();

Test test = new Test(DB.Connection);

test.Instance.testName = "customTest";
test.Instance.sucessful = 0;
test.Instance.testReturnNumber = 3;
Console.WriteLine(test.Instance.testName);
test.SaveInstance();

Console.ReadKey();


