using SharpEnd.Model;
using SharpEnd.MySQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tester
{
    internal class Test : BaseModel
    {
        public Test(MySqlDataBaseConnection connection) : base(connection) 
        {
            TableName = "testtable";
        }
    }
}
