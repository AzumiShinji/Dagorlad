using Dagorlad.classes;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Dagorlad
{
    /// <summary>
    /// Логика взаимодействия для Chat.xaml
    /// </summary>
    public partial class Chat : Window
    {

#if (DEBUG)
        string directory_FTD = @"\\krislechy\Downloads\";
#else
        string directory_FTD = @"\\webservice\FTD\";
#endif
        public DispatcherTimer timer = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(700) };
        bool isfirsttimecarrete = false;
        string from_id = String.Empty;
        string to_id = String.Empty;
        string group = String.Empty;
        bool IsGroupAddedOnce = false;
        public Chat()
        {
            InitializeComponent();
            Fill_InformationsTable(0);
            FillGroupConversation();
            timer.Tick += async (q, e) =>
            {
                await Dispatcher.BeginInvoke(new Action(async () =>
                {
                    try
                    {
                        if (ServiceHost.client != null)
                        {
                            if (ServiceHost.list != null)
                            {
                                bool wasadded_users = false;
                                var list_users_online = ServiceHost.list.ToList();
                                foreach (var user in list_users_online)
                                {
                                    bool is_conversation_already_exist = false;
                                    foreach (var item in ConversationsList)
                                    {

                                        if (item != null)
                                        {
                                            var obj = item.converstion;
                                            if (obj != null)
                                                if (obj.perhabs_email == user.perhabs_email && obj.perhabs_fio == user.perhabs_fio)
                                                {
                                                    is_conversation_already_exist = true;
                                                    break;
                                                }
                                        }
                                    }
                                    //if (!is_conversation_already_exist)
                                    if (!is_conversation_already_exist && user.perhabs_email != Properties.Settings.Default.perhabsemail)
                                    {
                                        FillConversationListBox(user);
                                        wasadded_users = true;
                                    }
                                }

                                if (wasadded_users)
                                {
                                    listofusersLB.ItemsSource = null;
                                    listofusersLB.ItemsSource = ConversationsList;
                                    //listofusersLB.Items.Refresh();
                                }
                                else
                                {
                                    if (!IsGroupAddedOnce)
                                    {

                                        listofusersLB.ItemsSource = ConversationsList;
                                        listofusersLB.Items.Refresh();
                                        //listofusersLB.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("unread",
                                        //System.ComponentModel.ListSortDirection.Descending));
                                        IsGroupAddedOnce = true;
                                    }
                                }

                                if (listofusersLB.SelectedItem != null)
                                {
                                    timer.Stop();
                                    await FillConversation(group);
                                    timer.Start();
                                    CountOnlinelbl.Content = ServiceHost.list.Count();
                                }
                                await GetUnreadForAllConversations();
                            }
                        }
                    }
                    catch (Exception ex) { Console.WriteLine("ChatException " + ex.ToString()); }
                }), DispatcherPriority.Background);
            };
            timer.Start();
        }
        List<string> info_table_list = new List<string>() {
            "Shift+Enter - переход на новую строку, Enter - отправка сообщения",
            "История сообщений хранится ровно 24 часа (00:00-24:00)",
            "PrintScreen, Ctrl+V - вставить скриншот\nПравая кнопка мыши на выбранном файле - сохранить вложенный файл",
            "По всем проблемам в чате просьба писать на PyatkoBV@fsfk.local",
            "Имеется возможность прикреплять вложения через соответствующую кнопку на панели ввода",
            "Можно скопировать текст прямо из диалога",
            "Окно чата будет работать после запуска Dagorlad в фоновом режиме",
        };
        int InformationsTable_CurrentPage=1;
        /// <summary>
        /// -1-previous
        /// 0-first
        /// 1-next
        /// </summary>
        /// <param name="action"></param>
        private void Fill_InformationsTable(int action)
        {
            var count_page = (info_table_list.Count());
            countpage_infotable.Text = InformationsTable_CurrentPage + "/" + count_page;
            if (action == 0)
                InformationTable.Text = info_table_list.FirstOrDefault();
            else if (action == -1)
            {
                if (InformationsTable_CurrentPage > 1)
                {
                    InformationsTable_CurrentPage = InformationsTable_CurrentPage - 1;
                    InformationTable.Text = info_table_list.ElementAt(InformationsTable_CurrentPage-1);
                }
                else
                {
                    InformationsTable_CurrentPage = count_page;
                    InformationTable.Text = info_table_list.ElementAt(InformationsTable_CurrentPage-1);
                }
            }
            else if (action == 1)
            {
                if (InformationsTable_CurrentPage < count_page)
                {
                    InformationsTable_CurrentPage = InformationsTable_CurrentPage + 1;
                    InformationTable.Text = info_table_list.ElementAt(InformationsTable_CurrentPage-1);
                }
                else
                {
                    InformationsTable_CurrentPage = 1;
                    InformationTable.Text = info_table_list.ElementAt(InformationsTable_CurrentPage-1);
                }
            }
            countpage_infotable.Text = InformationsTable_CurrentPage + "/" + count_page;
        }
        private void HideInformationTable_Click(object sender, RoutedEventArgs e)
        {
            if (InformationTableGrid.RowDefinitions[0].ActualHeight > 0)
            {
                InformationTableGrid.RowDefinitions[0].Height = new GridLength(0, GridUnitType.Pixel);
                InformationTableGrid.RowDefinitions[1].Height = new GridLength(0, GridUnitType.Pixel);
                HideInformationTable.Content = "Информация";
            }
            else
            {
                InformationTableGrid.RowDefinitions[0].Height = new GridLength(3, GridUnitType.Auto);
                InformationTableGrid.RowDefinitions[1].Height = new GridLength(3, GridUnitType.Auto);
                HideInformationTable.Content = "Понятно";
            }
        }
        private string[] ListOfGroupsArray = null;
        private void FillGroupConversation()
        {
            if (Properties.Settings.Default.perhabsemail != null)
            {
                var direction = GetInfoAboutUser.GetDirectionFromDB(Properties.Settings.Default.perhabsemail);
                if (!String.IsNullOrEmpty(direction))
                {
                    direction = "Общий/" + direction;
                    var splitted = direction.Split('/');
                    ListOfGroupsArray = splitted;
                    foreach (var s in splitted)
                    {
                        FillConversationListBox(new Service_Dagorlad.Info
                        {
                            enter_dt = new DateTime(),
                            ip = "0.0.0.0",
                            isanswered = false,
                            perhabs_email = "Groups@fsfk.local",
                            perhabs_fio = s,
                            update_dt = new DateTime(),
                            username = s,
                        });
                    }
                }
            }
        }
        public class ConversationsUsersAndGroupsClass : INotifyPropertyChanged
        {
            public BitmapImage image { get; set; }
            public string name { get; set; }
            #region unread
            private int _unread;
            public int unread
            {
                get { return this._unread; }
                set
                {
                    if (value == this._unread) return;
                    this._unread = value;
                    OnPropertyChanged();
                }
            }
            public Service_Dagorlad.Info converstion { get; set; }
            #endregion
            public event PropertyChangedEventHandler PropertyChanged;
            public void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                if (PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }
            }
        }
        private ObservableCollection<ConversationsUsersAndGroupsClass> ConversationsList = new ObservableCollection<ConversationsUsersAndGroupsClass>();
        private void FillConversationListBox(Service_Dagorlad.Info converstion)
        {
            string result_first_letter_of_fio = String.Empty;
            if (converstion.perhabs_fio.Length > 3)
            {
                List<char> first_letter_of_fio = new List<char>();
                converstion.perhabs_fio.Split(' ').ToList().ForEach(i => first_letter_of_fio.Add(i[0]));
                foreach (var s in first_letter_of_fio)
                {
                    result_first_letter_of_fio += s;
                }
            }
            else
            {
                result_first_letter_of_fio = converstion.perhabs_fio;
            }
            BitmapImage image=null;
            if(GetGroup(converstion.perhabs_fio)==null)
            image = DrawCircleText.CreateProfilePicture(result_first_letter_of_fio,false);
            else image= DrawCircleText.CreateProfilePicture(result_first_letter_of_fio, true);
            ConversationsList.Add(new ConversationsUsersAndGroupsClass
            {
                image = image,
                name = converstion.perhabs_fio,
                converstion = converstion,
            });
        }
        bool isgetoldhistory = false;
        static List<Service_Dagorlad.Conversations> allreadyrecieved = new List<Service_Dagorlad.Conversations>();
        private async Task GetUnreadForAllConversations()
        {
            try
            {
                await Dispatcher.BeginInvoke(new Action(async () =>
                {
                try
                {
                    foreach (var s in ConversationsList)
                    {
                        if (s != null)
                        {
                            string fio = String.Empty;
                            string _group = null;
                            string email = String.Empty;
                            if (s != null)
                            {
                                _group = GetGroup(s.converstion.perhabs_fio);
                                email = s.converstion.perhabs_email;
                                fio = GetGroup(s.converstion.perhabs_fio) == null ? s.converstion.perhabs_fio : null;
                            }
                            var temp_from_id = "Диалог с " + Properties.Settings.Default.perhabsfio + " - " + Properties.Settings.Default.perhabsemail;
                            var temp_to_id = "Диалог с " + (_group == null ? fio : _group) + " - " + email;
                            var unread = await ServiceHost.return_Conversation_Unread(temp_from_id, temp_to_id, _group);
                            if (unread != null)
                            {
                                if (s.converstion.perhabs_email == email &&
                                    s.converstion.perhabs_fio == (_group == null ? fio : _group))
                                {
                                    s.unread = unread.Count();
                                    if (unread.Count() > 0)
                                    {
                                        foreach (var mes in unread)
                                        {
                                            //if (!allreadyrecieved.Exists(x => x.dt == mes.dt && x.name == mes.name &&
                                            //x.message == mes.message && x.group == mes.group)&& mes.from!=from_id)
                                            if (!allreadyrecieved.Exists(x => x.dt == mes.dt && x.name == mes.name &&
                                            x.message == mes.message && x.group == mes.group) && mes.from != from_id)
                                            {
                                                bool filesisnull = mes.files_uploaded == null ? true : false;
                                                var text = (filesisnull == true ? (mes.message == null ? "[Cтикер]" : mes.message) : ("[Вложение]"));
                                                NotifyWindowCustom.ShowNotify(
                                                    mes.group == null ? mes.name : mes.group,
                                                    mes.group == null ? text : (mes.name + ": " + text),
                                                    mes.uri_images,
                                                    3600,
                                                    false,
                                                    typeof(Chat),
                                                    mes.group == null ? mes.name : mes.group,
                                                    getAvatarUser(mes.group == null ? mes.name : mes.group)
                                                    );
                                                FlashWindow.Do_FlashWindow(this, 3);
                                                allreadyrecieved.Add(new Service_Dagorlad.Conversations
                                                {
                                                    dt = mes.dt,
                                                    name = mes.name,
                                                    message = mes.message,
                                                    group = mes.group,
                                                });
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (this.IsActive)
                    {
                        foreach (var s in Application.Current.Windows)
                            if (s.GetType() == typeof(NotifyWindowCustom))
                            {
                                var win = (NotifyWindowCustom)s;
                                win.CloseMoveActionStart();
                            }
                    }
                }
                catch (Exception ex) { Logger.Do(ex.ToString(),Logger.LevelAttention.Warning); }
                }), DispatcherPriority.Background);
            }
            catch (Exception ex) { Console.WriteLine("GetUnreadForAllConversations " + ex.ToString()); }
        }
        private BitmapImage getAvatarUser(string username)
        {
            foreach (var s in listofusersLB.Items)
                if (s != null && s.GetType() == typeof(ConversationsUsersAndGroupsClass))
                {
                    var obj = (ConversationsUsersAndGroupsClass)s;
                    if (obj.name == username)
                    {
                        return obj.image;
                    }
                }
            return new BitmapImage(new Uri(@"Resources/notify_chat.png", UriKind.Relative));
        }
        private async Task FillConversation(string group)
        {
            try
            {
                bool isrefreshing = false;
                if (this.IsActive)
                    await ServiceHost.Set_Conversation_Readed(from_id, to_id, group);
                var gettings_history = await ServiceHost.return_Conversation(from_id, to_id, group);
                if (dg.ItemsSource != null)
                {
                    var current_history = dg.ItemsSource.Cast<Service_Dagorlad.Conversations>().ToList();
                    if (gettings_history != null && gettings_history.Count() > 0)
                    {
                        var unread = gettings_history.Where(p => !current_history.Any(l => p.dt == l.dt && p.message == l.message && p.email == l.email)).ToList();
                        if (unread.Count() > 0)
                        {
                            foreach (var s in unread)
                            {
                                if (s.email != Properties.Settings.Default.perhabsemail && !this.IsActive)
                                {
                                    // NotifyWindowCustom.ShowNotify(s.name, s.message ?? "[Cтикер]", "notify_chat.png", 5, false, typeof(Chat), "ALL");
                                    FlashWindow.Do_FlashWindow(this, 3);
                                }
                            }
                            if (listofusersLB.SelectedItem != null && (listofusersLB.SelectedValue as ConversationsUsersAndGroupsClass) != null)
                            {
                                var select_dialog_from_history = unread.ToList();
                                current_history.AddRange(select_dialog_from_history);
                                CarretToBottom();
                                isrefreshing = true;
                            }
                        }
                    }
                    if (isrefreshing)
                        dg.ItemsSource = current_history;
                }
                else
                {
                    if (gettings_history != null && !isgetoldhistory)
                    {
                        dg.ItemsSource = gettings_history;
                        isgetoldhistory = true;
                    }
                }
                if (!isfirsttimecarrete)
                {
                    CarretToBottom();
                    isfirsttimecarrete = true;
                }
            }
            catch (Exception ex) { Console.WriteLine("FillDialogException " + ex.ToString()); }
        }
        private string GetGroup(string name)
        {
            string _name = null;
            if (ListOfGroupsArray != null && ListOfGroupsArray.Count() > 0)
                foreach (var _group in ListOfGroupsArray)
                {
                    if (_group == name)
                    {
                        _name = _group;
                        break;
                    }
                    else _name = null;
                }
            return _name;
        }
        private void SelectedItemEvent(object sender, RoutedEventArgs e)
        {
            TypingGrid.IsEnabled = true;
            TypingGrid.Visibility = Visibility.Visible;
            isgetoldhistory = false;
            isfirsttimecarrete = false;
            dg.ItemsSource = null;
            var obj = (ListBoxItem)sender;
            var context = (ConversationsUsersAndGroupsClass)(obj.DataContext);
            group = GetGroup(context.converstion.perhabs_fio);
            CurrentDialog.Content = context.converstion.perhabs_fio;
            from_id = "Диалог с " + Properties.Settings.Default.perhabsfio + " - " + Properties.Settings.Default.perhabsemail;
            to_id = "Диалог с " + context.converstion.perhabs_fio + " - " + context.converstion.perhabs_email;
            if (group == null)
                IsOnline.Content = CheckOnline(context.converstion.perhabs_email) ? "Online" : "Offline";
            else IsOnline.Content = "Группа";
            MessageTB.Focus();
        }
        private bool CheckOnline(string email)
        {
            foreach (var online in ServiceHost.list)
                if (email == online.perhabs_email)
                    return true;
            return false;
        }
        private void CarretToBottom()
        {
            if (dg.Items.Count > 0)
            {
                var border = VisualTreeHelper.GetChild(dg, 0) as Decorator;
                if (border != null)
                {
                    var scroll = border.Child as ScrollViewer;
                    if (scroll != null) scroll.ScrollToEnd();
                }
            }
        }
        private void Typingtb_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void MessageTB_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if ((Keyboard.Modifiers == ModifierKeys.Shift) && e.Key == Key.Enter)
            {
                //MessageTB.Text += Environment.NewLine;
                MessageTB.CaretIndex = MessageTB.Text.Length;
            }
            else if (e.Key == Key.Enter)
            {
                var name = Properties.Settings.Default.perhabsfio;
                var email = Properties.Settings.Default.perhabsemail;
                if (!String.IsNullOrEmpty(MessageTB.Text))
                    ServiceHost.AddMessage(new Service_Dagorlad.Conversations
                    {
                        email = email,
                        name = String.IsNullOrEmpty(name) ? "Неизвестно" : name,
                        message = MessageTB.Text.Trim(),
                        uri_images = null,
                        group = group,
                        from = from_id,
                        to = to_id,
                    });
                MessageTB.Text = null;
                MessageTB.Clear();
                e.Handled = true;
            }
        }
        private void Typingtb_GotFocus(object sender, RoutedEventArgs e)
        {
            if (((TextBox)sender).Text == "Написать сообщение...")
                ((TextBox)sender).Text = String.Empty;
            ((TextBox)sender).Foreground = Brushes.White;
        }
        private void Typingtb_LostFocus(object sender, RoutedEventArgs e)
        {
            var text = ((TextBox)sender).Text.Trim();
            if (String.IsNullOrEmpty(text))
            {
                ((TextBox)sender).Text = "Написать сообщение...";
            }
           ((TextBox)sender).Foreground = Brushes.Gray;
            TypingBorder.Background = Brushes.Transparent;
        }
        private void TypingBorder_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!MessageTB.IsFocused)
            {
                TypingBorder.Background = Brushes.Transparent;
            }
        }
        private List<string> GetAllSmiles()
        {
            List<string> images = new List<string>();
            var resourceManager = new ResourceManager("Dagorlad.g", Assembly.GetExecutingAssembly());
            var resources = resourceManager.GetResourceSet(CultureInfo.CurrentUICulture, true, true);
            foreach (var res in resources)
            {
                var img = ((DictionaryEntry)res).Key;
                if (img.ToString().Contains("smiles"))
                    images.Add(img.ToString());
            }
            return images;
        }
        private void ChooseSmile_Click(object sender, RoutedEventArgs e)
        {
            folderstickers.Children.Clear();
            var f = GetAllSmiles();
            List<string> folders = new List<string>();
            foreach (var s in f)
            {
                var sf = Uri.UnescapeDataString(s.Split('/').ElementAt(1));
                folders.Add(sf);
            }
            folders = folders.GroupBy(x => x).Select(x => x.First()).OrderBy(x => x).ToList();
            foreach (var folder in folders)
            {
                var folder_btn = new Button() { Content = folder };
                folder_btn.Click += (q, g) =>
                {
                    smilesgrid.Children.Clear();
                    foreach (var s in f.Where(x => Uri.UnescapeDataString(x).Contains(folder)))
                    {
                        var button = new Button();
                        button.Click += (q, g) =>
                        {
                            var name = Properties.Settings.Default.perhabsfio;
                            var email = Properties.Settings.Default.perhabsemail;
                            ServiceHost.AddMessage(new Service_Dagorlad.Conversations
                            {
                                email = email,
                                name = String.IsNullOrEmpty(name) ? "Неизвестно" : name,
                                message = null,
                                uri_images = s,
                                from = from_id,
                                to = to_id,
                                group = group,
                            });
                            smilespopup.IsOpen = false;
                        };
                        var image = new Image();
                        image.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(@"/Dagorlad;component/" + s, UriKind.Relative));
                        if (folder == "emoji") { image.Width = 20; image.Height = 20; }
                        else { image.Width = 120; image.Height = 120; }
                        button.Content = image;
                        smilesgrid.Children.Add(button);
                    }
                };
                folderstickers.Children.Add(folder_btn);
            }
            smilespopup.IsOpen = true;
        }
        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == System.Windows.WindowState.Minimized)
            {
                this.Hide();
            }
            base.OnStateChanged(e);
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.ToString().Length == 1)
            {
                if (Char.IsLetterOrDigit(e.Key.ToString()[0]))
                    MessageTB.Focus();
            }
        }
        private void Attachemntbtn_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog() { Multiselect = true };
            if (dlg.ShowDialog() == true && Properties.Settings.Default.perhabsemail != null)
            {
                try
                {
                    var filenames = dlg.FileNames;
                    var list_files = new Dictionary<string, string>();
                    foreach (var s in filenames)
                    {
                        var path = s;
                        var filename = Path.GetFileNameWithoutExtension(s);
                        var ext = Path.GetExtension(s);
                        var directory_email = Properties.Settings.Default.perhabsemail;
                        var new_name = Guid.NewGuid() + ext;

                        var directory_string = directory_FTD + directory_email;

                        var wu = new WebClient();
                        if (!Directory.Exists(directory_string))
                        {
                            Directory.CreateDirectory(directory_string);
                        }
                        var result_path_upload = directory_string + "\\" + new_name;

                        wu.UploadFile(result_path_upload, path);

                        list_files.Add(filename + ext, result_path_upload);
                    }
                    var name = Properties.Settings.Default.perhabsfio;
                    var email = Properties.Settings.Default.perhabsemail;
                    ServiceHost.AddMessage(new Service_Dagorlad.Conversations
                    {
                        email = email,
                        name = String.IsNullOrEmpty(name) ? "Неизвестно" : name,
                        message = null,
                        uri_images = null,
                        group = group,
                        from = from_id,
                        to = to_id,
                        files_uploaded = list_files,
                    });
                }
                catch (Exception ex) { MessageBox.Show(ex.ToString()); }
            }
        }
        private void OpenFileBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var btn = (Button)sender;
                var context = (KeyValuePair<string, string>)btn.DataContext;
                Process.Start("explorer.exe", context.Value);
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }
        private void OpenFileBtn_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var cm = new ContextMenu();
            var item = new MenuItem() { Header = "Сохранить как..." };
            item.Click += (q, g) =>
            {
                try
                {
                    var dlg = new SaveFileDialog();
                    var btn = (Button)sender;
                    var context = (KeyValuePair<string, string>)btn.DataContext;
                    var namefile = context.Key;
                    var path = context.Value;
                    dlg.FileName = namefile;
                    string ext = Path.GetExtension(namefile);
                    if (!String.IsNullOrEmpty(ext))
                    {
                        dlg.Filter = "Определенный (*" + ext + ")|*" + ext;
                        dlg.AddExtension = true;
                        dlg.DefaultExt = ext;
                    }
                    if (dlg.ShowDialog() == true)
                    {
                        var readbyte = File.ReadAllBytes(path);
                        File.WriteAllBytes(dlg.FileName, readbyte);
                    }
                }
                catch (Exception ex) { MessageBox.Show(ex.ToString()); }
            };
            cm.Items.Add(item);
            cm.IsOpen = true;
        }
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.V && (Keyboard.Modifiers == ModifierKeys.Control) && listofusersLB.SelectedItem != null)
            {
                try
                {
                    var clip = Clipboard.GetImage();
                    if (clip != null)
                    {
                        if (MessageBox.Show("Отправить скриншот?",
    ((ConversationsUsersAndGroupsClass)listofusersLB.SelectedItem).name, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            var directory_email = Properties.Settings.Default.perhabsemail;
                            var name_resp = "Скриншот " + DateTime.Now.ToString().Replace(":", "_").Replace(".", "_") + ".png";
                            var new_name = Guid.NewGuid() + ".png";

                            var directory_string = directory_FTD + directory_email;

                            var path = directory_string + "\\" + new_name;

                            if (File.Exists(path))
                            {
                                new_name = Guid.NewGuid() + ".png";
                                path = directory_string + "\\" + new_name;
                            }
                            //save
                            MemoryStream ms = new MemoryStream();
                            FileStream stream = new FileStream(path, FileMode.Create);
                            BitmapEncoder encoder = new PngBitmapEncoder();
                            encoder.Frames.Add(BitmapFrame.Create(clip));
                            encoder.Save(stream);
                            stream.Close();
                            //
                            var name = Properties.Settings.Default.perhabsfio;
                            var email = Properties.Settings.Default.perhabsemail;
                            ServiceHost.AddMessage(new Service_Dagorlad.Conversations
                            {
                                email = email,
                                name = String.IsNullOrEmpty(name) ? "Неизвестно" : name,
                                message = null,
                                uri_images = null,
                                group = group,
                                from = from_id,
                                to = to_id,
                                files_uploaded = new Dictionary<string, string>() { { name_resp, path } },
                            });

                        }
                    }
                }
                catch { }
            }
        }

        private void InformationTablePreviousBtn_Click(object sender, RoutedEventArgs e)
        {
            Fill_InformationsTable(-1);
        }

        private void InformationTableNextBtn_Click(object sender, RoutedEventArgs e)
        {
            Fill_InformationsTable(1);
        }
    }
    #region ValueConverters
    [ValueConversion(typeof(string), typeof(HorizontalAlignment))]
    public class HorizontalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((string)value != null)
            {
                var email = (string)value;
                if (email == Properties.Settings.Default.perhabsemail)
                    return HorizontalAlignment.Right;
                else return HorizontalAlignment.Left;

            }
            return HorizontalAlignment.Left;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return HorizontalAlignment.Left;
        }
    }
    [ValueConversion(typeof(string), typeof(CornerRadius))]
    public class CornerRadiusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((string)value != null)
            {
                var email = (string)value;
                if (email == Properties.Settings.Default.perhabsemail)
                    return new CornerRadius(10, 10, 0, 10);
                else return new CornerRadius(10, 10, 10, 0);

            }
            return new CornerRadius(10, 10, 10, 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return new CornerRadius(10, 10, 10, 0);
        }
    }
    [ValueConversion(typeof(string), typeof(bool))]
    public class BrushChatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((string)value != null)
            {
                var email = (string)value;
                if (email == Properties.Settings.Default.perhabsemail)
                    return true;
                else return false;

            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return Brushes.Gray;
        }
    }
    [ValueConversion(typeof(string), typeof(bool))]
    public class StickersChatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var img_str = (string)value;
            if (img_str==null) return false;
            else
            {
                if (img_str.Contains("emoji")) return false;
                else return true;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return false;
        }
    }
    [ValueConversion(typeof(bool), typeof(string))]
    public class OnlineUserConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var isonline = (bool)value;
            if (isonline)
                return new System.Windows.Media.Imaging.BitmapImage(new Uri(@"/Dagorlad;component/Resources/greencircle.png", UriKind.Relative));
            else return new System.Windows.Media.Imaging.BitmapImage(new Uri(@"/Dagorlad;component/Resources/graycircle.png", UriKind.Relative));
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return new System.Windows.Media.Imaging.BitmapImage(new Uri(@"/Dagorlad;component/Resources/graycircle.png", UriKind.Relative));
        }
    }
    [ValueConversion(typeof(string), typeof(Visibility))]
    public class ImagesChatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                if (value.GetType() == typeof(Dictionary<string, string>))
                {
                    if (value == null)
                        return Visibility.Collapsed;
                    else return Visibility.Visible;
                }
                else
                {
                    var isimage = (string)value;
                    if (isimage != null)
                        return Visibility.Visible;
                    else return Visibility.Collapsed;
                }
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return Visibility.Collapsed;
        }
    }
    [ValueConversion(typeof(string), typeof(ImageSource))]
    public class ImagesSourceChatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var uri = (string)value;
            if (uri != null)
                return new System.Windows.Media.Imaging.BitmapImage(new Uri(@"/Dagorlad;component/"+uri, UriKind.Relative));
            else return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
    [ValueConversion(typeof(int), typeof(Visibility))]
    public class VisibleUnread : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value.GetType() == typeof(int))
            {
                var unread = (int)value;
                if(unread>0)
                    return Visibility.Visible;
                else return Visibility.Collapsed;
            }
            else return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return Visibility.Collapsed;
        }
    }
#endregion
}
