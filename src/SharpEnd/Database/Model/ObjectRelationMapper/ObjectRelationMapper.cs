using System.Data;
using System.Reflection;
using System.Reflection.Emit;
using MySql.Data.MySqlClient;
using SharpEnd.MySQL;

namespace SharpEnd.ORM
{
    internal class ObjectRelationMapper
    {

        private MySqlDataBaseConnection connection;
        private readonly string tableName;
        private Type type;

        public ObjectRelationMapper(MySqlDataBaseConnection connection, string tableName)
        {
            this.connection = connection;
            this.tableName = tableName;
        }

        public Type GetType()
        {
            if (type == null)
            {
                type = CreateType();
            }
            return type;
        }

        public MySqlConnection GetDbConnection()
        {
            return connection.GetConnection();
        }

        private Type CreateType()
        {
            TypeBuilder typeBuilder = GetTypeBuilder();
            CreateConstructor(typeBuilder);
            IEnumerable<ColumnInfo> columns = GetColumns();
            foreach (var column in columns)
            {
                CreateProperty(typeBuilder, column.Name, column.Type);
            }
            Type objectType = typeBuilder.CreateType();
            return objectType;
        }

        private TypeBuilder GetTypeBuilder()
        {
            string typeSignature = tableName + "Dynamic";
            AssemblyName assemblyName = new(typeSignature);
            AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName,
                                                                                    AssemblyBuilderAccess.Run);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("MainModule");
            TypeBuilder typeBuilder = moduleBuilder.DefineType(typeSignature,
                                                               TypeAttributes.Public 
                                                                   | TypeAttributes.Class 
                                                                   | TypeAttributes.AutoClass 
                                                                   | TypeAttributes.AnsiClass 
                                                                   | TypeAttributes.BeforeFieldInit 
                                                                   | TypeAttributes.AutoLayout,
                                                               null);
            return typeBuilder;
        }

        private static void CreateConstructor(TypeBuilder typeBuilder)
        {
            typeBuilder.DefineDefaultConstructor(MethodAttributes.Public 
                | MethodAttributes.SpecialName 
                | MethodAttributes.RTSpecialName);
        }

        private static void CreateProperty(TypeBuilder typeBuilder, string propertyName, Type propertyType)
        {
            Console.WriteLine(propertyType.Name);
            FieldBuilder fieldBuilder = typeBuilder.DefineField("_" + propertyName,
                                                                propertyType,
                                                                FieldAttributes.Private);
            PropertyBuilder propertyBuilder = typeBuilder.DefineProperty(propertyName,
                                                                         PropertyAttributes.HasDefault,
                                                                         propertyType,
                                                                         null);
            MethodBuilder getPropMthdBldr = typeBuilder.DefineMethod("get_" + propertyName,
                                                                     MethodAttributes.Public 
                                                                         | MethodAttributes.SpecialName 
                                                                         | MethodAttributes.HideBySig,
                                                                     propertyType,
                                                                     Type.EmptyTypes);
            ILGenerator getIl = getPropMthdBldr.GetILGenerator();

            getIl.Emit(OpCodes.Ldarg_0);
            getIl.Emit(OpCodes.Ldfld, fieldBuilder);
            getIl.Emit(OpCodes.Ret);

            MethodBuilder setPropMthdBldr = typeBuilder.DefineMethod("set_" + propertyName,
                                                                     MethodAttributes.Public 
                                                                         | MethodAttributes.SpecialName 
                                                                         | MethodAttributes.HideBySig,
                                                                     null,
                                                                     new[] { propertyType });
            ILGenerator setIl = setPropMthdBldr.GetILGenerator();
            setIl.Emit(OpCodes.Ldarg_0);
            setIl.Emit(OpCodes.Ldarg_1);
            setIl.Emit(OpCodes.Stfld, fieldBuilder);
            setIl.Emit(OpCodes.Ret);

            propertyBuilder.SetGetMethod(getPropMthdBldr);
            propertyBuilder.SetSetMethod(setPropMthdBldr);
        }
        private IEnumerable<ColumnInfo> GetColumns()
        {
            using MySqlConnection connection = GetDbConnection();
            connection.Open();
            MySqlCommand command = connection.CreateCommand();

            command.CommandText = new MySqlQuery().Select("*")
                                                  .From(tableName)
                                                  .Limit(1)
                                                  .ToString();

            MySqlDataReader reader = command.ExecuteReader();
            List<ColumnInfo> columns = Enumerable.Range(0, reader.FieldCount)
                                    .Select(reader.GetName)
                                    .Select(name => new ColumnInfo
                                    {
                                        Name = name,
                                        Type = reader.GetFieldType(reader.GetOrdinal(name))
                                    })
                                    .ToList();
            return columns;
        }

    }
}
