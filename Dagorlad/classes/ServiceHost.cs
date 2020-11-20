using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using System.Windows.Media;
using System.Windows.Threading;

namespace Dagorlad.classes
{
    class ServiceHost
    {
        public static Service_DagorladClient client = null;
        public Service_Dagorlad.Info new_info = new Service_Dagorlad.Info
        {
            enter_dt = DateTime.Now,
            username = Environment.UserName,
            ip = MainWindow.IP[0],
        };
        public async Task Create_New_Client()
        {
            await Task.Factory.StartNew(new Action(() =>
            {
                if (client == null)
                    try { Try_To_Create_New_Client(); }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        Console.WriteLine(ex.InnerException);
                        client = null;
                    }
                else
                {
                    Console.WriteLine("Cannot create a new client. The current Client is active.");
                }
            }), TaskCreationOptions.HideScheduler);
        }
        public static Service_Dagorlad.Info[] list = null;
        private void Try_To_Create_New_Client()
        {
            try
            {
                NetTcpBinding binding = new NetTcpBinding(SecurityMode.Transport);

                binding.Security.Message.ClientCredentialType = MessageCredentialType.Windows;
                binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
                binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;
                binding.TransferMode = TransferMode.Buffered;
                
#if(DEBUG)
                string uri = "net.tcp://"+MainWindow.IP[0]+":9002/Dagorlad";
#else
                string uri = "net.tcp://webservice:9002/Dagorlad";
#endif
                EndpointAddress endpoint = new EndpointAddress(new Uri(uri));

                client = new Service_DagorladClient(binding, endpoint);

                client.ClientCredentials.Windows.ClientCredential.Domain = "";
#if (DEBUG)
                client.ClientCredentials.Windows.ClientCredential.UserName = "krislechy";
                client.ClientCredentials.Windows.ClientCredential.Password = "SeriX45*";
#else
                client.ClientCredentials.Windows.ClientCredential.UserName = "sql";
                client.ClientCredentials.Windows.ClientCredential.Password = "4815162342";
#endif

                if (TestConnection())
                    Console.WriteLine("Connection successfully");
                else
                    Console.WriteLine("Connection deined");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.WriteLine(ex.InnerException);
                client = null;
            }
        }
        public static async void AddMessage(Service_Dagorlad.Conversations conversations)
        {
            try
            {
                if (client != null)
                {
                    await client.AddMessageAsync(conversations);
                }
            }
            catch { }
        }
        public static async Task<List<Service_Dagorlad.Conversations>> return_Conversation(string from,string to,string group)
        {
            try
            {
                if (client != null)
                {
                    var dlg = await client.return_ConversationAsync(from,to,group);
                    if (dlg != null)
                    {
                        List<Service_Dagorlad.Conversations> list = dlg.ToList();
                        return list;
                    }
                    else return null;
                }
            }
            catch { return null; }
            return null;
        }
        public static async Task<List<Service_Dagorlad.Conversations>> return_Conversation_Unread(string from, string to,string group)
        {
            try
            {
                if (client != null)
                {
                    var unreads = await client.return_Conversation_UnreadedAsync(from, to,group);
                    if (unreads != null)
                    {
                        List<Service_Dagorlad.Conversations> list = unreads.ToList();
                        return list;
                    }
                    else return null;
                }
            }
            catch { return null; }
            return null;
        }
        public static async Task Set_Conversation_Readed(string from, string to,string group)
        {
            try
            {
                if (client != null)
                {
                    await client.Set_Conversation_ReadedAsync(from, to,group);
                }
            }
            catch { }
        }
        public async void Update()
        {
            try
            {
                if (client != null)
                {
                    var result = client.Update(new_info);
                    if (result != null && result.Count() > 0)
                        list = result;
                }
            }
            catch { client = null;await Create_New_Client(); }
        }
        public bool TestConnection()
        {
            if (client.TestConnection())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void Close()
        {
            if (client != null)
            {
                Console.WriteLine("Closing a client ...");
                client.Close();
                client = null;
            }
            else
            {
                Console.WriteLine("Error! Client does not exist!");
            }
            this.Close();
        }
    }
}
