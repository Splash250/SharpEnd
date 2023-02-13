using SharpEnd.Packet;

namespace SharpEnd.Resources
{
    public class Route
    {
        public delegate ResponsePacket ControllerDelegate(RequestPacket request);
        public RequestMethod RequestMethod { get; set; }
        public ControllerDelegate Controller { get; set; }
        public string Path { get; set; }
        public Route(RequestMethod Method, string path, ControllerDelegate controller)
        {
            RequestMethod = Method;
            Path = path;
            Controller = controller;
        }
    }
}
