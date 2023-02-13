namespace SharpEnd.Packet
{
    public class RequestQuery
    {
        public Dictionary<string,string> Values { get; private set; }
        public RequestQuery(string Query) 
        {
            Values = new Dictionary<string, string>();
            if (Query.Contains('&'))
            {
                ReadQuery(Query);
            }

        }
        private void ReadQuery(string Query)
        {
            string[] QueryParts = Query.Split(new string[] { "&" }, StringSplitOptions.None);
            foreach (string QueryPart in QueryParts)
            {
                string[] QueryPartParts = QueryPart.Split(new string[] { "=" }, StringSplitOptions.None);
                Values.Add(QueryPartParts[0], QueryPartParts[1]);
            }
        }
    }
}
