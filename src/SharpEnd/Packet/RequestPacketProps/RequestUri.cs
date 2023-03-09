namespace SharpEnd.Packet
{
    public class RequestUri
    {
        public RequestHost Host { get; set; }
        public string Path { get; set; }

        public RequestUri(RequestHost host, string path)
        { 
            Host = host; 
            Path = path; 
        }

    }
}
