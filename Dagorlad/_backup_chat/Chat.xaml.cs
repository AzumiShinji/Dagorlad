using Dagorlad.classes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
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
        public DispatcherTimer timer = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(100) };
        bool isfirsttimecarrete = false;
        bool isCommonDialog = true;
        string from_id = String.Empty;
        string to_id = String.Empty;
        public Chat()
        {
            InitializeComponent();
            FillUsersListBox(null,true);
            timer.Tick += async (q, e) =>
            {
                await Dispatcher.BeginInvoke(new Action(async() =>
                {
                    try
                    {
                        if (ServiceHost.client != null)
                        {
                            if (ServiceHost.list != null)
                            {
                                var list_users_online = ServiceHost.list.ToList();
                                //fill users to listbox
                                foreach (var user in list_users_online)
                                {
                                    bool is_user_existing_in_lb = false;
                                    foreach (var existing_user in listofusersLB.Items)
                                    {
                                        var item = existing_user as ListBoxItem;
                                        var obj = (Service_Dagorlad.Info)item.DataContext;
                                        if (item != null && obj != null)
                                        {
                                            if (existing_user != null && obj.perhabs_email == user.perhabs_email && obj.perhabs_fio == user.perhabs_fio)
                                                is_user_existing_in_lb = true;
                                        }
                                    }
                                    if (!is_user_existing_in_lb)
                                    {
                                        FillUsersListBox(user, false);
                                    }
                                }
                                foreach (var user in list_users_online)
                                    await GetUnreadForAllConversations(user);
                                var selected_user_lb = listofusersLB.SelectedItem as ListBoxItem;
                                if (selected_user_lb != null)
                                {
                                    var selected_user = (Service_Dagorlad.Info)selected_user_lb.DataContext;
                                    if (selected_user_lb != null && selected_user != null)
                                    {
                                        from_id = "Диалог с " + Properties.Settings.Default.perhabsfio + " - " + Properties.Settings.Default.perhabsemail;
                                        to_id = "Диалог с " + selected_user.perhabs_fio + " - " + selected_user.perhabs_email;
                                    }
                                    if (selected_user_lb != null && isCommonDialog)
                                    {
                                        timer.Stop();
                                        await FillCommonChat();
                                        timer.Start();
                                    }
                                    else if (selected_user_lb != null && selected_user != null)
                                    {
                                        timer.Stop();
                                        await FillDialog();
                                        timer.Start();
                                    }
                                }
                                CountOnlinelbl.Content = ServiceHost.list.Count();
                            }
                        }
                    }
                    catch (Exception ex) { Console.WriteLine("ChatException " + ex.ToString()); }
                }), DispatcherPriority.Background);
            };
            timer.Start();
        }
        private void FillUsersListBox(Service_Dagorlad.Info user,bool iscommon)
        {
            // get first letter from FIO
            var image = new Image();
            if (!iscommon)
            {
                string result_first_letter_of_fio = String.Empty;
                List<char> first_letter_of_fio = new List<char>();
                user.perhabs_fio.Split(' ').ToList().ForEach(i => first_letter_of_fio.Add(i[0]));
                int count_letter = 0;
                foreach (var s in first_letter_of_fio)
                {
                    if (count_letter < 2)
                        result_first_letter_of_fio += s;
                    count_letter++;
                }
                image.Source = DrawCircleText.CreateProfilePicture(result_first_letter_of_fio);
            }
            else image.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(@"/Dagorlad;component/Resources/commonchat.png",
                            UriKind.Relative));
            var textblock = new TextBlock()
            {
                Text = iscommon? "Общий" : user.perhabs_fio,
                VerticalAlignment = VerticalAlignment.Center,
                Padding = new Thickness(5, 0, 0, 0),
            };
            // unread lbl
            var label = new Label() { 
                HorizontalContentAlignment=HorizontalAlignment.Center,
                VerticalContentAlignment=VerticalAlignment.Center,
                Foreground=Brushes.White,
                Padding=new Thickness(0),
                Margin=new Thickness(0),
                FontSize=10,
                Content=iscommon?"&":null,
            };
            var unread_border = new Border() { 
                Child=label, 
                CornerRadius=new CornerRadius(50),
                Background=(Brush) new BrushConverter().ConvertFromString("#009687"),
                Visibility=iscommon?Visibility.Visible:Visibility.Hidden,
                Width=15,
                Height=15,
                Margin=new Thickness(1,0,0,0),
                Padding = new Thickness(0),
            };
            //
            var grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(30) });
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(0, GridUnitType.Star) });
            Grid.SetColumn(image, 0);
            Grid.SetColumn(textblock, 1);
            Grid.SetColumn(unread_border, 2);
            grid.Children.Add(image);
            grid.Children.Add(textblock);
            grid.Children.Add(unread_border);
            var new_item = new ListBoxItem { DataContext = iscommon? new Service_Dagorlad.Info { perhabs_fio = "Общий" }:user, Content = grid };
            new_item.Selected += SelectedItemEvent;
            //
            listofusersLB.Items.Add(new_item);
        }
        static List<Service_Dagorlad.Chat> allreadyrecieved_common = new List<Service_Dagorlad.Chat>();
        private int allunread_common = 0;
        private async Task getUnreadCommonChat()
        {
            try
            {
                var gettings_history = await ServiceHost.client.return_ChatAsync();
                if (gettings_history != null && gettings_history.Count() > 0)
                {
                    var unread = gettings_history.Where(p => !allreadyrecieved_common.Any(l => p.dt == l.dt && p.message == l.message && p.email == l.email)).ToList();
                    if (unread != null && unread.Count() > 0 && unread.Where(x=>x.isread==false).Count()>0)
                    {
                        allreadyrecieved_common.AddRange(unread);
                        allunread_common += unread.Count();
                        foreach (var s in allreadyrecieved_common)
                        {
                            if (s.email != Properties.Settings.Default.perhabsemail && !s.isread && 
                                (((Service_Dagorlad.Info)((ListBoxItem)listofusersLB.SelectedItem).DataContext).perhabs_fio!="Общий"))
                            {
                                NotifyWindowCustom.ShowNotify(s.nickname, s.message ?? "[Cтикер]", "notify_chat.png", 5, false, typeof(Chat), "ALL");
                                FlashWindow.Do_FlashWindow(this, 3);
                                s.isread = true;
                            }
                        }

                    }
                }
                foreach (var s in listofusersLB.Items)
                {
                    if (s.GetType() == typeof(ListBoxItem))
                    {
                        var obj = (ListBoxItem)s;
                        if (obj.Content.GetType() == typeof(Grid))
                        {
                            var context = (Service_Dagorlad.Info)obj.DataContext;
                            if (context.perhabs_fio == "Общий")
                            {
                                var grid = (Grid)obj.Content;
                                if (grid != null && grid.Children != null && grid.Children.Count > 0)
                                {
                                    foreach (var el in grid.Children)
                                    {
                                        if (el.GetType() == typeof(Border))
                                        {
                                            var border = (Border)el;
                                            if (allunread_common > 0)
                                            {
                                                var label = (Label)border.Child;
                                                label.Content = allunread_common > 0 ? allunread_common.ToString() : null;
                                                border.Visibility = Visibility.Visible;
                                            }
                                            else border.Visibility = Visibility.Hidden;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine("FillChatException " + ex.ToString()); }
        }
        private async Task FillCommonChat()
        {
            try
            {
                bool isrefreshing = false;
                var gettings_history = await ServiceHost.client.return_ChatAsync();
                if (dg.ItemsSource != null)
                {
                    var current_history = dg.ItemsSource.Cast<Service_Dagorlad.Chat>().ToList();
                    if (gettings_history != null && gettings_history.Count() > 0)
                    {
                        var unread = gettings_history.Where(p => !current_history.Any(l => p.dt == l.dt && p.message == l.message && p.email == l.email)).ToList();
                        if (unread.Count() > 0)
                        {
                            //foreach (var s in unread)
                            //{
                            //    if (s.email != Properties.Settings.Default.perhabsemail)
                            //    {
                            //        NotifyWindowCustom.ShowNotify(s.nickname, s.message ?? "[Cтикер]", "notify_chat.png", 5, false, typeof(Chat), "ALL");
                            //        FlashWindow.Do_FlashWindow(this, 3);
                            //    }
                            //}
                            allunread_common = 0;
                            foreach (var un in allreadyrecieved_common)
                                un.isread = true;
                            if (listofusersLB.SelectedItem != null && (listofusersLB.SelectedValue as ListBoxItem) != null && isCommonDialog)
                            {
                                current_history.AddRange(unread);
                                CarretToBottom();
                                isrefreshing = true;
                            }
                        }
                    }
                    if (isrefreshing)
                        dg.ItemsSource = current_history;
                    //CheckOnline(current_history);
                }
                else
                {
                    if (!isgetoldhistory)
                    {
                        dg.ItemsSource = gettings_history.ToList();
                        isgetoldhistory = true;
                    }
                }
                if (!isfirsttimecarrete)
                {
                    CarretToBottom();
                    isfirsttimecarrete = true;
                }
            }
            catch (Exception ex) { Console.WriteLine("FillChatException " + ex.ToString()); }
        }
        bool isgetoldhistory = false;
        static List<Service_Dagorlad.Chat> allreadyrecieved = new List<Service_Dagorlad.Chat>();
        private async Task GetUnreadForAllConversations(Service_Dagorlad.Info user)
        {
            try
            {
                await Dispatcher.BeginInvoke(new Action(async () =>
                {
                    await getUnreadCommonChat();
                    if (listofusersLB != null && listofusersLB.Items.Count > 0)
                        foreach (var s in listofusersLB.Items)
                        {
                            if (s != null)
                            {
                                var temp_from_id = "Диалог с " + Properties.Settings.Default.perhabsfio + " - " + Properties.Settings.Default.perhabsemail;
                                var temp_to_id = "Диалог с " + user.perhabs_fio + " - " + user.perhabs_email;
                                var unread = await ServiceHost.return_Dialog_Unread_Count(temp_from_id, temp_to_id);
                                if (unread != null)
                                {
                                    if (s.GetType() == typeof(ListBoxItem))
                                    {
                                        var obj = (ListBoxItem)s;
                                        if (obj.Content.GetType() == typeof(Grid))
                                        {
                                            var context = (Service_Dagorlad.Info)obj.DataContext;
                                            if (context.perhabs_email == user.perhabs_email &&
                                                context.perhabs_fio == user.perhabs_fio)
                                            {
                                                var grid = (Grid)obj.Content;
                                                if (grid != null && grid.Children != null && grid.Children.Count > 0)
                                                {
                                                    foreach (var el in grid.Children)
                                                    {
                                                        if (el.GetType() == typeof(Border))
                                                        {
                                                            var border = (Border)el;
                                                            if (unread.Count() > 0)
                                                            {
                                                                var label = (Label)border.Child;
                                                                label.Content = unread.Count() > 0 ? unread.Count().ToString() : null;
                                                                border.Visibility = Visibility.Visible;
                                                                foreach (var mes in unread)
                                                                {
                                                                    if (!allreadyrecieved.Exists(x => x.dt == mes.dialog.dt && x.nickname == mes.dialog.nickname && 
                                                                    x.message == mes.dialog.message) && (((Service_Dagorlad.Info)((ListBoxItem)listofusersLB.SelectedItem).DataContext).perhabs_fio != context.perhabs_fio))
                                                                    {
                                                                        NotifyWindowCustom.ShowNotify(mes.dialog.nickname, mes.dialog.message ?? "[Cтикер]", "notify_chat.png", 5, false, typeof(Chat), "ALL");
                                                                        FlashWindow.Do_FlashWindow(this, 3);
                                                                        allreadyrecieved.Add(new Service_Dagorlad.Chat
                                                                        {
                                                                            dt = mes.dialog.dt,
                                                                            nickname = mes.dialog.nickname,
                                                                            message = mes.dialog.message,
                                                                        });
                                                                    }
                                                                }
                                                            }
                                                            else border.Visibility = Visibility.Hidden;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                }), DispatcherPriority.Background);
            }
            catch (Exception ex) { Console.WriteLine("GetUnreadForAllConversations " + ex.ToString()); }
        }
        private async Task FillDialog()
        {
            try
            {
                bool isrefreshing = false;
                await ServiceHost.Check_Dialog_Readed(from_id, to_id);
                var gettings_history = await ServiceHost.return_Dialog(from_id, to_id);
                if (dg.ItemsSource != null)
                {
                    var current_history = dg.ItemsSource.Cast<Service_Dagorlad.Chat>().ToList();
                    if (gettings_history != null && gettings_history.Count() > 0)
                    {
                        var unread = gettings_history.Where(p => !current_history.Any(l => p.dialog.dt == l.dt && p.dialog.message == l.message && p.dialog.email == l.email)).ToList();
                        if (unread.Count() > 0)
                        {
                            foreach (var s in unread)
                            {
                                if (s.dialog.email != Properties.Settings.Default.perhabsemail)
                                {
                                    NotifyWindowCustom.ShowNotify(s.dialog.nickname, s.dialog.message ?? "[Cтикер]", "notify_chat.png", 5, false, typeof(Chat), "ALL");
                                    FlashWindow.Do_FlashWindow(this, 3);
                                }
                            }
                            if (listofusersLB.SelectedItem != null && (listofusersLB.SelectedValue as ListBoxItem) != null && !isCommonDialog)
                            {
                                var select_dialog_from_history = unread.Select(x => x.dialog).ToList();
                                //var select_dialog_from_history = from i in unread select i.dialog;
                                current_history.AddRange(select_dialog_from_history);
                                CarretToBottom();
                                isrefreshing = true;
                            }
                        }
                    }
                    if (isrefreshing)
                        dg.ItemsSource = current_history;
                    //CheckOnline(current_history);
                }
                else
                {
                    if (gettings_history != null && !isgetoldhistory)
                    {
                        var select_dialog_from_history = gettings_history.Select(x => x.dialog).ToList();
                        // var select_dialog_from_history = from i in gettings_history select i.dialog;
                        dg.ItemsSource = select_dialog_from_history;
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
        private void SelectedItemEvent(object sender, RoutedEventArgs e)
        {
            isgetoldhistory = false;
            isfirsttimecarrete = false;
            dg.ItemsSource = null;
            var obj = (ListBoxItem)sender;
            if (((Service_Dagorlad.Info)(obj.DataContext)).perhabs_fio == "Общий")
            {
                allunread_common = 0;
                isCommonDialog = true;
                CurrentDialog.Content = ((Service_Dagorlad.Info)(obj.DataContext)).perhabs_fio;
                if (ServiceHost.list != null)
                    IsOnline.Content= ServiceHost.list.Count()+" человек";
            }
            else
            {
                isCommonDialog = false;
                if (obj.Content.GetType() == typeof(Grid))
                {
                    var grid = (Grid)obj.Content;
                    if (grid != null)
                    {
                        var item = (Service_Dagorlad.Info)grid.DataContext;
                        if (item != null)
                        {
                            CurrentDialog.Content = item.perhabs_fio;
                            IsOnline.Content = CheckOnline(item.perhabs_email) ? "Online" : "Offline";
                        }
                    }
                }
            }
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
            if ((Keyboard.Modifiers != ModifierKeys.Shift) && e.Key == Key.Enter)
            {
                var nickname = Properties.Settings.Default.perhabsfio;
                var email = Properties.Settings.Default.perhabsemail;
                if (!String.IsNullOrEmpty(MessageTB.Text) && isCommonDialog)
                    ServiceHost.AddMessage(email, String.IsNullOrEmpty(nickname) ? "Неизвестно" : nickname, MessageTB.Text, null);
                else if (!String.IsNullOrEmpty(MessageTB.Text) && !isCommonDialog)
                    ServiceHost.AddMessageDialog(new Service_Dagorlad.Dialog
                    {
                        dialog = new Service_Dagorlad.Chat
                        {
                            email = email,
                            nickname = String.IsNullOrEmpty(nickname) ? "Неизвестно" : nickname,
                            message = MessageTB.Text,
                            uri_images = null,
                        },
                        from = from_id,
                        to = to_id,
                    });
                MessageTB.Text = String.Empty;
            }
            if ((Keyboard.Modifiers == ModifierKeys.Shift) && e.Key == Key.Enter)
            { 
                MessageTB.Text += Environment.NewLine;
                MessageTB.CaretIndex = MessageTB.Text.Length;
            }
        }
        private void Typingtb_GotFocus(object sender, RoutedEventArgs e)
        {
            if(((TextBox)sender).Text == "Написать сообщение...")
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
                if(img.ToString().Contains("smiles"))
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
            folders = folders.GroupBy(x => x).Select(x => x.First()).OrderBy(x=>x).ToList();
            foreach(var folder in folders)
            {
                var folder_btn = new Button() { Content = folder };
                folder_btn.Click += (q, g) =>
                {
                    smilesgrid.Children.Clear();
                    foreach (var s in f.Where(x=> Uri.UnescapeDataString(x).Contains(folder)))
                    {
                        var button = new Button();
                        button.Click += (q, g) =>
                        {
                            var nickname = Properties.Settings.Default.perhabsfio;
                            var email = Properties.Settings.Default.perhabsemail;
                            if(isCommonDialog)
                            ServiceHost.AddMessage(email, String.IsNullOrEmpty(nickname) ? "Неизвестно" : nickname, null, s);
                            else if (!isCommonDialog)
                                ServiceHost.AddMessageDialog(new Service_Dagorlad.Dialog
                                {
                                    dialog = new Service_Dagorlad.Chat
                                    {
                                        email = email,
                                        nickname = String.IsNullOrEmpty(nickname) ? "Неизвестно" : nickname,
                                        message =null,
                                        uri_images = s,
                                    },
                                    from = from_id,
                                    to = to_id,
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

        private void ListUsersToShortess_Click(object sender, RoutedEventArgs e)
        {
            var obj = listofusersLB.Items.Cast<ListBoxItem>();
            foreach (var s in obj)
            {
                if(s.Content.GetType()==typeof(Grid))
                {
                    var grid = (Grid)s.Content;
                    if (grid.ColumnDefinitions[1].ActualWidth>0)
                    {
                        grid.ColumnDefinitions[1].Width = new GridLength(0, GridUnitType.Pixel);
                        //grid.ColumnDefinitions[2].Width = new GridLength(0, GridUnitType.Pixel);
                        ListUsersToShortess_Image.Source= new System.Windows.Media.Imaging.BitmapImage(new Uri(@"/Dagorlad;component/Resources/rightarrow.png", 
                            UriKind.Relative));
                    }
                    else
                    {
                        grid.ColumnDefinitions[1].Width = new GridLength(0, GridUnitType.Star);
                        //grid.ColumnDefinitions[2].Width = new GridLength(0, GridUnitType.Star);
                        ListUsersToShortess_Image.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(@"/Dagorlad;component/Resources/leftarrow.png",
                            UriKind.Relative));
                    }
                }
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            MessageTB.Focus();
        }
    }
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
            var isimage = (string)value;
            if (isimage != null)
                return Visibility.Visible;
            else return Visibility.Collapsed;
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
}
