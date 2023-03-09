namespace SharpEnd.Packet
{
    public class RequestHost
    {
        public string Domain { get; private set; }
        public int Port { get; private set; }
        public bool IsDNS 
        { 
            get 
            {
                return Domain.Any(c => char.IsLetter(c));
            } 
        }
        public RequestHost(string hostString) 
        {
            //if the string contains ':' split the host string by ':'
            if (hostString.Contains(':'))
            {
                string[] hostParts = hostString.Split(':');
                Domain = hostParts[0];
                Port = int.Parse(hostParts[1]);
            }
            else 
            {
                Domain = hostString;
                Port = 80;
            }  


        }
        public override string ToString()
        {
            return Domain + ":" + Port;
        }
    }
}
