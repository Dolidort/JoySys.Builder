using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoySys.Builder
{
    public class SqlDataAccess
    {
        private readonly string _connectionString;

        public SqlDataAccess(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }


        public DataTable ExecuteQuery(string query, params SqlParameter[] parameters)
        {
            var table = new DataTable();
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(query, conn))
            {
                if (parameters != null && parameters.Length > 0)
                    cmd.Parameters.AddRange(parameters);

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    table.Load(reader);
                }
            }
            return table;
        }

        public int ExecuteNonQuery(string query, params SqlParameter[] parameters)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(query, conn))
            {
                if (parameters != null && parameters.Length > 0)
                    cmd.Parameters.AddRange(parameters);

                conn.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        public object ExecuteScalar(string query, params SqlParameter[] parameters)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(query, conn))
            {
                if (parameters != null && parameters.Length > 0)
                    cmd.Parameters.AddRange(parameters);

                conn.Open();
                return cmd.ExecuteScalar();
            }
        }



        public async Task<DataTable> ExecuteQueryAsync(string query, SqlParameter[] parameters = null)
        {
            var table = new DataTable();
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(query, conn))
            {
                if (parameters != null && parameters.Length > 0)
                    cmd.Parameters.AddRange(parameters);

                await conn.OpenAsync().ConfigureAwait(false);
                using (var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    table.Load(reader);
                }
            }
            return table;
        }

        public async Task<int> ExecuteNonQueryAsync(string query, SqlParameter[] parameters = null)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(query, conn))
            {
                if (parameters != null && parameters.Length > 0)
                    cmd.Parameters.AddRange(parameters);

                await conn.OpenAsync().ConfigureAwait(false);
                return await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
            }
        }

        public async Task<object> ExecuteScalarAsync(string query, SqlParameter[] parameters = null)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(query, conn))
            {
                if (parameters != null && parameters.Length > 0)
                    cmd.Parameters.AddRange(parameters);

                await conn.OpenAsync().ConfigureAwait(false);
                return await cmd.ExecuteScalarAsync().ConfigureAwait(false);
            }
        }

        public Task<DataTable> CreateTableAsync(string tableName, Dictionary<string, string> fields, bool dropIfExists = false, bool addLastUpdated = false, bool addIdentityColumn = true)
        {
            return Task.Run(() => CreateTable(tableName, fields, dropIfExists, addLastUpdated, addIdentityColumn));
        }

        public Task DropTableAsync(string tableName)
        {
            return Task.Run(() => DropTable(tableName));
        }



        public bool TableExists(string tableName)
        {
            string sql = @"SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = @TableName";
            var param = new SqlParameter("@TableName", tableName);
            int count = Convert.ToInt32(ExecuteScalar(sql, param));
            return count > 0;
        }

        public void DropTable(string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName))
                throw new ArgumentException("Table name is required", nameof(tableName));

            string sql = string.Format("IF OBJECT_ID(N'{0}', N'U') IS NOT NULL DROP TABLE [{0}];", tableName);
            ExecuteNonQuery(sql);
        }


        public DataTable CreateTable(string tableName, Dictionary<string, string> fields, bool dropIfExists = false, bool addLastUpdated = false, bool addIdentityColumn = true, bool addCreatedTime = true,bool setAllTypeText254=false)
        {
            if (string.IsNullOrWhiteSpace(tableName))
                throw new ArgumentException("Table name is required", nameof(tableName));

            if (fields == null || fields.Count == 0)
                throw new ArgumentException("Fields must be provided", nameof(fields));

            bool tableExists = TableExists(tableName);

            if (tableExists)
            {
                if (dropIfExists)
                {
                    DropTable(tableName);
                    tableExists = false;
                }
            }

            if (!tableExists)
            {
                var sb = new StringBuilder();
                sb.AppendLine("CREATE TABLE [" + tableName + "] (");

                var definitions = new List<string>();

                if (addIdentityColumn)
                    definitions.Add("[ID] INT IDENTITY(1,1) PRIMARY KEY");

                if (addCreatedTime)
                    definitions.Add("[Created] DATETIME DEFAULT GETDATE()");

                if (addLastUpdated)
                    definitions.Add("[LastUpdated] DATETIME NULL");

                foreach (var kv in fields)
                    definitions.Add("[" + kv.Key + "] " +(setAllTypeText254? "nvarchar(254)" : kv.Value));

                sb.AppendLine(string.Join(",\n", definitions));
                sb.AppendLine(");");

                ExecuteNonQuery(sb.ToString());
            }
            else
            {
          
                var existingColumns = GetTableColumns(tableName)
                    .Select(c => c.ToLower()).ToHashSet();

                var alterCommands = new List<string>();

                if (addCreatedTime && !existingColumns.Contains("created"))
                    alterCommands.Add($"ALTER TABLE [{tableName}] ADD [Created] DATETIME DEFAULT GETDATE();");

                if (addLastUpdated && !existingColumns.Contains("lastupdated"))
                    alterCommands.Add($"ALTER TABLE [{tableName}] ADD [LastUpdated] DATETIME NULL;");

                foreach (var kv in fields)
                {
                    if (!existingColumns.Contains(kv.Key.ToLower()))
                    {
                        alterCommands.Add($"ALTER TABLE [{tableName}] ADD [{kv.Key}] "+(setAllTypeText254 ? "nvarchar(254)" : kv.Value) + " ;");
                    }
                }

                foreach (var sql in alterCommands)
                {
                    ExecuteNonQuery(sql);
                }
            }

            return ExecuteQuery("SELECT * FROM [" + tableName + "] WHERE 1 = 0");
        }

        public Dictionary<string, string> GetTableColumnsDataType(string tableName)
        {
            string sql = $@"
                SELECT COLUMN_NAME, DATA_TYPE
                FROM INFORMATION_SCHEMA.COLUMNS
                WHERE TABLE_NAME = '{tableName}'";

            var table = ExecuteQuery(sql);
            var dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            foreach (DataRow row in table.Rows)
            {
                string columnName = row["COLUMN_NAME"].ToString();
                string dataType = row["DATA_TYPE"].ToString();
                dict[columnName] = dataType;
            }

            return dict;
        }
        public void AddColumnToTable(string tableName, string columnName, string columnType)
        {
            string sql = $"ALTER TABLE [{tableName}] ADD [{columnName}] {columnType}";
            ExecuteNonQuery(sql);
        }



        //public DataTable CreateTable(string tableName, Dictionary<string, string> fields, bool dropIfExists = false, bool addLastUpdated = false, bool addIdentityColumn = true)
        //{
        //    if (string.IsNullOrWhiteSpace(tableName))
        //        throw new ArgumentException("Table name is required", nameof(tableName));

        //    if (fields == null || fields.Count == 0)
        //        throw new ArgumentException("Fields must be provided", nameof(fields));

        //    if (TableExists(tableName))
        //    {
        //        if (!dropIfExists)
        //            return ExecuteQuery("SELECT * FROM [" + tableName + "] WHERE 1 = 0");

        //        DropTable(tableName);
        //    }

        //    var sb = new StringBuilder();
        //    sb.AppendLine("CREATE TABLE [" + tableName + "] (");

        //    var definitions = new List<string>();
        //    if (addIdentityColumn)
        //        definitions.Add("[ID] INT IDENTITY(1,1) PRIMARY KEY");

        //    if (addLastUpdated)
        //        definitions.Add("[Created] DATETIME DEFAULT GETDATE()");

        //    foreach (var kv in fields)
        //        definitions.Add("[" + kv.Key + "] " + kv.Value);



        //    sb.AppendLine(string.Join(",\n", definitions));
        //    sb.AppendLine(");");

        //    ExecuteNonQuery(sb.ToString());
        //    return ExecuteQuery("SELECT * FROM [" + tableName + "] WHERE 1 = 0");
        //}

        public List<string> GetTableColumns(string tableName)
        {
            string sql = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @TableName";
            var param = new SqlParameter("@TableName", tableName);
            var dt = ExecuteQuery(sql, param);
            return dt.AsEnumerable().Select(r => r["COLUMN_NAME"].ToString()).ToList();
        }

        public void AddColumn(string tableName, string columnName, string columnType)
        {
            string sql = string.Format("ALTER TABLE [{0}] ADD [{1}] {2}", tableName, columnName, columnType);
            ExecuteNonQuery(sql);
        }

        public void AddMissingColumns(string tableName, Dictionary<string, string> newColumns)
        {
            if (string.IsNullOrWhiteSpace(tableName) || newColumns == null)
                return;

            var existing = GetTableColumns(tableName);

            foreach (var kv in newColumns)
            {
                if (!existing.Contains(kv.Key, StringComparer.OrdinalIgnoreCase))
                    AddColumn(tableName, kv.Key, kv.Value);
            }
        }

        public void AddLastUpdatedTrigger(string tableName, string columnName = "LastUpdated")
        {
            string sqlAddColumn =
        "IF COL_LENGTH('" + tableName + "', '" + columnName + @"') IS NULL "+
        "  BEGIN"+
        "     ALTER TABLE [" + tableName + "] ADD [" + columnName + "] DATETIME NULL; "+
        "  END";
 
    ExecuteNonQuery(sqlAddColumn);

            string triggerName = "TRG_" + tableName + "_LastUpdated";

            string triggerBody =
                " CREATE TRIGGER [dbo].[" + triggerName + "] ON [dbo].[" + tableName + "] " +
                " AFTER INSERT, UPDATE AS BEGIN SET NOCOUNT ON; " +
                " UPDATE [" + tableName + "] SET [" + columnName + "] = GETDATE() " +
                " WHERE [ID] IN (SELECT [ID] FROM INSERTED); END";

   
            string escapedTriggerBody = triggerBody.Replace("'", "''");

            string sqlTrigger =
                " IF OBJECT_ID(N'[dbo].[" + triggerName + "]', N'TR') IS NULL BEGIN " +
                " EXEC(N'" + escapedTriggerBody + "') END";

            ExecuteNonQuery(sqlTrigger);
        }


        public void CreateTableAndInsertRaw(string tableName, List<Dictionary<string, string>> rows, bool recreate)
        {
            if (rows == null || rows.Count == 0)
                throw new ArgumentException("At least one row is required", nameof(rows));

            var firstRow = rows[0];
            var fieldDefs = firstRow.ToDictionary(k => k.Key, v => "NVARCHAR(MAX)", StringComparer.OrdinalIgnoreCase);

            CreateTable(tableName, fieldDefs, recreate, true);

            string columns = string.Join(", ", firstRow.Keys.Select(k => "[" + k + "]"));
            string paramList = string.Join(", ", firstRow.Keys.Select(k => "@" + k));
            string sql = " INSERT INTO [" + tableName + "] (" + columns + ") VALUES (" + paramList + ")";

            foreach (var row in rows)
            {
                var parameters = row.Select(kv =>new SqlParameter("@" + kv.Key, kv.Value == null ? DBNull.Value : (object)kv.Value)).ToArray();
                ExecuteNonQuery(sql, parameters);
            }
        }

        public string MapTypeToSql(string inputType)
        {
            if (string.IsNullOrWhiteSpace(inputType))
                return "NVARCHAR(MAX)";

            switch (inputType.Trim().ToUpperInvariant())
            {
                case "STRING":
                case "TEXT":
                case "NVARCHAR":
                    return "NVARCHAR(254)";
                case "INT":
                case "INTEGER":
                    return "INT";
                case "FLOAT":
                case "DOUBLE":
                    return "FLOAT";
                case "DECIMAL":
                    return "DECIMAL(18,2)";
                case "BOOL":
                case "BOOLEAN":
                    return "BIT";
                case "BYTE":
                    return "TINYINT";
                case "DATETIME":
                    return "DATETIME";
                default:
                    return "NVARCHAR(MAX)";
            }
        }

      
    }
}
