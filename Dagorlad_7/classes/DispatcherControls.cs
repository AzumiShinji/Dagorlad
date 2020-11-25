using Dagorlad_7.Windows;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using static Dagorlad_7.Windows.MyDialogWindow;

namespace Dagorlad_7.classes
{
    class DispatcherControls
    {
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
        public static void HideAppToTaskMenu()
        {
            System.Windows.Forms.NotifyIcon Install_Notify = new System.Windows.Forms.NotifyIcon();
            Install_Notify.Icon = System.Drawing.Icon.ExtractAssociatedIcon(System.Reflection.Assembly.GetExecutingAssembly().Location);
            Install_Notify.Visible = true;
            Install_Notify.Text = Assembly.GetExecutingAssembly().GetName().Name;
            Install_Notify.Click +=
                (object sender, EventArgs args) =>
                {
                    Application.Current.MainWindow.Show();
                    Application.Current.MainWindow.WindowState = WindowState.Normal;
                    Application.Current.MainWindow.Activate();
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
                }
            }
            else
            {
                return type as BitmapImage;
            }
            return null;
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
