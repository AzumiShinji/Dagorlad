using Dagorlad_7.classes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
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

namespace Dagorlad_7.Windows
{
    /// <summary>
    /// Логика взаимодействия для MySettingsWindow.xaml
    /// </summary>
    public class MySettings
    {
        private static string path = AppDomain.CurrentDomain.BaseDirectory + "Settings_"+Environment.UserName+".json";
        public static MySettingsWindow.SettingsClass Settings = new MySettingsWindow.SettingsClass();
        public static Task Save()
        {
            var json = JsonConvert.SerializeObject(MySettings.Settings);
            var bytes = Encoding.UTF8.GetBytes(json);
            File.WriteAllBytes(path, bytes);
            return Task.CompletedTask;
        }
        public static void Load()
        {
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
        }
        private Task LoadSettings()
        {
            IsEnabledSmartMenuCheckBox.IsChecked = MySettings.Settings.SmartMenuIsEnabled;
            return Task.CompletedTask;
        }
        private async Task SaveSettings()
        {
            MySettings.Settings.SmartMenuIsEnabled = IsEnabledSmartMenuCheckBox.IsChecked.Value;
            await MySettings.Save();
        }
        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            await SaveSettings();
            this.DialogResult = true;
        }

    }
}
