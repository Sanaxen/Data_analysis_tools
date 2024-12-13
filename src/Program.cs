using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{

    static class Program
    {
        //static string Rversion = "R-3.6.1";
        //static string Rversion = "R-3.4.4";
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //if (!SetupPath(Rversion))
            //{
            //    return;
            //}
            Application.Run(new Form1(args));
        }

        private static bool SetupPath(string Rversion)
        {
            if (File.Exists("R_installed.txt"))
            {
                File.Delete("R_installed.txt");
            }
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = System.AppDomain.CurrentDomain.BaseDirectory + "ConsoleR.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            p.WaitForExit();
            if (File.Exists("R_installed.txt"))
            {
                File.Delete("R_installed.txt");
                return true;
            }
            return false;
        }
    }
}
