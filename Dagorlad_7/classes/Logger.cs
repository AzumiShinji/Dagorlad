using Dagorlad_7.Windows;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Dagorlad_7.classes
{
    class Logger
    {
        public enum TypeLogs
        {
            main = 0,
            chat = 1,
            updater = 2,
            clipboard=3,
            transferfiles = 4,
        }
        private static string root = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" + Assembly.GetExecutingAssembly().GetName().Name;
        private static string extenstion = ".log";
        public static void Write(TypeLogs type, string text)
        {
            if (!Directory.Exists(root)) Directory.CreateDirectory(root);
            string path = root + @"\";
            string file = path + type.ToString() + extenstion;
            CheckLimitSizeLog(file);
            string log_text = String.Format("[{0}]: {1}\n", DateTime.Now, text);
            File.AppendAllText(file, log_text);
        }
        private static void CheckLimitSizeLog(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    var filesize = new FileInfo(path);
                    var size = (double)filesize.Length / 1000000;
                    if (size > 1)
                        File.Delete(path);
                }
            }
            catch { }
        }
        public static void Open()
        {
            if (!Directory.Exists(root)) return;
            string path = root + @"\";
            System.Diagnostics.Process.Start(path);
        }
    }
}
