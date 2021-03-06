﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace nox_adb_connector
{
    /// <summary>
    /// Adb connector for Nox App Player
    /// </summary>
    public class NoxAdbConnector
    {
        private Process process = null;

        /// <summary>
        /// Process to run the command
        /// </summary>
        private Process Process
        {
            get
            {
                if (process == null)
                {
                    process = new Process();

                    var info = process.StartInfo;
                    info.FileName = "cmd.exe";
                    info.RedirectStandardInput = true;
                    info.RedirectStandardOutput = true;
                    info.CreateNoWindow = false;
                    info.UseShellExecute = false;
                    info.StandardOutputEncoding = Encoding.UTF8;

                    process.Start();
                }

                return process;
            }
        }

        /// <summary>
        /// Predefined ports
        /// </summary>
        private readonly List<int> PortList = new()
        {
            62001,
            62025,
            62026,
            62027,
        };

        /// <summary>
        /// Execute nox_adb connect and print result
        /// </summary>
        public void Connect()
        {
            MoveTNoxDirectory();
            RunAdbConnect();
            WaitForProcess();
            ShowResult();
        }

        /// <summary>
        /// Move to nox installation directory
        /// </summary>
        private void MoveTNoxDirectory()
        {
            Console.WriteLine("### Move to Nox App Player installation directory");

            // INFO : Nox should be installed in D-drive
            ExecuteLine(@"d:");
            ExecuteLine(@"cd D:\Program Files\Nox\bin\");

            Console.WriteLine("\n\n");
        }

        /// <summary>
        /// Execute nox_adb connect command
        /// </summary>
        private void RunAdbConnect()
        {
            Console.WriteLine("### Run adb connect");

            foreach (var port in PortList)
            {
                var line = $@"nox_adb.exe connect 127.0.0.1:{port}";
                ExecuteLine(line);

                Console.WriteLine($"Connect to 127.0.0.1:{port}");
            }

            Console.WriteLine("\n\n");
        }

        /// <summary>
        /// Wait for process
        /// </summary>
        private void WaitForProcess()
        {
            Console.WriteLine("### Please wait...");

            Process.StandardInput.Close();
            Process.WaitForExit();

            Console.WriteLine("\n\n");
        }

        /// <summary>
        /// Process output and print result
        /// </summary>
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

            Console.WriteLine("### ADB connect result");

            foreach (var port in resultMap.Keys)
                Console.WriteLine($"Port {port} - {resultMap[port]}");

            Console.WriteLine("\n\n");
        }

        /// <summary>
        /// Internal command line executor
        /// </summary>
        /// <param name="line">command line</param>
        private void ExecuteLine(string line)
        {
            Process.StandardInput.WriteLine(line);
            Process.StandardInput.Flush();
        }
    }
}