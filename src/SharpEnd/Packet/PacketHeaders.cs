namespace SharpEnd.Packet
{
    public class PacketHeaders
    {
        public List<PacketHeader> HeaderCollection { get; private set; }

        public static PacketHeaders Empty 
        {
            get { return new(); }
        }

        public PacketHeaders()
        {
            HeaderCollection = new();
        }

        public PacketHeaders(string[] headerLines)
        {
            HeaderCollection = new();
            foreach (string header in headerLines)
            {

                string[] headerParts = header.Split(new string[] { ": " }, StringSplitOptions.None);
                if (headerParts.Length == 2)
                {
                    HeaderCollection.Add(new(header));
                }
            }
        }
        public void Remove(string headerName)
        {
            HeaderCollection.RemoveAll(x => x.Name == headerName);
        }
        public string TakeHeader(string headerName) 
        {
            string headerValue = String.Empty;
            foreach (PacketHeader header in HeaderCollection)
            {
                if (header.Name == headerName)
                {
                    headerValue = header.Value;
                    HeaderCollection.Remove(header);
                    break;
                }
            }
            return headerValue;
        }
        public string GetHeaderValue(string headerName)
        {
            string headerValue = String.Empty;
            foreach (PacketHeader header in HeaderCollection)
            {
                if (header.Name == headerName)
                {
                    headerValue = header.Value;
                    break;
                }
            }
            return headerValue;
        }
        public void AddHeader(string header)
        {
            HeaderCollection.Add(new(header));
        }
        public override string ToString()
        {
            string headers = String.Empty;
            foreach (PacketHeader header in HeaderCollection)
            {
                headers += header.ToString() + "\r\n";
            }
            return headers;
        }
    }
}
