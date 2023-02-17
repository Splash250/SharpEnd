using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEnd.Resources
{
    public static class ViewUtils
    {
        public static string TableFromModelQuery(List<string> tableHeaders,IList? queryResults) 
        {
            string table = 
                $"<table>" +
                $"{ParseTableHeaders(tableHeaders)}" +
                $"{ParseTableBody(queryResults)}" +
                $"</table>";
            return table;
        }

        private static string ParseTableHeaders(List<string> tableHeaders)
        {
            string headers = "<tr>";
            foreach (string header in tableHeaders)
            {
                headers += $"<th>{header}</th>";
            }
            headers += "</tr>";
            return headers;
        }
        private static string ParseTableBody(IList? queryResults)
        {
            string body = "";
            foreach (dynamic item in queryResults)
            {
                body += $"<tr> " +
                    $"<td>{item.testId}</td>" +
                    $"<td>{item.testName}</td>" +
                    $"<td>{item.sucessful}</td>" +
                    $"<td>{item.testReturnNumber}</td>";

                body += "</tr>";
            }
            return body;
        }

    }
}
