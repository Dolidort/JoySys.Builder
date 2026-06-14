using Sylvan.Data.XBase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoySys.Builder
{
    class DbfDataAccess
    {
        public List<Dictionary<string, string>> ReadDbf(string dbfPath)
        {
            var result = new List<Dictionary<string, string>>();

            var stream = File.OpenRead(dbfPath);
            var reader = XBaseDataReader.Create(stream);

            while (reader.Read())
            {
                var row = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string fieldName = reader.GetName(i);
                    object value = reader.GetValue(i);
                    string strValue = value?.ToString() ?? string.Empty;

                    row[fieldName] = strValue;
                }

                result.Add(row);
            }

            return result;
        }


        public Dictionary<string, string> GetDbfSchema(string dbfPath)
        {
            var schema = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

             var stream = File.OpenRead(dbfPath);
             var reader = XBaseDataReader.Create(stream);

            for (int i = 0; i < reader.FieldCount; i++)
            {
                string fieldName = reader.GetName(i);
                Type fieldType = reader.GetFieldType(i);

                string sqlType = MapToSqlType(fieldType);
                schema[fieldName] = sqlType;
            }

            return schema;
        }

        private string MapToSqlType(Type type)
        {
            if (type == typeof(string))
                return "NVARCHAR(MAX)";
            if (type == typeof(int) || type == typeof(short))
                return "INT";
            if (type == typeof(long))
                return "BIGINT";
            if (type == typeof(bool))
                return "BIT";
            if (type == typeof(decimal) || type == typeof(float) || type == typeof(double))
                return "FLOAT";
            if (type == typeof(DateTime))
                return "DATETIME";

            // Default fallback
            return "NVARCHAR(MAX)";
        }






    }
}

