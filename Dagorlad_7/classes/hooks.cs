using Dagorlad_7.Windows;
using Gma.System.MouseKeyHook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Dagorlad_7.classes
{
    class hooks
    {
        private static IKeyboardMouseEvents m_Events;
        public static Task StartMouseHook()
        {
            Unsubscribe();
            Subscribe(Hook.GlobalEvents());
            return Task.CompletedTask;
        }
        private static void Subscribe(IKeyboardMouseEvents events)
        {
            m_Events = events;
            m_Events.MouseDownExt += GlobalHookMouseDownExt;
        }
        private static void Unsubscribe()
        {
            if (m_Events == null) return;
            m_Events.MouseDownExt -= GlobalHookMouseDownExt;
            m_Events.Dispose();
            m_Events = null;
        }
        private static void GlobalHookMouseDownExt(object sender, MouseEventExtArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                bool IsExistWindow = false;
                ContextMenuCustom cmc=null;
                foreach (var window in App.Current.Windows)
                    if(window.GetType()==typeof(ContextMenuCustom))
                        if(((ContextMenuCustom)window).IsLoaded)
                        {
                            cmc = ((ContextMenuCustom)window);
                            IsExistWindow = true;
                            break;
                        }
                if (!IsExistWindow)
                    cmc = new ContextMenuCustom();
                cmc.Topmost = true;
                cmc.WindowStartupLocation = WindowStartupLocation.Manual;
                Point mousePositionInApp = cursor_position.GetCursorPosition();
                cmc.Top = mousePositionInApp.Y;
                cmc.Left = mousePositionInApp.X;
                cmc.Show();
            }
        }
    }
}
