using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Dagorlad
{
    /// <summary>
    /// Логика взаимодействия для Settings.xaml
    /// </summary>
    public partial class SettingsMenu : Window
    {
        public SettingsMenu()
        {
            InitializeComponent();
            About();
            onMusiccb.IsChecked = Properties.Settings.Default.onMusic;
            LightTheme.IsChecked = Properties.Settings.Default.isInvertTheme;
            if (Properties.Settings.Default.perhabsfio != null && Properties.Settings.Default.perhabsemail != null)
            {
                FIOlbl.Text = Properties.Settings.Default.perhabsfio;
                Emaillbl.Text = Properties.Settings.Default.perhabsemail;
            }
            else clearperhabsfioemailbtn.IsEnabled = false;
            if (Properties.Settings.Default.isInvertTheme)
                toInvert();
            VersionRemoteFile.Content = Update.RemoteVersionFile == null ? new Version(0, 0, 0, 0) : Update.RemoteVersionFile;
        }
        void toInvert()
        {
            settingsimage.Source = new BitmapImage(new Uri(@"/Dagorlad;component/Resources/black/settings.png", UriKind.Relative));
            logoimage.Source = new BitmapImage(new Uri(@"/Dagorlad;component/Resources/black/logo.png", UriKind.Relative));
            this.Resources["ColorOne"] = new SolidColorBrush(Colors.Black);
            this.Resources["ColorTwo"] = new SolidColorBrush(Colors.White);
        }
        private void About()
        {
            NameApplication.Content = Assembly.GetExecutingAssembly().GetName().Name+"\n"+
                Logger.GetVersionApp() + "\n" +
                "Разработано: " +FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location).CompanyName+ "\n" +
                FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location).Comments+"\n"+
                FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location).LegalCopyright;
            if (!GetStatusStarUp())
            {
                DeleteReg.Content = "Удалено из автозапуска";
                DeleteReg.IsEnabled = false;
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void DeleteReg_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (Microsoft.Win32.TaskScheduler.TaskService ts = new Microsoft.Win32.TaskScheduler.TaskService())
                {
                    var exist = ts.FindTask("Dagorlad");
                    if (exist != null)
                    {
                        ts.RootFolder.DeleteTask("Dagorlad");
                        DeleteReg.Content = "Удалено из автозапуска";
                    }
                    else
                    {
                        DeleteReg.Content = "Удалено из автозапуска";
                    }
                }
                DeleteReg.IsEnabled = false;
            }
            catch { }
            //if (getRegistryKey() != null)
            //{
            //    getRegistryKey().DeleteValue(Assembly.GetEntryAssembly().GetName().Name, false);
            //    DeleteReg.Content = "Удалено из автозапуска";
            //    DeleteReg.IsEnabled = false;
            //}
        }
        RegistryKey getRegistryKey()
        {
            string _Name = Assembly.GetEntryAssembly().GetName().Name;

            RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            if (rkApp.GetValue(_Name) != null)
                return rkApp;
            else return null;
        }
        bool GetStatusStarUp()
        {
            try
            {
                using (Microsoft.Win32.TaskScheduler.TaskService ts = new Microsoft.Win32.TaskScheduler.TaskService())
                {
                    var exist = ts.FindTask("Dagorlad", true);
                    if (exist == null)
                    {
                        return false;
                    }
                    else return true;
                }
            }
            catch (Exception ex) { return false; }
        }

        private void OnMusiccb_Checked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.onMusic = true;
            Properties.Settings.Default.Save();
        }

        private void OnMusiccb_Unchecked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.onMusic = false;
            Properties.Settings.Default.Save();
        }

        private void DarkTheme_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.isInvertTheme = false;
            Properties.Settings.Default.Save();
            MessageBox.Show("Приложение будет перезапущено");
            Process.Start(System.AppDomain.CurrentDomain.BaseDirectory + "Dagorlad.exe");
            Process.GetCurrentProcess().Kill();
        }

        private void LightTheme_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.isInvertTheme = true;
            Properties.Settings.Default.Save();
            toInvert();
            MessageBox.Show("Приложение будет перезапущено");
            Process.Start(System.AppDomain.CurrentDomain.BaseDirectory + "Dagorlad.exe");
            Process.GetCurrentProcess().Kill();
        }

        private void clearperhabsfioemailbtn_Click(object sender, RoutedEventArgs e)
        {
            var g = MessageBox.Show("Вы уверены, что хотите стереть данные о себе, другие сотрудники не узнают Вас в чате?",
                "Вопрос", MessageBoxButton.YesNo,MessageBoxImage.Question);
            if (g == MessageBoxResult.Yes)
            {
                FIOlbl.Text = null;
                Emaillbl.Text = null;
                Properties.Settings.Default.perhabsfio = null;
                Properties.Settings.Default.perhabsemail = null;
                Properties.Settings.Default.Save();
                clearperhabsfioemailbtn.IsEnabled = false;
            }
        }
        void ModePerhabsBtn(bool turn)
        {
            if(turn)
            {
                FIOlbl.IsReadOnly = false;
                Emaillbl.IsReadOnly = false;
                saveperhabsesbtn.Visibility = Visibility.Visible;
                Passwordtb.Visibility = Visibility.Visible;
            }
            else
            {
                FIOlbl.IsReadOnly = true;
                Emaillbl.IsReadOnly = true;
                saveperhabsesbtn.Visibility = Visibility.Collapsed;
                Passwordtb.Password = null;
                Passwordtb.Visibility = Visibility.Collapsed;
                FIOlbl.Text = Properties.Settings.Default.perhabsfio;
                Emaillbl.Text = Properties.Settings.Default.perhabsemail;
            }
        }
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if ((Keyboard.Modifiers == ModifierKeys.Control) && e.Key == Key.Enter)
            {
                ModePerhabsBtn(true);
            }
        }

        private void saveperhabsesbtn_Click(object sender, RoutedEventArgs e)
        {
            if (Passwordtb.Password == "4815162342")
            {
                Properties.Settings.Default.perhabsfio = FIOlbl.Text;
                Properties.Settings.Default.perhabsemail = Emaillbl.Text;
                Properties.Settings.Default.Save();
            }
            ModePerhabsBtn(false);
        }
    }
}
