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
using UsBudget.classes;

namespace Dagorlad_7
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected async override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            if (!IsMutex)
            {
                //initialize the splash screen and set it as the application main window
                var splashScreen = new SplashWindow();
                this.MainWindow = splashScreen;
                splashScreen.Show();

                //in order to ensure the UI stays responsive, we need to
                //do the work on a different thread
                await Task.Factory.StartNew(() =>
                {
                    //simulate some work being done
#if (!DEBUG)
                System.Threading.Thread.Sleep(3000);
#endif
                    //since we're not on the UI thread
                    //once we're done we need to use the Dispatcher
                    //to create and show the main window
                    this.Dispatcher.Invoke(() =>
                        {
                        //initialize the main window, set it as the application main window
                        //and close the splash screen
                        var mainWindow = new MainWindow();
                            this.MainWindow = mainWindow;
#if (DEBUG)
                        //mainWindow.Show();
#endif
                        splashScreen.Closing -= (qq, ee) => { ee.Cancel = true; };
                            splashScreen.Closing += (qq, ee) => { ee.Cancel = false; };
                            splashScreen.Close();
                        });
                });
            }
        }
        private static string Guid = (Assembly.GetExecutingAssembly().GetCustomAttribute(typeof(AssemblyConfigurationAttribute)) as AssemblyConfigurationAttribute).Configuration;
        private static string User = Environment.UserName;
        static readonly Mutex Mutex = new Mutex(true, "{" + Guid + "}" + " - " + User);
        private bool IsMutex = false;
        public App()
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
        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            DispatcherControls.ShowMyDialog("Необработанное исключение",e.Exception.Message,MyDialogWindow.TypeMyDialog.Ok,null);
            Logger.Write(Logger.TypeLogs.main, e.Exception.ToString());
        }
    }
}