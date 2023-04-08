using SharpEnd.Packet;
using SharpEnd.Utils;
namespace SharpEnd.Cookies
{
    public class Session
    {
        private CookieContainer _cookies;
        private CookieContainer _modifiedCookies;
        private RequestUri _uri;
        private string _sessionId;
        
        internal CookieContainer ModifiedCookies
        {
            get { return _modifiedCookies; }
        }

        public CookieContainer Cookies
        {
            get { return _cookies; }
        }

        public Session()
        {
            _cookies = new CookieContainer();
            _modifiedCookies = new CookieContainer();
        }
        public static Session Start(RequestPacket request)
        {
            Session session = new Session();
            session._cookies = request.Cookies;
            session._uri = request.Uri;
            HandleSessionId(session);
            return session;
        }
        private static void HandleSessionId(Session sess)
        {
            if (sess._cookies.Has("SHARPSESSID"))
                sess._sessionId = sess._cookies.GetCookie(sess._uri, "SHARPSESSID").Value;
            else
            {
                sess._sessionId = BasicUtility.GenerateSessionId();
                Cookie sessCookie = new Cookie("SHARPSESSID", sess._sessionId, sess._uri.Path);
                sess._cookies.AddCookie(sessCookie);
                sess._modifiedCookies.AddCookie(sessCookie);
            }
        }
        public bool IsSet(string key)
        {
            return _cookies.Has(key);
        }

        private void HandleModifiedCookies(string key, string value)
        {
            if (_modifiedCookies.Has(key))
                _modifiedCookies.GetCookie(_uri, key).Value = value;
            else
                _modifiedCookies.AddCookie(new Cookie(key, value, _uri.Path));
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
                {
                    HandleModifiedCookies(key, value);
                    _cookies.GetCookie(_uri, key).Value = value;
                }
                else 
                {
                    _modifiedCookies.AddCookie(new Cookie(key, value, _uri.Path));
                    _cookies.AddCookie(new Cookie(key, value, _uri.Path));
                }

            }
        }

    }
}
