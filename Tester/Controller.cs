using SharpEnd.Server;
using SharpEnd;
using SharpEnd.Packet;

namespace Tester
{
    internal class Controller
    {
        public static ResponsePacket Index(RequestPacket requestPacket)
        {
            View view = View.Parse(
                "index",
                "../../../../html/index.html",
                new string[] {
                    "pathLocation=" + requestPacket.Path,
                    "randomNum=" + new Random().Next(1000)
                });

            return new ResponsePacket(
                PacketProtocol.Default,
                ResponseCode.OK,
                new PacketHeaders(new string[] {
                    "Content-Type: text/html; charset=UTF-8",
                    "Content-Length: " + view.Content.Length
                }),
                view);
        }

        public static ResponsePacket OtherPage(RequestPacket requestPacket)
        {
            View view = View.Parse(
                "other",
                "../../../../html/other.html",
                new string[] {
                    "currentDate=" + DateTime.UtcNow.Date.ToString("dd/MM/yyyy"),
                });

            return new ResponsePacket(
                PacketProtocol.Default,
                ResponseCode.OK,
                new PacketHeaders(new string[] {
                    "Content-Type: text/html; charset=UTF-8",
                    "Content-Length: " + view.Content.Length
                }),
                view);
        }
    }
}
