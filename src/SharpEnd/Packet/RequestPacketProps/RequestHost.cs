using System.Net;

namespace SharpEnd.Packet
{
    public class RequestHost
    {
        public string Domain { get; private set; }
        private int? _port;
        public int? Port { 
            get 
            {
                if (_port == null)
                    return 80;
                return _port;
            }
            private set 
            {
                _port = value;
            }
        }
        public bool IsDNS 
        { 
            get 
            {
                return Domain.Any(c => char.IsLetter(c));
            } 
        }
        public RequestHost Empty 
        {
            get
            {
                return new RequestHost();
            }
        }
        public RequestHost() 
        {
            Domain = String.Empty;
            Port = 80;
        }
        public RequestHost(string hostString) 
        {
            if (hostString.Contains(':'))
            {
                string[] hostParts = hostString.Split(':');
                Domain = hostParts[0];
                _port = int.Parse(hostParts[1]);
            }
            else 
            {
                Domain = hostString;
                _port = 80;
            }  
        }
        public IPAddress ResolveIp()
        {
            if (IsDNS) {
                IPHostEntry ipHostInfo = Dns.GetHostEntry(Domain);
                return ipHostInfo.AddressList[0];
            }
            else
                return IPAddress.Parse(Domain);
        }
        public override string ToString()
        {
            return Domain + ":" + Port;
        }
    }
}
