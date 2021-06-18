using Dagorlad_7.classes;
using Dagorlad_7.Windows;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Dagorlad_7.Pages
{
    /// <summary>
    /// Логика взаимодействия для SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();
            LoadAsyncEvents();
        }
        private async void LoadAsyncEvents()
        {
            await LoadSettings();
            AppNameLabel.Content = Assembly.GetExecutingAssembly().GetName().Name.ToUpper();
            var current_version = Assembly.GetExecutingAssembly().GetName().Version;
            var on_server_version = Updater.GetVersionOnServer();
            AppVersionLabel.Content = DispatcherControls.GetVersionApplication(DispatcherControls.TypeDisplayVersion.Fully) +
                "\nВерсия на сервере: " + (on_server_version == null ? "Нет данных" : ("v." + on_server_version));
            if (on_server_version > current_version)
                HandUpdateButton.Visibility = Visibility.Visible;
            else HandUpdateButton.Visibility = Visibility.Collapsed;
        }
        private Task LoadSettings()
        {
            IsEnabledSmartMenuCheckBox.IsChecked = MySettings.Settings.SmartMenuIsEnabled;
            IsAutorunCheckBox.IsChecked = DispatcherControls.Autorun(DispatcherControls.TypeAutoRunOperation.CheckStatus);
            EmailTextBox.Text = MySettings.Settings.Email;
            switch (MySettings.Settings.TypeColorScheme)
            {
                case (TypeColorScheme.dark):
                    DarkColorSchemeRadioButton.IsChecked = true; break;
                case (TypeColorScheme.light):
                    LightColorSchemeRadioButton.IsChecked = true; break;
            }
            IsRegGlobalEventCheckBox.IsChecked = MySettings.Settings.IsRegGlobalHook;
            IsSearchOrganizationsCheckBox.IsChecked = MySettings.Settings.IsSearchOrganizations;
            IsTransparentBackgroundDialogOfChatWindowCheckBox.IsChecked = MySettings.Settings.IsTransparentBackgroundDialogOfChatWindow;
            return Task.CompletedTask;
        }
        private async Task SaveSettings()
        {
            if (IsAutorunCheckBox.IsChecked == true)
                DispatcherControls.Autorun(DispatcherControls.TypeAutoRunOperation.On);
            else DispatcherControls.Autorun(DispatcherControls.TypeAutoRunOperation.Off);
            MySettings.Settings.SmartMenuIsEnabled = IsEnabledSmartMenuCheckBox.IsChecked.Value;
            if (!String.IsNullOrEmpty(EmailTextBox.Text))
            {
                if (IsAllowAccessEditEmail)
                {
                    MySettings.Settings.Email = EmailTextBox.Text.Trim();
                    await Hash.SetHashToWebServiceEmployees(EmailTextBox.Text.Trim());
                }
            }
            if (DarkColorSchemeRadioButton.IsChecked == true)
            {
                if (MySettings.Settings.TypeColorScheme != TypeColorScheme.dark)
                {
                    MySettings.Settings.TypeColorScheme = TypeColorScheme.dark;
                    DispatcherControls.SetSchemeColor(MySettings.Settings.TypeColorScheme, false);
                }
            }
            else if (LightColorSchemeRadioButton.IsChecked == true)
            {
                if (MySettings.Settings.TypeColorScheme != TypeColorScheme.light)
                {
                    MySettings.Settings.TypeColorScheme = TypeColorScheme.light;
                    DispatcherControls.SetSchemeColor(MySettings.Settings.TypeColorScheme, false);
                }
            }
            MySettings.Settings.IsRegGlobalHook = IsRegGlobalEventCheckBox.IsChecked.Value;
            MySettings.Settings.IsSearchOrganizations = IsSearchOrganizationsCheckBox.IsChecked.Value;
            MySettings.Settings.IsTransparentBackgroundDialogOfChatWindow = IsTransparentBackgroundDialogOfChatWindowCheckBox.IsChecked.Value;
            await MySettings.Save();
        }
        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            btn.IsEnabled = false;
            await SaveSettings();
            DispatcherControls.ShowMiniMenu();
            //DispatcherControls.SetSchemeColor(MySettings.Settings.TypeColorScheme, false);
            DispatcherControls.SetBackgroundDialog(MySettings.Settings.IsTransparentBackgroundDialogOfChatWindow);
            if (MySettings.Settings.IsRegGlobalHook)
                GlobalHook.StartHooking();
            else GlobalHook.StopHooking();
            var old_content = btn.Content;
            btn.Content = "Сохранено";
            await Task.Delay(2500);
            btn.Content = old_content;
            btn.IsEnabled = true;
        }
        private void OpenLogsFolder_Click(object sender, RoutedEventArgs e)
        {
            Logger.Open();
        }

        private void Window_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if ((Keyboard.Modifiers == ModifierKeys.Control) && e.Key == Key.Enter)
            {
                OpenLogsFolderButton.Visibility = Visibility.Visible;
            }
        }

        private void HandUpdateButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            button.IsEnabled = false;
            Updater.UpdateNowHandMade();
        }

        bool IsAllowAccessEditEmail = false;
        private void AllowingEditEmailButton_Click(object sender, RoutedEventArgs e)
        {
            var wnd = new AuthenticationWebServiceWindow();
            wnd.Owner = Application.Current.MainWindow;
            var result = wnd.ShowDialog();
            if (result.HasValue && result.Value == true)
            {
                EmailTextBox.IsReadOnly = false;
                EmailTextBox.SelectAll();
                EmailTextBox.Focus();
                IsAllowAccessEditEmail = true;
            }
            else
            {
                EmailTextBox.IsReadOnly = true;
                IsAllowAccessEditEmail = false;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadAsyncEvents();
        }
    }
}
