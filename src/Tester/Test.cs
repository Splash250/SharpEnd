using SharpEnd.Model;
using SharpEnd.MySQL;

namespace Tester
{
    //Test.cs 

    //this file acts as a model to interact with data inside the 'testTable' table in our database
    internal class Test : BaseModel
    {          
        //here we have to base our class to the BaseModel class so we can use this class like a model
        public Test(MySqlDataBaseConnection connection) : base(connection) 
        {

            //here we have to define which table we want to bind this model to
            TableName = "testtable";
        }
    }
}
