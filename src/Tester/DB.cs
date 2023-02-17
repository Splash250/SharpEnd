using SharpEnd.Database;
using SharpEnd.MySQL;

namespace Tester
{
    internal class DB : Database
    {
        public DB()
        {
            Address = "127.0.0.1";
            Port = 3306;
            UserName= "root";
            Password = "";
            DatabaseName = "sharpend";
        }
        public void Migrate() {
            Blueprint blueprint = new("testTable");
            blueprint.BigIncrements("testId");
            blueprint.VarChar("testName", 64);
            blueprint.Boolean("sucessful");
            blueprint.Int("testReturnNumber", 2);
            CreateSchema(blueprint);
        }
        public void Seed() 
        {
            for (int i = 0; i < 10; i++)
            {
                InsertRow("testTable",
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
