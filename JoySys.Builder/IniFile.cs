using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace JoySys.Builder
{
    public class IniFile
    {
        public string Path;

        public IniFile(string iniPath)
        {
            Path = iniPath;
        }
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        private static extern int GetPrivateProfileString(string section, string key, string defaultValue, StringBuilder retVal, int size, string filePath);

        public string Read(string section, string key, string defaultValue = "")
        
        {
            var retVal = new StringBuilder(1024);
            GetPrivateProfileString(section, key, defaultValue, retVal, retVal.Capacity, Path);
            return retVal.ToString();
        }

        public long Write(string section, string key, string value)
        {
            return WritePrivateProfileString(section, key, value, Path);
        }
    }
}
