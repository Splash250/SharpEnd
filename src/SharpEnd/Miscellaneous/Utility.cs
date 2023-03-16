namespace SharpEnd.Miscellaneous
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
        public static string GenerateSessionId(int length = 32)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static bool IsFilePath(string path)
        {
            return File.Exists(path);
        }
    }
}
