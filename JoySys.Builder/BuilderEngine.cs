using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JoySys.Builder
{
    public class BuilderEngine
    {
        static string connectionString;
        static readonly string dbfName = @"variable.DBF";
        static readonly string defaultPath = @"C:\ProgramData\";
        static string dbfPath;
        static readonly DbfDataAccess dbf = new DbfDataAccess();
        static List<Dictionary<string, string>> DicDBF = new List<Dictionary<string, string>>();

        public BuildResult Run(Action<string> log = null)
        {
            string curDomainIniPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "config.ini");

            if (!File.Exists(curDomainIniPath))
            {
                using (StreamWriter sw = File.CreateText(curDomainIniPath))
                {
                    sw.WriteLine("Server=.\\SQLEXPRESS");
                    sw.WriteLine("Database=VSTUnitDB");
                }
            }
            IniFile ini = new IniFile(curDomainIniPath);
            var iniProjectPath = ini.Read("Paths", "ProjectPath");

            if (string.IsNullOrWhiteSpace(iniProjectPath) || !Directory.Exists(iniProjectPath))
            {
                using (var dialog = new FolderBrowserDialog())
                {
                    if (Directory.Exists(defaultPath))
                        dialog.SelectedPath = defaultPath;

                    dialog.Description =
                        "Select SCADA Project Folder\n" +
                        "Please note that the folder must contain a file called '" + dbfName + "'";

                    if (dialog.ShowDialog() != DialogResult.OK)
                    {
                        string message = "SCADA project path was not selected. Builder cancelled.";
                        log?.Invoke(message);
                        return BuildResult.Cancelled(message);
                    }

                    iniProjectPath = dialog.SelectedPath;
                    ini.Write("Paths", "ProjectPath", iniProjectPath);

                    log?.Invoke("Selected:");
                    log?.Invoke(iniProjectPath);
                }
            }

            dbfPath = iniProjectPath;

            string dbfFilePath = Path.Combine(dbfPath, dbfName);

            if (!File.Exists(dbfFilePath))
            {
                string message = $"Required file not found: {dbfFilePath}";
                log?.Invoke(message);
                return BuildResult.Failed(message);
            }

            DicDBF = dbf.ReadDbf(dbfFilePath);

            string server = ini.Read("Database", "Server");
            string databaseName = ini.Read("Database", "Database");

            try
            {
                SqlDataAccess.EnsureDatabaseExists(
                    server,
                    databaseName);

                connectionString =
                    SqlDataAccess.BuildConnectionString(
                        server,
                        databaseName);

                var db = new SqlDataAccess(connectionString);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Failed to create or verify database:\n" + ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

            CreateRecipeTableIfNotExists(log);
            CreateAndInsert("tbTags", DicDBF, true, log);
            CreateRuntimeSupportTablesIfNotExists();
            Create_StoredProcedures_UpdateSchema();
            CreateAndLoadRecipeType("TypeRec", "dbo.tbRecipeType", "rType");
            CreateAndLoadRecipeType("ModuleGroupRec", "dbo.tbRecipeModuleGroup", "ModuleGroup");

            return BuildResult.Success();
        }

        private static void CreateRecipeTableIfNotExists(Action<string> log = null)
        {
            SqlDataAccess db = new SqlDataAccess(connectionString);

            if (!db.TableExists("tbRecipe"))
            {
                var columns = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    { "Name", "NVARCHAR(255)" },
                    { "Type", "NVARCHAR(255)" },
                    { "SubType", "NVARCHAR(255)" },
                    { "Description", "NVARCHAR(MAX)" },
                };

                db.CreateTable("tbRecipe", columns, false, false, true);

                log?.Invoke("Table 'tbRecipe' created.");
            }
            else
            {
                log?.Invoke("Table 'tbRecipe' already exists. Skipping creation.");
            }
        }

        private static void CreateRuntimeSupportTablesIfNotExists()
        {
            SqlDataAccess db = new SqlDataAccess(connectionString);
            db.ExecuteNonQuery(Querys.CreateMissingRuntimeTablesIfNotExists());
        }

        private static void Create_StoredProcedures_UpdateSchema()
        {
            SqlDataAccess db = new SqlDataAccess(connectionString);
            db.ExecuteNonQuery(Querys.StoredProcedures_UpdateSchema());
        }


        private static async Task CreateAndInsertAsync(string tableName, List<Dictionary<string, string>> rows, bool recreate = false)
        {
            if (string.IsNullOrWhiteSpace(tableName))
                throw new ArgumentException("Table name is required.", nameof(tableName));

            if (rows == null || rows.Count == 0)
                throw new ArgumentException("No data provided", nameof(rows));

            IniFile ini = new IniFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.ini"));
            string connStr = ini.Read("Database", "ConnectionString");
            SqlDataAccess db = new SqlDataAccess(connStr);

            var firstRow = rows[0];
            var fieldsDefinition = firstRow.ToDictionary(
                kv => kv.Key,
                kv => "NVARCHAR(MAX)",
                StringComparer.OrdinalIgnoreCase
            );

            await db.CreateTableAsync(tableName, fieldsDefinition, recreate, true);

            string columns = string.Join(", ", firstRow.Keys.Select(k => $"[{k}]"));
            string parameters = string.Join(", ", firstRow.Keys.Select(k => $"@{k}"));
            string insertSql = $"INSERT INTO [{tableName}] ({columns}) VALUES ({parameters});";

            foreach (var row in rows)
            {
                var sqlParams = row.Select(kv =>
                    new SqlParameter($"@{kv.Key}", kv.Value ?? (object)DBNull.Value)).ToArray();

                await db.ExecuteNonQueryAsync(insertSql, sqlParams);
            }
        }

        private static void CreateAndInsert(
            string tableName,
            List<Dictionary<string, string>> rows,
            bool recreate = false,
            Action<string> log = null)
        {
            log?.Invoke($" Starting CreateAndInsert for table '{tableName}' (recreate={recreate})");

            if (string.IsNullOrWhiteSpace(tableName))
                throw new ArgumentException("Table name is required.", nameof(tableName));

            if (rows == null || rows.Count == 0)
                throw new ArgumentException("No data provided", nameof(rows));

            log?.Invoke($" Total rows received: {rows.Count}");

            SqlDataAccess db = new SqlDataAccess(connectionString);
            log?.Invoke(" SqlDataAccess instance created.");

            var recipeFields = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var recipeColumns = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            var tagValColumns = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            log?.Invoke(" Creating and populating base table 'tbTagsInfo'...");
            db.CreateTableAndInsertRaw("tbTagsInfo", rows, recreate);
            db.AddLastUpdatedTrigger("tbTagsInfo");
            log?.Invoke(" Table 'tbTagsInfo' created and trigger added.");

            log?.Invoke(" Processing rows to determine recipe and tag columns...");

            int processedCount = 0;
            foreach (var row in rows)
            {
                processedCount++;

                if (row.TryGetValue("NAME", out string name) &&
                    row.TryGetValue("TYPE", out string type) &&
                    !string.IsNullOrWhiteSpace(name))
                {
                    log?.Invoke($"   Row #{processedCount}: NAME={name}, TYPE={type}");

                    if (name.StartsWith("rec_", StringComparison.OrdinalIgnoreCase))
                    {
                        if (!recipeColumns.ContainsKey("RecipeID"))
                        {
                            recipeColumns["RecipeID"] = "INT";
                            recipeColumns["PhaseNum"] = "INT";
                            recipeColumns["IntegratedModule"] = "INT";
                            log?.Invoke("   Initialized base columns for 'tbRecipeVal'.");
                        }

                        recipeFields.Add(name);
                        recipeColumns[name] = db.MapTypeToSql(type);
                        log?.Invoke($"   Added recipe field '{name}' ({type})");
                    }
                    else
                    {
                        tagValColumns[name] = db.MapTypeToSql(type);
                        log?.Invoke($"   Added tag field '{name}' ({type})");
                    }
                }
                else
                {
                    log?.Invoke($"   Row #{processedCount} skipped (missing NAME or TYPE).");
                }
            }

            log?.Invoke(" Row processing completed.");
            log?.Invoke($" Total recipe fields: {recipeFields.Count}");
            log?.Invoke($" Total tag fields: {tagValColumns.Count}");

            if (recipeColumns.Any())
            {
                log?.Invoke(" Creating 'tbRecipeVal' table...");
                db.CreateTable("tbRecipeVal", recipeColumns, false, true, true, true, true);
                db.AddLastUpdatedTrigger("tbRecipeVal");
                log?.Invoke(" Table 'tbRecipeVal' created successfully.");
            }
            else
            {
                log?.Invoke(" No recipe columns found — skipping 'tbRecipeVal' creation.");
            }

            if (tagValColumns.Any())
            {
                log?.Invoke(" Creating 'tbTags' table...");
                db.CreateTable("tbTags", tagValColumns, recreate, true);
                db.AddLastUpdatedTrigger("tbTags");
                log?.Invoke(" Table 'tbTags' created successfully.");
            }
            else
            {
                log?.Invoke(" No tag columns found — skipping 'tbTags' creation.");
            }


            if (recipeFields.Count() > 0)
            {
                string outputPath = Path.Combine(dbfPath, "aux_dispatch.ci");
                string outputPathFun = Path.Combine(dbfPath, "cfg_ValidValues.ci");
                string outputPathVerify = Path.Combine(dbfPath, "cfg_Verify.ci");

                log?.Invoke(" Generating CI files...");
                log?.Invoke($"  -> aux_dispatch.ci: {outputPath}");
                log?.Invoke($"  -> cfg_ValidValues.ci: {outputPathFun}");
                log?.Invoke($"  -> cfg_Verify.ci: {outputPathVerify}");

                CreateCIFile.GenerateRecipeFunction(recipeFields, outputPath);
                CreateCIFile.AppendMissingFunctions_ValidVal(recipeFields, outputPathFun);
                CreateCIFile.AppendMissingFunctions_Verify(recipeFields, outputPathVerify);

                log?.Invoke(" CI file generation completed successfully.");

                log?.Invoke(" CreateAndInsert finished.\n");
            }
            else
            {
                log?.Invoke(" No recipe fields found — skipping CI files creation.");

            }
        }




        private static void CreateAndLoadRecipeType(string sourceName, string targetTableName, string ColTypeName)
        {
            SqlDataAccess db = new SqlDataAccess(connectionString);
            db.ExecuteNonQuery(Querys.BuildRecreateTableQuery(sourceName, targetTableName, ColTypeName));
            db.ExecuteNonQuery(Querys.BuildRecipeTypesQuery(sourceName, targetTableName, ColTypeName));
        }


    }
}