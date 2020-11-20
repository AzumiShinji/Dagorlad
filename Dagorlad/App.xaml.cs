using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Dagorlad
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private const string Guid = "250C5587-BA73-40DF-B2CF-DD644F044846";
        private static string User = Environment.UserName;
        static readonly Mutex Mutex = new Mutex(true, "{" + Guid + "}"+" - "+ User);

        public App()
        {

            if (!Mutex.WaitOne(TimeSpan.Zero, true))
            {
                //already an instance running
                MessageBox.Show("Dagorlad уже запущен", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                Application.Current.Shutdown();
            }
            else
            {
                //no instance running
            }

        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            Logger.UnhandledException(e);
        }
    }
}
