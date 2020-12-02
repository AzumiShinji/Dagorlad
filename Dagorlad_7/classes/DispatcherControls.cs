using Dagorlad_7.Windows;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using static Dagorlad_7.Windows.MyDialogWindow;

namespace Dagorlad_7.classes
{
    public enum TypeColorScheme
    {
        dark = 0,
        light = 1,
    }
    class DispatcherControls
    {
#if (DEBUG)
        public static string ConnectionString_RUNBP = @"Data Source=(LocalDB)\db11;Initial Catalog=runbp;Integrated Security=True;Connect Timeout=30;";
        public static string ConnectionString_SUE = @"Data Source=(LocalDB)\db11;Initial Catalog=SUE;Integrated Security=True;Connect Timeout=30;";
#else
        public static string ConnectionString_RUNBP = @"Data Source=WebService\Carcharoth;Initial Catalog=runbp;User ID=sa;Password=iloveyoujesus";
        public static string ConnectionString_SUE = @"Data Source=WebService\Carcharoth;Initial Catalog=SUE;User ID=sa;Password=iloveyoujesus";
#endif
        public enum TypeDisplayVersion
        {
            Fully = 0,
            Shortly = 1,
        }
        public enum TypeBuild
        {
            Debug = 0,
            Release = 1,
        }
        public enum TypeAutoRunOperation
        {
            On = 0,
            Off = 1,
            CheckStatus = 2,
        }
        public static string GetVersionApplication(TypeDisplayVersion typedisplayversion)
        {
            var version = System.Reflection.Assembly.GetEntryAssembly().GetName().Version;
            DateTime buildDate = new DateTime(2000, 1, 1)
                        .AddDays(version.Build).AddSeconds(version.Revision * 2);
#if(DEBUG)
            TypeBuild typebuild = TypeBuild.Debug;
#else
            TypeBuild typebuild = TypeBuild.Release;
#endif
            string dt = "";
            switch (typedisplayversion)
            {
                case (TypeDisplayVersion.Fully):
                    {
                        dt = buildDate.ToString();
                        break;
                    }
                case (TypeDisplayVersion.Shortly):
                    {
                        dt = buildDate.ToShortDateString();
                        break;
                    }
            }
            return String.Format("v.{0}, [{1}] ({2})", version, dt, typebuild);
        }
        public static Task ChangeToStablePositionWindow(Window win)
        {
            //var current_window = System.Windows.Forms.Screen.FromHandle(new WindowInteropHelper(win).Handle);
            //System.Drawing.Rectangle current_window_size = current_window.WorkingArea;
            var main_window_size = System.Windows.SystemParameters.WorkArea;
            var screen_width = main_window_size.Width;
            var screen_height = main_window_size.Height;
            if((win.Left + win.ActualWidth) >= screen_width)
            win.Left = win.Left-(win.ActualWidth - (screen_width - win.Left));
            if((win.Top + win.ActualHeight) >= screen_height)
                win.Top = win.Top - (win.ActualHeight - (screen_height - win.Top));
            return Task.CompletedTask;
        }
        public static async void ShowSmartMenu()
        {
            bool IsExistWindow = false;
            SmartMenuWindow cmc = null;
            foreach (var window in App.Current.Windows)
                if (window.GetType() == typeof(SmartMenuWindow))
                {
                    var wnd = ((SmartMenuWindow)window);
                    if (wnd.IsLoaded)
                    {
                        if (wnd.IsActive)
                            return;
                        cmc = wnd;
                        IsExistWindow = true;
                        break;
                    }
                }
            if (!IsExistWindow)
                cmc = new SmartMenuWindow();
            cmc.Topmost = true;
            cmc.WindowStartupLocation = WindowStartupLocation.Manual;
            Point mousePositionInApp = CursorPosition.GetCursorPosition();
            cmc.Top = mousePositionInApp.Y;
            cmc.Left = mousePositionInApp.X;
            cmc.Opacity = 0;
            cmc.Show();
            await DispatcherControls.ChangeToStablePositionWindow(cmc);
            var da = new DoubleAnimation()
            {
                Duration = new Duration(TimeSpan.FromMilliseconds(150)),
                From = 0,
                To = 1,
            };
            cmc.BeginAnimation(Window.OpacityProperty, da);
        }
        public static void HideWindowToTaskMenu(Window win,string name)
        {
            System.Windows.Forms.NotifyIcon Install_Notify = new System.Windows.Forms.NotifyIcon();
            if (win.GetType() == typeof(MainWindow))
            {
                Install_Notify.Icon = System.Drawing.Icon.ExtractAssociatedIcon(System.Reflection.Assembly.GetExecutingAssembly().Location);
                Install_Notify.Text = Assembly.GetExecutingAssembly().GetName().Name;
            }
            else if (win.GetType() == typeof(ChatWindow))
            {
                Install_Notify.Icon = Dagorlad_7.Properties.Resources.chat;
                Install_Notify.Text = Assembly.GetExecutingAssembly().GetName().Name + " - " + name;
            }
            else return;
            Install_Notify.Visible = true;
            Install_Notify.Click +=
                (object sender, EventArgs args) =>
                {
                    win.Show();
                    win.WindowState = WindowState.Normal;
                    win.Activate();
                };
        }
        public static bool? Autorun(TypeAutoRunOperation operation)
        {
            var path_run = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";
            RegistryKey rkApp = Registry.CurrentUser.OpenSubKey(path_run, true);
            bool IsExistKey = rkApp.GetValue(Assembly.GetEntryAssembly().GetName().Name) == null ? false : true;
            switch (operation)
            {
                case (TypeAutoRunOperation.On):
                    {
                        if (!IsExistKey)
                            rkApp.SetValue(Assembly.GetEntryAssembly().GetName().Name, System.Windows.Forms.Application.ExecutablePath.ToString());
                        break;
                    }
                case (TypeAutoRunOperation.Off):
                    {
                        if (IsExistKey)
                            rkApp.DeleteValue(Assembly.GetEntryAssembly().GetName().Name, false);
                        break;
                    }
                case (TypeAutoRunOperation.CheckStatus):
                    {
                        return IsExistKey;
                    }
            }
            return null;
        }
        public static ResultMyDialog ShowMyDialog(string title,string text, TypeMyDialog type,Window win)
        {
            var g = new MyDialogWindow(title,text,type);
            if(win != null && win.IsLoaded)
            g.Owner = win;
            if(g.ShowDialog()==true)
            {
                return g.result;
            }
            return ResultMyDialog.Cancel;
        }
        public static ObservableCollection<MyNotifyClass> MyNotifyList = new ObservableCollection<MyNotifyClass>();
        public static MyNotifyWindow _MyNotifyWindow;
        public static void NewMyNotifyWindow(string title, string text, int closeafterseconds, Window win, object image)
        {
            var obj = new MyNotifyClass()
            {
                dt = DateTime.Now,
                title = title,
                text = text,
                image = SetImageNotify(image),
                timetoautoclose = TimeSpan.FromSeconds(closeafterseconds),
            };
            MyNotifyList.Add(obj);
            if (_MyNotifyWindow == null)
            {
                _MyNotifyWindow = new MyNotifyWindow(win);
                _MyNotifyWindow.Show();
            }
            _MyNotifyWindow.FromWindow = win;
            if (_MyNotifyWindow.Visibility == Visibility.Hidden)
            {
                _MyNotifyWindow.Show();
            }
        }
        private static BitmapImage SetImageNotify(object type)
        {
            if (type.GetType() == typeof(TypeImageNotify))
            {
                switch (type)
                {
                    case (TypeImageNotify.standart):
                        {
                            var SourceUri = new Uri("pack://application:,,,/Dagorlad;component/favicon.ico", UriKind.Absolute);
                            var thisIcon = new BitmapImage(SourceUri);
                            return thisIcon;
                        }
                    case (TypeImageNotify.buildings):
                        {
                            var SourceUri = new Uri("pack://application:,,,/Dagorlad;component/images/buildings_64.png", UriKind.Absolute);
                            var thisIcon = new BitmapImage(SourceUri);
                            return thisIcon;
                        }
                    case (TypeImageNotify.chat):
                        {
                            var SourceUri = new Uri("pack://application:,,,/Dagorlad;component/chat.ico", UriKind.Absolute);
                            var thisIcon = new BitmapImage(SourceUri);
                            return thisIcon;
                        }
                }
            }
            else
            {
                return type as BitmapImage;
            }
            return null;
        }
        public class Employees
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public string Direction { get; set; }
        }
        public static async Task<Employees> FindEmployees(string Email)
        {
            Employees employees=null;
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = DispatcherControls.ConnectionString_SUE;
                con.Open();
                using (SqlCommand sqlCommand = new SqlCommand("SELECT * FROM Users WHERE Email=@Email", con))
                {
                    sqlCommand.Parameters.AddWithValue("@Email",Email);
                    using (SqlDataReader reader = await sqlCommand.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            employees= new Employees
                            {
                                Email = reader["Email"] == DBNull.Value ? "" : (string)reader["Email"],
                                Name = reader["FIO"] == DBNull.Value ? "" : (string)reader["FIO"],
                                Direction = reader["Direction"] == DBNull.Value ? "" : (string)reader["Direction"],
                            };
                        }
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            return employees;
        }
        public static void CreateConfigFiles()
        {
            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory+ "Dagorlad.exe.config", "<?xml version=\"1.0\" encoding=\"utf-8\"?> <configuration> <startup> <supportedRuntime version=\"v4.0\" sku=\".NETFramework,Version=v4.7.2\"/> </startup> <system.serviceModel> <bindings> <netTcpBinding> <binding name=\"NetTcpBinding_IChat\"> <reliableSession inactivityTimeout=\"20:00:10\" enabled=\"true\" /> <security> <transport sslProtocols=\"None\" /> </security> </binding> </netTcpBinding> </bindings> <client> <endpoint address=\"net.tcp://webservice:9002/Dagorlad_Chat\" binding=\"netTcpBinding\" bindingConfiguration=\"NetTcpBinding_IChat\" contract=\"SVC.IChat\" name=\"NetTcpBinding_IChat\" /> </client> </system.serviceModel> </configuration>");
        }

        public static void SetSchemeColor(TypeColorScheme type)
        {
            switch(type)
            {
                case (TypeColorScheme.light):
                    {
                        Application.Current.Resources["Background_Inside"] = new SolidColorBrush(Colors.White);
                        Application.Current.Resources["Background_Outside"] = new SolidColorBrush(Colors.LightGray);
                        Application.Current.Resources["Background_Inside_TextBox"] = new SolidColorBrush(Colors.WhiteSmoke);
                        Application.Current.Resources["Background_Green"] = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF3A6FD8"));
                        Application.Current.Resources["Background_Border"] = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFD8BB3A"));
                        Application.Current.Resources["Background_Green_Color"] = (Color)ColorConverter.ConvertFromString("#FF3A6FD8");
                        Application.Current.Resources["Foreground"] = new SolidColorBrush(Colors.Black);
                        Application.Current.Resources["Foreground_Button"] = new SolidColorBrush(Colors.Black);
                        Application.Current.Resources["Foreground_Dark"] = new SolidColorBrush(Colors.Black);
                        Application.Current.Resources["ListViewMouseOverBackground"] = new SolidColorBrush(Colors.Gray);
                        Application.Current.Resources["ButtonMouseOverBackground"] = new SolidColorBrush(Colors.LightGray);
                        break;
                    }
                case (TypeColorScheme.dark):
                    {
                        Application.Current.Resources["Background_Inside"] = (SolidColorBrush)(new BrushConverter().ConvertFrom("#18191d"));
                        Application.Current.Resources["Background_Outside"] = (SolidColorBrush)(new BrushConverter().ConvertFrom("#282e33"));
                        Application.Current.Resources["Background_Inside_TextBox"] = (SolidColorBrush)(new BrushConverter().ConvertFrom("#33393f"));
                        Application.Current.Resources["Background_Green"] = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF3A6FD8"));
                        Application.Current.Resources["Background_Border"] = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFD8BB3A"));
                        Application.Current.Resources["Background_Green_Color"] = (Color)ColorConverter.ConvertFromString("#FF3A6FD8");
                        Application.Current.Resources["Foreground"] = new SolidColorBrush(Colors.WhiteSmoke);
                        Application.Current.Resources["Foreground_Button"] = new SolidColorBrush(Colors.LightGray);
                        Application.Current.Resources["Foreground_Dark"] = new SolidColorBrush(Colors.Gray);
                        Application.Current.Resources["ListViewMouseOverBackground"] = (SolidColorBrush)(new BrushConverter().ConvertFrom("#353c43"));
                        Application.Current.Resources["ButtonMouseOverBackground"] = (SolidColorBrush)(new BrushConverter().ConvertFrom("#353c43"));
                        break;
                    }
            }
            var styles = new Uri("pack://application:,,,/Dagorlad;component/Styles/Styles.xaml", UriKind.RelativeOrAbsolute);
            var canvas = new Uri("pack://application:,,,/Dagorlad;component/Styles/Canvases.xaml", UriKind.RelativeOrAbsolute);
            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = styles });
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = canvas });
        }
    }
    class CursorPosition
    {
        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int X;
            public int Y;

            public static implicit operator Point(POINT point)
            {
                return new Point(point.X, point.Y);
            }
        }
        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(out POINT lpPoint);

        public static Point GetCursorPosition()
        {
            POINT lpPoint;
            GetCursorPos(out lpPoint);
            // NOTE: If you need error handling
            // bool success = GetCursorPos(out lpPoint);
            // if (!success)

            return lpPoint;
        }
    }
}
