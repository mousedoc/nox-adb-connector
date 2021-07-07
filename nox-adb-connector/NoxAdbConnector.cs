using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace nox_adb_connector
{
    public class NoxAdbConnector
    {
        private Process process = null;

        private Process Process
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

        private readonly List<int> PortList = new()
        {
            62001,
            62025,
            62026,
            62027,
        };

        public void Connect()
        {
            MoveTNoxDirectory();
            RunAdbConnect();

            Process.StandardInput.Close();
            Process.WaitForExit();

            ShowResult();
        }

        private void MoveTNoxDirectory()
        {
            Console.WriteLine("Move to Nox App Player installation directory");

            // INFO : Nox should be installed in D-drive
            ExecuteLine(@"d:");
            ExecuteLine(@"cd D:\Program Files\Nox\bin\");

            Console.WriteLine("\n\n");
        }

        private void RunAdbConnect()
        {
            Console.WriteLine("Run adb connect");

            foreach (var port in PortList)
            {
                var line = $@"nox_adb.exe connect 127.0.0.1:{port}";
                ExecuteLine(line);

                Console.WriteLine($"Connect to 127.0.0.1:{port}");
            }

            Console.WriteLine("\n\n");
        }

        private void ShowResult()
        {
            var output = Process.StandardOutput.ReadToEnd();
            var elems = Regex.Split(output, "\r\n");

            var isNextResult = false;
            var nextPort = 0;
            var resultMap = new Dictionary<int, string>();

            foreach (var elem in elems)
            {
                if (elem.Contains("nox_adb.exe"))
                {
                    isNextResult = true;
                    nextPort = int.Parse(elem.Split(':')[2]);
                    continue;
                }
                else if (isNextResult)
                {
                    isNextResult = false;
                    resultMap[nextPort] = elem;
                }
            }

            Console.WriteLine("ADB connect result");

            foreach (var port in resultMap.Keys)
                Console.WriteLine($"Port {port} - {resultMap[port]}");

            Console.WriteLine("\n\n");
        }

        private void ExecuteLine(string line)
        {
            Process.StandardInput.WriteLine(line);
            Process.StandardInput.Flush();
        }
    }
}