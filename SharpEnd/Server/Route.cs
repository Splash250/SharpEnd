using SharpEnd.Packet;

namespace SharpEnd.Server
{
    public class Route
    {
        public delegate ResponsePacket ControllerDelegate(RequestPacket request);
        public ControllerDelegate Controller { get; set; }
        public string Path { get; set; }
        public Route(string path, ControllerDelegate controller)
        {
            Path = path;
            Controller = controller;
        }
    }
}
