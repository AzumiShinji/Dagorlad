using Dagorlad_7.classes;
using Dagorlad_7.Windows;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
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
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            //initialize the splash screen and set it as the application main window
            var splashScreen = new SplashWindow();
            this.MainWindow = splashScreen;
            splashScreen.Show();

            //in order to ensure the UI stays responsive, we need to
            //do the work on a different thread
            Task.Factory.StartNew(() =>
            {
                //simulate some work being done
#if (!DEBUG)
                System.Threading.Thread.Sleep(5000);
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
#if(DEBUG)
                    //mainWindow.Show();
#endif
                    splashScreen.Closing -= (qq,ee) => { ee.Cancel = true; };
                    splashScreen.Closing += (qq, ee) => { ee.Cancel = false; };
                    splashScreen.Close();
                });
            });
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            DispatcherControls.ShowMyDialog("Необработанное исключение",e.Exception.Message,MyDialogWindow.TypeMyDialog.Ok,null);
            Logger.Write(Logger.TypeLogs.main, e.Exception.ToString());
        }
    }
}