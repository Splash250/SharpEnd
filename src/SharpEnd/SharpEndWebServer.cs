using SharpEnd.Packet;
using SharpEnd.Resources;
using SharpEnd.Server;


namespace SharpEnd
{
    using RouteFunc = System.Func<SharpEnd.Packet.RequestPacket, System.Threading.Tasks.Task<SharpEnd.Packet.ResponsePacket>>;

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

        public void Route(RequestMethod method,string path, RouteFunc controller)
        { 
            _webServer.AddRoute(method, path, controller);
        }

        public void Stop()
        {
            _webServer.CloseServer();
        }
        public RouteCollection Routes
        {
            get { return _webServer.Routes; }
        }
    }
}
