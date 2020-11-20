using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Updater
{
    class Program
    {
        private static string current_file = "Dagorlad.exe";
        private static string guid_detect_need_update = "5F5E8B7D1FB486DEB735E23782E6B";
        private static string guid_start_done_updater = "645F2D72E774179B3ABF7BDC5FD6E";
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        static void HideConsole()
        {
            try
            {
                ShowWindow(GetConsoleWindow(), 0);
            }
            catch { }
        }
        static void Main(string[] args)
        {
            HideConsole();
            if (args.FirstOrDefault()== guid_detect_need_update)
            {
                try
                {
                    foreach (var s in Process.GetProcesses())
                        if (s.ProcessName == "Dagorlad")
                            Process.GetProcessById(s.Id).Kill();
                    File.Delete(System.AppDomain.CurrentDomain.BaseDirectory + current_file);
                    File.Move(System.AppDomain.CurrentDomain.BaseDirectory + "_"+ current_file, System.AppDomain.CurrentDomain.BaseDirectory + current_file);
                    if(File.Exists(System.AppDomain.CurrentDomain.BaseDirectory + "_"+ current_file))
                    File.Delete(System.AppDomain.CurrentDomain.BaseDirectory + "_"+ current_file);
                    ExecuteAsAdmin(System.AppDomain.CurrentDomain.BaseDirectory + current_file, guid_start_done_updater);
                   // Process.Start(System.AppDomain.CurrentDomain.BaseDirectory + current_file, guid_start_done_updater);
                }
                catch { }
            }
        }
        public static void ExecuteAsAdmin(string fileName, string arg)
        {
            Process proc = new Process();
            proc.StartInfo.FileName = fileName;
            proc.StartInfo.WorkingDirectory = Environment.CurrentDirectory;
            proc.StartInfo.Arguments = arg;
            proc.StartInfo.UseShellExecute = true;
            proc.StartInfo.Verb = "runas";
            proc.Start();
        }
    }
}
