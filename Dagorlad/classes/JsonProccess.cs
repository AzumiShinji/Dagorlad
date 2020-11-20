using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dagorlad
{
    class JsonProccess
    {
        public class _Settings
        {
            public List<CustomExample.CustomNameSpace> Examples { get; set; }
        }
        public static string JsonFileName = System.AppDomain.CurrentDomain.BaseDirectory + @"CustomExample_" +Environment.UserName+".json";
        static public bool WriteSettings(List<CustomExample.CustomNameSpace> _set)
        {
            try
            {
                _Settings setting = new _Settings
                {
                    Examples = _set
                };
                using (StreamWriter file = File.CreateText(JsonFileName))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(file, setting);
                }
                return true;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Error); return false; }
        }

        static public List<CustomExample.CustomNameSpace> ReadSettings()
        {
            try
            {
                if (!File.Exists(JsonFileName)) File.WriteAllText(JsonFileName, "{" + '\u0022' + "Examples" + '\u0022' + ":[]}");
                using (StreamReader file = File.OpenText(JsonFileName))
                {
                    return ((_Settings)new JsonSerializer().Deserialize(file, typeof(_Settings))).Examples;
                }
            }
            catch(Exception ex) { MessageBox.Show(ex.Message,ex.Source,MessageBoxButtons.OK,MessageBoxIcon.Error); return null; }
        }
    }
}
