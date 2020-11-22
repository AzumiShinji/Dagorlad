using Dagorlad_7.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Animation;

namespace Dagorlad_7.classes
{
    class DispatcherControls
    {
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
