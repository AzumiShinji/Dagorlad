using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Dagorlad_7.Windows
{
    /// <summary>
    /// Логика взаимодействия для ChatWindow.xaml
    /// </summary>
    public partial class ChatWindow : Window,SVC.IChatCallback
    {
        //public class ChatsClass
        //{
        //    public SVC.Client client { get; set; }
        //    public SVC.Message msg { get; set; }
        //}
        SVC.ChatClient proxy = null;
        SVC.Client receiver = null;
        SVC.Client localClient = null;
        ObservableCollection<SVC.Client> Users = new ObservableCollection<SVC.Client>();
        ObservableCollection<SVC.Message> Chats = new ObservableCollection<SVC.Message>();
        public ChatWindow()
        {
            InitializeComponent();
            Start();
            DialogListView.ItemsSource = Chats;
            UsersListView.ItemsSource = Users;
        }

        public void IsWritingCallback(SVC.Client client)
        {
           // throw new NotImplementedException();
        }

        public void Receive(SVC.Message msg)
        {
            Chats.Add(msg);
          //  throw new NotImplementedException();
        }

        public void ReceiverFile(SVC.FileMessage fileMsg, SVC.Client receiver)
        {
            throw new NotImplementedException();
        }

        public void ReceiveWhisper(SVC.Message msg, SVC.Client receiver)
        {
            throw new NotImplementedException();
        }

        public void RefreshClients(SVC.Client[] clients)
        {
            foreach(var s in clients)
            {
                if (!Users.Contains(s))
                    Users.Add(s);
                else continue;
            }
            var list = Users.ToList();
            foreach (var s in list)
                if (!clients.Contains(s))
                    Users.Remove(s);
          //  throw new NotImplementedException();
        }
        string email = String.Format("Email Random: {0}",new Random().Next(0,Int32.MaxValue));
        string name = String.Format("Name Random: {0}", new Random().Next(0, Int32.MaxValue));
        public void Start()
        {
            if (proxy == null)
            {
                localClient = new SVC.Client();
                localClient.Email = email;
                localClient.Name = name;
                localClient.Time = DateTime.Now;
                InstanceContext context = new InstanceContext(this);
                proxy = new SVC.ChatClient(context);
                string servicePath = proxy.Endpoint.ListenUri.AbsolutePath;
                string serviceListenPort = proxy.Endpoint.Address.Uri.Port.ToString();
                proxy.Endpoint.Address = new EndpointAddress("net.tcp://localhost:" + serviceListenPort + servicePath);
                proxy.Open();
                proxy.ConnectAsync(localClient);
            }
        }

        public void UserJoin(SVC.Client client)
        {
          //  throw new NotImplementedException();
        }

        public void UserLeave(SVC.Client client)
        {
            throw new NotImplementedException();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            var text = textbox.Text;
            SVC.Message msg = new SVC.Message();
            msg.Sender = localClient.Email;
            msg.Content = text;
            //if ((bool)chatCheckBoxWhisper.IsChecked)
            //{
            //    if (this.receiver != null)
            //    {
            //        proxy.WhisperAsync(msg, this.receiver);
            //        chatTxtBoxType.Text = "";
            //        chatTxtBoxType.Focus();
            //    }
            //}

            //else
            //{
                proxy.SayAsync(msg);
                
            //}
            proxy.IsWritingAsync(null);

        }
    }
}
