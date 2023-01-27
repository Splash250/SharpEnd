using SharpEnd.MySQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tester
{
    internal class DB
    {
        private static readonly MySqlConfig CFG = new("127.0.0.1", "3306", "root","", "test");
        private MySqlDataBaseConnection Connection;

        public DB() 
        {
            Connection = new(CFG);
        }

        public void Migrate() {

            Migration migration = new(Connection);
            Blueprint blueprint = new("testTable");
            blueprint.BigIncrements("testId");
            blueprint.VarChar("testName", 64);
            blueprint.Boolean("sucessful");
            blueprint.Int("testReturnNumber", 2);
            migration.Create(blueprint);
        }

        public void Seed() 
        {
            Seeder seeder = new Seeder(Connection);
            for (int i = 0; i < 10; i++)
            {
                seeder.Run("testTable",
                new()
                {
                    { "testName", $"test_{i}" },
                    { "sucessful", "1" },
                    { "testReturnNumber", new Random().Next(10).ToString() }
                });
            }

        }

    }
}
