using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Dagorlad_7.classes
{
    class MySettings
    {
        public class MySettingsClass : INotifyPropertyChanged
        {
            public bool IsFirstTimeLanuched = true;
            public bool SmartMenuIsEnabled = true;
            public ObservableCollection<SmartAnswersClass> SmartMenuList = new ObservableCollection<SmartAnswersClass>();
            public string Email { get; set; }
            public TypeColorScheme TypeColorScheme = TypeColorScheme.dark;
            public string ClearingFolder { get; set; }
            public bool IsSearchOrganizations = true;
            public bool IsTransparentBackgroundDialogOfChatWindow = false;



            private bool _IsRegGlobalHook = true;
            public bool IsRegGlobalHook
            {
                get { return this._IsRegGlobalHook; }

                set
                {
                    if (value != this._IsRegGlobalHook)
                    {
                        this._IsRegGlobalHook = value;
                        NotifyPropertyChanged("IsRegGlobalHook");
                    }
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }

        }

        public static string RoamingFolder = "";
        private static string path = "";
        public static MySettingsClass Settings = new MySettingsClass();
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
                    MySettingsClass json = JsonConvert.DeserializeObject<MySettingsClass>(content);
                    MySettings.Settings = json;
                }
            }
        }
    }
}
