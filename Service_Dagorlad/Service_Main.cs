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

namespace Service_Dagorlad
{
    public partial class Service_Main : ServiceBase
    {
        private ServiceHost service_host = null;
        public Service_Main()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + @"logs"))
                    Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + @"logs");
                if (service_host != null) service_host.Close();

               // string address_HTTP = "http://localhost:9001/Dagorlad";
                string address_TCP = "net.tcp://localhost:9002/Dagorlad";

                // Uri[] address_base = { new Uri(address_HTTP), new Uri(address_TCP) };
                Uri[] address_base = { new Uri(address_TCP) };
                service_host = new ServiceHost(typeof(Main_Class), address_base);

                ServiceMetadataBehavior behavior = new ServiceMetadataBehavior();
                service_host.Description.Behaviors.Add(behavior);

                //BasicHttpBinding binding_http = new BasicHttpBinding();
                //service_host.AddServiceEndpoint(typeof(IService_Dagorlad), binding_http, address_HTTP);
                //service_host.AddServiceEndpoint(typeof(IMetadataExchange), MetadataExchangeBindings.CreateMexHttpBinding(), "mex");

                NetTcpBinding binding_tcp = new NetTcpBinding();
                binding_tcp.Security.Mode = SecurityMode.Transport;
                binding_tcp.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
                binding_tcp.Security.Message.ClientCredentialType = MessageCredentialType.Windows;
                binding_tcp.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;
                service_host.AddServiceEndpoint(typeof(IService_Dagorlad), binding_tcp, address_TCP);
                service_host.AddServiceEndpoint(typeof(IMetadataExchange), MetadataExchangeBindings.CreateMexTcpBinding(), "mex");

                service_host.Open();
            }
            catch (Exception ex) { File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory+"logs.txt",ex.ToString()+"\n"); Environment.Exit(1); }
        }

        protected override void OnStop()
        {
            if (service_host != null)
            {
                service_host.Close();
                service_host = null;
            }
        }
    }
}
