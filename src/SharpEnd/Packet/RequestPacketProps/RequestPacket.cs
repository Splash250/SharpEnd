using SharpEnd.Cookies;
using SharpEnd.Miscellaneous;
using System.Dynamic;

namespace SharpEnd.Packet
{
    public class RequestPacket
    {

        public RequestMethod Method { get; private set; }
        public RequestQuery Query { get; private set; }
        public string Fragment { get; private set; }
        public PacketProtocol Protocol { get; private set; }
        public PacketHeaderCollection Headers { get; private set; }
        public ExpandoObject Payload { get; private set; }
        public CookieContainer Cookies { get; private set; }

        private string _rawPacket;

        private string _path;
        private RequestHost _host;
        public RequestUri Uri 
        {
            get 
            {
                return new RequestUri(_host, _path);
            }
        }


        public bool HasPayload
        {
            get
            {
                return !PayloadUtils.IsNullOrEmpty(Payload);
            }
        }
        public RequestPacket(string packet)
        {

            string[] lines = SplitPacket(packet);
            string[] requestLine = lines[0].Split(' ');
            _rawPacket = packet;
            ParseMethod(requestLine[0]);
            ParseQueryAndPath(requestLine[1]);
            ParseProtocol(requestLine[2]);
            ParseHeaders(lines);
            ParseHost(Headers.TakeHeader("Host"));
            ParseCookies();
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

            AddPayloadMethods();
        }

        private void AddPayloadMethods() 
        {
            AddHasMethod();
            AddHasTheseMethod();
        }
        private void AddHasTheseMethod() 
        {
            Func<string[], bool> func = new(propertyNames =>
            {
                foreach (var propertyName in propertyNames)
                    if (!((IDictionary<string, object>)Payload).ContainsKey(propertyName))
                        return false;
                return true;
            });
            AddMethodToPayload("HasThese", func);
        }
        private void AddHasMethod() 
        {
            Func<string, bool> func = new(propertyName => {
                return ((IDictionary<string, object>)Payload).ContainsKey(propertyName);
            });
            AddMethodToPayload("Has", func);
        }
        private void AddMethodToPayload<T, TResult>(string methodName, Func<T, TResult> method)
        {
            ((IDictionary<string, object>)Payload)[methodName] = method;
        }
        private void ParseHost(string hostString)
        {
            _host = new RequestHost(hostString);
        }

        private void ParseHeaders(string[] headerLines)
        {
            Headers = new PacketHeaderCollection(ReadHeaders(headerLines));
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
                _path = pathParts[0];
                Query = new RequestQuery(pathParts[1]);
            }
            else
            {
                _path = path;
                Query = new RequestQuery(String.Empty);
            }
        }

        private ExpandoObject GetRequestPayload(string wholePacket) 
        {
            string body = String.Empty;
            if (ContainsPayload(wholePacket)) 
            {
                string[] packetParts = wholePacket.Split(Utility.DoubleNewLineDelimiters, 2, StringSplitOptions.None);
                body = packetParts.Last();
                return PayloadUtils.ToExpandoObject(body, GetContentType());
            }
            return new ExpandoObject();
        }
        private string GetContentType()
        {
            //read the content type from the headers
            return Headers.GetHeaderValue("Content-Type");
        }
        private static bool ContainsPayload(string wholePacket) 
        {
            var sections = wholePacket.Split(Utility.DoubleNewLineDelimiters, StringSplitOptions.None);

            if (sections.Length < 2)
                return false;

            var payload = sections[1].Trim();
            return !string.IsNullOrEmpty(payload);
        }
        private string[] ReadHeaders(string[] rawPacketLines) 
        {
            return rawPacketLines.Skip(1)
                .TakeWhile(
                    line => !string.IsNullOrEmpty(line)
                ).ToArray();
        }

        private void ParseCookies()
        {
            string cookieHeader = Headers.TakeHeader("Cookie");
            Console.WriteLine(cookieHeader);
            Cookies = CookieContainer.Parse(cookieHeader, Uri);
        }

        public override string ToString()
        {
            return _rawPacket;
        }
    }   
}
