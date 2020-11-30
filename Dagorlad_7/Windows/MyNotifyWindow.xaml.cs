using Dagorlad_7.classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Dagorlad_7.Windows
{
    /// <summary>
    /// Логика взаимодействия для MyNotifyWindow.xaml
    /// </summary>
   public class MyNotifyClass
    {
        public BitmapImage image { get; set; }
        public DateTime dt { get; set; }
        public string title { get; set; }
        public string text { get; set; }
        public TimeSpan timetoautoclose { get; set; }
        public bool IsLock { get; set; }
    }
    public enum TypeImageNotify
    {
        standart = 0,
        buildings = 1,
    }
    public partial class MyNotifyWindow : Window
    {
        public Window FromWindow;
        public MyNotifyWindow(Window win)
        {
            InitializeComponent();
            FromWindow = win;
            LoadEvents();
        }
        private async void LoadEvents()
        {
            await StandartPositionWindow();
            this.DataContext = DispatcherControls.MyNotifyList;
            await StartAutoCloseTemporaryNotifies(TimeSpan.FromSeconds(1));
        }
        public async Task StandartPositionWindow()
        {
            var main_window_size = System.Windows.SystemParameters.WorkArea;
            this.Left = main_window_size.Width - this.Width;
            while (NotifiesListView.IsLoaded == false)
                await Task.Delay(100);
            this.Top = main_window_size.Height - this.ActualHeight-3;
            HideIfZero();
        }

        private async void NotifiesListView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (DispatcherControls.MyNotifyList.Count > 1)
                HideNotifiesListViewGrid.Visibility = Visibility.Visible;
            else HideNotifiesListViewGrid.Visibility = Visibility.Hidden;
            await StandartPositionWindow();
        }

        private void CloseButton(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            button.IsEnabled = false;
            MyNotifyClass obj = (MyNotifyClass)button.DataContext;
            var maingrid = button.Tag as Grid;
            var da = new DoubleAnimation()
            {
                Duration = new Duration(TimeSpan.FromMilliseconds(150)),
                From = 1,
                To = 0,
            };
            da.Completed += async (q, f) =>
            {
                await Task.Delay(250);
                DispatcherControls.MyNotifyList.Remove(obj);
                if (button.CommandParameter != null)
                {
                    var listview = (ListView)button.CommandParameter;
                    HideIfZero();
                }
            };
            maingrid.BeginAnimation(Grid.OpacityProperty, da);
        }
        private void HideIfZero()
        {
            if (DispatcherControls.MyNotifyList.Count == 0)
            {
                DispatcherControls.MyNotifyList.Clear();
                NotifiesListView.UpdateLayout();
                this.Hide();
            }
        }
        private async void HideNotifiesListView_Button(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var obj = button.DataContext as ObservableCollection<MyNotifyClass>;
            if (obj != null && obj.Count>0)
            {
                var list = obj.ToList();
                foreach (var s in list)
                {
                    await Task.Delay(150);
                    obj.Remove(s);
                }
                obj.Clear();
            }
            HideIfZero();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
        }

        private async Task StartAutoCloseTemporaryNotifies(TimeSpan seconds)
        {
            await Task.Delay(seconds);
            if (DispatcherControls.MyNotifyList != null && DispatcherControls.MyNotifyList.Count() > 0)
            {
                var list = DispatcherControls.MyNotifyList.ToList();
                foreach (var t in list)
                {
                    if (t.IsLock) { t.timetoautoclose = TimeSpan.FromSeconds(2); continue; }
                    if ((DateTime.Now - t.dt).TotalSeconds > t.timetoautoclose.TotalSeconds)
                    {
                        DispatcherControls.MyNotifyList.Remove(t);
                    }
                }
                HideIfZero();
            }
            await StartAutoCloseTemporaryNotifies(seconds);
        }

        private void Grid_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = sender as Grid;
            var textbox = item.FindName("TextBoxText") as TextBox;
            var titlebox = item.FindName("TitleBoxText") as TextBox;
            if (textbox.IsFocused || titlebox.IsFocused) return;
            if (item != null)
            {
                if (FromWindow != null)
                {
                    if (FromWindow.Visibility == Visibility.Hidden)
                        FromWindow.Visibility = Visibility.Visible;
                    FromWindow.Show();
                    FromWindow.WindowState = WindowState.Normal;
                    FromWindow.Activate();
                    FromWindow.Focus();
                }
                DispatcherControls.MyNotifyList.Remove((MyNotifyClass)item.DataContext);
                HideIfZero();
            }
        }

        private void ListViewItem_MouseEnter(object sender, MouseEventArgs e)
        {
            var item = (ListViewItem)sender;
            var obj = item.DataContext as MyNotifyClass;
            if(obj!=null)
            obj.IsLock = true;
        }

        private void ListViewItem_MouseLeave(object sender, MouseEventArgs e)
        {
            var item = (ListViewItem)sender;
            var obj = item.DataContext as MyNotifyClass;
            if(obj!=null)
            obj.IsLock = false;
        }
    }
}
