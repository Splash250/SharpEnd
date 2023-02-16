using SharpEnd.MySQL;

//DB.cs 
namespace Tester
{
    //this is an example class that is for the database management
    internal class DB
    {
        //we have to have a config that the mysql database uses like this:
        private static readonly MySqlConfig CFG = new("127.0.0.1", "3306", "root","", "sharpend");

        //here we define the mysql database connection
        public MySqlDataBaseConnection Connection { get; private set; }

        //we initialize the new database like this:
        public DB() 
        {
            Connection = new(CFG);
        }

        //here is an example where we can migrate the table called 'testTable' 
        public void Migrate() {
            //we inicialize the migration to use our mysql database
            Migration migration = new(Connection);
            //blueprints are used to construct the backbone of a table 
            //we define the table name and column types using blueprints
            //here is the example:
            Blueprint blueprint = new("testTable");
            blueprint.BigIncrements("testId");
            blueprint.VarChar("testName", 64);
            blueprint.Boolean("sucessful");
            blueprint.Int("testReturnNumber", 2);

            //we apply out blueprint to the migration like this:
            migration.Create(blueprint);
        }
        //here we apply filler data to our table that we migrated previously
        //we use the seeder for this to happen like this:
        public void Seed() 
        {
            //we define a seeder object that also needs the mysql connection like the migration
            Seeder seeder = new Seeder(Connection);

            //lets suppose that i want 10 rows of filler data to be inserted into our table we can do it like this:
            for (int i = 0; i < 10; i++)
            {
                //this runs 10 times
                seeder.Run("testTable",
                new()
                {
                    //here we define which column contains what data
                    { "testName", $"test_{i}" },
                    { "sucessful", "1" },
                    { "testReturnNumber", new Random().Next(10).ToString() }
                });
            }

        }

    }
}
