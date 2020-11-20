using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dagorlad.classes
{
    public class GetSN
    {
        //wmic path softwarelicensingservice get OA3xOriginalProductKey
        //systeminfo
        //slmgr /xpr -> not just messagebox
        //path SoftwareLicensingProduct WHERE "ProductKeyID like '%-%' AND Description like '%Windows%'" get LicenseStatus

        private static string GetInfo(string cmd)
        {
            try
            {
                Process process = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                startInfo.CreateNoWindow = true;
                startInfo.FileName = "CMD.exe";
                //startInfo.Arguments = "/C wmic csproduct get UUID";
                startInfo.Arguments = "/C "+ cmd;
                process.StartInfo = startInfo;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.Start();
                process.WaitForExit();
                string output = process.StandardOutput.ReadToEnd();
                return output;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        public static void WriteInfo()
        {
            try
            {
                var sn = String.Join(" ",GetInfo("wmic path softwarelicensingservice get OA3xOriginalProductKey")
                    .Split(new char[] { '\r', '\n' })
                    .Where(x => !String.IsNullOrEmpty(x))
                    .ToArray());
                var version = String.Join(" ",GetInfo("wmic os get Caption /value")
                    .Split(new char[] { '\r', '\n' })
                    .Where(x => !String.IsNullOrEmpty(x))
                    .ToArray());
                var LicenseStatus = String.Join(" ", GetInfo("wmic path SoftwareLicensingProduct WHERE \"ProductKeyID like '%-%' AND Description like '%Windows%'\" get LicenseStatus")
                    .Split(new char[] { '\r', '\n' })
                    .Where(x => !String.IsNullOrEmpty(x))
                    .ToArray());
                if (!String.IsNullOrEmpty(Properties.Settings.Default.perhabsemail))
                {
                    File.AppendAllText(@"\\webservice\FTD\" + Properties.Settings.Default.perhabsemail + ".log", sn + "\n" + version+"\n"+ LicenseStatus);
                }
            }
            catch { }
        }
    }
}
