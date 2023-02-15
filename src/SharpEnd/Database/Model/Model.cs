﻿using MySql.Data.MySqlClient;
using SharpEnd.MySQL;
using SharpEnd.ORM;
using System.Collections;
using System.Dynamic;
using System.Reflection;

namespace SharpEnd.Model
{
    public abstract class Model
    {
        private ObjectRelationMapper _ORMObject;
        private MySqlDataBaseConnection _connection;
        private MySqlQuery _mySqlQuery;
        private MySqlQuery _newMySqlQuery { get { return new MySqlQuery().Select("*").From(_tableName); } }
        private Type _modelType { get { return _ORMObject.GetType(); } }
        private string _tableName;
        private event EventHandler _tableNameSet;
        public dynamic Instance;

        public string TableName
        {
            get { return _tableName; }
            set
            {
                _tableName = value;
                if (!string.IsNullOrEmpty(_tableName))
                    _tableNameSet?.Invoke(this, EventArgs.Empty);
            }
        }

        public Model(MySqlDataBaseConnection Connection)
        {
            TableName = string.Empty;
            _connection = Connection;
            Instance = new ExpandoObject();
            _tableNameSet += (sender, args) =>
            {
                _ORMObject = new ObjectRelationMapper(_connection, _tableName);
                _mySqlQuery = _newMySqlQuery;
            };
        }

        public IList? Query(MySqlQuery Query)
        {
            IList Rows = ModelUtils.EmptyTypeList(_modelType);
            using MySqlConnection connection = _connection.GetConnection();
            connection.Open();
            MySqlCommand command = BuildCommand(Query);
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Rows.Add(ReadRow(reader));
            }
            return Rows;
        }

        private object? ReadRow(MySqlDataReader reader) 
        {
            object? tableRow = Activator.CreateInstance(_modelType);
            foreach (PropertyInfo property in _modelType.GetProperties())
            {
                property.SetValue(tableRow, Convert.ChangeType(reader[property.Name], property.PropertyType));
            }
            return tableRow;
        }

        public void SaveInstance()
        {
            if (Instance == null)
            {
                throw new Exception("Instance is not initialized. Cannot perform save operation.");
            }
            IDictionary<string, object>? expandoDict = Instance as IDictionary<string, object>;
            Dictionary<string, string> values = new Dictionary<string, string>();
            foreach (var key in expandoDict.Keys)
                values.Add(key, MySqlValueConverter.ConvertValue(expandoDict[key]));
            MySqlActions.InsertData(_connection, TableName, values);
        }
        public IList? All() 
        {
            return Query(_newMySqlQuery);
        }
        public List<string> GetColumns()
        {
            return ModelUtils.GetPropertyStrings(_ORMObject);
        }

        private MySqlCommand BuildCommand(MySqlQuery Query) 
        {
            MySqlCommand command = _connection.GetConnection().CreateCommand();
            command.CommandText = Query.ToString();
            return command;
        }
        public Model Where(string WhereClause)
        {
            _mySqlQuery.Where(WhereClause);
            return this;
        }
        public Model GroupBy(string FieldName)
        {
            _mySqlQuery.GroupBy(FieldName);
            return this;

        }
        public Model Having(string HavingClause)
        {
            _mySqlQuery.Having(HavingClause);
            return this;
        }
        public Model OrderBy(string OrderClause)
        {
            _mySqlQuery.OrderBy(OrderClause);
            return this;
        }
        public Model Take(int TakeNumber)
        {
            _mySqlQuery.Limit(TakeNumber);
            return this;
        }
        public Model Limit(int LimitNumber)
        {
            _mySqlQuery.Limit(LimitNumber);
            return this;
        }
        public IList? Get() 
        {
            _mySqlQuery = _newMySqlQuery;
            return Query(_mySqlQuery);
        }
        public override string ToString()
        {
            return TableName;
        }
    }
}
