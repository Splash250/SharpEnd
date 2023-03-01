using System.Dynamic;
using System.Text.Json;

namespace SharpEnd.Packet
{
    internal static class PayloadUtils
    {

        public static ExpandoObject ToExpandoObject(string payload, string contentType)
        {
            dynamic obj = new ExpandoObject();

            if (contentType.Contains("json"))
                obj = ParseJSON(payload);
            else
                ParseGenericQuery(obj, payload);

            return obj;
        }

        private static ExpandoObject ParseJSON(string payload) => JsonSerializer.Deserialize<ExpandoObject>(payload);

        private static void ParseGenericQuery(ExpandoObject toObj, string payload) 
        {
            Dictionary<string, string> parsed = ParseRequestPayload(payload);

            foreach (var key in parsed.Keys)
                ((IDictionary<string, object>)toObj)[key] = parsed[key];
        }
        private static Dictionary<string, string> ParseRequestPayload(string payload)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            if (payload.Contains('&'))
            {
                string[] payloadParts = payload.Split(new string[] { "&" }, StringSplitOptions.None);
                foreach (string payloadPart in payloadParts)
                {
                    string[] payloadPartParts = payloadPart.Split(new string[] { "=" }, StringSplitOptions.None);
                    data.Add(payloadPartParts[0], payloadPartParts[1]);
                }
            }
            return data;
        }

        public static bool IsNullOrEmpty(ExpandoObject obj) => obj == null || obj == new ExpandoObject();

    }
}
