using SharpEnd.Miscellaneous;
using System.Text;

namespace SharpEnd.Packet
{
    public class PacketHeaderCollection
    {
        private readonly MultiDictionary<string, PacketHeader> _headerDict;

        public PacketHeaderCollection()
        {
            _headerDict = new MultiDictionary<string, PacketHeader>();
        }

        public PacketHeaderCollection(string[] headerLines) : this()
        {
            foreach (string header in headerLines)
            {
                string[] headerParts = header.Split(new string[] { ": " }, StringSplitOptions.None);
                if (headerParts.Length == 2)
                    AddHeader(headerParts[0], headerParts[1]);
            }
        }
    
        public void AddHeader(string name, string value)
        {
            PacketHeader header = new PacketHeader(name, value);
            _headerDict.Add(name, header);
        }

        public void Remove(string headerName)
        {
            _headerDict.RemoveKey(headerName);
        }

        public string TakeHeader(string headerName)
        {
            if (_headerDict.TryGetValue(headerName, out PacketHeader header))
            {
                _headerDict.RemoveKey(headerName);
                return header.Value;
            }
            else
                return String.Empty;
        }
        public List<string> TakeHeaderValues (string headerName)
        {
            List<string> values = new List<string>();
            if (_headerDict.TryGetValues(headerName, out List<PacketHeader> headers))
            {
                foreach (PacketHeader header in headers)
                    values.Add(header.Value);
                _headerDict.RemoveKey(headerName);
            }
            return values;
        }
        public string? GetHeaderValue(string headerName)
        {
            if (_headerDict.TryGetValue(headerName, out PacketHeader header))
                return header.Value;
            else
                return null;
        }
        public List<string> GetHeaderValues(string headerName)
        {
            List<string> values = new List<string>();
            if (_headerDict.TryGetValues(headerName, out List<PacketHeader> headers))
                foreach (PacketHeader header in headers)
                    values.Add(header.Value);
            return values;
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (List<PacketHeader> headerValues in _headerDict.Values)
            {
                foreach (PacketHeader item in headerValues)
                {
                    sb.Append(item.ToString());
                    sb.Append("\r\n");
                }

            }
            return sb.ToString();
        }
        public bool Has(string headerName)
        {
            return _headerDict.Has(headerName);
        }

        public string this[string headerName]
        {
            get
            {
                if (_headerDict.TryGetValue(headerName, out PacketHeader header))
                    return header.Value;
                else
                    return String.Empty;
            }
            set
            {
                if (value == null)
                    _headerDict.RemoveKey(headerName);
                else
                    AddHeader(headerName, value);
            }
        }
    }
}