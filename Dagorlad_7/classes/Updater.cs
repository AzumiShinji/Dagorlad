using Dagorlad_7.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
        private static bool CheckNewVersion()
        {
            PathToRoaming = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" + Assembly.GetExecutingAssembly().GetName().Name + "\\";
            var target = Assembly.GetEntryAssembly().GetName().Name + ".exe";
            var RemoteVersionFile = new Version(FileVersionInfo.GetVersionInfo(PathToFile + target).FileVersion);
            if (RemoteVersionFile > Assembly.GetExecutingAssembly().GetName().Version)
                return true;
            return false;
        }
        private static Task Remote_Update_Local()
        {
            try
            {
                PathToRoaming = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" + Assembly.GetExecutingAssembly().GetName().Name+"\\";
                var target = Assembly.GetEntryAssembly().GetName().Name+".exe";
                if (File.Exists(PathToFile + target))
                {
                    var RemoteVersionFile = new Version(FileVersionInfo.GetVersionInfo(PathToFile + target).FileVersion);
                    if (RemoteVersionFile > Assembly.GetExecutingAssembly().GetName().Version)
                    {
                        Logger.Write(Logger.TypeLogs.updater, "Founded a new version of application!");
                        Logger.Write(Logger.TypeLogs.updater, "Trying update...");
                        Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            ((MainWindow)Application.Current.MainWindow).ExitFromApplication(true);
                        }));
                        Logger.Write(Logger.TypeLogs.updater, "All Events has been disposed...");
                        var path_bakfile = PathToRoaming + Path.GetFileName(target) + ".bak.remote";
                        var path_downloadedfile = PathToRoaming + target + "_";
                        if (File.Exists(path_bakfile))
                            File.Delete(path_bakfile);
                        if (File.Exists(path_downloadedfile))
                            File.Delete(path_downloadedfile);
                        var wc = new WebClient();
                        wc.DownloadFile(new Uri(PathToFile + target), path_downloadedfile);
                        File.Move(AppDomain.CurrentDomain.BaseDirectory + target, path_bakfile);
                        File.Copy(path_downloadedfile, AppDomain.CurrentDomain.BaseDirectory + target);
                        Logger.Write(Logger.TypeLogs.updater, "The Update was successfull...");
                        System.Diagnostics.Process.Start(Application.ResourceAssembly.Location, App.UpdateGuid);
                        System.Diagnostics.Process.GetCurrentProcess().Kill();
                    }
                    else { Logger.Write(Logger.TypeLogs.updater, "Not found new version."); }
                }
                else { Logger.Write(Logger.TypeLogs.updater, "File not exist on server!"); }
            }
            catch (Exception ex)
            {
                Logger.Write(Logger.TypeLogs.updater,ex.ToString());
                DispatcherControls.ShowMyDialog("Ошибка обновления",ex.ToString(),Windows.MyDialogWindow.TypeMyDialog.Ok,Application.Current.MainWindow);
            }
            return Task.CompletedTask;
        }
        public static Version GetVersionOnServer()
        {
            var target = Assembly.GetEntryAssembly().GetName().Name + ".exe";
            if (File.Exists(PathToFile + target))
            {
                return new Version(FileVersionInfo.GetVersionInfo(PathToFile + target).FileVersion);
            }
            else { Logger.Write(Logger.TypeLogs.updater, "File not exist on server!"); return null; }
        }
        public static void UpdateNowHandMade()
        {
            Logger.Write(Logger.TypeLogs.updater, "Hand Event Update.");
            CheckUpdate().GetAwaiter();
        }
        public static Task CheckUpdate()
        {
            Logger.Write(Logger.TypeLogs.updater, "Trying find new version of application...");
            try
            {
                bool isElevated;
                using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
                {
                    WindowsPrincipal principal = new WindowsPrincipal(identity);
                    isElevated = principal.IsInRole(WindowsBuiltInRole.Administrator);
                }

                Logger.Write(Logger.TypeLogs.updater, String.Format("IsElevated: {0}",isElevated));

                if (isElevated)
                {
#if (!DEBUG)
                    DispatcherControls.CreateConfigFiles();
#endif
                    Updater.Remote_Update_Local().GetAwaiter();
                }
            }
            catch (Exception ex) { Logger.Write(Logger.TypeLogs.updater, ex.ToString()); }
            return Task.CompletedTask;
        }
    }
}
