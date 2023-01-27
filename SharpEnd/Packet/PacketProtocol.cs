using System;

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

        public PacketProtocol(string Protocol) 
        {
            //if protocol contains a slash, split it into name and version
            //else, set name to protocol and version to 1.1
            if (Protocol.Contains('/'))
            {
                string[] ProtocolParts = Protocol.Split(new string[] { "/" }, StringSplitOptions.None);
                Name = ProtocolParts[0];
                Version = ProtocolParts[1];
            }
            else
            {
                Name = Protocol;
                Version = "";
            }
        }
    }
}
