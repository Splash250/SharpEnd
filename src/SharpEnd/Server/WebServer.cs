﻿using System.Net.Sockets;
using System.Net;
using System.Text;
using SharpEnd.Packet;
using SharpEnd.Resources;
using SharpEnd.Utils;
using SharpEnd;


namespace SharpEnd.Server
{
    using RouteFunc = System.Func<SharpEnd.Packet.RequestPacket, System.Threading.Tasks.Task<SharpEnd.Packet.ResponsePacket>>;

    internal class WebServer
    {
        private List<Socket> _clients { get; set; }
        private Socket _serverSocket;
        private bool _isRunning;
        public static string HTMLPath { get; private set; } = "html";
        public RouteCollection Routes { get; private set; }
        public WebServer(string htmlPath)
        {
            SetDefaults();
            WebServer.HTMLPath = htmlPath;
        }
        public WebServer()
        {
            SetDefaults();
        }
        private void SetDefaults() 
        {
            _serverSocket = new Socket(
                AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp);
            _clients = new List<Socket>();
            _isRunning = true;
            Routes = new RouteCollection();

        }
        public void Start(int port, int backlog)
        {
            try
            {
                _serverSocket.Bind(new IPEndPoint(
                    IPAddress.Any,
                    port));
                _serverSocket.Listen(backlog);
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
                while (_isRunning)
                {
                    Socket client = await Task.Factory.FromAsync(
                        _serverSocket.BeginAccept,
                        _serverSocket.EndAccept,
                        null);
                    TryLoopHandleClient(client);
                }
            });
        }

        private async void TryLoopHandleClient(Socket client)
        {
            _clients.Add(client);
            try
            {
                while (_isRunning)
                    await HandleClient(client);
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
            string path = HTMLPath + packet.Uri.Path;
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

        public void AddRoute(RequestMethod method, string path, RouteFunc controller)
        {
            Routes.Add(method, path, controller);
        }
        private void DisconnectClient(Socket client)
        {
            _clients.Remove(client);
            client.Close();
        }

        private ResponsePacket HandleRequest(RequestPacket requestPacket)
        {
            ResponsePacket? responsePacket = null;
            Route route = Routes.GetRoute(requestPacket.Uri.Path, requestPacket.Method);
            if (route != null) 
            {
                if (route.RequestMethod == requestPacket.Method)
                {
                    responsePacket = HandleRoute(route, requestPacket).Result;
                    return responsePacket;
                }
                else
                    //todo: custom error page support in .htaccess file
                    responsePacket = DefaultResponsePackets.MethodNotAllowed;
            }
            else
                //todo: custom error page support in .htaccess file
                responsePacket = DefaultResponsePackets.NotFoundPacket;
            return responsePacket;
        }
        //make an async method that runs a route's controller this method should return a response packet
        private async Task<ResponsePacket> HandleRoute(Route route, RequestPacket requestPacket)
        {
            return await route.Controller(requestPacket);
        }

        public void CloseServer()
        {
            _isRunning = false;
            foreach (Socket client in _clients)
            {
                DisconnectClient(client);
            }
            _serverSocket.Close();
        }
        
    }
}
