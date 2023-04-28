using SharpEnd.Packet;
using SharpEnd.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SharpEnd.HttpKit
{
    public class WebRequest
    {
        public string Method { get; set; }
        public RequestUri Uri { get; set; }
        public PacketHeaderCollection Headers { get; set; }
        private readonly Socket _webClientScoket;
        private WebRequest ()
        {
            Method = String.Empty;
            Uri = new RequestUri();
            Headers = new PacketHeaderCollection();
            _webClientScoket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public static WebRequest Create (string uri)
        {
            return new WebRequest
            {
                Uri = new RequestUri(uri)
            };
        }
        public static WebRequest Create(RequestUri uri)
        {
            return new WebRequest
            {
                Uri = uri
            };
        }
        public bool LoadHeaders (Dictionary<string, string> headers)
        {
            if (headers == null)
                return false;

            Headers = new PacketHeaderCollection();

            foreach (KeyValuePair<string, string> header in headers)
                Headers.AddHeader(header.Key, header.Value);

            return true;
        }
        public bool LoadHeaders (string[] headers)
        {
            if (headers == null)
                return false;

            Headers = new PacketHeaderCollection();

            foreach (string header in headers)
            {
                string[] split = header.Split(new string[] { ": ", ":" }, StringSplitOptions.None);
                if (split.Length == 2)
                    Headers.AddHeader(split[0], split[1]);
                else
                    Headers.AddHeader(split[0], String.Empty);

            }

            return true;
        }
        public void AddHeader (string name, string value)
        {
            Headers ??= new PacketHeaderCollection();

            Headers.AddHeader(name, value);
        }

        public ResponsePacket GetResponse()
        {
            _webClientScoket.Connect(Uri.GetEndPoint());
            Console.WriteLine($"Path: |{Uri.Path}|");
            RequestPacket requestPacket = new(Uri.ToString(), RequestMethod.GET, Headers);
            Console.WriteLine($"|{requestPacket}|");
            byte[] packetBytes = Encoding.UTF8.GetBytes(requestPacket.ToString());
            NetworkUtils.SendBytes(_webClientScoket, packetBytes);

            byte[] responseBytes = NetworkUtils.ReceiveAll(_webClientScoket);
            string responseString = Encoding.UTF8.GetString(responseBytes);
            Console.WriteLine(responseString);
            return new ResponsePacket(responseString);

        }

    }
}
