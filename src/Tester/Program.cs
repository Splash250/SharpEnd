//we have to include these usings in order to be able to worh with SharpEnd
using SharpEnd;
using SharpEnd.Packet;
using Tester;

namespace MyConsoleApp
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
            SharpEndWebServer app = new SharpEndWebServer("../../../../html");

            //here we can add routes to the app we created previously.
            //a route has 3 parameters:
            //first is the request method that the route refers to
            //second is the path that the user can interact with
            //third is the controller method that runs when the request is made to the specified path using the specified method
            app.Route(RequestMethod.GET, "/index", Controller.Index);
            app.Route(RequestMethod.GET, "/other", Controller.OtherPage);
            app.Route(RequestMethod.GET, "/db", Controller.DatabasePage);
            app.Route(RequestMethod.POST, "/db", Controller.DatabasePage);

            //here we start our app which starts the webserver on port 8080 and the backlog of 20

            app.Start(1234, 20);



            //here we create a model that is called Test
            //it stores the values inside a specific table inside the database
            //you have to pass the database connection at the initialization of the model 
            Test test = new Test(dataBase.Connection);

            //here we can assign single values to different attributes of the model that we made 
            //this is an example test result that we want to insert into the database:
            test.Instance.testName = "customTest";
            test.Instance.sucessful = 0;
            test.Instance.testReturnNumber = 3;

            //here we save the instance to the database 
            test.SaveInstance();

            //here we stop the webserver app
            Console.ReadLine();
        }
    }
}


