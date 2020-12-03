using Dagorlad_7.classes;
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
using System.Windows.Shapes;
using UsBudget.classes;

namespace Dagorlad_7.Windows
{
    /// <summary>
    /// Логика взаимодействия для MySettingsWindow.xaml
    /// </summary>
    public class MySettings
    {
        public static string RoamingFolder = "";
        private static string path = "";
        public static MySettingsWindow.SettingsClass Settings = new MySettingsWindow.SettingsClass();
        public static Task Save()
        {
            var json = JsonConvert.SerializeObject(MySettings.Settings);
            var bytes = Encoding.UTF8.GetBytes(json);
            File.WriteAllBytes(path, bytes);
            Console.WriteLine("Settings saved to: {0}", path);
            return Task.CompletedTask;
        }
        public static void Load()
        {
            string root = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" + Assembly.GetExecutingAssembly().GetName().Name;
            RoamingFolder = root;
            if (!Directory.Exists(root)) Directory.CreateDirectory(root);
            path = root + "\\Settings.json";
            Console.WriteLine("Trying opening settings from: {0}", path);
            if (File.Exists(path))
            {
                var bytes = File.ReadAllBytes(path);
                var content = Encoding.UTF8.GetString(bytes);
                if (content != null && content.Length > 0)
                {
                    MySettingsWindow.SettingsClass json = JsonConvert.DeserializeObject<MySettingsWindow.SettingsClass>(content);
                    MySettings.Settings = json;
                }
            }
        }
        public class OldCustomExample {
            public class Rootobject
            {
                public Example[] Examples { get; set; }
            }

            public class Example
            {
                public string Folder { get; set; }
                public string Name { get; set; }
                public string Text { get; set; }
            }
        }
        public static void LoadEmailFromOldDagorlad()
        {
            try
            {
                var emailfile = AppDomain.CurrentDomain.BaseDirectory + String.Format("{0}.email", Environment.UserName);
                if (File.Exists(emailfile))
                {
                    MySettings.Settings.Email = File.ReadAllText(emailfile);
                    try
                    {
                        File.Delete(emailfile);
                    }
                    catch { }
                    MySettings.Save();
                }
                var customexamplefile = AppDomain.CurrentDomain.BaseDirectory + String.Format("CustomExample_{0}.json", Environment.UserName);
                if (File.Exists(customexamplefile))
                {
                    if (MySettings.Settings.SmartMenuList.Count() == 0)
                    {
                        OldCustomExample.Example[] objs = null;
                        using (StreamReader file = File.OpenText(customexamplefile))
                        {
                            objs = ((OldCustomExample.Rootobject)new JsonSerializer().Deserialize(file, typeof(OldCustomExample.Rootobject))).Examples;
                        }
                        var list = new List<SmartAnswersClass>();
                        foreach (var s in objs.GroupBy(x => x.Folder).Select(x => x.First()).Select(x => x.Folder))
                        {
                            var name = s;
                            var items = new List<SmartAnswers_SubClass>();
                            foreach (var g in objs.Where(x => x.Folder == s))
                            {
                                items.Add(new SmartAnswers_SubClass { title = g.Name, text = g.Text });
                            }
                            MySettings.Settings.SmartMenuList.Add(new SmartAnswersClass { name = name, items = new ObservableCollection<SmartAnswers_SubClass>(items) });
                        }
                        File.Delete(customexamplefile);
                    }
                }
            }
            catch (Exception ex) { Logger.Write(Logger.TypeLogs.main,ex.ToString()); }
        }
    }
    public partial class MySettingsWindow : Window
    {
        public class SettingsClass
        {
            public bool IsFirstTimeLanuched = true;
            public bool SmartMenuIsEnabled = true;
            public ObservableCollection<SmartAnswersClass> SmartMenuList = new ObservableCollection<SmartAnswersClass>();
            public string Email { get; set; }
            public TypeColorScheme TypeColorScheme = TypeColorScheme.dark;
            //public string NSD_Login { get; set; }
            //public string NSD_Password { get; set; }
            //public string NSD_Token { get; set; }
        }
        public MySettingsWindow()
        {
            InitializeComponent();
            LoadAsyncEvents();
        }
        private async void LoadAsyncEvents()
        {
            await LoadSettings();
            AppNameLabel.Content = Assembly.GetExecutingAssembly().GetName().Name.ToUpper();
            AppVersionLabel.Content = DispatcherControls.GetVersionApplication(DispatcherControls.TypeDisplayVersion.Fully);
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
            if (!String.IsNullOrEmpty(EmailTextBox.Text))
                EmailTextBox.IsEnabled = false;
            return Task.CompletedTask;
        }
        private async Task SaveSettings()
        {
            if (IsAutorunCheckBox.IsChecked == true)
                DispatcherControls.Autorun(DispatcherControls.TypeAutoRunOperation.On);
            else DispatcherControls.Autorun(DispatcherControls.TypeAutoRunOperation.Off);
            MySettings.Settings.SmartMenuIsEnabled = IsEnabledSmartMenuCheckBox.IsChecked.Value;
            if (!String.IsNullOrEmpty(EmailTextBox.Text))
                MySettings.Settings.Email = EmailTextBox.Text.Trim();
            if (DarkColorSchemeRadioButton.IsChecked == true)
            {
                MySettings.Settings.TypeColorScheme = TypeColorScheme.dark;
            }
            else if (LightColorSchemeRadioButton.IsChecked == true)
            {
                MySettings.Settings.TypeColorScheme = TypeColorScheme.light;
            }
            await MySettings.Save();
        }
        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            await SaveSettings();
            this.DialogResult = true;
        }

        private void OpenLogsFolder_Click(object sender, RoutedEventArgs e)
        {
            Logger.Open();
        }

        private void Window_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if ((Keyboard.Modifiers == ModifierKeys.Control) && e.Key == Key.Enter)
            {
                EmailTextBox.IsEnabled = true;
            }
        }
    }
}
