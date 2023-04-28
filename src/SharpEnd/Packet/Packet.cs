using SharpEnd.Cookies;

namespace SharpEnd.Packet
{
    public abstract class Packet
    {
        public PacketProtocol Protocol { get; internal set; }
        public PacketHeaderCollection Headers { get; internal set; }
        public CookieContainer Cookies { get; internal set; }

    }
}
