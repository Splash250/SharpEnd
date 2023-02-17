namespace SharpEnd.Packet
{
    public class PacketProtocol
    {
        public string Name { get; private set; }
        public string Version { get; private set; }

        public static PacketProtocol Default
        {
            get
            {
                return new("HTTP/1.1");
            }
        }

        public PacketProtocol(string protocol) 
        {

            if (protocol.Contains('/'))
            {
                string[] protocolParts = protocol.Split(new string[] { "/" }, StringSplitOptions.None);
                Name = protocolParts[0];
                Version = protocolParts[1];
            }
            else
            {
                Name = protocol;
                Version = "";
            }
        }
    }
}
