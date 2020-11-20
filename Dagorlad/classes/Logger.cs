using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Dagorlad
{
    class Logger
    {
        public enum LevelAttention
        {
            Debug = 0,
            Information = 1,
            Warning = 2,
            Error = 3,
            Fatal = 4,
        }
        public static void Do(object obj, LevelAttention lvl)
        {
            string text;
            if (obj.GetType() == typeof(String))
                text = (string)obj;
            else text = ((StringBuilder)obj).ToString();
            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "logs"))
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "logs");
            var splittedtext = text.Split('\n');
            var retext = String.Join("\t\t\t\t\t\t", splittedtext);
            File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "logs/"+"["+lvl.ToString()+"] " + DateTime.Today.ToShortDateString().Replace(".", "-") + ".log",
                "[" + Assembly.GetEntryAssembly().GetName().Name + " " + GetVersionApp() + "]," +
                "[" + DateTime.Now + "]" + " - " + retext + Environment.NewLine);
        }
        public static void UnhandledException(System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            var comException = e.Exception as System.Runtime.InteropServices.COMException;

            if (comException != null && (comException.ErrorCode == -2147221040))
            {
                //Logger.Write(e.Exception.ToString(), false);
                e.Handled = true;
            }
            else
            {
#if (!DEBUG)
                Logger.Do(e.Exception.HResult + " | "+e.Exception.ToString(),LevelAttention.Fatal);
                MessageBox.Show(GetVersionApp() + "\nОшибка: " + e.Exception.HResult + "\nСообщите об ошибке!\nПриложение будет закрыто", Assembly.GetEntryAssembly().GetName().Name+" ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                Process.GetCurrentProcess().Kill();
#endif
            }
        }

        public static string GetVersionApp()
        {
            var version = System.Reflection.Assembly.GetEntryAssembly().GetName().Version;
            DateTime buildDate = new DateTime(2000, 1, 1)
                        .AddDays(version.Build).AddSeconds(version.Revision * 2);
            if (version.Build == 0 && version.Revision == 0 && version.Major == 0 && version.Minor == 0)
                return "v." + version + ", " + buildDate + " (DEBUG)";
            else return "v." + version + ", " + buildDate + " (Release)";
        }
    }
}
