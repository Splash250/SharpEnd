namespace SharpEnd.Packet
{
    public class RequestHost
    {
        public string Host { get; private set; }
        public int Port { get; private set; }
        public bool IsDNS { get; private set; }
        public RequestHost(string hostString) 
        {
            //if the string contains ':' split the host string by ':'
            if (hostString.Contains(':'))
            {
                string[] hostParts = hostString.Split(':');
                Host = hostParts[0];
                Port = int.Parse(hostParts[1]);
            }
            else 
            {
                Host = hostString;
                Port = 80;
            } 

            //check if the Host string contains alphabetic characters and set the IsDNS property accordingly
            IsDNS = Host.Any(c => char.IsLetter(c));


        }
    }
}
