using SharpEnd.Packet;
using SharpEnd.Resources;
using SharpEnd.Server;

namespace SharpEnd
{
    public class SharpEndWebServer
    {
        private WebServer WebServer;
        public SharpEndWebServer(string HTMLPath = "html")
        {
            WebServer = new WebServer(HTMLPath);
        }
        public void Start(int Port, int Backlog)
        {
            WebServer.Start(Port, Backlog);
        }

        public void Route(RequestMethod Method,string Path, Route.ControllerDelegate Controller)
        { 
            WebServer.AddRoute(Method, Path, Controller);
        }

        public void Stop()
        {
            WebServer.CloseServer();
        }

    }
}
