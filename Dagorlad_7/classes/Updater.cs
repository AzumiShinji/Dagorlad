using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using UsBudget.classes;

namespace Dagorlad_7.classes
{
    class Updater
    {
#if (DEBUG)
        private const string PathToFile = "//krislechy/Downloads/SharedFolder/";
#else
        private const string PathToFile = "//WebService/Dagorlad/";
#endif
        private static string PathToRoaming = "";

        private static async Task Remote_Update_Local()
        {
            try
            {
                PathToRoaming = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" + Assembly.GetExecutingAssembly().GetName().Name+"\\";
                var target = Assembly.GetEntryAssembly().GetName().Name+".exe";
                var RemoteVersionFile = new Version(FileVersionInfo.GetVersionInfo(PathToFile+ target).FileVersion);
                if (RemoteVersionFile > Assembly.GetExecutingAssembly().GetName().Version)
                {
                    var path_bakfile = PathToRoaming + Path.GetFileName(target) + ".bak.remote";
                    var path_downloadedfile = PathToRoaming + target + "_";
                    if (File.Exists(path_bakfile))
                        File.Delete(path_bakfile);
                    if (File.Exists(path_downloadedfile))
                        File.Delete(path_downloadedfile);
                    var wc = new WebClient();
                    await wc.DownloadFileTaskAsync(new Uri(PathToFile+ target), path_downloadedfile);
                    File.Move(AppDomain.CurrentDomain.BaseDirectory + target, path_bakfile);
                    File.Copy(path_downloadedfile, AppDomain.CurrentDomain.BaseDirectory + target);
                    System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                    var win=(MainWindow)Application.Current.MainWindow;
                    win.ExitFromApplication();
                }
            }
            catch (Exception ex)
            {
                Logger.Write(Logger.TypeLogs.main,ex.ToString());
                DispatcherControls.ShowMyDialog("Ошибка обновления",ex.ToString(),Windows.MyDialogWindow.TypeMyDialog.Ok,Application.Current.MainWindow);
            }
        }

        public static Task CheckUpdate()
        {
            Updater.Remote_Update_Local().GetAwaiter();
            return Task.CompletedTask;
        }
    }
}
