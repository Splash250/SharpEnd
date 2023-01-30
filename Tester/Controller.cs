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
                    $"pageData=<h1>{model.TableName}'s cell names are:</h1> <br>" + String.Join("<br>", model.GetPropertyStrings()),
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
            //List<string> tableHeads = fromModel.GetPropertyStrings();
            //List<string> tableRows = new List<string>();
            //foreach (dynamic row in fromModel.Rows)
            //{
            //    List<string> cells = new List<string>();
            //    foreach (var property in row.GetType().GetProperties())
            //    {
            //        string value = property.GetValue(row);
            //        cells.Add(value);
            //    }
            //    string rowString = $"<tr><td>{String.Join("</td><td>", cells)}</td></tr>";
            //    tableRows.Add(rowString);
            //}
            //string table = $"<table><tr><th>{String.Join("</th><th>", tableHeads)}</th></tr>{String.Join("", tableRows)}</table>";
            //return table;
            throw new NotImplementedException();
        }
    }
}
