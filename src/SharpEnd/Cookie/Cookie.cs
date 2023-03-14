using System.Text;

namespace SharpEnd.Cookies
{
    public class Cookie
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Path { get; set; }
        public string Domain { get; set; }
        public DateTime Expires { get; set; }
        public bool Secure { get; set; }
        public bool HttpOnly { get; set; }

        //create an empty constructor
        public Cookie() 
        {
            //give default values
            Name = string.Empty;
            Value = string.Empty;
            Path = string.Empty;
            Domain = string.Empty;
            Expires = GenerateDefaultEpiration();
            Secure = false;
            HttpOnly = false;
        }
        public Cookie(string name, string value)
        {
            Name = name;
            Value = value;
            Path = string.Empty;
            Domain = string.Empty;
            Expires = GenerateDefaultEpiration();
            Secure = false;
            HttpOnly = false;
        }

        public Cookie(string name, string value, string path)
        {
            Name = name;
            Value = value;
            Path = path;
            Domain = string.Empty;
            Expires = GenerateDefaultEpiration();
            Secure = false;
            HttpOnly = false;
        }

        public Cookie(string name, string value, string path, string domain)
        {
            Name = name;
            Value = value;
            Path = path;
            Domain = domain;
            Expires = GenerateDefaultEpiration();
            Secure = false;
            HttpOnly = false;
        }
        public Cookie(string name, string value, string path, string domain, DateTime expires)
        {
            Name = name;
            Value = value;
            Path = path;
            Domain = domain;
            Expires = expires;
            Secure = false;
            HttpOnly = false;
        }

        public static Cookie Parse(string cookieString)
        {
            string[] cookieParts = cookieString.Split(new string[] { "; " }, StringSplitOptions.None);
            string[] nameValue = cookieParts[0].Split(new string[] { "=" }, StringSplitOptions.None);
            string name = nameValue[0];
            string value = nameValue[1];
            Cookie cookie = new(name, value);
            for (int i = 1; i < cookieParts.Length; i++)
            {
                string[] parts = cookieParts[i].Split(new string[] { "=" }, StringSplitOptions.None);
                string key = parts[0];
                string val = parts[1];
                switch (key)
                {
                    case "Path":
                        cookie.Path = val;
                        break;
                    case "Domain":
                        cookie.Domain = val;
                        break;
                    case "Expires":
                        cookie.Expires = DateTime.Parse(val);
                        break;
                    case "Secure":
                        cookie.Secure = true;
                        break;
                    case "HttpOnly":
                        cookie.HttpOnly = true;
                        break;
                }
            }
            return cookie;
        }

        public bool Expired
        {
            get
            {
                return Expires != DateTime.MinValue && Expires < DateTime.UtcNow;
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{Name}={Value}");
            if (!string.IsNullOrEmpty(Path))
                sb.Append($"; Path={Path}");
            if (!string.IsNullOrEmpty(Domain))
                sb.Append($"; Domain={Domain}");
            if (Expires != DateTime.MinValue)
                sb.Append($"; Expires={Expires.ToString("R")}");
            if (Secure)
                sb.Append("; Secure");
            if (HttpOnly)
                sb.Append("; HttpOnly");
            return sb.ToString();
        }
        private static DateTime GenerateDefaultEpiration() 
        {
            DateTime dateTimeIn30Minutes = DateTime.Now.AddMinutes(30);
            return dateTimeIn30Minutes;
        }
    }
}
