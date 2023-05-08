using System.Net;
using System.Text;

namespace SharpEnd.Packet
{
    public class RequestUri
    {
        public RequestHost Host { get; set; }
        public string Path { get; set; }
        public string Fragment { get; set; }
        private string _rawUri;

        public RequestUri() 
        {

            Host = new RequestHost();
            Path = "/";
            Fragment = String.Empty;
        }
        public RequestUri(RequestHost host, string path)
        { 
            Host = host; 
            Path = path; 
        }
        public RequestUri(string host, string path)
        {
            Host = new RequestHost(host);
            Path = path;
        }
        public RequestUri(string host, string path, string fragment)
        {
            Host = new RequestHost(host);
            Path = path;
            Fragment = fragment;
        }

        public RequestUri(string url)
        {
            _rawUri = url;
            if (HasFragment(url))
            {
                string[] urlParts = url.Split('#');
                Fragment = urlParts[1];
                url = urlParts[0];
            }

            if (HasQuery(url))
            {
                string[] urlParts = url.Split('?');
                url = urlParts[0];
            }

            if (HasPath(url))
            {
                string[] urlParts = url.Split('/', 2);
                Host = new RequestHost(urlParts[0]);
                Path = urlParts[1];
            }
            else
            {
                Host = new RequestHost(url);
                Path = "/";
            }
        }

        public IPEndPoint GetEndPoint()
        {
            return new IPEndPoint(Host.ResolveIp(), (int)Host.Port);
        }

        private static bool HasPath(string url) 
        {
            return url.Contains('/') && url.IndexOf('/') != url.Length - 1; 
        }
        private static bool HasQuery(string url) => url.Contains('?');
        private static bool HasFragment(string url) => url.Contains('#');

        public override string ToString()
        {
            StringBuilder sb = new();
            sb.Append(Host.ToString());
            sb.Append('/');
            sb.Append(Path);

            if (Fragment != null)
                sb.Append('#').Append(Fragment);

            return sb.ToString();
        }
    }
}
