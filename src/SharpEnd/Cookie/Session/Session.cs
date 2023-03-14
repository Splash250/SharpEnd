using SharpEnd.Cookies;
using SharpEnd.Packet;

namespace SharpEnd.Cookies
{
    public class Session
    {
        private CookieContainer _cookies;
        private RequestUri _uri;

        public CookieContainer Cookies
        {
            get { return _cookies; }
        }

        public Session()
        {
            _cookies = new CookieContainer();
        }
        public static Session Start(RequestPacket request)
        {
            Session session = new Session();
            session._cookies = request.Cookies;
            session._uri = request.Uri;
            return session;
        }
        public bool IsSet(string key)
        {
            return _cookies.Has(key);
        }


        public string this[string key]
        {
            get
            {
                return _cookies.GetCookie(_uri, key).Value;
            }
            set
            {
                if (_cookies.Has(key))

                    _cookies.GetCookie(_uri, key).Value = value;
                else
                    _cookies.AddCookie(new Cookie(key, value, _uri.Path));
            }
        }

    }
}
