using Dagorlad_7.Pages;
using Dagorlad_7.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Threading;

namespace Dagorlad_7.classes
{
    class GlobalHook
    {
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;
        private static LowLevelKeyboardProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;

        public static void StartHooking()
        {
            _hookID = SetHook(_proc);
        }
        public static void StopHooking()
        {
            UnhookWindowsHookEx(_hookID);
        }

        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }
        }
        private static readonly List<int> KeysDown = new List<int>();
        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                if(wParam==(IntPtr)WM_KEYUP)
                    KeysDown.Clear();
                if (wParam == (IntPtr)WM_KEYDOWN)
                {
                    if (vkCode == (int)Keys.Pause)
                    {
                        DispatcherControls.ClearDirectory(MySettings.Settings.ClearingFolder);
                        Console.WriteLine("ClearingFolder has been started");
                        KeysDown.Clear();
                    }
                    KeysDown.Add(vkCode);
                    if ((KeysDown.Contains((int)Keys.LWin) || KeysDown.Contains((int)Keys.RWin)) &&
                     (KeysDown.Contains((int)Keys.LShiftKey) || KeysDown.Contains((int)Keys.RShiftKey)) && KeysDown.Contains((int)Keys.A))
                    {
                        Console.WriteLine("win shift a");
                        DispatcherControls.ShowSmartMenu();
                        KeysDown.Clear();
                    }
                }
            }

            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
    }
}
