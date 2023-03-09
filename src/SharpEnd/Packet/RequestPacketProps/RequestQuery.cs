namespace SharpEnd.Packet
{
    public class RequestQuery
    {
        public Dictionary<string,string> Values { get; private set; }
        public RequestQuery(string query) 
        {
            Values = new Dictionary<string, string>();
            if (query.Contains('&'))
                ReadQuery(query);

        }
        private void ReadQuery(string query)
        {
            string[] queryParts = query.Split(new string[] { "&" }, StringSplitOptions.None);
            foreach (string queryPart in queryParts)
            {
                string[] queryPartParts = queryPart.Split(new string[] { "=" }, StringSplitOptions.None);
                Values.Add(queryPartParts[0], queryPartParts[1]);
            }
        }
    }
}
