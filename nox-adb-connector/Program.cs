using System;

namespace nox_adb_connector
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var connector = new NoxAdbConnector();
            connector.Connect();

            Console.WriteLine("Press a key");
            Console.ReadKey();
        }
    }
}