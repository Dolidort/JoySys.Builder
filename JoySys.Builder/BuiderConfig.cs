using System;
using System.IO;

namespace JoySys.Builder
{
    public class BuilderConfig
    {
        public string ConfigPath { get; set; }
        public string ProjectPath { get; set; }
        public string SqlServer { get; set; }
        public string Database { get; set; }

        public string DbfFileName
        {
            get { return "variable.DBF"; }
        }

        public string DbfFilePath
        {
            get
            {
                if (string.IsNullOrWhiteSpace(ProjectPath))
                    return "";

                return Path.Combine(ProjectPath, DbfFileName);
            }
        }

        public bool ConfigExists
        {
            get { return !string.IsNullOrWhiteSpace(ConfigPath) && File.Exists(ConfigPath); }
        }

        public bool ProjectPathExists
        {
            get { return !string.IsNullOrWhiteSpace(ProjectPath) && Directory.Exists(ProjectPath); }
        }

        public bool DbfExists
        {
            get { return !string.IsNullOrWhiteSpace(DbfFilePath) && File.Exists(DbfFilePath); }
        }
    }
}