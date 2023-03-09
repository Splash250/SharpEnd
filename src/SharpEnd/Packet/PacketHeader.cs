namespace SharpEnd.Packet
{
    internal class PacketHeader
    {
        public string Name { get; private set; }
        public string Value { get; private set; }
        public PacketHeader(string header)
        {
            string[] HeaderParts = header.Split(new string[] { ": " }, StringSplitOptions.None);
            Name = HeaderParts[0];
            Value = HeaderParts[1];
        }
        public PacketHeader(string name, string value)
        {
            Name = name;
            Value = value;
        }
        public override string ToString()
        {
            return Name + ": " + Value;
        }
        public List<string> GetValues()
        {
            List<string> values = new();
            if (Value.Contains(','))
            {
                string[] splitValues = Value.Split(new string[] { ", ", "," }, StringSplitOptions.None);
                foreach (string value in splitValues)
                    values.Add(value);
            }
            else
                values.Add(Value);
            return values;
        } 
    }
}
