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
using System.Windows.Threading;
using System.IO;
using Microsoft.Win32;
using Dagorlad_7.SVC;
using System.Diagnostics;
using System.Net.Http;
using System.Net;

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
            StickersPopup.DataContext = list_stickers;
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

        public async void Receive(SVC.Message msg)
        {
            foreach (var s in ListChats)
            {
                if (s.user.Email == common_chat)
                {
                    s.msgs.Add(msg);
                    await HandleProxy();
                    if (SelectedUser != null)
                    {
                        if (SelectedUser.Email == common_chat && this.IsActive)
                            foreach (var m in s.msgs)
                                m.IsReaded = true;
                        ScrollDialogToEnd();
                    }
                    var unreaded = s.msgs.Where(x => x.IsReaded == false).Count();
                    if (unreaded == 0)
                        s.user.CountUnreaded = null;
                    else s.user.CountUnreaded=unreaded;
                    s.user.LastMessage= String.Format("{0}: {1}",msg.Sender==Me.Email?"Вы":msg.Sender,msg.Content);
                    if(msg.Sender!=Me.Email)
                    DispatcherControls.NewMyNotifyWindow(s.user.Name,String.Format("{0}: {1}",msg.Sender,msg.Content),10,this,s.image);
                }
            }
        }
 
        public void ReceiverFile(SVC.FileMessage fileMsg)
        {
            throw new NotImplementedException();
        }
        public void ReceiverFileWhisper(SVC.FileMessage fileMsg, SVC.Client receiver)
        {
            throw new NotImplementedException();
        }
        public async void ReceiveWhisper(SVC.Message msg, SVC.Client receiver)
        {
            foreach (var s in ListChats)
            {
                if (s.user.Email == receiver.Email || s.user.Email == msg.Sender)
                {
                    s.msgs.Add(msg);
                    await HandleProxy();
                    if (SelectedUser != null)
                    {
                        if (receiver.Email == SelectedUser.Email && this.IsActive)
                            foreach (var m in s.msgs)
                            { 
                                m.IsReaded = true;
                            }
                        ScrollDialogToEnd();
                    }
                    var unreaded = s.msgs.Where(x => x.IsReaded == false).Count();
                    if (unreaded == 0)
                        s.user.CountUnreaded = null;
                    else s.user.CountUnreaded = unreaded;
                    s.user.LastMessage = String.Format("{0}: {1}", msg.Sender == Me.Email ? "Вы" : msg.Sender, msg.Content);
                    if (msg.Sender != Me.Email && s.user.Email!=Me.Email)
                    {
                        Console.WriteLine("{0}:{1}:{2}",msg.Sender,Me.Email,receiver.Email);
                        DispatcherControls.NewMyNotifyWindow(s.user.Name, msg.Content, 10, this, s.image);
                    }
                }
            }
        }

        public async void RefreshClients(SVC.Client[] clients)
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
            await HandleProxy();
        }
        int reconnect_Timeout_sec = 2;
        string email = String.Format("Email {0}", new Random().Next(0, 200));
        string name = String.Format("Name {0}", new Random().Next(0, 200));
        public async Task Start()
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

                    proxy.Endpoint.Binding.OpenTimeout= new TimeSpan(0, 0, 3);
                    proxy.Endpoint.Address = new EndpointAddress(String.Format("net.tcp://{0}:{1}{2}", host, serviceListenPort, servicePath));
                    proxy.Open();
                    proxy.InnerDuplexChannel.Faulted += new EventHandler(InnerDuplexChannel_Event);
                    proxy.InnerDuplexChannel.Opened += new EventHandler(InnerDuplexChannel_Event);
                    proxy.InnerDuplexChannel.Closed += new EventHandler(InnerDuplexChannel_Event);
                    var result = await proxy.ConnectAsync(Me);
                    if (!result)
                    {
                        InformationBlockLabel.Content = String.Format("Подключение...", reconnect_Timeout_sec);
                        await Task.Delay(TimeSpan.FromSeconds(reconnect_Timeout_sec));
                        await proxy.DisconnectAsync(Me);
                        await HandleProxy();
                    }
                    else InformationBlockLabel.Content = null;
                }
            }
            catch (Exception ex) {
                Logger.Write(Logger.TypeLogs.chat, ex.ToString());
                proxy = null;
                InformationBlockLabel.Content = String.Format("Подключение...", reconnect_Timeout_sec);
                await Task.Delay(TimeSpan.FromSeconds(reconnect_Timeout_sec));
                await Start();
            }
        }
        async void InnerDuplexChannel_Event(object sender, EventArgs e)
        {
            await Dispatcher.BeginInvoke(new Action(async () =>
            {
                await HandleProxy();
            }));
        }
        public async void UserJoin(SVC.Client client)
        {
            DispatcherControls.NewMyNotifyWindow(client.Name, "Присоединился(ась) к чату", 5, this, TypeImageNotify.standart);
            await HandleProxy();
        }

        public async void UserLeave(SVC.Client client)
        {
            DispatcherControls.NewMyNotifyWindow(client.Name, "Покинул(а) чат", 5, this, TypeImageNotify.standart);
            await HandleProxy();
        }

        private async Task HandleProxy()
        {
            if (proxy != null)
            {
                switch (proxy.State)
                {
                    case CommunicationState.Closed:
                        proxy = null;
                        await Start();
                        break;
                    case CommunicationState.Closing:
                        break;
                    case CommunicationState.Created:
                        break;
                    case CommunicationState.Faulted:
                        proxy.Abort();
                        proxy = null;
                        await Start();
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
                await HandleProxy();
                if (proxy != null)
                {
                    if (SelectedUser.Email == common_chat)
                        await proxy.SayAsync(msg);
                    else
                        await proxy.WhisperAsync(msg, SelectedUser);
                    await proxy.IsWritingAsync(null);
                    MessageTextBox.Text = null;
                }
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
                    ScrollDialogToEnd();
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
                          await HandleProxy();
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
#if (DEBUG)
        string directory_FTD = @"\\krislechy\Downloads\";
#else
        string directory_FTD = @"\\webservice\FTD\";
#endif
        private async void AttachmentSendButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await HandleProxy();
                if (proxy != null)
                {
                    if (SelectedUser != null)
                    {
                        OpenFileDialog fileDialog = new OpenFileDialog();
                        fileDialog.Multiselect = true;

                        if (fileDialog.ShowDialog() == DialogResult.HasValue)
                        {
                            return;
                        }

                        var filenames = fileDialog.FileNames;
                        if (filenames != null)
                        {
                            //UploadFiles
                            var list_files = new Dictionary<string, string>();
                            foreach (var s in filenames)
                            {
                                var path = s;
                                var filename = System.IO.Path.GetFileNameWithoutExtension(s);
                                var ext = System.IO.Path.GetExtension(s);
                                var directory_email = Me.Email;
                                var new_name = Guid.NewGuid() + ext;

                                var directory_string = directory_FTD + directory_email;

                                var wu = new System.Net.WebClient();
                                if (!Directory.Exists(directory_string))
                                {
                                    Directory.CreateDirectory(directory_string);
                                }

                                while(File.Exists(directory_string + "\\" + new_name))
                                    new_name= Guid.NewGuid() + ext;

                                var result_path_upload = directory_string + "\\" + new_name;

                                wu.UploadFile(result_path_upload, path);

                                list_files.Add(filename + ext, result_path_upload);
                            }
                            //
                            SVC.Message msg = new Message();
                            msg.FileLinks = list_files;
                            msg.Sender = Me.Email;
                            msg.IsFile = true;
                            msg.Content = String.Format("Отправлены файлы ({0})", list_files.Count());
                            msg.Time = DateTime.Now;
                            if (SelectedUser.Email == common_chat)
                            {
                                await proxy.SayAsync(msg);
                            }
                            else
                            {
                                await proxy.WhisperAsync(msg, SelectedUser);
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { DispatcherControls.ShowMyDialog("Ошибка отправки файлов",ex.Message,MyDialogWindow.TypeMyDialog.Ok,this); }
        }

        private void OpenDownloadedFile_Click(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            var cmd = btn.CommandParameter;
            if (btn.Content != null)
            {
                Process.Start((string)cmd);
            }
        }
#if (DEBUG)
        string directory_Stickers = @"\\krislechy\Downloads\Stickers\";
#else
        string directory_Stickers = @"\\webservice\STD\";
#endif
        public class StickerClass
        {
            public string name { get; set; }
            public ObservableCollection<StickerItemClass> items { get; set; }
        }
        public class StickerItemClass
        {
            public string link { get; set; }
            public ImageSource image { get; set; }
        }
        public ObservableCollection<StickerClass> list_stickers = new ObservableCollection<StickerClass>();
        private async void StickersPopupChooseButton_Click(object sender, RoutedEventArgs e)
        {
            StickersPopup.IsOpen = true;
            if (list_stickers.Count() == 0)
            {
                foreach (var s in Directory.GetDirectories(directory_Stickers))
                {
                    string fullPath = System.IO.Path.GetFullPath(s).TrimEnd(System.IO.Path.DirectorySeparatorChar);
                    string name = System.IO.Path.GetFileName(fullPath);
                    var items = new StickerClass { name = name };
                    var items_items = new ObservableCollection<StickerItemClass>();
                    foreach (var inner in Directory.GetFiles(s, "*.*", SearchOption.AllDirectories))
                    {
                        var wc = new WebClient();
                        var dwnld = await wc.DownloadDataTaskAsync(new Uri(inner));
                        items_items.Add(new StickerItemClass { link = inner, image = await ByteToImage(dwnld) });
                    }
                    items.items = items_items;
                    list_stickers.Add(items);
                }
                StickersTabControl.SelectedIndex = 0;
            }
        }
        public Task<ImageSource> ByteToImage(byte[] imageData)
        {
            try
            {
                BitmapImage biImg = new BitmapImage();
                MemoryStream ms = new MemoryStream(imageData);
                biImg.BeginInit();
                biImg.StreamSource = ms;
                biImg.EndInit();
                ImageSource imgSrc = biImg as ImageSource;
                return Task.FromResult(imgSrc);
            }
            catch { return null; }
        }

        private async void SendStickerButton_Click(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            var cmd = btn.CommandParameter as string;
            if (btn.Content != null)
            {
                SVC.Message msg = new Message();
                msg.IsSticker = true;
                msg.Sender = Me.Email;
                msg.LinkSticker = cmd;
                msg.Content = String.Format("[Стикер]");
                msg.Time = DateTime.Now;
                if (SelectedUser.Email == common_chat)
                {
                    await proxy.SayAsync(msg);
                }
                else
                {
                    await proxy.WhisperAsync(msg, SelectedUser);
                }
            }
            StickersPopup.IsOpen = false;
        }

        private void StickerIsLoaded(object sender, RoutedEventArgs e)
        {
            ScrollDialogToEnd();
        }

        private void AttachmentIsLoadedListView(object sender, RoutedEventArgs e)
        {
            ScrollDialogToEnd();
        }
        private void ScrollDialogToEnd()
        {
            ScrollViewer sv = FindVisualChild(DialogListView);
            sv.LineDown();
            sv.ScrollToEnd();
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
    public class TransparentIfSticker : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var issticker = System.Convert.ToBoolean(value);
            if (issticker)
                return Brushes.Transparent;
            else return Application.Current.FindResource("Background_Outside");
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return Application.Current.FindResource("Background_Outside");
        }
    }
    public class BooleanVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                if (value.GetType()!= typeof(SVC.Message))
                {
                    return Visibility.Collapsed;                   
                }
                var obj = (SVC.Message)value;
                var _parameter = (string)parameter;
                switch(_parameter)
                {
                    case ("text"):
                        {
                            if (!obj.IsFile && !obj.IsSticker)
                                return Visibility.Visible;
                            break;
                        }
                    case ("file"):
                        {
                            if (obj.IsFile && !obj.IsSticker)
                                return Visibility.Visible;
                            break;
                        }
                    case ("sticker"):
                        {
                            if (!obj.IsFile && obj.IsSticker)
                                return Visibility.Visible;
                            break;
                        }
                }
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            return Visibility.Collapsed;
        }
    }
}
