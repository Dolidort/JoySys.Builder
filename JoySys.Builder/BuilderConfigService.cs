using System.IO;

namespace JoySys.Builder
{
    public class BuilderConfigService
    {
        private readonly string _configPath;

        public BuilderConfigService()
        {
            _configPath = Path.Combine(
                System.AppDomain.CurrentDomain.BaseDirectory,
                "config.ini");
        }

        public BuilderConfig LoadOrCreate()
        {
            if (!File.Exists(_configPath))
            {
                using (StreamWriter sw = File.CreateText(_configPath))
                {
                    sw.WriteLine("[Database]");
                    sw.WriteLine("Server=.\\SQLEXPRESS");
                    sw.WriteLine("Database=VSTUnitDB");
                    sw.WriteLine("");
                    sw.WriteLine("[Paths]");
                    sw.WriteLine("ProjectPath=");
                }
            }

            IniFile ini = new IniFile(_configPath);

            return new BuilderConfig
            {
                ConfigPath = _configPath,
                ProjectPath = ini.Read("Paths", "ProjectPath"),
                SqlServer = ini.Read("Database", "Server"),
                Database = ini.Read("Database", "Database")
            };
        }

        public void Save(BuilderConfig config)
        {
            IniFile ini = new IniFile(_configPath);

            ini.Write("Paths", "ProjectPath", config.ProjectPath ?? "");
            ini.Write("Database", "Server", config.SqlServer ?? "");
            ini.Write("Database", "Database", config.Database ?? "");
        }
    }
}