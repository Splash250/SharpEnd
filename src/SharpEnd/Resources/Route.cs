using SharpEnd.Packet;

namespace SharpEnd.Resources
{
    public class Route
    {
        public delegate ResponsePacket ControllerDelegate(RequestPacket request);
        public RequestMethod RequestMethod { get; set; }
        public ControllerDelegate Controller { get; set; }
        public string Path { get; set; }
        public Route(RequestMethod method, string path, ControllerDelegate controller)
        {
            RequestMethod = method;
            Path = path;
            Controller = controller;
        }
    }
}
