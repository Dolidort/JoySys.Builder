using System;
using Sylvan.Data.XBase;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Windows.Forms;


namespace JoySys.Builder
{
    internal class Program
    {
        static readonly DbfDataAccess dbf = new DbfDataAccess();
        static List<Dictionary<string, string>> DicDBF = new List<Dictionary<string, string>>();


        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

    }
}
