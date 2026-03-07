using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.CompilerServices;

namespace TestTaskLKDS
{
    internal class Logger
    {
        private static string _filePath = "log.txt";
        public enum LogStatus
        {
            Info,
            Error
        }
        public static void Log(string message, LogStatus status)
        {
            string log = $"\n{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{status}] {message}";
            try
            {
                File.AppendAllText(_filePath, log);
            }
            catch (Exception ex) 
            { 
                Console.WriteLine(ex.ToString());
            }
        }
        public static void Info(string message) => Log(message, LogStatus.Info);
        public static void Error(string message) => Log(message, LogStatus.Error);
    }
}
