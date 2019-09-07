using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace nox_app_player_debug_connector
{
    class Program
    {
        private static Process process = null;  

        private static Process Process
        {
            get
            {
                if (process == null)
                {
                    process = new Process();
                    process.StartInfo.FileName = "cmd.exe";
                    process.StartInfo.RedirectStandardInput = true;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.CreateNoWindow = false;
                    process.StartInfo.UseShellExecute = false;
                    process.Start();
                }

                return process;
            }
        }

        static void Main(string[] args)
        {
            // INFO : Nox app player installed in drive D
            var lines = new List<string>()
            {
                @"cd C:Windows",
                @"d:",
                @"cd D:\Program Files\Nox\bin\",
                @"nox_adb.exe connect 127.0.0.1:62001",
            };

            // Run and wait for exit
            ProcessLines(lines);
            Console.WriteLine("\r\nPress a key");
            Console.ReadKey();
        }

        private static void ProcessLines(List<string> lines)
        {
            foreach (var line in lines)
            {
                Process.StandardInput.WriteLine(line);
                Process.StandardInput.Flush();
            }

            Process.StandardInput.Close();
            Process.WaitForExit();

            var outputs = Process.StandardOutput.ReadToEnd().Split('\n');
            Console.WriteLine(outputs[outputs.Length - 3]);
        }
    }
}
