namespace SharpEnd.Packet
{
    public class PacketHeader
    {
        public string Name { get; private set; }
        public string Value { get; private set; }
        public PacketHeader(string header)
        {
            string[] HeaderParts = header.Split(new string[] { ": " }, StringSplitOptions.None);
            Name = HeaderParts[0];
            Value = HeaderParts[1];
        }
        public override string ToString()
        {
            return Name + ": " + Value;
        }
    }
}
