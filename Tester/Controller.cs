using SharpEnd.Server;
using SharpEnd;
using SharpEnd.Packet;
using SharpEnd.Resources;
using SharpEnd.Model;
using SharpEnd.MySQL;

namespace Tester
{
    internal class Controller
    {
        public static ResponsePacket Index(RequestPacket requestPacket)
        {
            View view = View.Create(
                "index",
                "index.html",
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
            View view = View.Create(
                "other",
                "other.html",
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

        public static ResponsePacket DatabasePage(RequestPacket requestPacket)
        {
            DB DB = new();
            Test model = new(DB.Connection);
            
            View view = View.Create(
                "other",
                "db.html",
                new string[] {
                    $"pageData={CreatePageData(model)}"
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

        public static string CreatePageData(Test fromModel) 
        {
            string pageData = "<table><tr>";
            foreach (string header in fromModel.GetColumns())
            {
                pageData += $"<th>{header}</th>";
            }
            pageData += "</tr>";
            foreach (dynamic item in fromModel.All())
            {
                pageData += $"<tr> " +
                    $"<td>{item.testId}</td>" +
                    $"<td>{item.testName}</td>" +
                    $"<td>{item.sucessful}</td>" +
                    $"<td>{item.testReturnNumber}</td>";
                
                pageData += "</tr>";
            }
            pageData += "</table>";
            return pageData;
        }
    }
}
