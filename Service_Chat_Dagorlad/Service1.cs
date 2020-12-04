using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Service_Chat_Dagorlad
{
    public partial class Service1 : ServiceBase
    {
        private ServiceHost service_host;
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                if (service_host != null) service_host.Close();

                // string address_HTTP = "http://localhost:9001/Dagorlad_Chat";
                string address_TCP = "net.tcp://localhost:9002/Dagorlad_Chat";

                // Uri[] address_base = { new Uri(address_HTTP), new Uri(address_TCP) };
                Uri[] address_base = { new Uri(address_TCP) };
                service_host = new ServiceHost(typeof(ChatService), address_base);

                NetTcpBinding binding_tcp = new NetTcpBinding(SecurityMode.Transport, true);

                binding_tcp.MaxBufferPoolSize = Int32.MaxValue;
                binding_tcp.MaxBufferSize = Int32.MaxValue;
                binding_tcp.MaxReceivedMessageSize = Int32.MaxValue;
                binding_tcp.TransferMode = TransferMode.Buffered;
                binding_tcp.ReaderQuotas.MaxArrayLength = 2147483647;
                binding_tcp.ReaderQuotas.MaxBytesPerRead = 2147483647;
                binding_tcp.ReaderQuotas.MaxStringContentLength = 2147483647;

                //binding_tcp.MaxConnections = 1000;

                ServiceThrottlingBehavior throttle;
                throttle = service_host.Description.Behaviors.Find<ServiceThrottlingBehavior>();
                if (throttle == null)
                {
                    throttle = new ServiceThrottlingBehavior();
                    throttle.MaxConcurrentCalls = 1000;
                    throttle.MaxConcurrentSessions = 1000;
                    service_host.Description.Behaviors.Add(throttle);
                }

                binding_tcp.Security.Mode = SecurityMode.Transport;
                binding_tcp.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
                binding_tcp.Security.Message.ClientCredentialType = MessageCredentialType.Windows;
                binding_tcp.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;

                //Enable reliable session and keep the connection alive for 20 hours.
                //binding_tcp.ReceiveTimeout = new TimeSpan(48, 0, 0);
                //binding_tcp.ReliableSession.Enabled = true;
                //binding_tcp.ReliableSession.InactivityTimeout = new TimeSpan(48, 0, 10);

                service_host.AddServiceEndpoint(typeof(IChat), binding_tcp, address_TCP);


                ServiceMetadataBehavior mBehave = new ServiceMetadataBehavior();
                service_host.Description.Behaviors.Add(mBehave);

                service_host.AddServiceEndpoint(typeof(IMetadataExchange), MetadataExchangeBindings.CreateMexTcpBinding(), "mex");
                if (service_host.State != CommunicationState.Opened)
                    service_host.Open();

                service_host.Faulted += Service_host_Faulted;

                File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "logs.txt", String.Format("{0}: Connected\n", DateTime.Now));
            }
            catch (Exception ex)
            {
                LogWrite(ex.ToString());
                Environment.Exit(1);
            }
        }

        private void Service_host_Faulted(object sender, EventArgs e)
        {
            OnStop();
            OnStart(null);
        }

        protected override void OnStop()
        {
            if (service_host != null)
            {
                try
                {
                    service_host.Close();
                }
                catch (Exception ex)
                {
                    LogWrite(ex.ToString());
                }
                service_host.Faulted -= Service_host_Faulted;
                service_host = null;
            }
        }
        private void LogWrite(string text)
        {
            File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "ServiceLog.txt", String.Format("{0}: {1}\n", DateTime.Now, text));
        }
    }
}
