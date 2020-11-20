using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Dagorlad
{
    class Access
    {
        class ListMacUser
        {
            public string mac { get; set; }
            public string user { get; set; }
        }

        private static List<ListMacUser> GetMacAddressAndUser()
        {
            var ListOfmacs = new List<ListMacUser>();
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
                ListOfmacs.Add(new ListMacUser { mac = nic.GetPhysicalAddress().ToString().Trim(), user = Environment.UserName });
            return ListOfmacs;
        }

        private static List<ListMacUser> ListOfAllowMacAdress()
        {
            var ListMacs = new List<ListMacUser>();
            ListMacs.Add( new ListMacUser
            {
                mac= "2C56DC2CB9C3",
                user = "krislechy"
            });
            ListMacs.Add(new ListMacUser
            {
                mac = "1C1B0DFEB933",
                user = "Admin",
            });
            ListMacs.Add(new ListMacUser
            {
                mac = "1C1B0DFEB928",
                user = "PyatkoB",
            });
            ListMacs.Add(new ListMacUser
            {
                mac = "1C1B0DFEB8C8",
                user = "Ефимов МА",
            });
            ListMacs.Add(new ListMacUser
            {
                mac = "1C1B0DFEB75C",
                user = "OsipovVA",
            });
            ListMacs.Add(new ListMacUser
            {
                mac = "1C1B0DFEB8C8",
                user = "PiskunovaVV",
            });
            ListMacs.Add(new ListMacUser
            {
                mac = "1C1B0DFEB928",
                user = "SomovK",
            });
            return ListMacs;
        }

        public static void CheckAllow()
        {
            bool Access = false;
            foreach (var m in GetMacAddressAndUser())
                foreach (var am in ListOfAllowMacAdress())
                {
                    if (m.mac == am.mac && m.user == am.user)
                    {
                        Access = true;
                        break;
                    }
                }

            if (!Access)
            {
                MessageBox.Show("Пользователь не имеет доступа, программа будет закрыта", "Нет доступа", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                Process.GetCurrentProcess().Kill();
            }
        }
    }
}
