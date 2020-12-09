using Dagorlad_7.Windows;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UsBudget.classes
{
    class Logger
    {
        public enum TypeLogs
        {
            main = 0,
            chat = 1,
            updater = 2,
            clipboard=3,
        }
        private static string root = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" + Assembly.GetExecutingAssembly().GetName().Name;
        private static string extenstion = ".log";
        public static void Write(TypeLogs type, string text)
        {
            if (!Directory.Exists(root)) Directory.CreateDirectory(root);
            string path = root + @"\";
            string log_text = String.Format("[{0}]: {1}\n", DateTime.Now, text);
            File.AppendAllText(path + type.ToString() + extenstion, log_text);
        }
        public static void Open()
        {
            if (!Directory.Exists(root)) return;
            string path = root + @"\";
            System.Diagnostics.Process.Start(path);
        }
    }
}
