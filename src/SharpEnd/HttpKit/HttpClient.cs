using SharpEnd.Packet;
using SharpEnd.Utils;
using System.Net.Sockets;
using System.Text;

namespace SharpEnd.HttpKit
{
    internal class HttpClient
    {
        private Socket _httpSocket;
        public HttpClient() 
        {
            _httpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
        public void Connect(RequestUri uri)
        {
            _httpSocket.Connect(uri.Host.ResolveIp(), (int)uri.Host.Port);
        }
        public void SendRequest(RequestPacket request)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(request.ToString());
            NetworkUtils.SendBytes(_httpSocket, bytes);
        }
        public string GetResponse()
        {
            byte[] buffer = NetworkUtils.ReceiveAll(_httpSocket);
            return Encoding.ASCII.GetString(buffer);
        }
    }
}
