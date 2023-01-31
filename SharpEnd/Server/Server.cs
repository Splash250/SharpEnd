using System.Net.Sockets;
using System.Net;
using System.Text;
using SharpEnd.Packet;
using SharpEnd.Resources;

namespace SharpEnd.Server
{
    public class Server
    {
        private List<Socket> Clients { get; set; }
        private readonly Socket serverSocket;
        private bool isRunning;
        public Routes routes;
        public Server()
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
                    Console.WriteLine("Client Connected!");
                    HandleClient(client);
                }
            });
        }

        private async void HandleClient(Socket client)
        {
            Clients.Add(client);
            try
            {
                while (isRunning)
                {
                    byte[] receivedBytes = await NetworkUtils.ReadAllBytesAsync(client);
                    string data = Encoding.UTF8.GetString(receivedBytes, 0, receivedBytes.Length);
                    RequestPacket requestPacket = new(data);

                    ResponsePacket responsePacket = HandleRequest(requestPacket);

                    Console.WriteLine("Received: {0}", data);

                    byte[] message = Encoding.UTF8.GetBytes(responsePacket.ToString());
                    NetworkUtils.SendBytesAsync(client, message);
                    Console.WriteLine("Sent: {0}", responsePacket.ToString());
                }
            }
            catch (Exception)
            {
                return;
            }

        }
        public void AddRoute(string path, Route.ControllerDelegate controller)
        {
            routes.Add(path, controller);
        }
        private void DisconnectClient(Socket client)
        {
            Clients.Remove(client);
            client.Close();
        }

        private ResponsePacket HandleRequest(RequestPacket requestPacket)
        {
            ResponsePacket? responsePacket = null;

            if (!Utility.IsFilePath(requestPacket.Path)) 
            {
                Route route = routes.GetRoute(requestPacket.Path);
                if (route != null)
                    responsePacket = route.Controller(requestPacket);
                else

                    responsePacket = DefaultResponsePackets.NotFoundPacket;
            }
            else
                //not yet implemented but here we should send the actual file bytes 
                responsePacket = DefaultResponsePackets.NotFoundPacket;


            return responsePacket;
        }

        private void CloseServer()
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
