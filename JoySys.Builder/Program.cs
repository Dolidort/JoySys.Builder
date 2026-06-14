using System;
using Sylvan.Data.XBase;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.IO;

//Update-Package -reinstall

namespace JoySys.Builder
{
    internal class Program
    {
        static string exePath;
        static string iniPath;
        static string connectionString;
        static string dbfName = @"variable.DBF";
        static string dbfPath;// = "C:\\ProgramData\\AVEVA\\Citect SCADA 2018 R2\\User\\VST_HMI_Core_\\";
        static DbfDataAccess dbf = new DbfDataAccess();
        static List<Dictionary<string, string>> DicDBF = new List<Dictionary<string, string>>();

        static void Main()
        {
          //  exePath = AppDomain.CurrentDomain.BaseDirectory;
            //iniPath = Path.Combine(exePath, "config.ini");
            //IniFile ini = new IniFile(iniPath);


            string iniPath = Path.Combine(@"C:\Program Files\JoySys", "config.ini");
            IniFile ini = new IniFile(iniPath);

            //dbfPath = exePath;
            connectionString = ini.Read("Database", "ConnectionString");
            string tmpdbfPath = ini.Read("Paths", "ProjectPath");
            if (!string.IsNullOrWhiteSpace(tmpdbfPath))
            {
                dbfPath = tmpdbfPath;
            }

            DicDBF = dbf.ReadDbf(dbfPath + dbfName);

            Console.WriteLine($"Reading from: {dbfPath + dbfName}");
            Console.WriteLine($"Records read: {DicDBF.Count}");
            Signpost.VSTAscii();


            CreateRecipeTableIfNotExists();
            CreateAndInsert("tbTags", DicDBF, true);
            Create_StoredProcedures_UpdateSchema();
            //Create_StoredProcedures_UpdateIntegratedModuleRecipeFields();
            CreateAndLoadRecipeType("TypeRec", "dbo.tbRecipeType","rType");
            CreateAndLoadRecipeType("ModuleGroupRec", "dbo.tbRecipeModuleGroup", "ModuleGroup");

            Signpost.SpinnerHelper.StartSpinner("Please wait...");
            Thread.Sleep(2000);


            Signpost.SpinnerHelper.StopSpinner();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Done!");
        }

        public static async Task CreateAndInsertAsync(string tableName, List<Dictionary<string, string>> rows, bool recreate = false)
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

        public static void CreateAndInsert(string tableName, List<Dictionary<string, string>> rows, bool recreate = false)
        {
            Console.WriteLine($"[DEBUG] Starting CreateAndInsert for table '{tableName}' (recreate={recreate})");

            if (string.IsNullOrWhiteSpace(tableName))
                throw new ArgumentException("Table name is required.", nameof(tableName));

            if (rows == null || rows.Count == 0)
                throw new ArgumentException("No data provided", nameof(rows));

            Console.WriteLine($"[DEBUG] Total rows received: {rows.Count}");

            SqlDataAccess db = new SqlDataAccess(connectionString);
            Console.WriteLine("[DEBUG] SqlDataAccess instance created.");

            var recipeFields = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var recipeColumns = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            var tagValColumns = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            Console.WriteLine("[DEBUG] Creating and populating base table 'tbTagsInfo'...");
            db.CreateTableAndInsertRaw("tbTagsInfo", rows, recreate);
            db.AddLastUpdatedTrigger("tbTagsInfo");
            Console.WriteLine("[DEBUG] Table 'tbTagsInfo' created and trigger added.");

            Console.WriteLine("[DEBUG] Processing rows to determine recipe and tag columns...");

            int processedCount = 0;
            foreach (var row in rows)
            {
                processedCount++;

                if (row.TryGetValue("NAME", out string name) &&
                    row.TryGetValue("TYPE", out string type) &&
                    !string.IsNullOrWhiteSpace(name))
                {
                    Console.WriteLine($"  [DEBUG] Row #{processedCount}: NAME={name}, TYPE={type}");

                    if (name.StartsWith("rec_", StringComparison.OrdinalIgnoreCase))
                    {
                        if (!recipeColumns.ContainsKey("RecipeID"))
                        {
                            recipeColumns["RecipeID"] = "INT";
                            recipeColumns["PhaseNum"] = "INT";
                            recipeColumns["IntegratedModule"] = "INT";
                            Console.WriteLine("  [DEBUG] Initialized base columns for 'tbRecipeVal'.");
                        }

                        recipeFields.Add(name);
                        recipeColumns[name] = db.MapTypeToSql(type);
                        Console.WriteLine($"  [DEBUG] Added recipe field '{name}' ({type})");
                    }
                    else
                    {
                        tagValColumns[name] = db.MapTypeToSql(type);
                        Console.WriteLine($"  [DEBUG] Added tag field '{name}' ({type})");
                    }
                }
                else
                {
                    Console.WriteLine($"  [DEBUG] Row #{processedCount} skipped (missing NAME or TYPE).");
                }
            }

            Console.WriteLine("[DEBUG] Row processing completed.");
            Console.WriteLine($"[DEBUG] Total recipe fields: {recipeFields.Count}");
            Console.WriteLine($"[DEBUG] Total tag fields: {tagValColumns.Count}");

            if (recipeColumns.Any())
            {
                Console.WriteLine("[DEBUG] Creating 'tbRecipeVal' table...");
                db.CreateTable("tbRecipeVal", recipeColumns, false, true, true, true, true);
                db.AddLastUpdatedTrigger("tbRecipeVal");
                Console.WriteLine("[DEBUG] Table 'tbRecipeVal' created successfully.");
            }
            else
            {
                Console.WriteLine("[DEBUG] No recipe columns found — skipping 'tbRecipeVal' creation.");
            }

            if (tagValColumns.Any())
            {
                Console.WriteLine("[DEBUG] Creating 'tbTags' table...");
                db.CreateTable("tbTags", tagValColumns, recreate, true);
                db.AddLastUpdatedTrigger("tbTags");
                Console.WriteLine("[DEBUG] Table 'tbTags' created successfully.");
            }
            else
            {
                Console.WriteLine("[DEBUG] No tag columns found — skipping 'tbTags' creation.");
            }

            string outputPath = Path.Combine(dbfPath, "aux_dispatch.ci");
            string outputPathFun = Path.Combine(dbfPath, "cfg_ValidValues.ci");
            string outputPathVerify = Path.Combine(dbfPath, "cfg_Verify.ci");

            Console.WriteLine("[DEBUG] Generating CI files...");
            Console.WriteLine($"  -> aux_dispatch.ci: {outputPath}");
            Console.WriteLine($"  -> cfg_ValidValues.ci: {outputPathFun}");
            Console.WriteLine($"  -> cfg_Verify.ci: {outputPathVerify}");

            if (recipeFields.Count() > 0)
            {
                CreateCIFile.GenerateRecipeFunction(recipeFields, outputPath);
                CreateCIFile.AppendMissingFunctions_ValidVal(recipeFields, outputPathFun);
                CreateCIFile.AppendMissingFunctions_Verify(recipeFields, outputPathVerify);

                Console.WriteLine("[DEBUG] CI file generation completed successfully.");

                Console.WriteLine("[DEBUG] CreateAndInsert finished.\n");
            }



            var key = Console.ReadKey(intercept: true);
            if (key.Key == ConsoleKey.Escape)
                Console.WriteLine("\n[DEBUG] Execution terminated by user.");
            else
                Console.WriteLine("\n[DEBUG] Exiting CreateAndInsert normally.");
        }


        public static void CreateRecipeTableIfNotExists()
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

                db.CreateTable("tbRecipe", columns, false,false,true);

              //  Console.WriteLine("Table 'tbRecipe' created.");
            }
            else
            {
               // Console.WriteLine("Table 'tbRecipe' already exists. Skipping creation.");
            }
        }


        public static void Create_StoredProcedures_UpdateSchema()
        {
            SqlDataAccess db = new SqlDataAccess(connectionString);
            db.ExecuteNonQuery(Querys.StoredProcedures_UpdateSchema());
        }


        //public static void Create_StoredProcedures_UpdateIntegratedModuleRecipeFields()
        //{
        //    SqlDataAccess db = new SqlDataAccess(connectionString);
        //    db.ExecuteNonQuery(Querys.StoredProcedures_UpdateIntegratedModuleRecipeFields());
        //}


        public static void CreateAndLoadRecipeType(string sourceName, string targetTableName,string ColTypeName)
        {
            SqlDataAccess db = new SqlDataAccess(connectionString);
            db.ExecuteNonQuery(Querys.BuildRecreateTableQuery(sourceName, targetTableName, ColTypeName));
            db.ExecuteNonQuery(Querys.BuildRecipeTypesQuery(sourceName,targetTableName, ColTypeName));
        }






    }
}
