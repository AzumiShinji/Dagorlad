using Dagorlad_7.classes;
using Dagorlad_7.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using UsBudget.classes;

namespace Dagorlad_7
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {      
        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == System.Windows.WindowState.Minimized)
                this.Hide();

            base.OnStateChanged(e);
        }
        public MainWindow()
        {
            MySettings.Load();
#if (!DEBUG)
            Updater.CheckUpdate().GetAwaiter();
#endif
            DispatcherControls.SetSchemeColor(MySettings.Settings.TypeColorScheme, true);
            DispatcherControls.HideWindowToTaskMenu(this, null);
            InitializeComponent();
            LoadEvents();
            new ChatWindow();
#if (!DEBUG)
            CheckingUpdateApplicationStart();
#endif
        }
        private DispatcherTimer timerToUpdate = new DispatcherTimer();
        private void CheckingUpdateApplicationStart()
        {
            var time = new Random().Next(5, 15);
            timerToUpdate.Interval = TimeSpan.FromMinutes(time);
            Logger.Write(Logger.TypeLogs.updater, "Started checking update, next update attempt in " + time + " minutes.");
            timerToUpdate.Tick += async (q, e) =>
            {
                await Updater.CheckUpdate();
            };
            timerToUpdate.Start();
        }
        private async void LoadEvents()
        {
            ShowMiniMenu();
            if (MySettings.Settings.IsFirstTimeLanuched)
            {
                DispatcherControls.Autorun(DispatcherControls.TypeAutoRunOperation.On);
                MySettings.Settings.IsFirstTimeLanuched = false;
            }
            await MySettings.Save();
            InitClipboardMonitor();
            UpdateLabelAboutUpdate();
            if (!String.IsNullOrEmpty(MySettings.Settings.ClearingFolder))
                FolderToBeClearedTextBlock.Text = MySettings.Settings.ClearingFolder;
            GlobalHookEventGrid.IsEnabled = MySettings.Settings.IsRegGlobalHook;
            if (MySettings.Settings.IsRegGlobalHook)
                GlobalHook.StartHooking();
            else GlobalHook.StopHooking();
        }
        private async void UpdateLabelAboutUpdate()
        {
            DateUpdateDataBaseOfOrganizationsLabel.Content = "Загрузка...";
            var dtupdateorganization = await SearchOrganizations.GetUpdateDate();
            if (dtupdateorganization != null && dtupdateorganization.HasValue)
            {
                var result = Math.Round(((DateTime.Now - dtupdateorganization).Value.TotalDays), 0);
                if (result == 0)
                    DateUpdateDataBaseOfOrganizationsLabel.Content = String.Format("База организаций была обновлена сегодня.");
                else
                    DateUpdateDataBaseOfOrganizationsLabel.Content = String.Format("База организаций была обновлена {0} дн. назад", result);
            }
            else DateUpdateDataBaseOfOrganizationsLabel.Content = "Неизвестно";
        }
        private void InitClipboardMonitor()
        {
            try
            {
                Clipboard.Clear();
            }
            catch (Exception ex) { Logger.Write(Logger.TypeLogs.clipboard, ex.ToString()); }
            ClipboardMonitor.OnClipboardChange += new ClipboardMonitor.OnClipboardChangeEventHandler(ClipboardMonitor_OnClipboardChange);
            ClipboardMonitor.Start();
        }

        bool IsAlreadyLaunched = false;
        public async void ClipboardMonitor_OnClipboardChange(ClipboardFormat format, object data)
        {
            try
            {
                if (!IsAlreadyLaunched)
                {
                    if (format == ClipboardFormat.Text)
                    {
                        IsAlreadyLaunched = true;
                        string text = data as string;
                        if (!String.IsNullOrEmpty(text) && text.Length < 20)
                        {
                            await Dispatcher.BeginInvoke(new Action(async () =>
                            {
                                var code = SearchOrganizations.CheckIfStringAsNumberOfOrganizations(text);
                                if (code != 0)
                                {
                                    var list = await SearchOrganizations.TryFindOrganizations(code);
                                    if (list != null && list.Count() > 0)
                                    {
                                        DispatcherControls.NewMyNotifyWindow(text, String.Format("Найдено {0} орг. по данному коду", list.Count()), 15, this, TypeImageNotify.buildings);
                                        OrganizationsListMain = list;
                                        OrganizationsListView.ItemsSource = OrganizationsListMain;
                                        UpdateLabelAboutUpdate();
                                    }
                                }
                                else
                                {
                                    if (IsNumberHandling(text))
                                    {
                                        var number = text.Trim();
                                        DispatcherControls.LastNumberOfHandling = number;
                                        foreach (var window in App.Current.Windows)
                                        {
                                            if (window.GetType() == typeof(SmartMenuWindow))
                                            {
                                                var wnd = ((SmartMenuWindow)window);
                                                if (wnd.IsLoaded)
                                                    wnd.TemporaryNumberOfHandlingLabel.Content = 
                                                    DispatcherControls.LastNumberOfHandling;
                                            }
                                        }
                                        DispatcherControls.NewMyNotifyWindow(number, "Номер запомнен программой.\nМожно использовать \"СМАРТ-МЕНЮ\".", 15, this, TypeImageNotify.number_handling);
                                    }
                                }
                            }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Write(Logger.TypeLogs.clipboard, ex.ToString());
                IsAlreadyLaunched = false;
            }
            finally
            {
                IsAlreadyLaunched = false;
            }
        }
        public bool IsNumberHandling(string text)
        {
            try
            {
                if (text.Length >= 8)
                {
                    int count_prefix = 2;
                    var FirstTwoLetter = text.Trim().Substring(0, count_prefix);
                    if (FirstTwoLetter == "SD" || FirstTwoLetter == "IM")
                    {
                        var RemainingLetter = text.Trim();
                        var WithoutPrefix = RemainingLetter.Skip(count_prefix);
                        RemainingLetter = String.Concat(WithoutPrefix);
                        if (!IsDigit(RemainingLetter)) return false;

                        var number = FirstTwoLetter + RemainingLetter;
                        Console.WriteLine("Number: {0}", number);
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                Logger.Write(Logger.TypeLogs.main, ex.ToString());
                return false;
            }
        }
        private bool IsDigit(string text)
        {
            if (String.IsNullOrEmpty(text)) return false;
            var IsDigit = Int64.TryParse(text, out long result);
            if (IsDigit)
                return true;
            else return false;
        }
        MiniMenuWindow minimenu;
        private void ShowMiniMenu()
        {
            if (MySettings.Settings.SmartMenuIsEnabled)
            {
                if (minimenu == null)
                {
                    minimenu = new MiniMenuWindow();
                    minimenu.Show();
                }
            }
            else
            {
                if (minimenu != null)
                {
                    minimenu.Closing -= (qq, ee) => { ee.Cancel = true; };
                    minimenu.Closing += (qq, ee) => { ee.Cancel = false; };
                    minimenu.Close();
                    minimenu = null;
                }
            }
        }
        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            ShowSettingsWindow();
        }
        private void ShowSettingsWindow()
        {
            var g = new MySettingsWindow();
            g.Owner = this;
            if (g.ShowDialog() == true)
            {
                ShowMiniMenu();
                DispatcherControls.SetSchemeColor(MySettings.Settings.TypeColorScheme,false);
                    GlobalHookEventGrid.IsEnabled = MySettings.Settings.IsRegGlobalHook;
                if (MySettings.Settings.IsRegGlobalHook)
                    GlobalHook.StartHooking();
                else GlobalHook.StopHooking();
            }
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.WindowState = WindowState.Minimized;
        }
        private void CloseApplicationButton_Click(object sender, RoutedEventArgs e)
        {
            var g = DispatcherControls.ShowMyDialog("Выход", "Вы уверены, что хотите выйти?", MyDialogWindow.TypeMyDialog.YesNo, this);
            if (g == MyDialogWindow.ResultMyDialog.Yes)
            {
                ExitFromApplication();
            }
        }
        public async void ExitFromApplication()
        {
            ClipboardMonitor.Stop();
            try
            {
                if (ChatWindow.Me != null)
                    await ChatWindow.proxy.DisconnectAsync(ChatWindow.Me);
            }
            catch (Exception ex) { Logger.Write(Logger.TypeLogs.chat, ex.ToString()); }
            Application.Current.Shutdown();
        }
        List<OrganizationsClass> OrganizationsListMain;
        private Task SearchOrganizationFromListView()
        {
            var key = SearchTextBox.Text;
            if (!String.IsNullOrEmpty(key))
            {
                key = key.ToLower();
                var obj = OrganizationsListMain;
                if (obj != null)
                {
                    var founded = new List<OrganizationsClass>();
                    var list = obj.Cast<OrganizationsClass>();
                    var properties = typeof(OrganizationsClass).GetProperties();
                    foreach (var s in list)
                    {
                        foreach (var property in properties)
                        {
                            var p = property.GetValue(s);
                            if (p.GetType() == typeof(String))
                            {
                                string text = p as string;
                                if (!String.IsNullOrEmpty(text))
                                {
                                    text = text.ToLower();
                                    if (text == key || text.Contains(key))
                                    {
                                        founded.Add(s);
                                    }
                                }
                            }
                        }
                    }
                    OrganizationsListView.ItemsSource = founded;
                }
            }
            return Task.CompletedTask;
        }
        private async void SearchOrganizationFromListView_Click(object sender, RoutedEventArgs e)
        {
            SearchGrid.IsEnabled = false;
            await SearchOrganizationFromListView();
            SearchGrid.IsEnabled = true;
        }
        private void Cancel_SearchOrganizationFromListView_Click(object sender, RoutedEventArgs e)
        {
            OrganizationsListView.ItemsSource = OrganizationsListMain;
            SearchTextBox.Text = null;
        }

        private void SearchTextBox_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case (Key.Enter):
                    {
                        if (SearchTextBox.IsFocused)
                        {
                            SearchOrganizationFromListViewButton.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                        }
                        break;
                    }
                case (Key.Escape):
                    {
                        if (SearchTextBox.IsFocused)
                        {
                            Cancel_SearchOrganizationFromListView_Button.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                        }
                        break;
                    }
            }
        }

        private async void CopyOrganizationInformation_Click(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            btn.IsEnabled = false;
            var cmd = (OrganizationsClass)btn.CommandParameter;
            var label = (Label)btn.Tag;
            if (cmd == null || label==null) return;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(String.Format("ИНН:{0}",cmd.inn??""));
            sb.AppendLine(String.Format("ОГРН:{0}", cmd.ogrn ?? ""));
            sb.AppendLine(String.Format("КПП:{0}", cmd.kpp ?? ""));
            sb.AppendLine(String.Format("СВР:{0}", cmd.code ?? ""));
            try
            {
                Clipboard.SetText(sb.ToString());
                label.Content = "Скопировано!";
            }
            catch { label.Content = "Попробуйте еще раз!"; }
            await Task.Delay(2000);
            label.Content = null;
            btn.IsEnabled = true;
        }

        private void FolderToBeClearedButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                MySettings.Settings.ClearingFolder = dialog.SelectedPath;
                MySettings.Save();
                FolderToBeClearedTextBlock.Text = MySettings.Settings.ClearingFolder;
            }
        }
    }
    public class WidthFixedListViewConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null || parameter == null) return 0;
            var result = (double)value - System.Convert.ToDouble(parameter);
            if (result >= 0)
                return (double)value - System.Convert.ToDouble(parameter);
            else return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return 0;
        }
    }
    public class statusNameBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if(value!=null)
            {
                var str = (string)value;
                if (!String.IsNullOrEmpty(str))
                    if (str.ToLower() != "действующая")
                        return Brushes.OrangeRed;
            }
            return (Brush)new BrushConverter().ConvertFromString("#FF3A6FD8");
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (Brush)new BrushConverter().ConvertFromString("#FF3A6FD8");
        }
    }
}
