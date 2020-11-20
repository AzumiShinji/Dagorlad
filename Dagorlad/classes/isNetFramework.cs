using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Dagorlad
{
    class isNetFramework
    {
        private static Version Get45PlusFromRegistry()
        {
            const string subkey = @"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\";
            using (RegistryKey ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(subkey))
            if (ndpKey != null && ndpKey.GetValue("Release") != null)
                return CheckFor45PlusVersion((int)ndpKey.GetValue("Release"));
            else
                return null;
        }

        private static Version CheckFor45PlusVersion(int releaseKey)
        {
            if (releaseKey >= 461808)
                return new Version("4.7.2");
            if (releaseKey >= 461308)
                return new Version("4.7.1");
            if (releaseKey >= 460798)
                return new Version("4.7");
            if (releaseKey >= 394802)
                return new Version("4.6.2");
            if (releaseKey >= 394254)
                return new Version("4.6.1");
            if (releaseKey >= 393295)
                return new Version("4.6");
            if (releaseKey >= 379893)
                return new Version("4.5.2");
            if (releaseKey >= 378675)
                return new Version("4.5.1");
            if (releaseKey >= 378389)
                return new Version("4.5");
            return null;
        }

        public static bool GetDotNet()
        {
            if (Get45PlusFromRegistry() < new Version("4.7.1"))
            {
                switch (MessageBox.Show("Не установлен пакет .NET Framework 4.7.1 необходимый для стабильной " +
                    "работы Dagorlad!\nДа - Перейти в локальную папку для установки\nНет - Продолжить и запустить\nОтмена - Выйти из программы.", ".NET Framework 4.7.1",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation))
                {
                    case DialogResult.Yes:
                        Process.Start("explorer.exe", Update.PathUpdate.Replace('/', '\u005c') +@"install\");
                        Process.GetCurrentProcess().Kill();
                        return false;
                    case DialogResult.Cancel:
                        Process.GetCurrentProcess().Kill();
                        return false;
                    case DialogResult.No:
                        return true;
                }
            }
            return true;
        }
    }
}
