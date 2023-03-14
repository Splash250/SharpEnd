using SharpEnd.Model;
using SharpEnd.Packet;
using SharpEnd.Resources;
using SharpEnd.Cookies;
using System;

namespace Tester
{
    //Controller.cs

    //this class contains all the methods that will be executed when the request is made to the specified path using the specified method
    internal class Controller
    {
        //all of the methods are static 
        //this class contains all methods that are associated with routes
        public static ResponsePacket Index(RequestPacket requestPacket)
        {
            //here we have to create a view that has 3 parameters
            //first the name of the view
            //second is the file where the content of the view is located
            //third parameter is an optional parameter that if defined can replace variables (defined inside the html file like this: {{exampleVariable}} ) with custom dynamic content
            Session sess = Session.Start(requestPacket);

            
            int count = 0;
            if (sess.IsSet("count"))
                count = int.Parse(sess["count"]);

            //string cookieValues = String.Join(", ", requestPacket.Cookies.GetCookies(requestPacket.Uri).Select(x => x.Value));

            int rand = 0;
            if (sess.IsSet("random"))
                rand = int.Parse(sess["random"]);

            View view = View.Create(
                "index",
                "index.html",
                new string[] {
                    "pathLocation=" + requestPacket.Uri.Path,
                    "randomNum=" + rand,
                    "cookieCount=" + count,
                });
            //every request has to have a response 
            //here we have to define the response object that is sent to the client after the method is called
            //the response packet has 4 parameters (some of it are optional)
            //the first parameter is an optional parameter which defines what http protocol the response packet use
            //the second parameter is the response code 
            //the third one defines the headers that the response has
            //the fourth one is optional aswell. it can be either a view object or a string which represents the response body 
            ResponsePacket response = ResponsePacket.HTMLResponsePacket(view);
            Cookie cookie = new Cookie("count", (++count).ToString(), requestPacket.Uri.Path, requestPacket.Uri.Host.Domain);
            response.SetCookie(cookie);
            cookie = new Cookie("random", new Random().Next(1000).ToString(), requestPacket.Uri.Path, requestPacket.Uri.Host.Domain);
            response.SetCookie(cookie);

            return response;

        }

        //the same implementation happens here
        public static ResponsePacket OtherPage(RequestPacket requestPacket)
        {
            View view = View.Create(
                "other",
                "other.html",
                new string[] {
                    "currentDate=" + DateTime.UtcNow.Date.ToString("dd/MM/yyyy"),
                });

            return new ResponsePacket(
                ResponseCode.OK,
                new PacketHeaderCollection(new string[] {
                    "Content-Type: text/html; charset=UTF-8",
                    "Content-Length: " + view.Content.Length
                }),
                view);
        }

        //here we include a table from the test model's data to the webpage
        //everything else is the same as above
        public static ResponsePacket DatabasePage(RequestPacket requestPacket)
        {
            DB DB = new();
            Test model = new(DB.Connection);

            //todo: make a custom request class that can implement guards and easier handling to the payload's values
            //also should make some extension methods to the request packet for example: Url() or Is() or RouteIs() or IsMethod() ect. to make things cleaner
            if (requestPacket.Method == RequestMethod.POST)
            {
                dynamic Payload = requestPacket.Payload;
                if (Payload.HasThese(new[] { "testname", "sucessful", "returnOutput" }))
                    model.Instance.testName = Payload.testname;
                    model.Instance.sucessful = Payload.sucessful;
                    model.Instance.testReturnNumber = Payload.returnOutput;
                model.SaveInstance();
            }
            View view = CreateDBPage(model);
            return ResponsePacket.HTMLResponsePacket(view);
        }
        //here we create a table from all the data inside 'testtable' table
        public static View CreateDBPage(Test fromModel) 
        {

            string pageData = ViewUtils.TableFromModelQuery(fromModel.GetColumns(),
                                                            fromModel.All());
            View view = View.Create(
                "other",
                "db.html",
            new string[] {
                    $"pageData={pageData}"
                });
            return view;
        }
    }
}
