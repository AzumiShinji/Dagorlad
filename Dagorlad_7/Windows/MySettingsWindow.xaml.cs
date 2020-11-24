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
            string root = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +"\\"+ Assembly.GetExecutingAssembly().GetName().Name;
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
    }
    public partial class MySettingsWindow : Window
    {
        public class SettingsClass
        {
            public bool IsFirstTimeLanuched = true;
            public bool SmartMenuIsEnabled = true;
            public ObservableCollection<SmartAnswersClass> SmartMenuList = new ObservableCollection<SmartAnswersClass>();
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
            return Task.CompletedTask;
        }
        private async Task SaveSettings()
        {
            if (IsAutorunCheckBox.IsChecked == true)
                DispatcherControls.Autorun(DispatcherControls.TypeAutoRunOperation.On);
            else DispatcherControls.Autorun(DispatcherControls.TypeAutoRunOperation.Off);

            MySettings.Settings.SmartMenuIsEnabled = IsEnabledSmartMenuCheckBox.IsChecked.Value;
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
    }
}
