namespace SharpEnd.Packet
{
    public class RequestPacket
    {

        public RequestMethod Method { get; private set; }
        public string Path { get; private set; }
        public RequestQuery Query { get; private set; }
        public string Fragment { get; private set; }
        public PacketProtocol Protocol { get; private set; }
        public RequestHost Host { get; private set; }
        public PacketHeaders Headers { get; private set; }
        public string Payload { get; private set; }

        public bool HasPayload
        {
            get
            {
                return !String.IsNullOrEmpty(Payload);
            }
        }

        public static Dictionary<string, string> DictifyPayload(string payload) 
        {
            return Utility.ParseRequestPayload(payload);
        }
        public RequestPacket(string packet)
        {

            string[] lines = SplitPacket(packet);
            string[] requestLine = lines[0].Split(' ');
            ParseMethod(requestLine[0]);
            ParseQueryAndPath(requestLine[1]);
            ParseProtocol(requestLine[2]);
            ParseHeaders(lines);
            ParseHost(Headers.TakeHeader("Host"));
            ParsePayload(packet);
        }
        private string[] SplitPacket(string packet) 
        {
            return packet.Split(new string[] { "\r\n" }, StringSplitOptions.None);
        }
        private void ParseMethod(string method) 
        {
            Method = (RequestMethod)Enum.Parse(typeof(RequestMethod), method);
        }
        private void ParsePayload(string packet) 
        {
            Payload = GetRequestPayload(packet);
        }
        private void ParseHost(string hostString)
        {
            Host = new RequestHost(hostString);
        }

        private void ParseHeaders(string[] headerLines)
        {
            Headers = new PacketHeaders(ReadHeaders(headerLines));
        }

        private void ParseProtocol(string protocol)
        {
            Protocol = new PacketProtocol(protocol);
        }

        private void ParseQueryAndPath(string path)
        {
            if (path.Contains('?'))
            {
                string[] pathParts = path.Split('?');
                Path = pathParts[0];
                Query = new RequestQuery(pathParts[1]);
            }
            else
            {
                Path = path;
                Query = new RequestQuery(String.Empty);
            }
        }

        private static string GetRequestPayload(string wholePacket) 
        {
            string body = String.Empty;
            if (ContainsPayload(wholePacket)) 
            {
                string[] packetParts = wholePacket.Split(Utility.DoubleNewLineDelimiters, 2, StringSplitOptions.None);
                body = packetParts.Last();
            }
            return body;
        }
        private static bool ContainsPayload(string wholePacket) 
        {
            return Utility.ContainsAny(wholePacket, Utility.DoubleNewLineDelimiters);
        }
        private string[] ReadHeaders(string[] rawPacketLines) 
        {
            return rawPacketLines.Skip(1)
                .TakeWhile(
                    line => !string.IsNullOrEmpty(line)
                ).ToArray();
        }

        public override string ToString()
        {
            throw new NotImplementedException();
        }
    }   
}
