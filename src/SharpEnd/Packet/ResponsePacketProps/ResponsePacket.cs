using SharpEnd.Resources;

namespace SharpEnd.Packet
{
    public class ResponsePacket
    {
        public PacketProtocol Protocol { get; set; }
        public ResponseStatus Status { get; set; }
        public PacketHeaders Headers { get; set; }
        public string Body { get; set; }

        public ResponsePacket(PacketProtocol Protocol, ResponseCode Code, PacketHeaders Headers)
        {
            this.Protocol = Protocol;
            Status = new ResponseStatus(Code);
            this.Headers = Headers;
            Body = String.Empty;
        }

        public ResponsePacket(PacketProtocol Protocol, ResponseCode Code, PacketHeaders Headers, string Body)
        {
            this.Protocol = Protocol;
            Status = new ResponseStatus(Code);
            this.Headers = Headers;
            this.Body = Body;
        }

        public ResponsePacket(PacketProtocol Protocol, ResponseCode Code, PacketHeaders Headers, View Body)
        {
            this.Protocol = Protocol;
            Status = new ResponseStatus(Code);
            this.Headers = Headers;
            this.Body = Body.Content;
        }

        public ResponsePacket(string packet)
        {

            if (!packet.Contains("\r\n\r\n"))
            {
                packet += "\r\n\r\n";
            }
            ParsePacket(packet);

        }

        private void ParsePacket(string PacketText) 
        {
            string[] packetParts = PacketText.Split(Utility.DoubleNewLineDelimiters, StringSplitOptions.None);
            string[] headerParts = packetParts[0].Split(Utility.NewLineDelimiters, StringSplitOptions.None);
            string[] protocolParts = headerParts[0].Split(new string[] { " " }, StringSplitOptions.None);
            ParseProtocol(protocolParts[0]);
            ParseStatus(protocolParts[1]);
            ParseHeaders(headerParts);
            ParseBody(packetParts[1]);
        }

        private void ParseProtocol(string ProtocolText) 
        {
            Protocol = new PacketProtocol(ProtocolText);
        }

        private void ParseStatus(string StatusText) 
        {
            Status = new ResponseStatus((ResponseCode)Convert.ToInt32(StatusText));
        }

        private void ParseHeaders(string[] HeaderParts) 
        {
            string[] headersArray = HeaderParts.Skip(1).ToArray();
            Headers = new PacketHeaders(headersArray);
        }

        private void ParseBody(string BodyText) 
        {
            Body = BodyText;
        }
        //override ToString() to return the packet as a string
        public override string ToString()
        {
            string packet = String.Empty;
            packet += Protocol.Name + "/" + Protocol.Version + " " + Status.Code + " " + Status.Message + "\r\n";
            packet += Headers.ToString();
            packet += "\n";
            packet += Body;
            return packet;
        }

    }
}
