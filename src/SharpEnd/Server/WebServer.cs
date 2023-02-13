using System.Net.Sockets;
using System.Net;
using System.Text;
using SharpEnd.Packet;
using SharpEnd.Resources;

namespace SharpEnd.Server
{
    internal class WebServer
    {
        private List<Socket> Clients { get; set; }
        private Socket serverSocket;
        private bool isRunning;
        public static string HTMLPath { get; private set; } = "html";
        public Routes routes;
        public WebServer(string HTMLPath)
        {
            SetDefaults();
            WebServer.HTMLPath = HTMLPath;
        }
        public WebServer()
        {
            SetDefaults();
        }
        private void SetDefaults() 
        {
            serverSocket = new Socket(
                AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp);
            Clients = new List<Socket>();
            isRunning = true;
            routes = new Routes();

        }
        public void Start(int port, int backlog)
        {
            try
            {
                serverSocket.Bind(new IPEndPoint(
                    IPAddress.Any,
                    port));
                serverSocket.Listen(backlog);
                StartAccepting();
            }
            catch(Exception)
            {
                throw;
            }
        }
        private void StartAccepting()
        {
            Task.Run(async () =>
            {
                while (isRunning)
                {
                    Socket client = await Task.Factory.FromAsync(
                        serverSocket.BeginAccept,
                        serverSocket.EndAccept,
                        null);
                    TryLoopHandleClient(client);
                }
            });
        }

        private async void TryLoopHandleClient(Socket client)
        {
            Clients.Add(client);
            try
            {
                while (isRunning)
                {
                    await HandleClient(client);
                }
            }
            catch (Exception)
            {
                return;
            }

        }
        private async Task HandleClient(Socket client)
        {
            byte[] receivedBytes = await NetworkUtils.ReadAllBytesAsync(client);
            string data = Encoding.UTF8.GetString(receivedBytes, 0, receivedBytes.Length);
            RequestPacket requestPacket = new(data);

            if (NetworkUtils.IsFileRequest(requestPacket))
                HandleFileResponse(client, requestPacket);
            else
                HandleNonFileResponse(client, requestPacket);

        }

        private void HandleFileResponse(Socket client, RequestPacket packet)
        {
            string path = HTMLPath + packet.Path;
            int contentLength = File.ReadAllBytes(path).Length;
            ResponsePacket responsePacket = NetworkUtils.CraftFileSendHeaderPacket(contentLength);
            NetworkUtils.SendResponsePacketAsync(client, responsePacket);
            NetworkUtils.SendFileAsync(client, path);
        }

        private void HandleNonFileResponse(Socket client, RequestPacket packet) 
        {
            ResponsePacket responsePacket = HandleRequest(packet);
            NetworkUtils.SendResponsePacketAsync(client, responsePacket);
        }

        public void AddRoute(RequestMethod Method, string path, Route.ControllerDelegate controller)
        {
            routes.Add(Method, path, controller);
        }
        private void DisconnectClient(Socket client)
        {
            Clients.Remove(client);
            client.Close();
        }

        private ResponsePacket HandleRequest(RequestPacket requestPacket)
        {
            ResponsePacket? responsePacket = null;
            Route route = routes.GetRoute(requestPacket.Path, requestPacket.Method);
            if (route != null) 
            {
                if (route.RequestMethod == requestPacket.Method)
                {
                    responsePacket = route.Controller(requestPacket);
                    return responsePacket;
                }
                else
                {
                    //todo: custom error page support in .htaccess file
                    responsePacket = DefaultResponsePackets.MethodNotAllowed;
                }
            }
            else
            {
                //todo: custom error page support in .htaccess file
                responsePacket = DefaultResponsePackets.NotFoundPacket;
            }
            return responsePacket;
        }

        public void CloseServer()
        {
            isRunning = false;
            foreach (Socket client in Clients)
            {
                DisconnectClient(client);
            }
            serverSocket.Close();
        }
        
    }
}
