using System.Collections;
using System.Text;

namespace SharpEnd.Packet
{
    public class RequestQuery : IEnumerable<KeyValuePair<string, string>>
    {
        public Dictionary<string,string> _values { get; private set; }
        public bool IsEmpty => _values.Count == 0;
        public static RequestQuery Empty => new RequestQuery();
        public RequestQuery() => _values = new Dictionary<string, string>();
        public RequestQuery(string query) 
        {
            _values = new Dictionary<string, string>();
            if (query.Contains('&'))
                ReadMultipartQuery(query);
            else
                ReadSingleQuery(query);
        }
        private void ReadSingleQuery(string query)
        {
            string[] queryParts = query.Split('=');
            _values.Add(queryParts[0], queryParts[1]);
        }
        private void ReadMultipartQuery(string query)
        {
            string[] queryParts = query.Split('&');
            foreach (string queryPart in queryParts)
            {
                ReadSingleQuery(queryPart);
            }
        }

        public bool Has (string key) => _values.ContainsKey(key);

        public string this[string key]
        {
            get
            {
                if (Has(key))
                    return _values[key];
                else
                    return String.Empty;
            }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, string> queryPart in _values)
            {
                sb.Append(queryPart.Key);
                sb.Append('=');
                sb.Append(queryPart.Value);
                sb.Append('&');
            }
            return sb.ToString().TrimEnd('&');
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            foreach (KeyValuePair<string, string> pair in _values)
            {
                yield return pair;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
