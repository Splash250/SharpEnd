using SharpEnd.Packet;
using SharpEnd.Resources;
using SharpEnd.Server;

namespace SharpEnd
{
    public class SharpEndWebServer
    {
        private WebServer _webServer;
        public SharpEndWebServer(string htmlPath = "html")
        {
            _webServer = new WebServer(htmlPath);
        }
        public void Start(int port, int backlog)
        {
            _webServer.Start(port, backlog);
        }

        public void Route(RequestMethod method,string path, Route.ControllerDelegate controller)
        { 
            _webServer.AddRoute(method, path, controller);
        }

        public void Stop()
        {
            _webServer.CloseServer();
        }

    }
}
