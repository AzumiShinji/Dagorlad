using Dagorlad_7.classes;
using Dagorlad_7.Windows;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Dagorlad_7
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public const string UpdateGuid = "{8E06A225-F9B4-48BA-A95A-FCE56D275B25}";
        protected async override void OnStartup(StartupEventArgs e)
        {
            try
            {
                base.OnStartup(e);
                if (!IsMutex)
                {
                    SplashWindow splashScreen = null;
                    if (e.Args.Length == 0)
                    {
                        splashScreen = new SplashWindow();
                        this.MainWindow = splashScreen;
                        splashScreen.Show();
                    }
                    await Task.Factory.StartNew(() =>
                    {
                        this.Dispatcher.Invoke(() =>
                                {
                                    var mainWindow = new MainWindow();
                                    this.MainWindow = mainWindow;
                                    if (e.Args.Length > 0 && e.Args[0] == UpdateGuid)
                                    {
                                        var new_version = DispatcherControls.GetVersionApplication(DispatcherControls.TypeDisplayVersion.Fully);
                                        Logger.Write(Logger.TypeLogs.updater, "Application has been updated to "+ new_version);
                                        DispatcherControls.NewMyNotifyWindow(Assembly.GetExecutingAssembly().GetName().Name + " обновился", "Текущая версия: \n" +
                                            new_version, TimeSpan.FromSeconds(8), mainWindow, TypeImageNotify.update);
                                    }
                                    if (splashScreen != null)
                                    {
                                        splashScreen.Closing -= (qq, ee) => { ee.Cancel = true; };
                                        splashScreen.Closing += (qq, ee) => { ee.Cancel = false; };
                                        splashScreen.Close();
                                    }
                                });
                    });
                }
            }
            catch (Exception ex) { Logger.Write(Logger.TypeLogs.main, ex.ToString()); }
        }
        private static string Guid = (Assembly.GetExecutingAssembly().GetCustomAttribute(typeof(AssemblyConfigurationAttribute)) as AssemblyConfigurationAttribute).Configuration;
        private static string User = Environment.UserName;
        static readonly Mutex Mutex = new Mutex(true, "{" + Guid + "}" + " - " + User);
        private bool IsMutex = false;
        public App()
        {
            try
            {
    #if (!DEBUG)
                if (!Mutex.WaitOne(TimeSpan.Zero, true))
                {
                    IsMutex = true;
                    var styles = new Uri("pack://application:,,,/Dagorlad;component/Styles/Styles.xaml", UriKind.RelativeOrAbsolute);
                    var canvas = new Uri("pack://application:,,,/Dagorlad;component/Styles/Canvases.xaml", UriKind.RelativeOrAbsolute);
                    Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = styles });
                    Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = canvas });
                    var g=DispatcherControls.ShowMyDialog(Assembly.GetEntryAssembly().GetName().Name + " уже запущен", "Разрешен запуск только одной копии приложения.",MyDialogWindow.TypeMyDialog.Ok,null);
                    if (g == MyDialogWindow.ResultMyDialog.Ok)
                    Process.GetCurrentProcess().Kill();
                }
                else
                {
                    //no instance running
                }
    #endif
            }
            catch (Exception ex) { Logger.Write(Logger.TypeLogs.main, ex.ToString()); }
        }
        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            Logger.Write(Logger.TypeLogs.main, e.Exception.ToString());
            DispatcherControls.ShowMyDialog("Необработанное исключение",e.Exception.Message,MyDialogWindow.TypeMyDialog.Ok,null);
        }
    }
}