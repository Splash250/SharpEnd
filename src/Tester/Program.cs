//we have to include these usings in order to be able to worh with SharpEnd
using SharpEnd;
using SharpEnd.HttpKit;
using SharpEnd.Packet;
using SharpEnd.Resources;

namespace Tester
{
    class Program
    {
        public static DB dataBase;
        static void Main()
        {

            //here is the code where we use the contents of DB.cs:
            //first we initiate a new DB variable
            dataBase = new();
            //you can call the Migrate method of the DB to create the specific table with the specific columns
            dataBase.Migrate();
            //when you call the seed method you fill up the database with example values that are generated inside te code
            dataBase.Seed();

            //here we create our web app that has an optional parameter that defines the default path where all the frontend files are located (html files, css, images, javascript, ect..)
            SharpEndWebServer app = new("../../../../html");

            //here we can add routes to the app we created previously.
            //a route has 3 parameters:
            //first is the request method that the route refers to
            //second is the path that the user can interact with
            //third is the controller method that runs when the request is made to the specified path using the specified method
            app.Route(RequestMethod.GET, "/index", Controller.Index);
            app.Route(RequestMethod.GET, "/other", Controller.OtherPage);
            app.Route(RequestMethod.GET, "/db", Controller.DatabasePage);
            app.Route(RequestMethod.GET, "/cleardb", Controller.ClearDatabase);
            app.Route(RequestMethod.POST, "/db", Controller.DatabasePage);


            //you can also achieve the same but by passing anonymous async methods
            app.Route(RequestMethod.GET, "/test", 
                async request =>
                {
                    string body = "Anonymous :3";
                    return new ResponsePacket(
                    PacketProtocol.Default, ResponseCode.OK,
                    new PacketHeaderCollection(new string[] {
                        "Content-Type: text/html; charset=UTF-8",
                        "Content-Length: " + body.Length
                    }),
                    body);
                }); 
            //here we start our app which starts the webserver on port 8080 and the backlog of 20

            app.Start(1234, 20);



            //here we create a model that is called Test
            //it stores the values inside a specific table inside the database
            //you have to pass the database connection at the initialization of the model 
            Test test = new(dataBase.Connection);

            //here we can assign single values to different attributes of the model that we made 
            //this is an example test result that we want to insert into the database:
            test.Instance.testName = "customTest";
            test.Instance.sucessful = 0;
            test.Instance.testReturnNumber = 3;

            //here we save the instance to the database 
            test.SaveInstance();

            ////here we stop the webserver app




            //string[] headerStrings = new[]
            //{
            //    "Host: info.cern.ch",
            //    "User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/112.0.0.0 Safari/537.36",
            //    "Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7"
            //};


            //WebRequest wb = WebRequest.Create("info.cern.ch");
            //wb.LoadHeaders(headerStrings);
            //ResponsePacket p = wb.GetResponse();
            //Console.WriteLine(p.ToString());


            Console.ReadLine();
        }
    }
}


