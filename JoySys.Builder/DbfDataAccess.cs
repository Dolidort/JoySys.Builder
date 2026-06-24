using Sylvan.Data.XBase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JoySys.Builder
{
    class DbfDataAccess
    {
        public List<Dictionary<string, string>> ReadDbf(string dbfPath)
        {
            var result = new List<Dictionary<string, string>>();

            try
            {
                using (var stream = new FileStream(
                    dbfPath,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.ReadWrite))
                {
                    XBaseDataReader reader = null;

                    try
                    {
                        reader = XBaseDataReader.Create(stream);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error opening DBF file: " + ex.Message);
                    }

                    using (reader)
                    {
                        while (reader.Read())
                        {
                            var row = new Dictionary<string, string>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                string columnName = reader.GetName(i);
                                object value = reader.GetValue(i);

                                row[columnName] = value == null || value == DBNull.Value
                                    ? ""
                                    : value.ToString();
                            }

                            result.Add(row);
                        }
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.ToString(),
                    "ReadDbf failed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                throw;
            }
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

