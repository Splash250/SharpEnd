using SharpEnd.Cookies;
using SharpEnd.Utils;
using System.Dynamic;
using System.Text;

namespace SharpEnd.Packet
{
    public class RequestPacket : Packet
    {

        public RequestMethod Method { get; internal set; }
        public ExpandoObject Payload { get; internal set; }
        public RequestUri Uri { get; internal set; }
        public RequestQuery Query { get; set; }

        private readonly string _rawPacket;
        private string _payloadString;



        public bool HasPayload => !PayloadUtils.IsNullOrEmpty(Payload);
        public bool HasQuery 
        {
            get 
            {
                if (Query == null)
                    return false;
                return !Query.IsEmpty;
            }            
        }


        public RequestPacket(string uri, RequestMethod requestMethod, PacketHeaderCollection headerCollection)
        {
            _rawPacket = String.Empty;
            Uri = new RequestUri(uri);
            Headers = headerCollection;
            _payloadString = String.Empty;
            Protocol = PacketProtocol.Default;
        }
        public RequestPacket(string uri, RequestMethod requestMethod, string payload, PacketHeaderCollection headerCollection)
        {
            _rawPacket = String.Empty;
            Uri = new RequestUri(uri);
            Headers = headerCollection;
            _payloadString = payload;
            Protocol = PacketProtocol.Default;

        }
        public RequestPacket(string uri, RequestMethod requestMethod, PacketHeaderCollection headerCollection, CookieContainer cookies)
        {
            _rawPacket = String.Empty;
            Uri = new RequestUri(uri);

            Headers = headerCollection;
            Cookies = cookies;
            Protocol = PacketProtocol.Default;

        }
        public RequestPacket(string uri, RequestMethod requestMethod, string payload, PacketHeaderCollection headerCollection, CookieContainer cookies)
        {
            _rawPacket = String.Empty;
            Uri = new RequestUri(uri);
            Headers = headerCollection;
            Cookies = cookies;
            _payloadString = payload;
            Cookies = cookies;
            Protocol = PacketProtocol.Default;

        }
        public RequestPacket(string packet)
        {
            string[] lines = SplitPacket(packet);
            string[] requestLine = lines[0].Split(' ');
            _rawPacket = packet;
            Uri = new RequestUri();
            Query = RequestQuery.Empty;
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
            Uri.Host = new RequestHost(hostString);
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
            RequestQuery query = RequestQuery.Empty;
            string uriPath;
            Console.WriteLine(path);
            if (path.Contains('?'))
            {
                string[] pathParts = path.Split('?');
                uriPath = pathParts[0];
                try { query = new RequestQuery(pathParts[1]); }
                catch (QueryException ex) { throw ex; }
            }
            else
                uriPath = path;

            Query = query;
            Uri.Path= uriPath;

        }

        private ExpandoObject GetRequestPayload(string wholePacket) 
        {
            if (ContainsPayload(wholePacket)) 
            {
                string[] packetParts = wholePacket.Split(BasicUtility.DoubleNewLineDelimiters, 2, StringSplitOptions.None);
                string body = packetParts.Last();
                _payloadString = body;
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
            var sections = wholePacket.Split(BasicUtility.DoubleNewLineDelimiters, StringSplitOptions.None);

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
            
            if (string.IsNullOrEmpty(_rawPacket))
            {
                StringBuilder builder = new StringBuilder();
                builder.Append(Method.ToString().ToUpper());
                builder.Append(' ');
                builder.Append(Uri.Path);
                if (HasQuery)
                {
                    builder.Append('?');
                    builder.Append(Query.ToString());
                }
                builder.Append(' ');
                builder.Append(Protocol.ToString());
                builder.Append("\r\n");
                builder.Append(Headers.ToString());
                builder.Append("\r\n");
                if (HasPayload)
                {
                    builder.Append("\r\n");
                    builder.Append(_payloadString);
                }
                return builder.ToString();
            }
            return _rawPacket;
        }
    }   
}
