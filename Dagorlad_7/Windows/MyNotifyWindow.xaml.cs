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
        public string text { get; set; }
    }
    public partial class MyNotifyWindow : Window
    {
        Window FromWindow;
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
            StartAutoCloseTemporaryNotifies(150);
        }
        public async Task StandartPositionWindow()
        {
            var main_window_size = System.Windows.SystemParameters.WorkArea;
            this.Left = main_window_size.Width - this.Width;
            while (NotifiesListView.IsLoaded == false)
                await Task.Delay(100);
            this.Top = main_window_size.Height - this.ActualHeight-3;
        }

        private async void NotifiesListView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (DispatcherControls.MyNotifyList.Count > 1)
                HideNotifiesListViewButton.Visibility = Visibility.Visible;
            else HideNotifiesListViewButton.Visibility = Visibility.Hidden;
            await StandartPositionWindow();
        }

        private void CloseButton(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
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
                    if (listview.Items.Count == 0)
                    {
                        this.Hide();
                    }
                }
            };
            maingrid.BeginAnimation(Grid.OpacityProperty, da);
        }

        private void HideNotifiesListView_Button(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var obj = button.DataContext as ObservableCollection<MyNotifyClass>;
            var da = new DoubleAnimation()
            {
                Duration = new Duration(TimeSpan.FromMilliseconds(150)),
                From = 1,
                To = 0,
            };
            da.Completed += async (q, f) =>
            {
                await Task.Delay(250);
                if (obj != null)
                    obj.Clear();
                this.Hide();
            };
            this.BeginAnimation(Window.OpacityProperty, da);
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
        }

        private async void StartAutoCloseTemporaryNotifies(int seconds)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(seconds));
            if (DispatcherControls.MyNotifyList != null && DispatcherControls.MyNotifyList.Count() > 0)
            {
                var list = DispatcherControls.MyNotifyList.ToList();
                foreach (var t in list)
                {
                    if ((DateTime.Now-t.dt).TotalSeconds > 5)
                    {
                        DispatcherControls.MyNotifyList.Remove(t);
                    }

                }
            }
            StartAutoCloseTemporaryNotifies(seconds);
        }
    }
}
