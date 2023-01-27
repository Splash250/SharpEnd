using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEnd.MySQL
{
    public class Seeder
    {
        private MySqlDataBaseConnection Connection;
        public Seeder(MySqlDataBaseConnection Connection ) 
        {
            this.Connection = Connection;
        }

        public void Run(string TableName, Dictionary<string, string> Data)
        {
            MySqlActions.InsertData(Connection, TableName, Data);
        }

    }
}
