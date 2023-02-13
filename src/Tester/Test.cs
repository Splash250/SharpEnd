using SharpEnd.Model;
using SharpEnd.MySQL;

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
