namespace SharpEnd.Packet
{
    public class PacketHeader
    {
        public string Name { get; private set; }
        public string Value { get; private set; }
        public PacketHeader(string Header)
        {
            string[] HeaderParts = Header.Split(new string[] { ": " }, StringSplitOptions.None);
            Name = HeaderParts[0];
            Value = HeaderParts[1];
        }

        //override ToString() to return the header in the format "Name: Value"
        public override string ToString()
        {
            return Name + ": " + Value;
        }
    }
}
