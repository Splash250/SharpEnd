using System.Text;

namespace SharpEnd.Packet
{
    public class RequestUri
    {
        public RequestHost Host { get; set; }
        public string Path { get; set; }
        public RequestQuery Query { get; set; }
        public string Fragment { get; set; }

        public bool HasQuery => Query != null;
        public bool HasFragment => !String.IsNullOrEmpty(Fragment);

        public RequestUri() 
        {

            Host = new RequestHost();
            Path = String.Empty;
            Query = RequestQuery.Empty;
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
        public RequestUri(string host, string path, string query)
        {
            Host = new RequestHost(host);
            Path = path;
            Query = new RequestQuery(query);
        }
        public RequestUri(string host, string path, string query, string fragment)
        {
            Host = new RequestHost(host);
            Path = path;
            Query = new RequestQuery(query);
            Fragment = fragment;
        }
        public RequestUri(string host, string path, RequestQuery query)
        {
            Host = new RequestHost(host);
            Path = path;
            Query = query;
        }
        public RequestUri(string host, string path, RequestQuery query, string fragment)
        {
            Host = new RequestHost(host);
            Path = path;
            Query = query;
            Fragment = fragment;
        }
        public RequestUri(string url)
        {
            string[] urlParts = url.Split('?');
            string[] hostParts = urlParts[0].Split('/');
            Host = new RequestHost(hostParts[0]);
            Path = hostParts[1];
            if (urlParts.Length > 1)
                Query = new RequestQuery(urlParts[1]);

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

            if (Query != null)
                sb.Append('?').Append(Query.ToString());

            if (Fragment != null)
                sb.Append('#').Append(Fragment);

            return sb.ToString();
        }
    }
}
