using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace killprocByname
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.Write(args.Length+"\n");
            //for (int i = 0; i < args.Length; i++)
            //    Console.Write(args[i] + "\n");
            if ( args.Length < 1)
            {
                return;
            }

            System.Diagnostics.Process[] ps = System.Diagnostics.Process.GetProcessesByName(args[0].Replace(".exe", ""));

            //Console.Write(ps.Length + "\n");
            foreach (System.Diagnostics.Process p in ps)
            {
                try
                {
                    Console.Write("kill -> " + p.ProcessName + "\n");

                    p.Kill();
                }catch
                {
                    Console.Write("Error:"+p.ProcessName + "\n");
                }
            }
        }
    }
}
