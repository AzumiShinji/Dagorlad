using Dagorlad_7.classes;
using Dagorlad_7.Pages;
using Dagorlad_7.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Dagorlad_7
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {      
        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == System.Windows.WindowState.Minimized)
                this.Hide();

            base.OnStateChanged(e);
        }

        public MainWindow()
        {
            MySettings.Load();
#if (!DEBUG)
            Updater.CheckUpdate().GetAwaiter();
#endif
            DispatcherControls.SetSchemeColor(MySettings.Settings.TypeColorScheme, true);
            DispatcherControls.HideWindowToTaskMenu(this, null);
            //////////////////////////////////////////////
            InitializeComponent();
            //////////////////////////////////////////////
            LoadEvents();
#if (!DEBUG)
            CheckingUpdateApplicationStart();
#endif
            organizationsPage = new Pages.SearchOrganizationsPage();
            MainFrame.Content = organizationsPage;
            PreviousButton = OrganizationButton;
            OrganizationButton.IsEnabled = false;
            chatPage = new Pages.ChatPage();
        }
        
        private DispatcherTimer timerToUpdate = new DispatcherTimer();
        private void CheckingUpdateApplicationStart()
        {
            var time = new Random().Next(5, 15);
            timerToUpdate.Interval = TimeSpan.FromMinutes(time);
            Logger.Write(Logger.TypeLogs.updater, "Started checking update, next update attempt in " + time + " minutes.");
            timerToUpdate.Tick += async (q, e) =>
            {
                await Updater.CheckUpdate();
            };
            timerToUpdate.Start();
        }
        private async void LoadEvents()
        {
            DispatcherControls.ShowMiniMenu();
            if (MySettings.Settings.IsFirstTimeLanuched)
            {
                DispatcherControls.Autorun(DispatcherControls.TypeAutoRunOperation.On);
                MySettings.Settings.IsFirstTimeLanuched = false;
            }
            await MySettings.Save();
            if (MySettings.Settings.IsRegGlobalHook)
                GlobalHook.StartHooking();
            else GlobalHook.StopHooking();
            
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.WindowState = WindowState.Minimized;
        }
        private void CloseApplicationButton_Click(object sender, RoutedEventArgs e)
        {
            var g = DispatcherControls.ShowMyDialog("Выход", "Вы уверены, что хотите выйти?", MyDialogWindow.TypeMyDialog.YesNo, this);
            if (g == MyDialogWindow.ResultMyDialog.Yes)
            {
                ExitFromApplication(false);
            }
        }
        public async void ExitFromApplication(bool OnlyEvent)
        {
            try
            {
                ClipboardMonitor.Stop();
            }
            catch (Exception ex) { Logger.Write(Logger.TypeLogs.chat, "Exit ClipboardMonitor Exception: " + ex.ToString()); }
            try
            {
                if (ChatPage.Me != null)
                {
                    await ChatPage.Proxy.DisconnectAsync(ChatPage.Me);
                    Logger.Write(Logger.TypeLogs.chat, "Disconnected: " + ChatPage.Me.Email);
                }
            }
            catch (Exception ex) { Logger.Write(Logger.TypeLogs.chat, "Exit ChatWindow Exception: " + ex.ToString()); }
            DispatcherControls.CloseAllNotifyIcon();
            if (!OnlyEvent)
            {
                Application.Current.Shutdown();
                Process.GetCurrentProcess().Kill();
            }
        }

        Pages.SearchOrganizationsPage organizationsPage;
        Pages.SettingsPage settingsPage;
        Pages.UtilitiesPage utilitiesPage;
        Pages.ChatPage chatPage;
        Button PreviousButton;
        private void SelectPageClick_Button(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            btn.IsEnabled = false;
            var cmd = btn.CommandParameter as string;
            switch(cmd)
            {
                case ("Organizations"):
                    {
                        MainFrame.Content = organizationsPage;
                        break;
                    }
                case ("Settings"):
                    {
                        if (settingsPage == null)
                            settingsPage = new Pages.SettingsPage();
                        MainFrame.Content = settingsPage;
                        break;
                    }
                case ("Utilities"):
                    {
                        if (utilitiesPage == null)
                            utilitiesPage = new Pages.UtilitiesPage();
                        MainFrame.Content = utilitiesPage;
                        break;
                    }
                case ("Chat"):
                    {
                        MainFrame.Content = chatPage;
                        break;
                    }
            }
            if(PreviousButton!=null)
            {
                if (PreviousButton != btn)
                { 
                    PreviousButton.IsEnabled = true;
                    PreviousButton = btn;
                }

            }
            else
            {
                PreviousButton = btn;
            }
        }
        private void MainFrame_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            if (e.NavigationMode != NavigationMode.New)
                e.Cancel = true;
            else e.Cancel = false;
        }
    }
    public class WidthFixedListViewConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null || parameter == null) return 0;
            var result = (double)value - System.Convert.ToDouble(parameter);
            if (result >= 0)
                return (double)value - System.Convert.ToDouble(parameter);
            else return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return 0;
        }
    }
}
