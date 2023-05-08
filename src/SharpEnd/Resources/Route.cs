using SharpEnd.Packet;

namespace SharpEnd.Resources
{
    public class Route
    {
        public Func<RequestPacket, Task<ResponsePacket>> Controller;
        public RequestMethod RequestMethod { get; set; }
        public string Path { get; set; }
        public Route(RequestMethod method, string path, Func<RequestPacket, Task<ResponsePacket>> controller)
        {
            RequestMethod = method;
            Path = path;
            Controller = controller;
        }
    }
}
