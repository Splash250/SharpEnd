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

        public static ResponsePacket DatabasePage(RequestPacket requestPacket)
        {
            DB DB = new();
            Model model = new(DB.Connection, "testTable");
            MySqlQuery query = new MySqlQuery();
            model.Query(DB.Connection,
                              query.Select("*").From("testTable"));

            View view = View.Parse(
                "other",
                "../../../../html/db.html",
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

        public static string CreatePageData(Model fromModel) 
        {
            List<string>[] RowData = fromModel.GetRowDataMatrix();
            List<string> Headers = fromModel.GetPropertyStrings();
            string pageData = "<table><tr>";
            foreach (string header in Headers)
            {
                pageData += $"<th>{header}</th>";
            }
            pageData += "</tr>";
            foreach (List<string> row in RowData)
            {
                pageData += "<tr>";
                foreach (string value in row)
                {
                    pageData += $"<td>{value}</td>";
                }
                pageData += "</tr>";
            }
            pageData += "</table>";
            return pageData;
        }
    }
}
