using SharpEnd.Model;
using SharpEnd.MySQL;

namespace Tester
{
    internal class Test : Model
    {
        public Test(MySqlDataBaseConnection connection) : base(connection) 
        {
            TableName = "testtable";
        }
    }
}
