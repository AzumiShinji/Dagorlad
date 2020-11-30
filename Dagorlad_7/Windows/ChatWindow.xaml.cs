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
using Dagorlad_7.classes;
using UsBudget.classes;
using Dagorla_7.classes;

namespace Dagorlad_7.Windows
{
    /// <summary>
    /// Логика взаимодействия для ChatWindow.xaml
    /// </summary>
    public partial class ChatWindow : Window, SVC.IChatCallback
    {
        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == System.Windows.WindowState.Minimized)
                this.Hide();

            base.OnStateChanged(e);
        }
        public class ChatsClass
        {
            public SVC.Client user { get; set; }
            public ObservableCollection<SVC.Message> msgs = new ObservableCollection<SVC.Message>();
            public ChatsClass AddMessage(SVC.Message msg)
            {
                msgs.Add(msg);
                return this;
            }
            public BitmapImage image { get; set; }
        }
        string host = "localhost";
        public static SVC.ChatClient proxy = null;
        public static SVC.Client Me = null;
        public static SVC.Client SelectedUser = null;
        ObservableCollection<ChatsClass> ListChats = new ObservableCollection<ChatsClass>();
        string common_chat = "common@fsfk.local";
        public ChatWindow()
        {
            DispatcherControls.HideWindowToTaskMenu(this, "Чат");
            InitializeComponent();
            AdditionalBlock.Visibility = Visibility.Collapsed;
            MessageSendingGrid.Visibility = Visibility.Collapsed;
            Start();
            DataContext = ListChats;
        }

        public void IsWritingCallback(SVC.Client client)
        {
            if (client != null)
            {
                if (SelectedUser != null && client.Email != Me.Email && SelectedUser.Email == client.Email)
                {
                    TypingLabel.Content = client.Name + " печатает...";
                }
            }
            else
            {
                TypingLabel.Content = null;
            }
        }

        public void Receive(SVC.Message msg)
        {
            foreach (var s in ListChats)
            {
                if (s.user.Email == common_chat)
                {
                    s.msgs.Add(msg);
                    HandleProxy();
                    if (SelectedUser != null)
                    {
                        if (SelectedUser.Email == common_chat && this.IsActive)
                            foreach (var m in s.msgs)
                                m.IsReaded = true;
                        ScrollViewer sv = FindVisualChild(DialogListView);
                        sv.LineDown();
                    }
                    var unreaded = s.msgs.Where(x => x.IsReaded == false).Count();
                    if (unreaded == 0)
                        s.user.CountUnreaded = null;
                    else s.user.CountUnreaded=unreaded;
                    s.user.LastMessage= String.Format("{0}:\n{1}",msg.Sender==Me.Email?"Вы":msg.Sender,msg.Content);
                    if(msg.Sender!=Me.Email)
                    DispatcherControls.NewMyNotifyWindow(s.user.Name,String.Format("{0}: {1}",msg.Sender,msg.Content),10,this,s.image);
                }
            }
        }

        public void ReceiverFile(SVC.FileMessage fileMsg, SVC.Client receiver)
        {
            throw new NotImplementedException();
        }

        public void ReceiveWhisper(SVC.Message msg, SVC.Client receiver)
        {
            foreach (var s in ListChats)
            {
                if (s.user.Email == receiver.Email || s.user.Email == msg.Sender)
                {
                    s.msgs.Add(msg);
                    HandleProxy();
                    if (SelectedUser != null)
                    {
                        if (receiver.Email == SelectedUser.Email && this.IsActive)
                            foreach (var m in s.msgs)
                            { 
                                m.IsReaded = true;
                            }
                        ScrollViewer sv = FindVisualChild(DialogListView);
                        sv.LineDown();
                    }
                    var unreaded = s.msgs.Where(x => x.IsReaded == false).Count();
                    if (unreaded == 0)
                        s.user.CountUnreaded = null;
                    else s.user.CountUnreaded = unreaded;
                    s.user.LastMessage = String.Format("{0}:\n{1}", msg.Sender == Me.Email ? "Вы" : msg.Sender, msg.Content);
                    if (msg.Sender != Me.Email && s.user.Email!=Me.Email)
                    {
                        Console.WriteLine("{0}:{1}:{2}",msg.Sender,Me.Email,receiver.Email);
                        DispatcherControls.NewMyNotifyWindow(s.user.Name, msg.Content, 10, this, s.image);
                    }
                }
            }
        }

        public void RefreshClients(SVC.Client[] clients)
        {
            if (ListChats.Where(x => x.user.Email == common_chat).Count() == 0)
                ListChats.Add(new ChatsClass
                {
                    user = new SVC.Client
                    {
                        Name = "Общий",
                        Email = common_chat,
                    },
                    image= UserImageMaster.CreateProfilePicture("Общий", true),
                });
            foreach (var s in clients)
            {
                if (ListChats.Where(x => x.user == s).Count() == 0)
                {
                    ListChats.Add(new ChatsClass { 
                        user = s, 
                        image= UserImageMaster.CreateProfilePicture(s.Name, false),
                    });
                }
                else continue;
            }
            var list = ListChats.ToList();
            foreach (var s in list)
                if (!clients.Contains(s.user))
                    if (s.user.Email != common_chat)
                        ListChats.Remove(s);
            HandleProxy();
        }
        int reconnect_Timeout_sec = 2;
        string email = String.Format("Email Random: {0}", new Random().Next(0, 200));
        string name = String.Format("Name Random: {0}", new Random().Next(0, 200));
        public async void Start()
        {
            try
            {
                if (proxy == null)
                {
                    Me = new SVC.Client();
                    Me.Email = email;
                    Me.Name = name;
                    Me.Time = DateTime.Now;
                    this.Title = Me.Name;
                    InstanceContext context = new InstanceContext(this);
                    proxy = new SVC.ChatClient(context);
                    string servicePath = proxy.Endpoint.ListenUri.AbsolutePath;
                    string serviceListenPort = proxy.Endpoint.Address.Uri.Port.ToString();
                    proxy.Endpoint.Address = new EndpointAddress(String.Format("net.tcp://{0}:{1}{2}", host, serviceListenPort, servicePath));
                    proxy.Open();
                    var result = await proxy.ConnectAsync(Me);
                    if (!result)
                    {
                        InformationBlockLabel.Content = String.Format("Подключение...", reconnect_Timeout_sec);
                        await Task.Delay(TimeSpan.FromSeconds(reconnect_Timeout_sec));
                        await proxy.DisconnectAsync(Me);
                        HandleProxy();
                        //if (proxy != null)
                        //{
                        //    try
                        //    {
                        //        await proxy.DisconnectAsync(Me);
                        //        HandleProxy();
                        //    }
                        //    catch (Exception ex)
                        //    {
                        //        Logger.Write(Logger.TypeLogs.chat, ex.ToString());
                        //    }
                        //}
                        //proxy = null;
                        //Start();
                    }
                    else InformationBlockLabel.Content = null;
                }
            }
            catch (Exception ex) { DispatcherControls.ShowMyDialog("Ошибка", ex.Message, MyDialogWindow.TypeMyDialog.Ok, this); }
        }

        public void UserJoin(SVC.Client client)
        {
            DispatcherControls.NewMyNotifyWindow(client.Name, "Присоединился(ась) к чату", 5, this, TypeImageNotify.standart);
            HandleProxy();
        }

        public void UserLeave(SVC.Client client)
        {
            DispatcherControls.NewMyNotifyWindow(client.Name, "Покинул(а) чат", 5, this, TypeImageNotify.standart);
            HandleProxy();
        }
        //protected async override void OnClosing(System.ComponentModel.CancelEventArgs e)
        //{
        //    if (proxy != null)
        //    {
        //        if (proxy.State == CommunicationState.Opened)
        //        {
        //            await proxy.DisconnectAsync(Me);
        //            //dont set proxy.Close(); because isTerminating = true on Disconnect()
        //            //and this by default will call HandleProxy() to take care of this.
        //        }
        //        else
        //        {
        //            HandleProxy();
        //        }
        //    }
        //}
        private void HandleProxy()
        {
            if (proxy != null)
            {
                switch (proxy.State)
                {
                    case CommunicationState.Closed:
                        proxy = null;
                        Start();
                        break;
                    case CommunicationState.Closing:
                        break;
                    case CommunicationState.Created:
                        break;
                    case CommunicationState.Faulted:
                        proxy.Abort();
                        proxy = null;
                        Start();
                        break;
                    case CommunicationState.Opened:
                        break;
                    case CommunicationState.Opening:
                        break;
                    default:
                        break;
                }
            }
        }

        private async void SendMessageButton_Click(object sender, RoutedEventArgs e)
        {
            await SendMessage();
        }
        private async Task SendMessage()
        {
            var text = MessageTextBox.Text;
            if (!String.IsNullOrEmpty(text) && SelectedUser!=null)
            {
                SVC.Message msg = new SVC.Message();
                msg.Sender = Me.Email;
                msg.Content = text;
                msg.Time = DateTime.Now;
                HandleProxy();
                await proxy.WhisperAsync(msg, SelectedUser);
                await proxy.IsWritingAsync(null);
                MessageTextBox.Text = null;
                if (SelectedUser.Email == common_chat)
                    await proxy.SayAsync(msg);
            }
        }

        private void UsersListViewItem_Selected(object sender, RoutedEventArgs e)
        {
            var obj = (ListBoxItem)sender;
            if (obj.DataContext != null)
            {
                var dc = obj.DataContext as ChatsClass;
                if (dc != null)
                {
                    if (AdditionalBlock.Visibility == Visibility.Collapsed)
                        AdditionalBlock.Visibility = Visibility.Visible;
                    if (MessageSendingGrid.Visibility == Visibility.Collapsed)
                        MessageSendingGrid.Visibility = Visibility.Visible;
                    DialogListView.ItemsSource = dc.msgs;
                    SelectedUser = dc.user;
                    AdditionalBlockLabel.Content = dc.user.Name;
                    MarkCurrentDialogLikeReaded();
                }
            }
        }

        private void UsersListViewItem_Loaded(object sender, RoutedEventArgs e)
        {
            var obj = (ListBoxItem)sender;
            if (obj.DataContext != null)
            {
                var dc = obj.DataContext as ChatsClass;
                if (dc != null)
                {
                    if (dc.user.Email == Me.Email)
                        obj.Visibility = Visibility.Collapsed;
                }
            }
        }
        private void DialogListViewItem_Loaded(object sender, RoutedEventArgs e)
        {
            var obj = (ListBoxItem)sender;
            if (obj.DataContext != null)
            {
                var dc = obj.DataContext as SVC.Message;
                if (dc != null)
                {
                    if (dc.Sender == Me.Email)
                        obj.HorizontalContentAlignment = HorizontalAlignment.Right;
                    ScrollViewer sv = FindVisualChild(DialogListView);
                    sv.LineDown();
                }
            }
        }
        bool IsTyping = false;
        private async void MessageTextBox_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (IsTyping == false)
            {
                var s = Task.Factory.StartNew(new Action(async () =>
                  {
                      if (SelectedUser != null && SelectedUser.Email!=common_chat)
                      {
                          HandleProxy();
                          IsTyping = true;
                          await proxy.IsWritingAsync(Me);
                          await Task.Delay(2000);
                          if (proxy.State == CommunicationState.Opened)
                              await proxy.IsWritingAsync(null);
                          IsTyping = false;
                      }
                  }));
            }
            if (e.Key == Key.Enter)
            {
                await SendMessage();
            }
        }
        private ScrollViewer FindVisualChild(DependencyObject obj)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is ScrollViewer)
                {
                    return (ScrollViewer)child;
                }
                else
                {
                    ScrollViewer childOfChild = FindVisualChild(child);
                    if (childOfChild != null)
                    {
                        return childOfChild;
                    }
                }
            }
            return null;
        }
 
        private void MarkCurrentDialogLikeReaded()
        {
            if (SelectedUser == null) return;
            bool IsFound = false;
            foreach (var s in ListChats)
            {
                if (s.user.Email == SelectedUser.Email)
                {
                    IsFound = true;
                    foreach (var m in s.msgs)
                    {
                        m.IsReaded = true;
                    }
                    s.user.CountUnreaded = null;
                }
                if (IsFound) return;
            }
        }

        private void MessageTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            MarkCurrentDialogLikeReaded();
        }

        private void DialogListView_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (this.IsActive)
                MarkCurrentDialogLikeReaded();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.WindowState = WindowState.Minimized;
        }
    }
    public class NullableContentToHidden : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return Visibility.Collapsed;
            else return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return Visibility.Collapsed;
        }
    }
}
