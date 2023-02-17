
namespace SharpEnd
{
    internal static class Utility
    {
        public static readonly string[] DoubleNewLineDelimiters = new string[] {
                    "\r\n\r\n",
                    "\n\n",
                    "\r\r"
                };
        public static readonly string[] NewLineDelimiters = new string[] {
                    "\r\n",
                    "\n",
                    "\r"
                };

        public static readonly int DefaultBufferSize = 1024;
        public static bool ContainsAny(string haystack, string[] needles)
        {
            foreach (string needle in needles)
            {
                if (haystack.Contains(needle))
                    return true;
            }

            return false;
        }
        public static string[] GetKeysFromDictionary(Dictionary<string, string> data) 
        {
            string[] keys = new string[data.Count];
            int i = 0;
            foreach (KeyValuePair<string, string> pair in data)
            {
                keys[i] = pair.Key;
                i++;
            }
            return keys;
        }
        public static bool IsFilePath(string path)
        {
            return File.Exists(path);
        }
        public static Dictionary<string, string> ParseRequestPayload(string payload)
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
        

    }
}
