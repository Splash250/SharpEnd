using SharpEnd.Packet;
using System.Collections;

namespace SharpEnd.Cookies
{
    public class CookieContainer : IEnumerable<Cookie>
    {
        private readonly Dictionary<string, Cookie> _cookies;
        public static CookieContainer Empty
        {
            get
            {
                return new CookieContainer();
            }
        }
        public int Count
        {
            get
            {
                return _cookies.Count;
            }
        }
        public CookieContainer()
        {
            _cookies = new Dictionary<string, Cookie>();
        }
        public CookieContainer(string cookieHeader, RequestUri uri) : this()
        {
            ParseCookies(cookieHeader, uri);
        }
        public void AddCookie(Cookie cookie)
        {
            if (cookie == null)
                throw new ArgumentNullException(nameof(cookie));

            string key = cookie.Name;
            _cookies[key] = cookie;
        }
        public bool Has(string name)
        {
            return _cookies.ContainsKey(name);
        }
        public Cookie GetCookie(RequestUri uri, string name)
        {
            string key = name;
            if (_cookies.TryGetValue(key, out Cookie cookie))
            {
                //if (ValidCookie(cookie, uri))
                return cookie;
            }
            return new Cookie();
        }
        public IEnumerable<Cookie> GetCookies(RequestUri uri)
        {
            List<Cookie> cookies = new();
            foreach (Cookie cookie in _cookies.Values)
            {
                //if (ValidCookie(cookie, uri))
                cookies.Add(cookie);
            }
            return cookies;
        }

        private static bool ValidCookie(Cookie cookie, RequestUri uri) 
        {
            return !cookie.Expired && cookie.Domain == uri.Host.ToString() && cookie.Path == uri.Path;
        }
        public static CookieContainer Parse(string cookieHeader, RequestUri uri)
        {
            CookieContainer container = new();
            container.ParseCookies(cookieHeader, uri);
            return container;
        }
        public void ParseCookies(string cookieHeader, RequestUri uri)
        {
            if (cookieHeader == null) return;

            if (cookieHeader.Contains("; ")) 
            {
                string[] cookieStrings = cookieHeader.Split(new string[] { "; " }, StringSplitOptions.None);
                foreach (string cookieString in cookieStrings)
                    ParseCookie(cookieString, uri);
            }
            else
                ParseCookie(cookieHeader, uri);
        }
        private void ParseCookie(string cookieString, RequestUri uri) 
        {
            string[] cookieParts = cookieString.Trim().Split('=');
            if (cookieParts.Length == 2)
            {
                string name = cookieParts[0];
                string value = cookieParts[1];
                Cookie cookie = new(name, value)
                {
                    Domain = uri.Host.Domain,
                    Path = uri.Path
                };
                AddCookie(cookie);
            }
        }

        public IEnumerator<Cookie> GetEnumerator()
        {
            foreach (Cookie cookie in _cookies.Values)
            {
                yield return cookie;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
