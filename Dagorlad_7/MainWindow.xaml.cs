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
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            DispatcherControls.HideAppToTaskMenu();
            MySettings.Load();
            InitializeComponent();
            LoadEvents();
        }
        private async void LoadEvents()
        {
            ShowMiniMenu();
            if(MySettings.Settings.IsFirstTimeLanuched)
            {
                DispatcherControls.Autorun(DispatcherControls.TypeAutoRunOperation.On);
                MySettings.Settings.IsFirstTimeLanuched = false;
            }
            await MySettings.Save();
            InitClipboardMonitor();
            var dtupdateorganization = await SearchOrganizations.GetUpdateDate();
            if (dtupdateorganization != null && dtupdateorganization.HasValue)
                DateUpdateDataBaseOfOrganizationsLabel.Content = String.Format("База организаций была обновлена {0} дн. назад",Math.Round(((DateTime.Now-dtupdateorganization).Value.TotalDays),0));
            else DateUpdateDataBaseOfOrganizationsLabel.Content = "Неизвестно";
            DispatcherControls.NewMyNotifyWindow(Assembly.GetExecutingAssembly().GetName().Name, "Программа запущена и работает в фоновом режиме.", 8, this, TypeImageNotify.standart);
        }
        private void InitClipboardMonitor()
        {
            Clipboard.Clear();
            ClipboardMonitor.OnClipboardChange += new ClipboardMonitor.OnClipboardChangeEventHandler(ClipboardMonitor_OnClipboardChange);
            ClipboardMonitor.Start();
        }
        public void ClipboardMonitor_OnClipboardChange(ClipboardFormat format, object data)
        {
            if (format == ClipboardFormat.Text)
            {
                string text = data as string;
                if (!String.IsNullOrEmpty(text))
                {
                    Dispatcher.BeginInvoke(new Action(async () =>
                    {
                        var code = SearchOrganizations.CheckIfStringAsNumberOfOrganizations(text);
                        if (code != 0)
                        {
                            var list = await SearchOrganizations.TryFindOrganizations(code);
                            if (list != null && list.Count() > 0)
                            {
                                DispatcherControls.NewMyNotifyWindow(text, String.Format("Найдено {0} орг. по данному коду", list.Count()), 15, this, TypeImageNotify.buildings);
                                OrganizationsListView.ItemsSource = list;
                            }
                        }
                    }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);
                }
            }
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
            }
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.WindowState = WindowState.Minimized;
        }
        private void CloseApplicationButton_Click(object sender, RoutedEventArgs e)
        {
            var g = DispatcherControls.ShowMyDialog("Выход", "Вы уверены, что хотите выйти?", MyDialogWindow.TypeMyDialog.YesNo,this);
            if (g == MyDialogWindow.ResultMyDialog.Yes)
            {
                Application.Current.Shutdown();
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
}
