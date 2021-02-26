using Dagorlad_7.classes;
using Dagorlad_7.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Dagorlad_7.Pages
{
    /// <summary>
    /// Логика взаимодействия для SearchOrganizationsPage.xaml
    /// </summary>
    public partial class SearchOrganizationsPage : Page
    {
        public SearchOrganizationsPage()
        {
            InitializeComponent();
            InitClipboardMonitor();
            RestartClipboardMonitor_Start();
        }
        private DispatcherTimer timerToRestartClipboardWatcher = new DispatcherTimer();
        private void RestartClipboardMonitor_Start()
        {
            timerToRestartClipboardWatcher.Interval = TimeSpan.FromMinutes(10);
            Logger.Write(Logger.TypeLogs.clipboard, "Started update, next update attempt in " + timerToRestartClipboardWatcher.Interval.TotalSeconds + " minutes.");
            timerToRestartClipboardWatcher.Tick += (q, e) =>
            {
                Logger.Write(Logger.TypeLogs.clipboard, "Clipboard Monitor has been Updated.");
                //try
                //{
                //    Clipboard.Clear();
                //}
                //catch { }
                InitClipboardMonitor();
            };
            timerToRestartClipboardWatcher.Start();
        }
        private void InitClipboardMonitor()
        {
            try
            {
                ClipboardMonitor.Stop();
                ClipboardMonitor.OnClipboardChange -= ClipboardMonitor_OnClipboardChange;
                ClipboardMonitor.OnClipboardChange += ClipboardMonitor_OnClipboardChange;
                ClipboardMonitor.Start();
            }
            catch (Exception ex) { Logger.Write(Logger.TypeLogs.clipboard, ex.ToString()); }
        }
        private async Task UpdateLabelAboutUpdate()
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
        bool IsAlreadyLaunched = false;
        string previousSearchKey = "";
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
                        if (!String.IsNullOrEmpty(text) && text.Length < 20 && previousSearchKey != text)
                        {
                            await Dispatcher.BeginInvoke(new Action(async () =>
                            {
                                if (MySettings.Settings.SmartMenuIsEnabled && IsNumberHandling(text))
                                {
                                    var number = text.Trim().ToUpper();
                                    previousSearchKey = number;
                                    DispatcherControls.LastNumberOfHandling = number;
                                    foreach (var window in App.Current.Windows)
                                    {
                                        if (window.GetType() == typeof(SmartMenuWindow))
                                        {
                                            var wnd = ((SmartMenuWindow)window);
                                            if (wnd.IsLoaded)
                                            {
                                                wnd.TemporaryNumberOfHandlingLabel.Content =
                                                DispatcherControls.LastNumberOfHandling;
                                                break;
                                            }
                                        }
                                    }
                                    DispatcherControls.NewMyNotifyWindow(number, "Номер запомнен программой.\nМожно использовать \"СМАРТ-МЕНЮ\".", TimeSpan.FromSeconds(15), Application.Current.MainWindow, TypeImageNotify.number_handling, null);
                                    previousSearchKey = "";
                                }
                                else if (MySettings.Settings.IsSearchOrganizations)
                                {
                                    var code = SearchOrganizations.CheckIfStringAsNumberOfOrganizations(text);
                                    if (code != null)
                                    {
                                        previousSearchKey = code;
                                        var list = await SearchOrganizations.TryFindOrganizations(code);
                                        if (list != null && list.Count() > 0)
                                        {
                                            DispatcherControls.NewMyNotifyWindow(text, String.Format("Найдено {0} орг. по данному коду", list.Count()), TimeSpan.FromSeconds(15), Application.Current.MainWindow, TypeImageNotify.buildings, this);
                                            OrganizationsListMain = list;
                                            OrganizationsListView.ItemsSource = OrganizationsListMain;
                                            await UpdateLabelAboutUpdate();
                                        }
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
        public bool IsNumberHandling(string _text)
        {
            try
            {
                var text = _text.Trim().ToUpper();
                if (text.StartsWith("SD") || text.StartsWith("IM") && text.Length >= 8)
                {
                    int count_prefix = 2;
                    var FirstTwoLetter = text.Substring(0, count_prefix);
                    var RemainingLetter = text;
                    var WithoutPrefix = RemainingLetter.Skip(count_prefix);
                    RemainingLetter = String.Concat(WithoutPrefix);
                    if (!IsDigit(RemainingLetter)) return false;
                    var number = FirstTwoLetter + RemainingLetter;
                    Console.WriteLine("Number: {0}", number);
                    return true;
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
            if (cmd == null || label == null) return;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(String.Format("ИНН:{0}", cmd.inn ?? ""));
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
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await UpdateLabelAboutUpdate();
        }
    }
    public class statusNameBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
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
