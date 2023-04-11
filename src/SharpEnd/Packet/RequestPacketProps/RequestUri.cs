using System.Text;

namespace SharpEnd.Packet
{
    public class RequestUri
    {
        public RequestHost Host { get; set; }
        public string Path { get; set; }
        public string Fragment { get; set; }

        public bool HasFragment => !String.IsNullOrEmpty(Fragment);

        public RequestUri() 
        {

            Host = new RequestHost();
            Path = String.Empty;
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
            string[] urlParts = url.Split('?');
            string[] hostParts = urlParts[0].Split('/');
            Host = new RequestHost(hostParts[0]);
            Path = hostParts[1];

            if(url.Contains("#"))
            {
                string[] urlParts2 = url.Split('#');
                Fragment = urlParts2[1];
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Host.ToString());
            sb.Append('/');
            sb.Append(Path);

            if (Fragment != null)
                sb.Append('#').Append(Fragment);

            return sb.ToString();
        }
    }
}
