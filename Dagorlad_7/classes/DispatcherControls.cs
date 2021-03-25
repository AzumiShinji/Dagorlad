using Dagorlad_7.Pages;
using Dagorlad_7.Windows;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace Dagorlad_7.classes
{
    public enum TypeImageNotify
    {
        standart = 0,
        chat = 1,
        buildings = 2,
        sad = 3,
        completed=4,
        update =5,
        number_handling=6,
    }
    public enum TypeColorScheme
    {
        dark = 0,
        light = 1,
    }

    public sealed class BitmapImageClassBackgroundChatClass : INotifyPropertyChanged
    {
        private static readonly BitmapImageClassBackgroundChatClass instance = new BitmapImageClassBackgroundChatClass();
        private BitmapImageClassBackgroundChatClass() { }
        public static BitmapImageClassBackgroundChatClass Instance
        {
            get
            {
                return instance;
            }
        }

        private BitmapImage _BitmapImage = null;
        public BitmapImage BitmapImage
        {
            get { return this._BitmapImage; }

            set
            {
                if (value != this._BitmapImage)
                {
                    this._BitmapImage = value;
                    NotifyPropertyChanged("BitmapImage");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    public class DispatcherControls
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
            MiniMenuWindow mm = null;
            foreach (var window in App.Current.Windows)
            {
                if (window.GetType() == typeof(SmartMenuWindow))
                {
                    var wnd = ((SmartMenuWindow)window);
                    if (wnd.IsLoaded)
                    {
                        if (wnd.Visibility == Visibility.Visible)
                        {
                            wnd.Visibility = Visibility.Hidden;
                            return;
                        }
                        else wnd.Visibility = Visibility.Visible;
                        if (wnd.IsActive)
                            return;
                        cmc = wnd;
                        IsExistWindow = true;
                        break;
                    }
                }
                if (window.GetType() == typeof(MiniMenuWindow))
                    mm = (MiniMenuWindow)window;
            }
            if (!IsExistWindow)
            {
                cmc = new SmartMenuWindow();
                cmc.TemporaryNumberOfHandlingLabel.Content = DispatcherControls.LastNumberOfHandling;
            }
            cmc.Topmost = true;
            cmc.WindowStartupLocation = WindowStartupLocation.Manual;
            //Point mousePositionInApp = CursorPosition.GetCursorPosition();
            //cmc.Top = mousePositionInApp.Y;
            //cmc.Left = mousePositionInApp.X;
            cmc.Top = mm.Top+mm.ActualHeight;
            cmc.Left= mm.Left==0? mm.Left + mm.ActualWidth: mm.Left - cmc.ActualWidth;
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
        public static MiniMenuWindow minimenu;
        public static void ShowMiniMenu()
        {
            if (MySettings.Settings.SmartMenuIsEnabled)
            {
                if (minimenu == null)
                {
                    minimenu = new MiniMenuWindow();
                    minimenu.Show();
                }
            }
            else
            {
                if (minimenu != null)
                {
                    minimenu.Closing -= (qq, ee) => { ee.Cancel = true; };
                    minimenu.Closing += (qq, ee) => { ee.Cancel = false; };
                    minimenu.Close();
                    minimenu = null;
                }
            }
        }
        public static System.Windows.Forms.NotifyIcon Install_Notify_Main;
        public static System.Windows.Forms.NotifyIcon Install_Notify_Dialog;
        public static void HideWindowToTaskMenu(Window win,string name)
        {
            if (win.GetType() == typeof(MainWindow))
            {
                Install_Notify_Main = new System.Windows.Forms.NotifyIcon();
                Install_Notify_Main.Icon = System.Drawing.Icon.ExtractAssociatedIcon(System.Reflection.Assembly.GetExecutingAssembly().Location);
                Install_Notify_Main.Text = Assembly.GetExecutingAssembly().GetName().Name;
                Install_Notify_Main.Visible = true;
                Install_Notify_Main.Click +=
                    (object sender, EventArgs args) =>
                    {
                        win.Show();
                        win.WindowState = WindowState.Normal;
                        win.Activate();
                    };
            }
        }
        public static void CloseAllNotifyIcon()
        {
            if (Install_Notify_Main != null)
            {
                Install_Notify_Main.Visible = false;
                Install_Notify_Main.Icon = null;
                Install_Notify_Main.Dispose();
            }
            if (Install_Notify_Dialog != null)
            {
                Install_Notify_Dialog.Visible = false;
                Install_Notify_Dialog.Icon = null;
                Install_Notify_Dialog.Dispose();
            }
        }
        public static bool Autorun(TypeAutoRunOperation operation)
        {
            try
            {
                bool IsExistTask = false;
                string application_name = Assembly.GetExecutingAssembly().GetName().Name;
                using (Microsoft.Win32.TaskScheduler.TaskService ts = new Microsoft.Win32.TaskScheduler.TaskService())
                {
                    IsExistTask = ts.FindTask(application_name, true) == null ? false : true;
                }
                switch (operation)
                {
                    case (TypeAutoRunOperation.On):
                        {
                            using (Microsoft.Win32.TaskScheduler.TaskService ts = new Microsoft.Win32.TaskScheduler.TaskService())
                            {
                                var userid = System.Security.Principal.WindowsIdentity.GetCurrent().User.Value;
                                var xml = Properties.Resources.startup
                                    .Replace("%name%", application_name)
                                    .Replace("%userid%", userid)
                                    .Replace("%user%", Environment.MachineName + @"\" + Environment.UserName)
                                    .Replace("%path%", AppDomain.CurrentDomain.BaseDirectory + application_name + ".exe")
                                    .Replace("%path_wd%", AppDomain.CurrentDomain.BaseDirectory);
                                var new_task = ts.NewTask();
                                new_task.XmlText = xml;
                                ts.RootFolder.RegisterTaskDefinition(
                                    application_name,
                                    new_task,
                                    Microsoft.Win32.TaskScheduler.TaskCreation.CreateOrUpdate,
                                    userid,
                                    null,
                                    Microsoft.Win32.TaskScheduler.TaskLogonType.InteractiveToken);
                            }
                            return true;
                        }
                    case (TypeAutoRunOperation.Off):
                        {
                            if (IsExistTask)
                                using (Microsoft.Win32.TaskScheduler.TaskService ts = new Microsoft.Win32.TaskScheduler.TaskService())
                                {
                                    ts.RootFolder.DeleteTask(application_name);
                                }
                            return false;
                        }
                    case (TypeAutoRunOperation.CheckStatus):
                        {
                            return IsExistTask;
                        }
                }
                return IsExistTask;
            }
            catch (Exception ex){ Logger.Write(Logger.TypeLogs.main,ex.ToString()); return false; }
        }
        public static MyDialogWindow.ResultMyDialog ShowMyDialog(string title,string text, MyDialogWindow.TypeMyDialog type,Window win)
        {
            var g = new MyDialogWindow(title,text,type);
            if(win != null && win.IsLoaded)
            g.Owner = win;
            var result = g.ShowDialog();
            Console.WriteLine("Dialog closed: {0}",g.DialogResult);
            if (result.HasValue && result.Value==true)
            {
                Console.WriteLine("Dialog result: {0}", g.result);
                return g.result;
            }
            return MyDialogWindow.ResultMyDialog.Cancel;
        }
        public static ObservableCollection<MyNotifyClass> MyNotifyList = new ObservableCollection<MyNotifyClass>();
        public static MyNotifyWindow _MyNotifyWindow;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="title">Title</param>
        /// <param name="text">Text</param>
        /// <param name="closeafterseconds">After what notify will be closed, TimeSpan</param>
        /// <param name="win">initiator window, NULL</param>
        /// <param name="image">TypeImageNotify</param>
        /// <param name="LinkFocusObject">Link to what should be focused, NULL</param>
        public static void NewMyNotifyWindow(string title, string text, TimeSpan closeafterseconds, Window win, object image,object LinkFocusObject)
        {
            var obj = new MyNotifyClass()
            {
                dt = DateTime.Now,
                title = title,
                text = text,
                image = SetImageNotify(image),
                timetoautoclose = closeafterseconds,
            };
            MyNotifyList.Add(obj);
            if (_MyNotifyWindow == null)
            {
                _MyNotifyWindow = new MyNotifyWindow(win, LinkFocusObject);
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
                    case (TypeImageNotify.sad):
                        {
                            var SourceUri = new Uri("pack://application:,,,/Dagorlad;component/images/sad_64.png", UriKind.Absolute);
                            var thisIcon = new BitmapImage(SourceUri);
                            return thisIcon;
                        }
                    case (TypeImageNotify.completed):
                        {
                            var SourceUri = new Uri("pack://application:,,,/Dagorlad;component/images/completed_64.png", UriKind.Absolute);
                            var thisIcon = new BitmapImage(SourceUri);
                            return thisIcon;
                        }
                    case (TypeImageNotify.update):
                        {
                            var SourceUri = new Uri("pack://application:,,,/Dagorlad;component/images/update_64.png", UriKind.Absolute);
                            var thisIcon = new BitmapImage(SourceUri);
                            return thisIcon;
                        }
                    case (TypeImageNotify.number_handling):
                        {
                            var SourceUri = new Uri("pack://application:,,,/Dagorlad;component/images/number_handling_64.png", UriKind.Absolute);
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
        public class EmployeesClass
        {
            public string Code { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public string Direction { get; set; }
            public string Position { get; set; }
            public string Phone { get; set; }
            public DateTime BirthDate { get; set; }
            public string Rest { get; set; }
        }
        public static async Task<EmployeesClass> FindEmployees(string Email)
        {
            EmployeesClass employees =null;
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
                            DateTime? birthdate = null;
                            var string_dt = reader["BirthDate"] == DBNull.Value ? "" : (string)reader["BirthDate"];
                            if(!String.IsNullOrEmpty(string_dt))
                            {
                                var isdt = DateTime.TryParse(string_dt, out DateTime result);
                                if (isdt)
                                    birthdate = result;
                            }
                            string rest = null;
                            var _Rest = reader["Rest"] == DBNull.Value ? "" : (string)reader["Rest"];
                            if(!String.IsNullOrEmpty(_Rest))
                            {
                                rest = "\n"+String.Join("\n", _Rest.Split('|'));
                            }
                            employees = new EmployeesClass
                            {
                                Code = reader["Code"] == DBNull.Value ? "" : (string)reader["Code"],
                                Name = reader["FIO"] == DBNull.Value ? "" : (string)reader["FIO"],
                                Email = reader["Email"] == DBNull.Value ? "" : (string)reader["Email"],
                                Direction = reader["Direction"] == DBNull.Value ? "" : (string)reader["Direction"],
                                Position = reader["Position"] == DBNull.Value ? "" : (string)reader["Position"],
                                Phone = reader["Phone"] == DBNull.Value ? "" : (string)reader["Phone"],
                                BirthDate = birthdate.Value,
                                Rest = rest,
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
            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory+ "Dagorlad.exe.config", "<?xml version=\"1.0\" encoding=\"utf-8\"?> <configuration> <startup> <supportedRuntime version=\"v4.0\" sku=\".NETFramework,Version=v4.7.1\"/> </startup> <system.serviceModel> <bindings> <netTcpBinding> <binding name=\"NetTcpBinding_IChat\"> <reliableSession inactivityTimeout=\"20:00:10\" enabled=\"true\" /> <security> <transport sslProtocols=\"None\" /> </security> </binding> </netTcpBinding> </bindings> <client> <endpoint address=\"net.tcp://webservice:9002/Dagorlad_Chat\" binding=\"netTcpBinding\" bindingConfiguration=\"NetTcpBinding_IChat\" contract=\"SVC.IChat\" name=\"NetTcpBinding_IChat\" /> </client> </system.serviceModel> </configuration>");
        }

        public static void SetSchemeColor(TypeColorScheme type,bool IsFirstExecute)
        {
            switch(type)
            {
                case (TypeColorScheme.light):
                    {
                        Application.Current.Resources["Background.Inside"] = new SolidColorBrush(Colors.White);
                        Application.Current.Resources["Background.Inside.Blob"] = new SolidColorBrush(Colors.LightGray);
                        Application.Current.Resources["Background.Outside"] = new SolidColorBrush(Colors.WhiteSmoke);
                        Application.Current.Resources["Background.HalfOutside"] = new SolidColorBrush(Colors.GhostWhite);
                        Application.Current.Resources["Background.Highlight"] = (SolidColorBrush)(new BrushConverter().ConvertFrom("#009687"));
                        Application.Current.Resources["Background.Highlight.Color"] = (Color)ColorConverter.ConvertFromString("#009687");
                        Application.Current.Resources["Background.MouseOver"] = new SolidColorBrush(Colors.Gray);
                        Application.Current.Resources["Foreground"] = new SolidColorBrush(Colors.Black);
                        Application.Current.Resources["Foreground.History"] = new SolidColorBrush(Colors.DimGray);
                        Application.Current.Resources["Foreground.Pressed"] = new SolidColorBrush(Colors.White);
                        break;
                    }
                case (TypeColorScheme.dark):
                    {
                        if (IsFirstExecute) return;
                        Application.Current.Resources["Background.Inside"] = (SolidColorBrush)(new BrushConverter().ConvertFrom("#18191d"));
                        Application.Current.Resources["Background.Inside.Blob"] = (SolidColorBrush)(new BrushConverter().ConvertFrom("#33393f"));
                        Application.Current.Resources["Background.Outside"] = (SolidColorBrush)(new BrushConverter().ConvertFrom("#282e33"));
                        Application.Current.Resources["Background.HalfOutside"] = (SolidColorBrush)(new BrushConverter().ConvertFrom("#20262b"));
                        Application.Current.Resources["Background.Highlight"] = (SolidColorBrush)(new BrushConverter().ConvertFrom("#009687"));
                        Application.Current.Resources["Background.Highlight.Color"] = (Color)ColorConverter.ConvertFromString("#009687");
                        Application.Current.Resources["Background.MouseOver"] = (SolidColorBrush)(new BrushConverter().ConvertFrom("#43474d"));
                        Application.Current.Resources["Foreground"] = (SolidColorBrush)(new BrushConverter().ConvertFrom("#f5f5f5"));
                        Application.Current.Resources["Foreground.History"] = (SolidColorBrush)(new BrushConverter().ConvertFrom("#8d939e"));
                        Application.Current.Resources["Foreground.Pressed"] = (SolidColorBrush)(new BrushConverter().ConvertFrom("#313b43"));
                        break;
                    }
            }
            var styles = new Uri("pack://application:,,,/Dagorlad;component/Styles/Styles.xaml", UriKind.RelativeOrAbsolute);
            var canvas = new Uri("pack://application:,,,/Dagorlad;component/Styles/Canvases.xaml", UriKind.RelativeOrAbsolute);
            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = styles });
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = canvas });
        }
        public static Task ClearDirectory(string dir)
        {
            if (String.IsNullOrEmpty(dir)) return Task.CompletedTask;
            bool _error = false;
            try
            {
                foreach (FileInfo d in (new DirectoryInfo(@dir).GetFiles()))
                {
                    d.Delete();
                }

                foreach (DirectoryInfo d in (new DirectoryInfo(@dir).GetDirectories()))
                {
                    d.Delete(true);
                }
            }
            catch (Exception g)
            {
                _error = true;
                NewMyNotifyWindow("Не удалось очистить директорию", g.Message, TimeSpan.FromSeconds(5), Application.Current.MainWindow, TypeImageNotify.sad,null);
            }
            finally
            {
                if (!_error)
                    NewMyNotifyWindow("Директория очищена", dir, TimeSpan.FromSeconds(5), Application.Current.MainWindow, TypeImageNotify.completed,null);
            }
            return Task.CompletedTask;
        }
        public static string LastNumberOfHandling = null;
        public static string GetMySystemInformation()
        {
            var username = Environment.UserName;
            var MachineName = Environment.MachineName;
            var os = Environment.OSVersion;
            var userdomainname = Environment.UserDomainName;
            var versionapp = GetVersionApplication(TypeDisplayVersion.Fully);
            var result = String.Format(
                "UserName: {0}\n"+
                "MachineName: {1}\n" +
                "OS: {2}\n" +
                "UserDomainName: {3}\n"+
                "Version Application: {4}\n",
                username,
                MachineName,
                os,
                userdomainname,
                versionapp
                );
            return result;
        }

        public static void SetBackgroundDialog(bool IsTransparent)
        {

            if (IsTransparent)
            {
                BitmapImageClassBackgroundChatClass.Instance.BitmapImage = null;
                return;
            }
            switch (MySettings.Settings.TypeColorScheme)
            {
                case (TypeColorScheme.dark):
                    {

                        BitmapImageClassBackgroundChatClass.Instance.BitmapImage = new BitmapImage(new Uri("pack://application:,,,/Dagorlad;component/background/nautre_dark.jpg", UriKind.Absolute));
                        break;
                    }
                case (TypeColorScheme.light):
                    {
                        BitmapImageClassBackgroundChatClass.Instance.BitmapImage = new BitmapImage(new Uri("pack://application:,,,/Dagorlad;component/background/nature_light.jpg", UriKind.Absolute));
                        break;
                    }
            }
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
