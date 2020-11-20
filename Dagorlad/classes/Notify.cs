using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Dagorlad
{
    class Notify
    {
        public static void ShowInTray()
        {
            System.Windows.Forms.NotifyIcon Install_Notify = new System.Windows.Forms.NotifyIcon();
            Install_Notify.Icon = System.Drawing.Icon.ExtractAssociatedIcon(System.Reflection.Assembly.GetExecutingAssembly().Location);
            Install_Notify.Visible = true;
            Install_Notify.Text = "Dagorlad v." + Assembly.GetExecutingAssembly().GetName().Version.ToString();
            Install_Notify.Click +=
                (object sender, EventArgs args) =>
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(MainWindow))
                        {
                            (window as MainWindow).Show();
                            (window as MainWindow).WindowState = WindowState.Normal;
                            (window as MainWindow).Activate();
                        }
                    }
                }));
            };
        }
        public static void ShowInTrayDialog()
        {
            System.Windows.Forms.NotifyIcon Install_Notify = new System.Windows.Forms.NotifyIcon();
            Install_Notify.Icon = new System.Drawing.Icon(Application.GetResourceStream(new Uri("pack://application:,,,/system_files/chat.ico")).Stream);
            Install_Notify.Visible = true;
            Install_Notify.Text = "Dagorlad - Чат";
            Install_Notify.Click +=
                (object sender, EventArgs args) =>
                {
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window.GetType() == typeof(Chat))
                            {
                                (window as Chat).Show();
                                (window as Chat).WindowState = WindowState.Normal;
                                (window as Chat).Activate();
                            }
                        }
                    }));
                };
        }
    }
}
