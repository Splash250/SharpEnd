using System.Security.Cryptography;
using System.Text;

namespace SharpEnd.MySQL
{
    internal static class MySqlSecurity
    {
        public static string ComputeHash(string value)
        {
            StringBuilder sb = new StringBuilder();
            using (SHA256 hash = SHA256.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(value));
                foreach (byte byteVal in result)
                    sb.Append(byteVal.ToString("x2"));
            }
            return sb.ToString();
        }
    }
}
