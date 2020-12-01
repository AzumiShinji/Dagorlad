using Dagorlad.classes;
using Gma.System.MouseKeyHook;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace Dagorlad
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool isInvertThemes = Properties.Settings.Default.isInvertTheme;
        private void Themes()
        {
            if (isInvertThemes)
            {
                BackgroundApp.ImageSource = null;
                border.Background = System.Windows.Media.Brushes.White;
                this.Resources["ColorOne"] = new SolidColorBrush(Colors.Black);
                this.Resources["ColorTwo"] = new SolidColorBrush(Colors.White);
                this.Resources["ColorTextBox"] = new SolidColorBrush(Colors.Black);
                chatimg.Source= new BitmapImage(new Uri(@"/Dagorlad;component/Resources/black/chat.png", UriKind.Relative));
                imageplus.Source = new BitmapImage(new Uri(@"/Dagorlad;component/Resources/black/plus.png", UriKind.Relative));
                OnOff.Source= new BitmapImage(new Uri(@"/Dagorlad;component/Resources/black/button_off.png", UriKind.Relative));
                RefreshHookImg.Source= new BitmapImage(new Uri(@"/Dagorlad;component/Resources/black/refresh.png", UriKind.Relative));
                filesimg.Source= new BitmapImage(new Uri(@"/Dagorlad;component/Resources/black/files.png", UriKind.Relative));
                folderimg.Source= new BitmapImage(new Uri(@"/Dagorlad;component/Resources/black/folder.png", UriKind.Relative));
                bugsfiles.Source=new BitmapImage(new Uri(@"/Dagorlad;component/Resources/black/bug.png", UriKind.Relative));
                image.Source= new BitmapImage(new Uri(@"/Dagorlad;component/Resources/black/logo.png", UriKind.Relative));
                employeesimg.Source= new BitmapImage(new Uri(@"/Dagorlad;component/Resources/black/Employees.png", UriKind.Relative));
                settingsimg.Source = new BitmapImage(new Uri(@"/Dagorlad;component/Resources/black/settings.png", UriKind.Relative));
                minimizeimg.Source = new BitmapImage(new Uri(@"/Dagorlad;component/Resources/black/minimize.png", UriKind.Relative));
                closeimg.Source = new BitmapImage(new Uri(@"/Dagorlad;component/Resources/black/close.png", UriKind.Relative));
                PinOnOff.Source = new BitmapImage(new Uri(@"/Dagorlad;component/Resources/black/pinunlock.png", UriKind.Relative));
                GetScriptCoordinatesImg.Source = new BitmapImage(new Uri(@"/Dagorlad;component/Resources/black/plus.png", UriKind.Relative));
                SearchCatalog_img.Source= new BitmapImage(new Uri(@"/Dagorlad;component/Resources/black/lupa.png", UriKind.Relative));
                SearchHandbook_img.Source = new BitmapImage(new Uri(@"/Dagorlad;component/Resources/black/lupa.png", UriKind.Relative));
                InfoAboutOrg_RichTextBox_Search_img.Source = new BitmapImage(new Uri(@"/Dagorlad;component/Resources/black/lupa.png", UriKind.Relative));
                maximg.Source = new BitmapImage(new Uri(@"/Dagorlad;component/Resources/black/maximize.png", UriKind.Relative));
                ContextMenu CMS = FindResource("CMS") as ContextMenu;
                CMS.Resources["ColorOne"]= new SolidColorBrush(Colors.Black);
                CMS.Resources["ColorTwo"] = new SolidColorBrush(Colors.White);
            }
            else
            {
                this.Resources["ColorTextBox"] = new SolidColorBrush(Colors.White);
            }
        }
        private IKeyboardMouseEvents m_Events;
        public MainWindow()
        {
#if (!DEBUG)
                isNetFramework.GetDotNet();
                Update.Start_Update(false);
                //Access.CheckAllow();
                ReplaceOldConfig.CheckAndReplace(false);
#endif
            InitializeComponent();
            StartHideAndCheckRegKeyAutoStartUp();
            TurnOffScrollLock();
            LoadProperties();
            installCMS_default();
            VersionApp.Content = Logger.GetVersionApp() + " (" + Environment.UserName + ")";
            TimerToRefresh();
            Clipboard.Clear();
            ClipboardMonitor.OnClipboardChange += new ClipboardMonitor.OnClipboardChangeEventHandler(ClipboardMonitor_OnClipboardChange);
            ClipboardMonitor.Start();
            SubscribeGlobal();
            ForceTimerToUpdate_Start(30);//Force Updater for 24/7
            TimeNotifyBirthDay();
            InfoUpdateDataFunc();
            if (!CheckNeededIP())
                NotifyWindowCustom.ShowNotify(Assembly.GetEntryAssembly().GetName().Name + " есть органичения",
                    Assembly.GetEntryAssembly().GetName().Name + " запущен не в той подсети, некоторые функции будут недоступны" + Environment.NewLine +
                    "Текущий IP: " + IP[0] +
                    ", необходимый: " + IP[1], "No_network.png", 2, true, typeof(MainWindow), null, null);
            NotifyWindowCustom.ShowNotify(Assembly.GetEntryAssembly().GetName().Name, Assembly.GetEntryAssembly().GetName().Name + " работает в фоновом режиме", "logo.png", 2, true, typeof(MainWindow), null, null);
#if (!DEBUG)
            ConnectToCloud(false);
#endif
            this.Height = System.Windows.SystemParameters.WorkArea.Height * 0.6;
            this.Width = System.Windows.SystemParameters.WorkArea.Width * 0.65;
            Themes();
            //control cpu
#if (!DEBUG)
            ControlCPUUsage.CheckCPUUsage(true, 75, 3, TimeSpan.FromSeconds(110), TimeSpan.FromSeconds(10));
#endif
            //GetSN.WriteInfo();
            var email = Properties.Settings.Default.perhabsemail;
            if(!String.IsNullOrEmpty(email))
            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory+String.Format("{0}.email",Environment.UserName), email);
        }

#region Properties

        private void LoadProperties()
        {
            X.Text = Properties.Settings.Default.a_X.ToString();
            Y.Text = Properties.Settings.Default.a_Y.ToString();
            TimeUpToClick.Text = Properties.Settings.Default.a_Interval.ToString();
            PathDirectory.Text = Properties.Settings.Default.FolderDestination;
            isTurnOn = Properties.Settings.Default.OnOff; TurnFromProperties();
            isTurnOnScript = Properties.Settings.Default.isTurnOnScript; TurnFromProperties();
            LoadFromPropertiesToScriptGrid();
            LoadFromPropertiesToGridResources();
        }

        private void SaveToPropertiesFromScriptGrid()
        {
            Properties.Settings.Default.ScriptValues.Clear();
            foreach (var item in ListCoordinates)
                Properties.Settings.Default.ScriptValues.Add(item.OwnName + ";" + item.X + ";" + item.Y + ";" + item.Scenario);
            Properties.Settings.Default.Save();
        }

        private void LoadFromPropertiesToScriptGrid()
        {
            foreach (var item in Properties.Settings.Default.ScriptValues)
            {
                string[] _item = item.Split(';');
                try
                {
                    if (_item[1] != "0" && _item[2] != "0")
                        installScriptDataGrid(_item[0], Convert.ToDouble(_item[1]), Convert.ToDouble(_item[2]), Convert.ToInt32(_item[3]));
                }
                catch { }
            }
            ScriptDataGrid.CurrentCellChanged += (q, e) => { SaveToPropertiesFromScriptGrid(); };
        }

        private void SaveToPropertiesFromGridResources()
        {
            Properties.Settings.Default.ResourcesList.Clear();
            foreach (var item in ListResources)
                Properties.Settings.Default.ResourcesList.Add(item._Name + ";" + item._Source);
            Properties.Settings.Default.Save();
        }

        private void LoadFromPropertiesToGridResources()
        {
            foreach (var item in Properties.Settings.Default.ResourcesList)
            {
                string[] _item = item.Split(';');
                if (_item[0] != "0" && _item[0] != "0")
                    installItemResource(_item[0], _item[1]);
            }
        }
#endregion

#region AutoClicker
        int G_mode;
        private void ToExpansion(int _mode)
        {
            G_mode = _mode;
            window.WindowState = WindowState.Maximized;
            border.Margin = new Thickness(0);
            border.Opacity = 0.3;
            border.CornerRadius = new CornerRadius(0);
            MainGrid.Visibility = Visibility.Hidden;
            Cursor = Cursors.Cross;
        }
        private void OverExpansion()
        {
            window.WindowState = WindowState.Normal;
            border.Margin = new Thickness(10);
            border.Opacity = 1;
            border.CornerRadius = new CornerRadius(15);
            MainGrid.Visibility = Visibility.Visible;
            Cursor = Cursors.Arrow;
            if (G_mode == 1)
            {
                X.Text = System.Windows.Forms.Cursor.Position.X.ToString();
                Y.Text = System.Windows.Forms.Cursor.Position.Y.ToString();
                Properties.Settings.Default.a_X = System.Windows.Forms.Cursor.Position.X;
                Properties.Settings.Default.a_Y = System.Windows.Forms.Cursor.Position.Y;
                Properties.Settings.Default.Save();
            }
            if (G_mode == 2)
            {
                installScriptDataGrid("", System.Windows.Forms.Cursor.Position.X, System.Windows.Forms.Cursor.Position.Y, 0);
                SaveToPropertiesFromScriptGrid();
            }
        }

        private void GetCoordinates_Click(object sender, RoutedEventArgs e)
        {
            ToExpansion(1);
        }

        private void window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape && window.WindowState == WindowState.Maximized)
            {
                OverExpansion();
            }
        }

        private void window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && window.WindowState == WindowState.Maximized)
            {
                OverExpansion();
            }
        }

        bool ChangedStartUp = false;
        private void TimeUpToClick_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ChangedStartUp)
            {
                var g = Int32.TryParse(TimeUpToClick.Text, out int res);
                if (g)
                {
                    Properties.Settings.Default.a_Interval = Convert.ToInt32(res);
                    Properties.Settings.Default.Save();
                }
            }
            ChangedStartUp = true;
        }

        //This is a replacement for Cursor.Position in WinForms
        [DllImport("user32.dll")]
        static extern bool SetCursorPos(uint x, uint y);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, UIntPtr dwExtraInfo);

        public const int MOUSEEVENTF_LEFTDOWN = 0x02;
        public const int MOUSEEVENTF_LEFTUP = 0x04;

        //This simulates a left mouse click
        public static void LeftMouseClick(uint xpos, uint ypos)
        {
            SetCursorPos(xpos, ypos);
            mouse_event((uint)(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP), xpos, ypos, 0, UIntPtr.Zero);
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            LeftMouseClick(Convert.ToUInt32(X.Text), Convert.ToUInt32(Y.Text));
        }

        bool isAutoClickerDo = false;
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        private void AutoClickerDo()
        {
            try
            {
                if (isAutoClickerDo == false)
                {
                    dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
                    dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, Convert.ToInt32(TimeUpToClick.Text));
                    isAutoClickerDo = true;
                    dispatcherTimer.Start();
                    NotifyWindowCustom.ShowNotify("Автокликер включен", "Выключить автокликер - ScrollLock\nИнтервал: " + TimeUpToClick.Text + "ms", "notify_autoclicker.png", 2, false, typeof(MainWindow),null, null);
                }
                else
                {
                    NotifyWindowCustom.ShowNotify("Автокликер выключен", "Включить автокликер - ScrollLock\nИнтервал: " + TimeUpToClick.Text + "ms", "notify_autoclicker.png", 2, false, typeof(MainWindow),null, null);
                    dispatcherTimer.Stop(); dispatcherTimer.Tick -= new EventHandler(dispatcherTimer_Tick); isAutoClickerDo = false;
                }
            }
            catch { MessageBox.Show("Только натуральные целые числа!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning); }
        }

#endregion

#region GlobalHook

        private void SubscribeGlobal()
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                Unsubscribe();
                Subscribe(Hook.GlobalEvents());
            }), DispatcherPriority.Background);
        }

        private void Subscribe(IKeyboardMouseEvents events)
        {
            m_Events = events;
            m_Events.KeyDown += GlobalHookKeyPress;
            m_Events.MouseDownExt += GlobalHookMouseDownExt;
        }

        private void Unsubscribe()
        {
            if (m_Events == null) return;
            m_Events.KeyDown -= GlobalHookKeyPress;
            m_Events.MouseDownExt -= GlobalHookMouseDownExt;

            m_Events.Dispose();
            m_Events = null;
        }

        private void GlobalHookKeyPress(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.Scroll) //scrolllock
            {
                AutoClickerDo();
            }
            if (e.KeyCode == System.Windows.Forms.Keys.Pause) //pause
            {
                ClearFolder(PathDirectory.Text);
            }
            if (e.Control &&
                e.KeyCode == System.Windows.Forms.Keys.Space && InformationOrgs.Count == 1)
            {
                SendOrg();
            }
            if (e.Control && (
                e.KeyCode == System.Windows.Forms.Keys.D1 ||
                e.KeyCode == System.Windows.Forms.Keys.D2 ||
                e.KeyCode == System.Windows.Forms.Keys.D3 ||
                e.KeyCode == System.Windows.Forms.Keys.D4 ||
                e.KeyCode == System.Windows.Forms.Keys.D5 ||
                e.KeyCode == System.Windows.Forms.Keys.D6 ||
                e.KeyCode == System.Windows.Forms.Keys.D7 ||
                e.KeyCode == System.Windows.Forms.Keys.D8 ||
                e.KeyCode == System.Windows.Forms.Keys.D9
                ) && isTurnOnScript)
                DoScript(Convert.ToInt32(e.KeyCode.ToString().Substring(1, 1)));

            if (e.Control && (
                 e.KeyCode == System.Windows.Forms.Keys.NumPad1 ||
                 e.KeyCode == System.Windows.Forms.Keys.NumPad2 ||
                 e.KeyCode == System.Windows.Forms.Keys.NumPad3 ||
                 e.KeyCode == System.Windows.Forms.Keys.NumPad4 ||
                 e.KeyCode == System.Windows.Forms.Keys.NumPad5 ||
                 e.KeyCode == System.Windows.Forms.Keys.NumPad6 ||
                 e.KeyCode == System.Windows.Forms.Keys.NumPad7 ||
                 e.KeyCode == System.Windows.Forms.Keys.NumPad8 ||
                 e.KeyCode == System.Windows.Forms.Keys.NumPad9
                ) && isTurnOnScript)
                DoScript(Convert.ToInt32(e.KeyCode.ToString().Substring(6, 1)));
        }

        private void GlobalHookMouseDownExt(object sender, MouseEventExtArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right && isTurnOn)
            {
                Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                {
                    ContextMenu CMS = FindResource("CMS") as ContextMenu;
                    CMS.Placement = System.Windows.Controls.Primitives.PlacementMode.MousePoint;
                    CMS.HorizontalOffset = -CMS.ActualWidth;
                    CMS.VerticalOffset = -CMS.ActualHeight;
                    CMS.IsOpen = false;
                    CMS.IsOpen = true;
                }));
            }
            if (e.Button == System.Windows.Forms.MouseButtons.Middle && isTurnOnScript)
            {
                DoScript();
            }
        }

#endregion

#region Other
        ServiceHost client;
        DispatcherTimer host_timer;
        public async Task ConnectToCloud(bool isreconnect)
        {
            client = new ServiceHost();
            host_timer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(1), };
            try
            {
                await client.Create_New_Client();
                if (client != null)
                {
                    var pro_email = Properties.Settings.Default.perhabsemail;
                    var pro_fio = Properties.Settings.Default.perhabsfio;
                    if (String.IsNullOrEmpty(pro_email) || String.IsNullOrEmpty(pro_fio))
                    {
                        var perhabslistnames = await GetInfoAboutUser.FindName();
                        if (perhabslistnames != null && perhabslistnames.Count() > 0)
                        {
                            var values = perhabslistnames.OrderByDescending(x => x.count).ToList();
                            bool isanswered = false;
                            foreach (var s in values)
                            {
                                var g = MessageBox.Show("Привет!\nПросьба уточнить: Вы " + s.fio + "?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
                                if (g == MessageBoxResult.Yes)
                                {
                                    isanswered = true;
                                    client.new_info.perhabs_email = Properties.Settings.Default.perhabsemail = s.email;
                                    client.new_info.perhabs_fio = Properties.Settings.Default.perhabsfio = s.fio;
                                    client.new_info.isanswered = true;
                                    Properties.Settings.Default.Save();
                                    break;
                                }
                            }
                            if (!isanswered)
                            {
                                client.new_info.perhabs_email = String.Join(",\n", values.Select(x => x.email));
                                client.new_info.perhabs_fio = String.Join(",\n", values.Select(x => x.fio));
                                client.new_info.isanswered = false;
                            }
                        }
                    }
                    else
                    {
                        client.new_info.perhabs_email = pro_email;
                        client.new_info.perhabs_fio = pro_fio;
                        client.new_info.isanswered = true;
                    }
                    host_timer.Tick += async (q, e) =>
                    {
                        await Dispatcher.BeginInvoke(new Action(async () =>
                        {
                            if (ServiceHost.client != null)
                            {
                                if (isWasDisconnected)
                                {
                                    isWasDisconnected = false;
                                    SetDisconnected(set_status_connection.StandartStatus, 0);
                                }

                                client.Update();
                                if (ServiceHost.list != null && ServiceHost.list.Count() > 0)
                                {
                                    CountUserslbl.Content = ServiceHost.list.Count();
                                    CountUsersdg.ItemsSource = ServiceHost.list;
                                }
                            }
                            else
                            {
                                isWasDisconnected = true;
                                host_timer.Stop();
                                SetDisconnected(set_status_connection.NotConnection, timeout);
                                await Task.Delay(timeout);
                                SetDisconnected(set_status_connection.TryConnection, 0);
                                await ConnectToCloud(true);
                                host_timer.Start();
                            }
                        }));
                    };
                    host_timer.Start();
                    //start
                    if (!isreconnect)
                    {
                        var chat = new Chat();
                        Notify.ShowInTrayDialog();
                        chat.Hide();
                    }
                }

            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        }
        bool isWasDisconnected = false;
        private int timeout = 10000;
        enum set_status_connection
        {
            NotConnection=0,
            TryConnection=1,
            StandartStatus=2,
        }
        private async void SetDisconnected(set_status_connection status, int timeout)
        {
            switch ((int)status)
            {
                case 0:
                    {
                        await Application.Current.Dispatcher.BeginInvoke(new Action(async () =>
                        {
                            foreach (Window window in Application.Current.Windows)
                            {
                                if (window.GetType() == typeof(Chat))
                                {
                                    (window as Chat).timer.Stop();
                                    (window as Chat).StatusConnectionlbl.Background = System.Windows.Media.Brushes.IndianRed;
                                    (window as Chat).StatusConnectionlbl.Content = "Нет подключения, следующая попытка через " + (timeout / 1000) + " сек.";
                                    (window as Chat).HeaderGrid.RowDefinitions[0].Height = new GridLength(10, GridUnitType.Star);
                                    (window as Chat).HeaderGrid.RowDefinitions[1].Height = new GridLength(0, GridUnitType.Pixel);
                                    (window as Chat).HeaderGrid.RowDefinitions[2].Height = new GridLength(0, GridUnitType.Pixel);
                                }
                            }
                        }));
                        break;
                    }
                case 1:
                    {
                        await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            foreach (Window window in Application.Current.Windows)
                            {
                                if (window.GetType() == typeof(Chat))
                                {
                                    (window as Chat).StatusConnectionlbl.Background = System.Windows.Media.Brushes.CadetBlue;
                                    (window as Chat).StatusConnectionlbl.Content = "Попытка подключения...";
                                    (window as Chat).HeaderGrid.RowDefinitions[0].Height = new GridLength(10, GridUnitType.Star);
                                    (window as Chat).HeaderGrid.RowDefinitions[1].Height = new GridLength(0, GridUnitType.Pixel);
                                    (window as Chat).HeaderGrid.RowDefinitions[2].Height = new GridLength(0, GridUnitType.Pixel);
                                    timeout = 10000;
                                }
                            }
                        }));
                        break;
                    }
                case 2:
                    {
                        await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            foreach (Window window in Application.Current.Windows)
                            {
                                if (window.GetType() == typeof(Chat))
                                {
                                    (window as Chat).timer.Start();
                                    (window as Chat).StatusConnectionlbl.Background = System.Windows.Media.Brushes.Transparent;
                                    (window as Chat).StatusConnectionlbl.Content = null;
                                    (window as Chat).HeaderGrid.RowDefinitions[0].Height = new GridLength(0, GridUnitType.Pixel);
                                    (window as Chat).HeaderGrid.RowDefinitions[1].Height = new GridLength(10, GridUnitType.Star);
                                    (window as Chat).HeaderGrid.RowDefinitions[2].Height = new GridLength(10, GridUnitType.Star);
                                }
                            }
                        }));
                        break;
                    }
            }
        }
        private void Chatbtn_Click(object sender, RoutedEventArgs e)
        {
            if (NotifyWindowCustom.IsWindowOpen(typeof(Chat)))
            {
                foreach (Window window in Application.Current.Windows)
                {
                    if (window.GetType() == typeof(Chat))
                    {
                        (window).Show();
                        (window).Activate();
                        (window).WindowState=WindowState.Normal;
                    }
                }
            }
            else
            {
                var g = new Chat();
                g.WindowState = WindowState.Normal;
                g.Show();
            }
        }
        private void CountUsersBtn_Click(object sender, RoutedEventArgs e)
        {
            CountUsersPopup.IsOpen = true;
        }
        public static string[] IP = new string[2];
        public static bool CheckNeededIP()
        {
            try
            {
                string NeededIp = null;
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    IP[0] = ip.ToString();
                    IP[1] = "10.97.*.*";
                    if (ip != null && ip.ToString().Contains('.'))
                        if (ip.ToString().Split('.')[1] == "97")
                            NeededIp = ip.ToString();
                }
                if (NeededIp == null) return false;
                else return true;
            }
            catch (Exception ex) { NotifyWindowCustom.ShowNotify("Ошибка: " + ex.HResult.ToString(), ex.Message, "No_network.png", 15000, false, typeof(MainWindow), null, null); return false; }
        }

        private void WindowStateHidden_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try { this.DragMove(); } catch { }
        }

        private async void RefreshHook_Click(object sender, RoutedEventArgs e)
        {
            RefreshHook.IsEnabled = false;
            if (!isInvertThemes)
                RefreshHookImg.Source = new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), "Resources/wait.png"));
            else RefreshHookImg.Source = new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), "Resources/black/wait.png"));
            SubscribeGlobal();
            await After5secEnableRefresh();
        }

        private async Task After5secEnableRefresh()
        {
            await Task.Delay(5000);
            RefreshHook.IsEnabled = true;
            if (!isInvertThemes)
                RefreshHookImg.Source = new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), "Resources/refresh.png"));
            else RefreshHookImg.Source = new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), "Resources/black/refresh.png"));
        }

        DispatcherTimer ExecuteTimerRefresh = new DispatcherTimer();
        private void TimerToRefresh()
        {
            ExecuteTimerRefresh.Interval = new TimeSpan(0, 1, 0);
            ExecuteTimerRefresh.Tick += (q, e) =>
            {
                RefreshHook.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Primitives.ButtonBase.ClickEvent));
            };
            ExecuteTimerRefresh.Start();
        }

        //Notify Tray
        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == System.Windows.WindowState.Minimized)
            {
                this.Hide();
            }
            base.OnStateChanged(e);
        }

        private void SettingsWindow_Click(object sender, RoutedEventArgs e)
        {
            Window _Settings = new SettingsMenu();
            _Settings.Owner = this;
            _Settings.ShowInTaskbar = false;
            _Settings.Topmost = isTurnOnPin;
            _Settings.ShowDialog();
        }

        [DllImport("user32.dll")]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);
        public void TurnOffScrollLock()
        {
            if (Keyboard.GetKeyStates(Key.Scroll) == KeyStates.Toggled)
            {
                keybd_event(0x91, 0x45, 0x1, (UIntPtr)0);
                keybd_event(0x91, 0x45, 0x1 | 0x2, (UIntPtr)0);
            }
        }

        private void CloseApplication_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Process.GetCurrentProcess().Kill();
        }
#endregion

#region ClearFolder
        private void TextBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                PathDirectory.Text = dialog.SelectedPath;
                Properties.Settings.Default.FolderDestination = PathDirectory.Text;
                Properties.Settings.Default.Save();
            }
        }

        public async void ClearFolder(string dir)
        {
            await Task.Run(() =>
            {
                bool _error = false;
                try
                {
                    foreach (FileInfo d in (new DirectoryInfo(@dir).GetFiles()))
                    {
                        d.Delete();
                    }

                    foreach (DirectoryInfo d in (new DirectoryInfo(@dir).GetDirectories()))
                    {
                        d.Delete(true);
                    }
                }
                catch (Exception g)
                {
                    _error = true;
                    NotifyWindowCustom.ShowNotify("Не удалось очистить директорию", g.Message, "notify_warning.png", 2, false, typeof(MainWindow), null, null);
                }
                finally
                {
                    Dispatcher.Invoke(() =>
                    {
                        if (!_error)
                            NotifyWindowCustom.ShowNotify("Директория очищена", PathDirectory.Text, "notify_clear.png", 2, false, typeof(MainWindow), null, null);
                    }, DispatcherPriority.ContextIdle);
                }
            });
        }
#endregion

#region CMS
        string RDYSD;
        private void SendText(string textClipBoard)
        {
            if (textClipBoard != null && textClipBoard.Trim() != "")
                try
                {
                    Clipboard.Clear();
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        textClipBoard = textClipBoard.Replace("{SD}", RDYSD).Replace("{time}", DateTime.Now.ToString());
                        Clipboard.SetDataObject(textClipBoard);
                        System.Windows.Forms.InputLanguage.CurrentInputLanguage =
                        System.Windows.Forms.InputLanguage.FromCulture(new System.Globalization.CultureInfo("en-US"));
                        System.Windows.Forms.SendKeys.SendWait("^{v}");
                        RDYSD = "__";
                        isFromCMS = true;
                    })).Wait();
                }
                catch (Exception ex)
                {
                    SendBugsReport(ex);
                    Thread.Sleep(10);
                    SubscribeGlobal();
                }
        }

        private void installCMS_default()
        {
            ContextMenu CMS = FindResource("CMS") as ContextMenu;
            CMS.Items.Clear();

            var _Header = new MenuItem();
            _Header.HorizontalAlignment = HorizontalAlignment.Center;
            _Header.HorizontalContentAlignment = HorizontalAlignment.Center;
            _Header.IsEnabled = false;
            _Header.Margin = new Thickness(20,0,0,0);

#region Обращения
            var _SD = new MenuItem(); _SD.Header = "Обращения"; //0
            var _InformationAdded = new MenuItem(); _InformationAdded.Header = "Информация добавлена (SD)";
            _InformationAdded.Click += (s, e) => { SendText("Информация была добавлена в обращение - {SD}."); };
            var _SCRResolve = new MenuItem(); _SCRResolve.Header = "СЦ: Решение предоставлено (SCR)";
            _SCRResolve.Click += (s, e) => { SendText("СЦ: Решение предоставлено - {SD}."); };
            var _WorkContinues = new MenuItem(); _WorkContinues.Header = "Работы продолжаются (SD)";
            _WorkContinues.Click += (s, e) => { SendText("Уважаемый пользователь, Работы над Вашим обращением продолжаются в {SD}."); };
            var _ReturnToWork = new MenuItem(); _ReturnToWork.Header = "Возвращено на доработку (SD)";
            _ReturnToWork.Click += (s, e) => { SendText("Обращение {SD} возвращено на доработку с дополнительной информацией."); };
#endregion
#region Решено на 1-ой линии
            var _Resolve = new MenuItem(); _Resolve.Header = "Решено на 1-ой линии"; //1
            var _Consultation = new MenuItem(); _Consultation.Header = "Консультация (SD)";
            _Consultation.Click += (s, e) => { SendText("Консультация по {SD}."); };
            var _OnMail = new MenuItem(); _OnMail.Header = "Вышлет информацию по почте";
            _OnMail.Click += (s, e) => { SendText("Пользователь сообщил, что собирается отправить информацию для обращения по почте."); };
            var _Dublicate = new MenuItem(); _Dublicate.Header = "Дубликат (SD)";
            _Dublicate.Click += (s, e) => { SendText("Закрыто по причине: Текущее обращение является дубликатом {SD}."); };
            var _SPAM = new MenuItem(); _SPAM.Header = "СПАМ";
            _SPAM.Click += (s, e) => { SendText("Данное обращение судя по описанию является СПАМОМ, было решено его закрыть."); };
            var _HungUp = new MenuItem(); _HungUp.Header = "Пользователь бросил трубку";
            _HungUp.Click += (s, e) => { SendText("Обращение не предоставляется возможным зарегистрировать, поскольку пользователь прервал разговор - бросил трубку."); };
            var _Refused = new MenuItem(); _Refused.Header = "Отказался от обращения";
            _Refused.Click += (s, e) => { SendText("Пользователь отказался от регистрации обращения."); };
            var _AskData = new MenuItem(); _AskData.Header = "Запрос данных (Скриншот,ИНН,ФИО) (SD)";
            _AskData.Click += (s, e) => { SendText("Добрый день.\nДля подробного анализа просим предоставить детальное описание проблемы, пошаговые скриншоты всех выполняемых действий, " +
                "на которых будет видна фиксирующая данное описание информация.\nТак же просьба указать ИНН организации и ФИО контактного лица (полностью).\n " +
                "Запрашиваемую информацию просьба прислать на адрес support_###@roskazna.ru с указанием в теме письма номера обращения {SD}."); };
            var _GISGMPURN = new MenuItem(); _GISGMPURN.Header = "Запрос данных (УРН) (SD)";
            _GISGMPURN.Click += (s, e) => { SendText("Добрый день\nПросьба указать УРН (Уникальный регистрационный номер, который выдается в Федеральном казначействе " +
                "(либо в Территориальном органе Федерального казначейства) при регистрации как участника ГИС ГМП) и прислать по адресу support_gisgmp@roskazna.ru" +
                " указав в теме письма номера обращения {SD}."); };
            var _USER_SMEV = new MenuItem(); _USER_SMEV.Header = "СМЭВ (SMEV@ROSKAZNA.RU)";
            _USER_SMEV.Click += (s, e) => { SendText("СМЭВ (SMEV@ROSKAZNA.RU)"); };
            var _USER_PHYS = new MenuItem(); _USER_PHYS.Header = "ФИЗИЧЕСКОЕ ЛИЦО (NOREPLY@MAIL.RU)";
            _USER_PHYS.Click += (s, e) => { SendText("ФИЗИЧЕСКОЕ ЛИЦО (NOREPLY@MAIL.RU)"); };
            var _EIS = new MenuItem(); _EIS.Header = "ЕИС";
            _EIS.Click += (s, e) =>
            {
                SendText("По вопросам работы с Официальным сайтом ЕИС Вы можете обращаться в единую круглосуточную службу поддержки пользователей:" +
"\n- многоканальные телефоны: 8-495-811-03-33; 8-800-333-81-11;" +
"\n- электронная почта: helpdesk@zakupki.gov.ru;" +
"\n- факс: (499) 811-03-33.");
            };
            var _EB_PUZ = new MenuItem(); _EB_PUZ.Header = "ЭБ ПУЗ";
            _EB_PUZ.Click += (s, e) =>
            {
                SendText("Формирование и утверждение Плана закупок и Плана-графика закупок на 2019 год и плановый период 2020 и 2021 год (внесение изменений) необходимо осуществлять в Подсистеме бюджетного планирования ГИИС «Электронный бюджет» МинФина." +
"\nРуководство пользователя размещено на официальном сайте Минфина России в сети «Интернет» в разделе «Информационные системы Минфина России / Подсистема управления закупками системы «Электронный бюджет»." +
"\nДля решения вопросов, связанных с функционированием системы «Электронный бюджет», необходимо обращаться в службу поддержки по телефонам 8 - 800 - 250 - 12 - 17, 8 - 800 - 350 - 02 - 18 или формировать заявки в разделе «Техническая поддержка / Обращения в техническую поддержку» системы «Электронный бюджет», так же можно отправить обращение для регистрации на  support.budgetplan@minfin.ru.");
            };
            var _EB = new MenuItem(); _EB.Header = "Бюджетное планирование";
            _EB.Click += (s, e) =>
            {
                SendText("Уважаемый пользователь! По вопросам работы программного комплекса «Бюджетное планирование» Минфина России рекомендуется обращаться в круглосуточную службу поддержки пользователей: " +
"\nМногоканальные телефоны: 8 (800) 350-02-18,8 (800) 333-62-26, 8 (800) 250-12-17 ;\nЭлектронная почта: support.budgetplan@minfin.ru ;\nСайт: https://help.bars-open.ru/index.php?/BARS .");
            };
            var _SMEV = new MenuItem(); _SMEV.Header = "Вопрос по СМЭВ";
            _SMEV.Click += (s, e) => { SendText("Рекомендуем ознакомиться с информацией на портале: http://smev.gosuslugi.ru/portal/ ."); };
            var _ESIA = new MenuItem(); _ESIA.Header = "Служба поддержки ЕСИА";
            _ESIA.Click += (s, e) =>
            {
                SendText("Служба поддержки ЕСИА (Единая система идентификации и аутентификации):" +
"\n- Ответы на популярные вопросы: https://www.gosuslugi.ru/help ;" +
"\n- Бесплатный номер по России: 8 800 100 70 10;" +
"\n- Для звонков из-за границы: +7 495 727-47-47;" +
"\n- Для мобильных телефонов: 115;" +
"\n- Задайте вопрос через форму обратной связи: https://www.gosuslugi.ru/feedback .");
            };
            var _OGRN = new MenuItem(); _OGRN.Header = "ОГРН не найден в БД";
            _OGRN.Click += (s, e) => { SendText("Проблема связанная с функционированием РРГУ необходимо обратиться в службу технической поддержки: rgu@gosuslugi.ru ."); };
            var _GISZKH = new MenuItem(); _GISZKH.Header = "Вопрос по ГИС ЖКХ";
            _GISZKH.Click += (s, e) => { SendText("Уважаемый пользователь!\nВам необходимо обратиться в службу поддержки ГИС ЖКХ:\nТелефон службы поддержки ГИС ЖКХ - 8 800 302-03-05 ."); };
            var _ConnEB = new MenuItem(); _ConnEB.Header = "Вопрос по подключению к ЭБ";
            _ConnEB.Click += (s, e) => { SendText("Уважаемый пользователь!\nПо данному вопросу Вам необходимо обратиться к регистратору в Ваш ТоФК."); };
            var _UVEDREESTR = new MenuItem(); _UVEDREESTR.Header = "Уведомления ГАСУ срок рассмотрения Минэкономразвития";
            _UVEDREESTR.Click += (s, e) => { SendText("Уважаемый пользователь!\nУведомления рассматривают исключительно сотрудники Минэкономразвития России. К сожалению, у нас нет информации о сроках рассмотрения, и мы не можем ускорить данный процесс. Данный запрос необходимо отправить на почту Минэкономразвития России: reestr@economy.gov.ru  По вопросам выставленных несоответствий также необходимо обращаться в Минэкономразвития."); };
            var _GOSSLUZHBA = new MenuItem(); _GOSSLUZHBA.Header = "Госслужба РФ";
            _GOSSLUZHBA.Click += (s, e) => { SendText("Уважаемый пользователь!\nПо данному вопросу Вам необходимо обратиться по телефону: 8-800-444-01-99 или на почту: rezerv@minsvyaz.ru (https://gossluzhba.gov.ru) ."); };
            var _SZ_ARM = new MenuItem(); _SZ_ARM.Header = "Стандартный запрос";
            _SZ_ARM.Click += (s, e) =>
            {
                SendText("Заявка на «{SD}» инициируется пользователем на портале самообслуживания ПУПЭ, через соответствующий стандартный запрос." +
"\nДля этого вам необходимо:" +
"\n-Выполнить вход на портал под своей учетной записью;" +
"\n-Перейти в пункт меню «Каталог стандартных запросов»;" +
"\n-В соответствии с задачей выбрать тип «Стандартного запроса»;" +
"\n-Заполнить все необходимые поля;" +
"\n-Нажать кнопку «Отправка»." +
"\nТакже Вы можете узнать дополнительную информацию о портале в инструкции по данной ссылке: http://10.128.50.12/spec/instr/ess.html");
            };
            var _VIP = new MenuItem(); _VIP.Header = "VIP";
            _VIP.Click += (s, e) => { SendText("Уважаемый пользователь!\nДля Вашей организации создана выделенная линия для звонков для скорейшего решения Ваших проблем 8 495 214-77-48."); };
#endregion
#region Пользовательское
            var _SelfOwn = new MenuItem(); _SelfOwn.Header = "Пользовательское"; //3
            var ListSelfOwnJson = JsonProccess.ReadSettings();

            List<string> List_Already_Folder = new List<string>();

            foreach (var _citem in ListSelfOwnJson)
            {
                var item = new MenuItem(); item.Header = _citem.Name; item.Click += (s, e) => { SendText(_citem.Text); };

                if (_citem.Folder == null || _citem.Folder == "")
                    _SelfOwn.Items.Add(item);
                else
                {
                    var Folder = new MenuItem(); Folder.Header = _citem.Folder;

                    var ListMenuWithSimilarName = ListSelfOwnJson.Where(x => x.Folder == _citem.Folder).ToList();

                    if (ListMenuWithSimilarName.Count > 1)
                    {
                        foreach (var menu in ListMenuWithSimilarName)
                        {
                            if (!List_Already_Folder.Exists(x => x == menu.Folder.ToString()))
                            {
                                _SelfOwn.Items.Add(Folder);
                                List_Already_Folder.Add(Folder.Header.ToString());
                            }
                            var SubItem = new MenuItem(); SubItem.Header = menu.Name; SubItem.Click += (s, e) => { SendText(menu.Text); };
                            Folder.Items.Add(SubItem);
                        }
                    }
                    else
                        _SelfOwn.Items.Add(item);
                }
            }
            var _Custom = new MenuItem(); _Custom.Header = "Редактор меню";
            _Custom.Click += (s, e) =>
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    bool AllowToStartWindowCE = true;
                    foreach (var window in Application.Current.Windows)
                        if (window.GetType().Name == "CustomExample")
                            AllowToStartWindowCE = false;

                    if (AllowToStartWindowCE)
                    {
                        var WindowCE = new CustomExample();
                        bool isPrevState = false;
                        if (this.Visibility == Visibility.Visible)
                        {
                            this.Hide();
                            isPrevState = true;
                        }
                        if (WindowCE.ShowDialog() == true)
                        {
                            installCMS_default();
                            if (isPrevState)
                                this.Show();
                        }
                        else
                            if (isPrevState)
                            this.Show();
                        RefreshHook.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Primitives.ButtonBase.ClickEvent));
                    }
                    else
                    {
                        foreach (var window in Application.Current.Windows)
                            if (window.GetType().Name == "CustomExample")
                                (window as CustomExample).Activate();
                    }
                }));
            };

            var _Close = new MenuItem(); _Close.Header = "Закрыть";//4 
            _Close.Click += (s, e) => { CMS.IsOpen = false; };
#endregion
#region Эксперт
            var _Expert = new MenuItem(); _Expert.Header = "Эксперт"; //2
            var _GASU = new MenuItem(); _GASU.Header = "Запрос ГАСУ (SD)";
            _GASU.Click += (s, e) =>
            {
                SendText("Тема: Запрос дополнительной информации по обращению __ \n" +
"\n\nДля регистрации обращения в Службе технической поддержки ГАС Управления, необходимо указать следующие данные (желательно ответным письмом): " +
"\n- Данные контактного лица: ФИО полностью, рабочий номер телефона, должность;" +
"\n- Данные организации контактного лица (наименование полностью, ИНН);" +
"\n- Актуальный адрес электронной почты;" +
"\n- Подробное описание проблемы;" +
"\n- Пошаговые скриншоты воспроизведения проблемы." +
"\n\nПри направлении детализированной информации укажите в теме или теле письма номер обращения.");
            };
            var _GIS_GMP = new MenuItem(); _GIS_GMP.Header = "Запрос ГИС ГМП (SD)";
            _GIS_GMP.Click += (s, e) =>
            {
                SendText("Тема: Запрос дополнительной информации по обращению __ \n" +
"\n\nДля регистрации обращения в Службе технической поддержки ГИС ГМП, необходимо указать следующие данные (желательно ответным письмом): " +
"\n- УРН - уникальный регистрационный номер участника ГИС ГМП (Выдается в Вашем Территориальном Казначействе при регистрации);" +
"\n- ИНН, КПП;" +
"\n- Данные контактного лица: ФИО полностью, рабочий номер телефона, должность;" +
"\n- Актуальный адрес электронной почты;" +
"\n- Подробное описание проблемы;" +
"\n- Пошаговые скриншоты воспроизведения проблемы;" +
"\n- При ошибках в работе с начислениями: приложить XML-файлы (запроса (request), ответа (response))." +
"\n\nПри направлении детализированной информации укажите в теме или теле письма номер обращения.");
            };
            var _EBE = new MenuItem(); _EBE.Header = "Запрос ЭБ (SD)";
            _EBE.Click += (s, e) =>
            {
                SendText("Тема: Запрос дополнительной информации по обращению __ \n" +
                    "\n\nДля регистрации обращения в Службе технической поддержки Электронного бюджета, необходимо указать следующие данные (желательно ответным письмом): " +
                    "\n- Данные контактного лица: ФИО полностью, рабочий номер телефона, должность;" +
                    "\n- Данные организации контактного лица (наименование полностью, ИНН);" +
                    "\n- Актуальный адрес электронной почты;" +
                    "\n- Подсистема (Модуль) в которой возникает ошибка (Подсистема учета и отчетности, подсистема управления расходами итд...);" +
                    "\n- Подробное описание проблемы;" +
                    "\n- Пошаговые скриншоты воспроизведения проблемы." +
                    "\n\nПри направлении детализированной информации укажите в теме или теле письма номер обращения.");
            };
            var _GMU = new MenuItem(); _GMU.Header = "Запрос ГМУ (SD)";
            _GMU.Click += (s, e) =>
            {
                SendText("Тема: Запрос дополнительной информации по обращению __ \n" +
                    "\n\nДля регистрации обращения в Службе технической поддержки ГМУ (bus.gov.ru), необходимо указать следующие данные (желательно ответным письмом): " +
                    "\n- Данные контактного лица: ФИО полностью, рабочий номер телефона, должность;" +
                    "\n- Данные организации контактного лица (наименование полностью, ИНН);" +
                    "\n- Актуальный адрес электронной почты;" +
                    "\n- Подробное описание проблемы;" +
                    "\n- Пошаговые скриншоты воспроизведения проблемы." +
                    "\n\nПри направлении детализированной информации укажите в теме или теле письма номер обращения.");
            };
            var _SUFD = new MenuItem(); _SUFD.Header = "Запрос СУФД (SD)";
            _SUFD.Click += (s, e) =>
            {
                SendText("Тема: Запрос дополнительной информации по обращению __ \n" +
                    "\n\nДля регистрации обращения в Службе технической поддержки СУФД, необходимо указать следующие данные (желательно ответным письмом): " +
                    "\n- Данные контактного лица: ФИО полностью, рабочий номер телефона, должность;" +
                    "\n- Данные организации контактного лица (наименование полностью, ИНН);" +
                    "\n- Актуальный адрес электронной почты;" +
                    "\n- Подробное описание проблемы;" +
                    "\n- Пошаговые скриншоты воспроизведения проблемы;" +
                    "\n- Версия Континента АП (ПО для работы в СУФД);" +
                    "\n- В каком УФК обслуживается пользователь (область, либо код (Московская область - 73))." +
                    "\n\nПри направлении детализированной информации укажите в теме или теле письма номер обращения.");
            };
            var _ReportInfo = new MenuItem(); _ReportInfo.Header = "Запроса оставлен (Дата)";
            _ReportInfo.Click += (s, e) => { SendText(DateTime.Now + " По электронной почте направлен запрос на дополнительную информацию по обращению."); };
            var _Didnt = new MenuItem(); _Didnt.Header = "Информация непоступила (Дата)";
            _Didnt.Click += (s, e) => { SendText(DateTime.Now + " По Вашему обращению не поступил ответ на запрос дополнительной информации, заявка будет закрыта."); };
#endregion
            CMS.Items.Add(_Header);
            CMS.Items.Add(_SD);
            _SD.Items.Add(_InformationAdded);
            _SD.Items.Add(_SCRResolve);
            _SD.Items.Add(new Separator());
            _SD.Items.Add(_WorkContinues);
            _SD.Items.Add(_ReturnToWork);

            CMS.Items.Add(_Resolve);
            _Resolve.Items.Add(_Consultation);
            _Resolve.Items.Add(_OnMail);
            _Resolve.Items.Add(_Dublicate);
            _Resolve.Items.Add(_SPAM);
            _Resolve.Items.Add(_HungUp);
            _Resolve.Items.Add(_Refused);
            _Resolve.Items.Add(new Separator());
            _Resolve.Items.Add(_AskData);
            _Resolve.Items.Add(_GISGMPURN);
            _Resolve.Items.Add(_USER_SMEV);
            _Resolve.Items.Add(_USER_PHYS);
            _Resolve.Items.Add(new Separator());
            _Resolve.Items.Add(_EIS);
            _Resolve.Items.Add(_EB_PUZ);
            _Resolve.Items.Add(_EB);
            _Resolve.Items.Add(_SMEV);
            _Resolve.Items.Add(_ESIA);
            _Resolve.Items.Add(_OGRN);
            _Resolve.Items.Add(_GISZKH);
            _Resolve.Items.Add(_ConnEB);
            _Resolve.Items.Add(_UVEDREESTR);
            _Resolve.Items.Add(_GOSSLUZHBA);
            _Resolve.Items.Add(new Separator());
            _Resolve.Items.Add(_SZ_ARM);
            _Resolve.Items.Add(_VIP);

            CMS.Items.Add(_Expert);
            _Expert.Items.Add(_GASU);
            _Expert.Items.Add(_GIS_GMP);
            _Expert.Items.Add(_EBE);
            _Expert.Items.Add(_GMU);
            _Expert.Items.Add(_SUFD);
            _Expert.Items.Add(new Separator());
            _Expert.Items.Add(_ReportInfo);
            _Expert.Items.Add(new Separator());
            _Expert.Items.Add(_Didnt);

            CMS.Items.Add(_SelfOwn);
            _SelfOwn.Items.Add(new Separator());
            _SelfOwn.Items.Add(_Custom);

            CMS.Items.Add(new Separator());
            CMS.Items.Add(_Close);

#region CMS Style
            if (!Properties.Settings.Default.isInvertTheme)
            {
                _Close.Icon = new System.Windows.Controls.Image { Width = 16, Height = 16, VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center, Source = Imaging.CreateBitmapSourceFromHBitmap(Properties.Resources.CMS_Close.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions()) };
                _Resolve.Icon = new System.Windows.Controls.Image { Width = 16, Height = 16, VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center, Source = Imaging.CreateBitmapSourceFromHBitmap(Properties.Resources.CMS_Solve.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions()) };
                _SD.Icon = new System.Windows.Controls.Image { Width = 16, Height = 16, VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center, Source = Imaging.CreateBitmapSourceFromHBitmap(Properties.Resources.CMS_SD.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions()) };
                _Expert.Icon = new System.Windows.Controls.Image { Width = 16, Height = 16, VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center, Source = Imaging.CreateBitmapSourceFromHBitmap(Properties.Resources.CMS_Expert.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions()) };
                _SelfOwn.Icon = new System.Windows.Controls.Image { Width = 16, Height = 16, VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center, Source = Imaging.CreateBitmapSourceFromHBitmap(Properties.Resources.CMS_User.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions()) };
                _Custom.Icon = new System.Windows.Controls.Image { Width = 16, Height = 16, VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center, Source = Imaging.CreateBitmapSourceFromHBitmap(Properties.Resources.CMS_User_Settings.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions()) };
                var image = new System.Windows.Controls.Image { Width = 16, Height = 16, VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center, Source = Imaging.CreateBitmapSourceFromHBitmap(Properties.Resources.logomin.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions()) };
                RenderOptions.SetBitmapScalingMode(image, BitmapScalingMode.HighQuality);
                _Header.Icon = image;
            }
            else
            {
                _Close.Icon = new System.Windows.Controls.Image { Width = 16, Height = 16, VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center, Source = new BitmapImage(new Uri("pack://application:,,,/Dagorlad;component/Resources/black/CMS_Close.png")) };
                _Resolve.Icon = new System.Windows.Controls.Image { Width = 16, Height = 16, VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center, Source = new BitmapImage(new Uri("pack://application:,,,/Dagorlad;component/Resources/black/CMS_Solve.png")) };
                _SD.Icon = new System.Windows.Controls.Image { Width = 16, Height = 16, VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center, Source = new BitmapImage(new Uri("pack://application:,,,/Dagorlad;component/Resources/black/CMS_SD.png")) };
                _Expert.Icon = new System.Windows.Controls.Image { Width = 16, Height = 16, VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center, Source = new BitmapImage(new Uri("pack://application:,,,/Dagorlad;component/Resources/black/CMS_Expert.png")) };
                _SelfOwn.Icon = new System.Windows.Controls.Image { Width = 16, Height = 16, VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center, Source = new BitmapImage(new Uri("pack://application:,,,/Dagorlad;component/Resources/black/CMS_User.png")) };
                _Custom.Icon = new System.Windows.Controls.Image { Width = 16, Height = 16, VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center, Source = new BitmapImage(new Uri("pack://application:,,,/Dagorlad;component/Resources/black/CMS_User_Settings.png")) };
                var image = new System.Windows.Controls.Image { Width = 16, Height = 16, VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center, Source = new BitmapImage(new Uri("pack://application:,,,/Dagorlad;component/Resources/black/logo.png")) };
                RenderOptions.SetBitmapScalingMode(image, BitmapScalingMode.HighQuality);
                _Header.Icon = image;
            }
#endregion
        }

        Task task;
        public async void ClipboardMonitor_OnClipboardChange(ClipboardFormat format, object data)
        {
            if (format != ClipboardFormat.Text) return;
            if (task != null && !task.IsCompleted) return;
            task = Clipboard_Work();
            await task;
        }

        bool isFromCMS = false;
        public async Task Clipboard_Work()
        {
            if (!isFromCMS)
                try
                {
                    await Dispatcher.Invoke(async () =>
                     {
                         if (Clipboard.ContainsText(TextDataFormat.Text))
                         {
                             string _Clipboard = (string)Clipboard.GetData(DataFormats.UnicodeText);
                             if (_Clipboard.Length < 20 && _Clipboard.Trim() != "" && _Clipboard.Trim() != String.Empty && _Clipboard.Trim() != null)
                             {
                                 string ClipboardForSD = _Clipboard.Trim().ToUpper();
                                 string Temp_SD, Completed_SD = String.Empty;
                                 bool IM = false;
                                 if (ClipboardForSD.StartsWith("SD"))
                                     Temp_SD = ClipboardForSD.Substring(2);
                                 else if (ClipboardForSD.StartsWith("IM"))
                                 { IM = true; Temp_SD = ClipboardForSD.Substring(2); }
                                 else
                                     Temp_SD = ClipboardForSD;

                                 foreach (var s in Temp_SD)
                                 {
                                     if (int.TryParse(s.ToString(), out int INT))
                                     {
                                         Completed_SD += INT;
                                     }
                                     else break;
                                 }

                                 var inn = await LookingListRUNBP(_Clipboard.Trim());
                                 if (!inn)
                                 {
                                     if (Completed_SD.Length >= 10 && !IM) Completed_SD = "SD" + Completed_SD;
                                     else if (IM && Completed_SD.Length >= 10) Completed_SD = "IM" + Completed_SD;
                                     if (Completed_SD.Length == 7) Completed_SD = "SCR#" + Completed_SD;

                                     if (Completed_SD.StartsWith("SD") || Completed_SD.StartsWith("IM") || Completed_SD.StartsWith("SCR#"))
                                     {
                                         RDYSD = Completed_SD;

                                         string Notify_SD = String.Empty;
                                         foreach (var s in Completed_SD)
                                             if (System.Text.RegularExpressions.Regex.IsMatch(s.ToString(), @"^[a-zA-Z]+$"))
                                                 Notify_SD += s;
                                         NotifyWindowCustom.ShowNotify(Completed_SD, "Номер " + Notify_SD + " скопирован\nОрганизаций по данному коду не обнаружено", "Notify_SD.png", 6, false, typeof(MainWindow), null, null);
                                     }
                                 }
                             }
                         }
                     }, DispatcherPriority.Background);
                }
                catch (Exception ex)
                {
                    SendBugsReport(ex);
                    SubscribeGlobal();
                }
            isFromCMS = false;
        }
#endregion

#region Script
        public class CoordinatesScript
        {
            public string OwnName { get; set; }
            public double X { get; set; }
            public double Y { get; set; }
            public int Scenario { get; set; }
        }
        List<CoordinatesScript> ListCoordinates = new List<CoordinatesScript>();
        private void installScriptDataGrid(string _OwnName, double _X, double _Y, int _Scenario)
        {
            ListCoordinates.Add(new CoordinatesScript()
            {
                OwnName = _OwnName,
                X = _X,
                Y = _Y,
                Scenario = _Scenario
            });

            ScriptDataGrid.ItemsSource = ListCoordinates;
            ScriptDataGrid.Items.Refresh();
        }
        private void GetScriptCoordinates_Click(object sender, RoutedEventArgs e)
        {
            ToExpansion(2);
        }
        private void DoScript()
        {
            double oldX = System.Windows.Forms.Cursor.Position.X, oldY = System.Windows.Forms.Cursor.Position.Y;
            foreach (var c in ListCoordinates)
                if (c.Scenario == 0)
                    LeftMouseClick(Convert.ToUInt32(c.X), Convert.ToUInt32(c.Y));
            System.Windows.Forms.Cursor.Position = new System.Drawing.Point(Convert.ToInt32(oldX), Convert.ToInt32(oldY));
        }
        private void DoScript(int Scenario)
        {
            double oldX = System.Windows.Forms.Cursor.Position.X, oldY = System.Windows.Forms.Cursor.Position.Y;
            foreach (var c in ListCoordinates)
                if (c.Scenario == Scenario)
                    LeftMouseClick(Convert.ToUInt32(c.X), Convert.ToUInt32(c.Y));
            System.Windows.Forms.Cursor.Position = new System.Drawing.Point(Convert.ToInt32(oldX), Convert.ToInt32(oldY));
        }
#endregion

#region CMS_Script_Button_OnOff
        bool isTurnOn = false;
        private void OnOff_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!isTurnOn)
            {
                if (!isInvertThemes) OnOff.Source = new BitmapImage(new Uri(@"/Dagorlad;component/Resources/button_on.png", UriKind.Relative));
                else OnOff.Source = new BitmapImage(new Uri(@"/Dagorlad;component/Resources/black/button_on.png", UriKind.Relative));
                isTurnOn = true;
            }
            else
            {
                if (!isInvertThemes) OnOff.Source = new BitmapImage(new Uri(@"/Dagorlad;component/Resources/button_off.png", UriKind.Relative));
                else OnOff.Source = new BitmapImage(new Uri(@"/Dagorlad;component/Resources/black/button_off.png", UriKind.Relative));
                isTurnOn = false;
            }
            Properties.Settings.Default.OnOff = isTurnOn;
            Properties.Settings.Default.Save();
        }
        bool isTurnOnScript = false;
        private void OnOff_ScriptBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!isTurnOnScript)
            {
                if (!isInvertThemes) OnOff_ScriptBtn.Source = new BitmapImage(new Uri(@"/Dagorlad;component/Resources/button_on.png", UriKind.Relative));
                else OnOff_ScriptBtn.Source = new BitmapImage(new Uri(@"/Dagorlad;component/Resources/black/button_on.png", UriKind.Relative));
                isTurnOnScript = true;
            }
            else
            {
                if (!isInvertThemes) OnOff_ScriptBtn.Source = new BitmapImage(new Uri(@"/Dagorlad;component/Resources/button_off.png", UriKind.Relative));
                else OnOff_ScriptBtn.Source = new BitmapImage(new Uri(@"/Dagorlad;component/Resources/black/button_off.png", UriKind.Relative));
                isTurnOnScript = false;
            }
            Properties.Settings.Default.isTurnOnScript = isTurnOnScript;
            Properties.Settings.Default.Save();
        }
        private void TurnFromProperties()
        {
            if (isTurnOn)
            {
                if (!isInvertThemes)
                    OnOff.Source = new BitmapImage(new Uri(@"/Dagorlad;component/Resources/button_on.png", UriKind.Relative));
                else OnOff.Source = new BitmapImage(new Uri(@"/Dagorlad;component/Resources/black/button_on.png", UriKind.Relative));
            }
            else
            {
                if (!isInvertThemes)
                    OnOff.Source = new BitmapImage(new Uri(@"/Dagorlad;component/Resources/button_off.png", UriKind.Relative));
                else OnOff.Source = new BitmapImage(new Uri(@"/Dagorlad;component/Resources/black/button_off.png", UriKind.Relative));
            }
            if (isTurnOnScript)
            {
                if (!isInvertThemes)
                    OnOff_ScriptBtn.Source = new BitmapImage(new Uri(@"/Dagorlad;component/Resources/button_on.png", UriKind.Relative));
                else OnOff_ScriptBtn.Source = new BitmapImage(new Uri(@"/Dagorlad;component/Resources/black/button_on.png", UriKind.Relative));
            }
            else
            {
                if (!isInvertThemes)
                    OnOff_ScriptBtn.Source = new BitmapImage(new Uri(@"/Dagorlad;component/Resources/button_off.png", UriKind.Relative));
                else OnOff_ScriptBtn.Source = new BitmapImage(new Uri(@"/Dagorlad;component/Resources/black/button_off.png", UriKind.Relative));
            }
        }
#endregion

#region Resources
        public class _ResourcesGrid
        {
            public string _Name { get; set; }
            public string _Source { get; set; }
        }
        List<_ResourcesGrid> ListResources = new List<_ResourcesGrid>();

        private void AddListItemResources_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.OpenFileDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                installItemResource(dialog.SafeFileName, dialog.FileName);
                SaveToPropertiesFromGridResources();
            }
        }

        private void AddListItemResources_Click_Folder(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var name = dialog.SelectedPath.Split('\\');
                installItemResource(name.LastOrDefault(), dialog.SelectedPath);
                SaveToPropertiesFromGridResources();
            }
        }

        private void installItemResource(string SafeFileName, string FileName)
        {
            ListResources.Add(new _ResourcesGrid()
            {
                _Name = SafeFileName,
                _Source = FileName
            });
            ResourcesGrid.ItemsSource = ListResources;
            ResourcesGrid.Items.Refresh();
        }

        private void ResourcesGrid_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var s = new System.Diagnostics.Process();
                s.StartInfo.FileName = ((_ResourcesGrid)ResourcesGrid.SelectedItem)._Source;
                s.StartInfo.UseShellExecute = true;
                s.Start();
            }
            catch (Exception exp) { MessageBox.Show(exp.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); }
        }

        private void ResourcesGrid_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                SaveToPropertiesFromGridResources();
            }
        }
#endregion

#region RUNBP
        public class RUNBPJson
        {
            public string statusName { get; set; }
            public string inn { get; set; }
            public string code { get; set; }
            public string ogrn { get; set; }
            public string kpp { get; set; }
            public string okpoCode { get; set; }
            public string pgmu { get; set; }
            public string fullName { get; set; }
            public string fio { get; set; }
            public string recordNum { get; set; }
            public string cityName { get; set; }
            public string phone { get; set; }
            public string mail { get; set; }
            public string orfkCode { get; set; }
            public string orfkName { get; set; }
        }

        List<RUNBPJson> RUNBP_List = new List<RUNBPJson>();
        public class InformationOrgs
        {
            public static int Count = 0;
            public static string InfoOrg = "*";
        }

        public async Task<bool> SearchInDataBase(string code)
        {
            return await Task.Factory.StartNew(() =>
            {
                try
                {
                    InformationOrgs.Count = 0;
                    InformationOrgs.InfoOrg = "*";
                    RUNBP_List.Clear();
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = @"Data Source=WebService\Carcharoth;Initial Catalog=runbp;User ID=sa;Password=iloveyoujesus";
                    //con.ConnectionString = @"Data Source=(LocalDB)\db11;Initial Catalog=runbp;Integrated Security=True;Connect Timeout=30;";
                    con.Open();
                    using (SqlCommand sqlCommand = new SqlCommand("SELECT * FROM runbp WHERE @code = inn OR @code=code OR @code=ogrn OR @code=kpp OR @code=pgmu", con))
                    {
                        sqlCommand.Parameters.AddWithValue("@code", code);
                        using (SqlDataReader reader = sqlCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                RUNBP_List.Add(new RUNBPJson
                                {
                                    statusName = (string)reader["statusName"],
                                    inn = (string)reader["inn"],
                                    code = (string)reader["code"],
                                    ogrn = (string)reader["ogrn"],
                                    kpp = (string)reader["kpp"],
                                    okpoCode = (string)reader["okpoCode"],
                                    pgmu = (string)reader["pgmu"],
                                    fullName = (string)reader["fullName"],
                                    fio = (string)reader["fio"],
                                    recordNum = (string)reader["recordNum"],
                                    cityName = (string)reader["cityName"],
                                    phone = (string)reader["phone"],
                                    mail = (string)reader["mail"],
                                    orfkCode = (string)reader["orfkCode"],
                                    orfkName = (string)reader["orfkName"],
                                });
                            }
                        }
                    }
                    con.Close();
                    return true;
                }
                catch (Exception ex)
                {
                    NotifyWindowCustom.ShowNotify("Не удалось получить данные", ex.Message, "Notify_org.png", 3, true, typeof(MainWindow), null, null);
                    SendBugsReport(ex);
                    return false;
                }
            }, TaskCreationOptions.LongRunning);
        }

        public async Task<bool> LookingListRUNBP(string code)
        {
            if (!CheckNeededIP()) return false;
            LoaderOrg.IsEnabled = true;
            LoaderOrg.Visibility = Visibility.Visible;
            await SearchInDataBase(code);
            InformationOrgs.Count = RUNBP_List.Count();
            if (InformationOrgs.Count != 0)
            {
                bool isFirstEntry = false;
                foreach (var s in RUNBP_List)
                    isFirstEntry = AddToRichTextBox(s, isFirstEntry);
                NotifyReestrResult(code, InformationOrgs.Count);
                LoaderOrg.IsEnabled = false;
                LoaderOrg.Visibility = Visibility.Collapsed;
                return true;
            }
            else
            {
                NotifyReestrResult(code, InformationOrgs.Count);
                LoaderOrg.IsEnabled = false;
                LoaderOrg.Visibility = Visibility.Collapsed;
                return false;
            }
        }

        public void AddParagraph(string s, System.Windows.Media.SolidColorBrush brushes)
        {
            Paragraph newExternalParagraph = new Paragraph(new Run(s));
            newExternalParagraph.TextAlignment = TextAlignment.Center;
            newExternalParagraph.Foreground = brushes;
            newExternalParagraph.Background = System.Windows.Media.Brushes.White;
            InfoAboutOrg_RichTextBox.Document.Blocks.Add(newExternalParagraph);
        }
        public bool AddToRichTextBox(RUNBPJson s, bool isFirstEntry)
        {
            if (!isFirstEntry)
                InfoAboutOrg_RichTextBox.Document.Blocks.Clear();
            if (s.statusName == "действующая")
                AddParagraph(s.statusName, System.Windows.Media.Brushes.Green);
            else if (s.statusName == "недействующая")
                AddParagraph(s.statusName, System.Windows.Media.Brushes.Red);
            else if (s.statusName == "Специальные указания")
                AddParagraph(s.statusName, System.Windows.Media.Brushes.Blue);
            else
                AddParagraph(s.statusName, System.Windows.Media.Brushes.Gray);

            var ReOrg = s.fullName.Replace('\"', '\'');

            InformationOrgs.InfoOrg = "ИНН: " + s.inn + Environment.NewLine
                                    + "СВР: " + s.code + Environment.NewLine
                                    + "ОГРН: " + s.ogrn + Environment.NewLine
                                    + "КПП: " + s.kpp + Environment.NewLine
                                    + "ОКПО: " + s.okpoCode + Environment.NewLine
                                    + "ПГМУ: " + s.pgmu + Environment.NewLine
                                    + "Наименование: " + ReOrg + Environment.NewLine
                                    + "ФИО руководителя: " + s.fio + Environment.NewLine
                                    + "Номер реестровой записи: " + s.recordNum + Environment.NewLine
                                    + "Город: " + s.cityName + Environment.NewLine
                                    + "Наименование УФК: " + s.orfkName + Environment.NewLine
                                    + "Номер УФК: " + s.orfkCode + Environment.NewLine
                                    + "Номер телефона: " + s.phone + Environment.NewLine
                                    + "Почта: " + s.mail;

            InfoAboutOrg_RichTextBox.Document.Blocks.Add(new Paragraph(new Run(InformationOrgs.InfoOrg)));
            return true;
        }

        public void NotifyReestrResult(string code, int OrgCount)
        {
            if (OrgCount > 0)
            {
                Status_CountOrg.Content = "Найдено организаций: " + OrgCount;
                NotifyWindowCustom.ShowNotify("Есть совпадения (" + OrgCount + ") по коду организаций", "Найдено - " + OrgCount + " организаций по коду: " + code + "\nCtrl+Space - Вставить информацию в активное поле (если найдена 1 организация)", "Notify_org.png", 6, false, typeof(MainWindow), null, null);
            }
        }

        private async void SendOrg()
        {
            try
            {
                Clipboard.Clear();
                await Dispatcher.BeginInvoke(new Action(() =>
                {
                    Clipboard.SetDataObject(
                        Environment.NewLine +
                        "_________________________" + Environment.NewLine +
                        "Информация о пользователе: " + Environment.NewLine + InformationOrgs.InfoOrg + Environment.NewLine
                        );
                    System.Windows.Forms.InputLanguage.CurrentInputLanguage =
                    System.Windows.Forms.InputLanguage.FromCulture(new System.Globalization.CultureInfo("en-US"));
                    System.Windows.Forms.SendKeys.SendWait("^{v}");
                }));
            }
            catch (Exception ex)
            {
                SendBugsReport(ex);
                await Task.Delay(10);
                SubscribeGlobal();
            }
        }

        //search
        private void InfoAboutOrg_RichTextBox_Search_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            bool isFirstEntry = false;
            var text = InfoAboutOrg_RichTextBox_Search.Text.ToUpper().Trim();
            Dispatcher.BeginInvoke(new Action(() =>
            {
                InfoAboutOrg_RichTextBox.Document.Blocks.Clear();
                var g = new List<RUNBPJson>();
                if (text == null || text == "")
                    g = RUNBP_List;
                else
                {
                    try
                    {
                        g = RUNBP_List.FindAll(x =>
                        x.cityName.ToUpper().Contains(text) ||
                        x.code.ToUpper().Contains(text) ||
                        x.fio.ToUpper().Contains(text) ||
                        x.fullName.ToUpper().Contains(text) ||
                        x.inn.ToUpper().Contains(text) ||
                        x.kpp.ToUpper().Contains(text) ||
                        x.mail.ToUpper().Contains(text) ||
                        x.ogrn.ToUpper().Contains(text) ||
                        x.okpoCode.ToUpper().Contains(text) ||
                        x.orfkCode.ToUpper().Contains(text) ||
                        x.orfkName.ToUpper().Contains(text) ||
                        x.pgmu.ToUpper().Contains(text) ||
                        x.phone.ToUpper().Contains(text) ||
                        x.recordNum.ToUpper().Contains(text)
                        );
                    }
                    catch { }
                }

                foreach (var s in g)
                    isFirstEntry = AddToRichTextBox(s, isFirstEntry);
            }));
            if (text.Length > 2)
                GetAllWordRanges(text);
        }

        public async void GetAllWordRanges(string IncomingText)
        {
            await Task.Factory.StartNew(new Action(() =>
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    var g = InfoAboutOrg_RichTextBox;
                    g.SelectAll();
                    g.Selection.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(Colors.Black));
                    g.Selection.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Normal);

                    Regex reg = new Regex(IncomingText.Trim(), RegexOptions.Compiled | RegexOptions.IgnoreCase);
                    TextPointer position = g.Document.ContentStart;
                    List<TextRange> ranges = new List<TextRange>();
                    while (position != null)
                    {
                        if (position.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text)
                        {
                            string text = position.GetTextInRun(LogicalDirection.Forward);
                            var matchs = reg.Matches(text);

                            foreach (Match match in matchs)
                            {

                                TextPointer start = position.GetPositionAtOffset(match.Index);
                                TextPointer end = start.GetPositionAtOffset(IncomingText.Trim().Length);

                                TextRange textrange = new TextRange(start, end);
                                ranges.Add(textrange);
                            }
                        }
                        position = position.GetNextContextPosition(LogicalDirection.Forward);
                    }

                    foreach (TextRange range in ranges)
                    {
                        range.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(Colors.Red));
                        range.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
                    }
                }));
            }));
        }

        private void InfoAboutOrg_RichTextBox_Search_GotFocus(object sender, RoutedEventArgs e)
        {
            InfoAboutOrg_RichTextBox_Search.Foreground = (SolidColorBrush)this.Resources["ColorTextBox"];
            InfoAboutOrg_RichTextBox_Search.Text = InfoAboutOrg_RichTextBox_Search_Text;
        }

        string InfoAboutOrg_RichTextBox_Search_Text = "";
        private void InfoAboutOrg_RichTextBox_Search_LostFocus(object sender, RoutedEventArgs e)
        {
            InfoAboutOrg_RichTextBox_Search.Foreground = new SolidColorBrush(Colors.Gray);
            InfoAboutOrg_RichTextBox_Search_Text = InfoAboutOrg_RichTextBox_Search.Text;
            if (InfoAboutOrg_RichTextBox_Search_Text == "")
            {
                InfoAboutOrg_RichTextBox_Search.Text = "Поиск";
            }
        }
#endregion

#region StartHideAndCheckRegKeyAutoStartUp
        private void StartHideAndCheckRegKeyAutoStartUp()
        {
           // Dispatcher.BeginInvoke(new Action(() =>
           // {
                Notify.ShowInTray();
                this.Hide();
                //string _Name = Assembly.GetEntryAssembly().GetName().Name;

                //RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

                //if (rkApp.GetValue(_Name) == null)
                //{
                //    rkApp.SetValue(_Name, System.Reflection.Assembly.GetExecutingAssembly().Location);
                //}
                try
                {
                    using (Microsoft.Win32.TaskScheduler.TaskService ts = new Microsoft.Win32.TaskScheduler.TaskService())
                    {
                        var exist = ts.FindTask("Dagorlad",true);
                        if (exist == null)
                        {
                            var userid=System.Security.Principal.WindowsIdentity.GetCurrent().User.Value;
                            var file_text = Properties.Resources.startup.ToString()
                                .Replace("%name%", "Dagorlad")
                                .Replace("%userid%", userid)
                                .Replace("%user%", Environment.MachineName + @"\" + Environment.UserName)
                                .Replace("%path%", AppDomain.CurrentDomain.BaseDirectory + "Dagorlad.exe")
                                .Replace("%path_wd%", AppDomain.CurrentDomain.BaseDirectory);
                            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + Environment.UserName + "_startup.xml", file_text);
                            ts.RootFolder.RegisterTaskDefinition(
                                "Dagorlad",
                                ts.NewTaskFromFile(AppDomain.CurrentDomain.BaseDirectory + Environment.UserName + "_startup.xml"),
                                Microsoft.Win32.TaskScheduler.TaskCreation.CreateOrUpdate,
                                userid,
                                null,
                                Microsoft.Win32.TaskScheduler.TaskLogonType.InteractiveToken);
                        }
                    }
                }
                catch (Exception ex) { MessageBox.Show(ex.ToString()); }
          //  }), DispatcherPriority.Background).Wait();
        }
#endregion

#region ForceTimerToUpdate
        DispatcherTimer ForceUpdateTimer = new DispatcherTimer();
        public void ForceTimerToUpdate_Start(int Minutes)
        {
            ForceUpdateTimer.Interval = new TimeSpan(0, Minutes, 0);
            ForceUpdateTimer.IsEnabled = true;
            ForceUpdateTimer.Tick += (q, e) => { Update.Start_Update(true); };
            ForceUpdateTimer.Start();
        }
#endregion

#region Catalog
        public class CommandsCA
        {
            public string id { get; set; }
            public string Groups { get; set; }
            public string Services { get; set; }
            public string Function { get; set; }
        }
        List<CommandsCA> ListCatalog = new List<CommandsCA>();
        public async Task<bool> QueryDBCatalog()
        {
            try
            {
                string txt = SearchCatalog.Text.ToLower().Trim();
                ListCatalog.Clear();
                await Task.Factory.StartNew(new Action(() =>
                {
                    string sqlcmd = "";
                    if (txt.Trim() == "" || String.IsNullOrEmpty(txt.Trim()))
                        sqlcmd = "SELECT * FROM ctr";
                    else
                        sqlcmd = "SELECT * FROM ctr WHERE [Groups] LIKE N'%" + txt + "%' OR [Services] LIKE N'%" + txt + "%'";
                    using (SqlConnection sqlConnection = new SqlConnection(@"Data Source=WebService\Carcharoth;Initial Catalog=SUE;User ID=sa;Password=iloveyoujesus"))
                    //using (SqlConnection sqlConnection = new SqlConnection(@"Data Source=(localdb)\db11;Initial Catalog=SUE;Integrated Security = true"))
                    using (SqlCommand sqlCommand = new SqlCommand(sqlcmd, sqlConnection))
                    {
                        sqlConnection.Open();
                        using (SqlDataReader reader = sqlCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ListCatalog.Add(new CommandsCA
                                {
                                    id = ((int)reader["id"]).ToString(),
                                    Groups = (string)reader["Groups"],
                                    Services = (string)reader["Services"],
                                    Function = (string)reader["Function"],
                                });
                            }
                        }
                        sqlConnection.Close();
                        sqlCommand.Dispose();
                    }
                }), TaskCreationOptions.HideScheduler);
                return true;
            }
            catch (Exception ex) { SendBugsReport(ex); MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); return false; }
        }

        public async Task SearchWords()
        {
            await Task.Factory.StartNew(new Action(() =>
            {
                Dispatcher.Invoke(() =>
                {
                    ListCollectionView collection = new ListCollectionView(ListCatalog);
                    collection.GroupDescriptions.Add(new PropertyGroupDescription("Groups"));
                    DataGrid_Catalog.ItemsSource = collection;
                    DataGrid_Catalog.Items.Refresh();
                }, DispatcherPriority.Background);
            }), TaskCreationOptions.HideScheduler);
        }

        private async void SearchCatalog_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                LoaderCatalog.IsEnabled = true;
                LoaderCatalog.Visibility = Visibility.Visible;
                await QueryDBCatalog();
                await SearchWords();
                LoaderCatalog.IsEnabled = false;
                LoaderCatalog.Visibility = Visibility.Collapsed;
            }
        }

        private void SearchCatalog_GotFocus(object sender, RoutedEventArgs e)
        {
            SearchCatalog.Foreground = (SolidColorBrush)this.Resources["ColorTextBox"];
            SearchCatalog.Text = SearchCatalog_Text;
        }

        string SearchCatalog_Text = "";
        private void SearchCatalog_LostFocus(object sender, RoutedEventArgs e)
        {
            SearchCatalog.Foreground = new SolidColorBrush(Colors.Gray);
            SearchCatalog_Text = SearchCatalog.Text;
            if (SearchCatalog_Text == "")
            {
                SearchCatalog.Text = "Введите ключевые слова и нажмите 'Enter', оставьте поле пустым для получения всего списка";
            }
        }
#endregion

#region Handbook
        public class Handbook_Class
        {
            public string Target { get; set; }
            public string Position { get; set; }
            public string FIO { get; set; }
            public string Phone_City { get; set; }
            public string Phone_Inside { get; set; }
            public string Address { get; set; }
            public string Email { get; set; }
        }
        List<Handbook_Class> ListHandbook = new List<Handbook_Class>();
        public async Task<bool> QueryDBHandbook()
        {
            try
            {
                if (SearchHandbook.Text != null && SearchHandbook.Text != "")
                {
                    ListHandbook.Clear();
                    var txt = SearchHandbook.Text;
                    await Task.Factory.StartNew(new Action(() =>
                    {
                        int count = 0;
                        using (SqlConnection sqlConnection = new SqlConnection(@"Data Source=WebService\Carcharoth;Initial Catalog=SUE;User ID=sa;Password=iloveyoujesus"))
                        //using (SqlConnection sqlConnection = new SqlConnection(@"Data Source=(localdb)\db11;Initial Catalog=SUE;Integrated Security = true"))
                        using (SqlCommand sqlCommand = new SqlCommand("SELECT TOP 71 * FROM ctr_tel WHERE [FIO] LIKE N'%" + txt + "%' OR [Phone_City] LIKE N'%" + txt + "%' OR [Phone_Inside] LIKE N'%" + txt + "%' OR [Email] LIKE N'%" + txt + "%'", sqlConnection))
                        {
                            sqlConnection.Open();
                            using (SqlDataReader reader = sqlCommand.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    ListHandbook.Add(new Handbook_Class
                                    {
                                        Target = (string)reader["Target"],
                                        Position = (string)reader["Position"],
                                        FIO = (string)reader["FIO"],
                                        Phone_City = (string)reader["Phone_City"],
                                        Phone_Inside = (string)reader["Phone_Inside"],
                                        Address = (string)reader["Address"],
                                        Email = (string)reader["Email"],
                                    });
                                    count++;
                                    if (count > 51) break;
                                }
                                sqlConnection.Close();
                                sqlCommand.Dispose();
                            }
                        }
                    }), TaskCreationOptions.HideScheduler);
                }
                else
                {
                    List<Handbook_Class> error = new List<Handbook_Class>();
                    error.Add(new Handbook_Class { FIO = "Поле не может быть пустым" });
                    DataGrid_Handbook.ItemsSource = error;
                    DataGrid_Handbook.Items.Refresh();
                };
                return true;
            }
            catch (Exception ex) { SendBugsReport(ex); MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); return false; }
        }

        public async Task SearchWords_Handbook()
        {
            await Task.Run(() =>
            {
                if (ListHandbook.Count() > 50)
                {
                    ListHandbook.Add(new Handbook_Class
                    {
                        Target = "...",
                        Position = "...",
                        FIO = "...",
                    });
                    ListHandbook.Add(new Handbook_Class
                    {
                        FIO = "Cлишком много значений, уточните запрос...",
                    });
                }
                Dispatcher.Invoke(() =>
                {
                    DataGrid_Handbook.ItemsSource = ListHandbook;
                    DataGrid_Handbook.Items.Refresh();
                }, DispatcherPriority.ContextIdle);
            });
        }
        private async void SearchHandbook_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                LoaderHandbook.IsEnabled = true;
                LoaderHandbook.Visibility = Visibility.Visible;
                await QueryDBHandbook();
                await SearchWords_Handbook();
                LoaderHandbook.IsEnabled = false;
                LoaderHandbook.Visibility = Visibility.Collapsed;
            }
        }

        private void SearchHandbook_GotFocus(object sender, RoutedEventArgs e)
        {
            SearchHandbook.Foreground = (SolidColorBrush)this.Resources["ColorTextBox"];
            SearchHandbook.Text = SearchHandbook_Text;
        }

        string SearchHandbook_Text = "";
        private void SearchHandbook_LostFocus(object sender, RoutedEventArgs e)
        {
            SearchHandbook.Foreground = new SolidColorBrush(Colors.Gray);
            SearchHandbook_Text = SearchHandbook.Text;
            if (SearchHandbook_Text == "")
            {
                SearchHandbook.Text = "Введите ключевые слова (Номер, ФИО) и нажмите 'Enter'";
            }
        }
#endregion

#region BugsReporting
        public static void SendBugsReport(Exception ex)
        {
            NotifyWindowCustom.ShowNotify("Ошибка: " + ex.HResult.ToString(), ex.Message, "Notify_error.png", 15000, false, typeof(MainWindow), null, null);
            //Process.Start("mailto:PyatkoBV@fsfk.local?subject=Error%20-%20Dagorlad&body=Error:" + ex.ToString());
        }

        private void BugsReports_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start("mailto:PyatkoBV@fsfk.local?subject=Инцидент%20-%20Dagorlad&body=Текст%20письма");
            }
            catch { }
        }
#endregion

#region ListUsers
        private void ShowUsersBtn_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                var Employees = new Employees();
                Employees.Topmost = isTurnOnPin;
                Employees.Show();
            }));
        }

        DispatcherTimer BirthDayNotifyTimer = new DispatcherTimer();
        private async void TimeNotifyBirthDay()
        {
            await Dispatcher.BeginInvoke(new Action(() =>
             {
                 try
                 {
                     BirthDayNotifyTimer.Interval = new TimeSpan(0, 30, 0);
                     BirthDayNotifyTimer.Tick += (q, e) =>
                     {
                         if (DateTime.Now.Hour >= new DateTime().AddHours(10).Hour)
                         {
                             Employees.JsonReadNotify();
                             BirthDayLabel.Content = Employees.FIO_BirthDay.Trim();
                             if (BirthDayLabel.Content.ToString() != "")
                             {
                                 BirthDayPanelInfo.Visibility = Visibility.Visible;
                                 BirthDayNotifyTimer.Interval = new TimeSpan(5, 0, 0);
                             }
                             else
                             {
                                 BirthDayPanelInfo.Visibility = Visibility.Hidden;
                                 BirthDayNotifyTimer.Interval = new TimeSpan(5, 0, 0);
                             }
                         }
                         else BirthDayNotifyTimer.Interval = new TimeSpan(0, 30, 0);
                     };
                     BirthDayNotifyTimer.Start();
                 }
                 catch (Exception ex) { File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "logs.log", ex.ToString()); }
             }), DispatcherPriority.Background);
        }
#endregion

#region Pin
        bool isTurnOnPin = false;
        private void PinOnOff_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (isTurnOnPin)
            {
                if(!isInvertThemes)
                PinOnOff.Source = new BitmapImage(new Uri(@"/Dagorlad;component/Resources/pinunlock.png", UriKind.Relative));
                else PinOnOff.Source = new BitmapImage(new Uri(@"/Dagorlad;component/Resources/black/pinunlock.png", UriKind.Relative));
                isTurnOnPin = false;
                this.Topmost = false;
            }
            else
            {
                if (!isInvertThemes)
                    PinOnOff.Source = new BitmapImage(new Uri(@"/Dagorlad;component/Resources/pinlock.png", UriKind.Relative));
                else PinOnOff.Source = new BitmapImage(new Uri(@"/Dagorlad;component/Resources/black/pinlock.png", UriKind.Relative));
                isTurnOnPin = true;
                this.Topmost = true;
            }
        }
#endregion

#region InfoUpdateData
        private async void InfoUpdateDataFunc()
        {
            InfoUpdateData.Content = "Получение данных RUNBP...";
            string RUNBP_Date = String.Empty;
            string RUNBP_Count = String.Empty;

            await Task.Factory.StartNew(new Action(() =>
            {
                try
                {
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = @"Data Source=WebService\Carcharoth;Initial Catalog=runbp;User ID=sa;Password=iloveyoujesus";
                    con.Open();
                    using (SqlCommand sqlCommand = new SqlCommand("SELECT statusName,inn,code FROM runbp WHERE statusName like 'last'", con))
                    {
                        using (SqlDataReader reader = sqlCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                RUNBP_Count = (string)reader["inn"];
                                RUNBP_Date = (string)reader["code"];
                            }
                        }
                    }
                    con.Close();
                    InfoUpdateData_Content(RUNBP_Date, RUNBP_Count, null);
                }
                catch (Exception ex)
                {
                    if (ex.HResult == -2146232060)
                    {
                        InfoUpdateData_Content("Не удалось подключиться к БД", null, ex);

                    }
                    else InfoUpdateData_Content("Невозможно получить данные", null, ex);
                }
            }), TaskCreationOptions.HideScheduler);
        }

        private void InfoUpdateData_Content(string RUNBP_Date, string RUNBP_Count, Exception ex)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                InfoUpdateDataGif.Source = null;
                InfoUpdateDataGif.IsEnabled = false;
                InfoUpdateDataGif.Visibility = Visibility.Collapsed;
                InfoUpdateData.Content = RUNBP_Count == null ? RUNBP_Date : "Реестр РУНБП обновлен: " + RUNBP_Date + " (" + RUNBP_Count + " записей)";
                if (RUNBP_Count == null)
                    InfoUpdateDataImg.Source = new BitmapImage(new Uri(@"/Dagorlad;component/Resources/dbfailed.png", UriKind.Relative));
                else InfoUpdateDataImg.Source = new BitmapImage(new Uri(@"/Dagorlad;component/Resources/receivedatabase.png", UriKind.Relative));
                if (ex != null)
                    SP_InfoUpdateDate.ToolTip = ex.ToString();
            }), DispatcherPriority.Background);
        }
#endregion

        double standartHeight = 0;
        double standartWidth = 0;
        bool isReState = false;
        private void WindowStateMax_Click(object sender, RoutedEventArgs e)
        {
            if (isReState==false)
            {
                standartHeight = this.Height;
                standartWidth = this.Width;
                this.Width = System.Windows.SystemParameters.WorkArea.Width;
                this.Height = System.Windows.SystemParameters.WorkArea.Height;
                this.Left = 0;
                this.Top = 0;
                this.border.CornerRadius = new CornerRadius(0);
                border.Margin = new Thickness(0);
                isReState = true;
                bottomBorder.CornerRadius = new CornerRadius(0);
            }
            else {
                isReState = false;
                this.Width = standartWidth;
                this.Height = standartHeight;
                this.border.CornerRadius = new CornerRadius(15);
                border.Margin = new Thickness(10);
                bottomBorder.CornerRadius = new CornerRadius(0, 0 ,14, 14);
                DefaultFunction.CenterWindowOnScreen(this);
            }
        }
 
        private void About_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://webservice/Dagorlad/about.html");
        }
    }
    public class DefaultFunction
    {
        public static async void CenterWindowOnScreen(Window window)
        {
            await System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
                double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
                double windowWidth = window.Width;
                double windowHeight = window.Height;
                window.Left = (screenWidth / 2) - (windowWidth / 2);
                window.Top = (screenHeight / 2) - (windowHeight / 2);
            }), DispatcherPriority.Background);
        }
    }
}