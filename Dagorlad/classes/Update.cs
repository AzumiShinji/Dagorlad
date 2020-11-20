using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Dagorlad
{
    class Update
    {
        public const string PathUpdate = "//WebService/SharedFolder/Dagorlad/";
        private const string _Key = "645F2D72E774179B3ABF7BDC5FD6E";
        private const string _KeyUpdate = "5F5E8B7D1FB486DEB735E23782E6B";
        public const string _KeyConfig = "4A5E8B7D1FB596DEB745E26782E2H";
        public const string fileupdate = "Updater.exe";
        private static string current_file = System.AppDomain.CurrentDomain.FriendlyName;
        private const string beepfile = "beep.mp3";
        private static string configsfile = System.AppDomain.CurrentDomain.FriendlyName+".config";
        public static void Start_Update(bool isForce)
        {
            if (!isForce)
            {
                foreach (var key in Environment.GetCommandLineArgs())
                {
                    if (key != _Key)
                    {
                        if (!UpdateProcess())
                        { 
                            NotifyWindowCustom.ShowNotify("Обновление не удалось","Текущая версия: "+ Logger.GetVersionApp(), "notify_notupdate.png", 2, true, typeof(MainWindow), null, null);
                        }
                    }
                    else
                    {
                        DownloadUpdater();
                        DownloadConfigs();
                        DownloadBeep();
                        NotifyWindowCustom.ShowNotify("Успешное обновление", "Установлена новая версия: " + Logger.GetVersionApp(), "Notify_update.png", 2, true, typeof(MainWindow), null, null);
                    }

                    if (key == _KeyConfig)
                    {
                        NotifyWindowCustom.ShowNotify("Настройки успешно перенесены", "Пользовательская конфигурация установлена", "Notify_configs.png", 2, true, typeof(MainWindow), null, null);
                    }
                }
            }
            else UpdateProcess();
        }
        public static void ExecuteAsAdmin(string fileName,string arg)
        {
            Process proc = new Process();
            proc.StartInfo.FileName = fileName;
            proc.StartInfo.WorkingDirectory = Environment.CurrentDirectory;
            proc.StartInfo.Arguments = arg;
            proc.StartInfo.UseShellExecute = true;
            proc.StartInfo.Verb = "runas";
            proc.Start();
        }
        public static Version RemoteVersionFile = null;
        private static bool UpdateProcess()
        {
            try
            {
                try
                {
                    RemoteVersionFile = new Version(FileVersionInfo.GetVersionInfo(PathUpdate + current_file).FileVersion);
                }
                catch (Exception ex) { Logger.Do(ex.ToString(), Logger.LevelAttention.Warning); }
                if (RemoteVersionFile != null && (RemoteVersionFile > Assembly.GetExecutingAssembly().GetName().Version))
                {
                    if (Download())
                    {
                        if (new Version(FileVersionInfo.GetVersionInfo(System.AppDomain.CurrentDomain.BaseDirectory + "_" + current_file).FileVersion) > Assembly.GetExecutingAssembly().GetName().Version)
                        {
                            if (!File.Exists(System.AppDomain.CurrentDomain.BaseDirectory + fileupdate))
                            {
                                if (DownloadUpdater())
                                {
                                    // Process.Start(System.AppDomain.CurrentDomain.BaseDirectory + fileupdate, _KeyUpdate);
                                    ExecuteAsAdmin(System.AppDomain.CurrentDomain.BaseDirectory + fileupdate, _KeyUpdate);
                                    Process.GetCurrentProcess().Kill();
                                    return true;
                                }
                                else { return false; }
                            }
                            else
                            {
                                //Process.Start(System.AppDomain.CurrentDomain.BaseDirectory + fileupdate, _KeyUpdate);
                                ExecuteAsAdmin(System.AppDomain.CurrentDomain.BaseDirectory + fileupdate, _KeyUpdate);
                                Process.GetCurrentProcess().Kill();
                                return true;
                            }
                        }
                        else { File.Delete(System.AppDomain.CurrentDomain.BaseDirectory + "_" + current_file); return true; }
                    }
                    else { return false; }
                }
                else return true;
            }
            catch (Exception ex) { Logger.Do(ex.ToString(), Logger.LevelAttention.Error); return false; }
        }
        private static bool Download()
        {
            WebClient myWebClient = new WebClient();
            try
            {
                myWebClient.DownloadFile(PathUpdate + current_file,
                    System.AppDomain.CurrentDomain.BaseDirectory + "_" + current_file);
                return true;
            }
            catch (Exception ex) { Logger.Do(ex.ToString(), Logger.LevelAttention.Error); return false; }
            finally { myWebClient.Dispose(); }
        }
        private static bool DownloadUpdater()
        {
            WebClient myWebClient = new WebClient();
            try
            {
                myWebClient.DownloadFile(PathUpdate + fileupdate,
                    System.AppDomain.CurrentDomain.BaseDirectory + fileupdate);
                return true;
            }
            catch (Exception ex) { Logger.Do(ex.ToString(), Logger.LevelAttention.Error); return false; }
            finally { myWebClient.Dispose(); }
        }
        private static bool DownloadBeep()
        {
            WebClient myWebClient = new WebClient();
            try
            {
                myWebClient.DownloadFile(PathUpdate + beepfile,
                    System.AppDomain.CurrentDomain.BaseDirectory + beepfile);
                return true;
            }
            catch (Exception ex) { Logger.Do(ex.ToString(), Logger.LevelAttention.Error); return false; }
            finally { myWebClient.Dispose(); }
        }
        private static bool DownloadConfigs()
        {
            WebClient myWebClient = new WebClient();
            try
            {
                myWebClient.DownloadFile(PathUpdate + configsfile,
              System.AppDomain.CurrentDomain.BaseDirectory + configsfile);
                return true;
            }
            catch (Exception ex) { Logger.Do(ex.ToString(), Logger.LevelAttention.Error); return false; }
            finally { myWebClient.Dispose(); }
        }
    }
}
