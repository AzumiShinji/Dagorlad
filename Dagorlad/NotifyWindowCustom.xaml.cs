using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Dagorlad
{
    /// <summary>
    /// Логика взаимодействия для NotifyWindowCustom.xaml
    /// </summary>
    public partial class NotifyWindowCustom : Window
    {
        DispatcherTimer TimeToCloseNotify = new DispatcherTimer();
        public NotifyWindowCustom(string title, string text, string image, int TimeShow, BitmapImage bitmapimage)
        {
            InitializeComponent();
            if (Properties.Settings.Default.isInvertTheme)
            {
                _border.Background = Brushes.White;
                backgroundimg.ImageSource = null;
                this.Resources["ColorOne"] = new SolidColorBrush(Colors.Black);
                this.Resources["ColorTwo"] = new SolidColorBrush(Colors.White);
            }
            setParameters(title, text, image, TimeShow, bitmapimage);
            ShowMoveActionStart();
        }

        private void setParameters(string title, string text, string image, int TimeShow, BitmapImage bitmapimage)
        {
            AppInfo.Content = Process.GetCurrentProcess().ProcessName + ", " + Logger.GetVersionApp();

            TitleNotify.Text = title;
            TextNotify.Text = text;
            if (TextNotify.Text.Length > 100)
            {
                TextNotify.FontSize = 10;
            }
            else TextNotify.FontSize = 12;

            if (image != null && !image.Contains("smiles"))
            {
                if (Properties.Settings.Default.isInvertTheme)
                    Image.Source = new BitmapImage(new Uri(@"Resources/black/" + image, UriKind.Relative));
                else
                    Image.Source = new BitmapImage(new Uri(@"Resources/" + image, UriKind.Relative));
            }
            else
            {
                if (image != null)
                    Image.Source = new BitmapImage(new Uri(image, UriKind.Relative));
                else
                {
                    if (bitmapimage != null)
                        Image.Source = bitmapimage;
                }
            }
            TitleNotify.Opacity = 0;
            TextNotify.Opacity = 0;
            Image.Opacity = 0;

            var w = System.Windows.SystemParameters.FullPrimaryScreenWidth;
            var h = System.Windows.SystemParameters.FullPrimaryScreenHeight;
            this.Left = w - this.Width;
            this.Top = h - this.Height;
            this.Width = 0;

            TimeToCloseNotify.Interval = TimeSpan.FromSeconds(TimeShow);
            TimeToCloseNotify.IsEnabled = true;
            TimeToCloseNotify.Tick += (object sender, EventArgs e) =>
            {
                CloseMoveActionStart();
            };
            TimeToCloseNotify.Start();

            this.MouseEnter += (a, b) => { TimeToCloseNotify.Stop(); };
            this.MouseLeave += (a, b) => { TimeToCloseNotify.Start(); };
        }

        public void CloseMoveActionStart()
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(async () =>
            {
                TimeToCloseNotify.Stop();
                DoubleAnimation da = new DoubleAnimation()
                {
                    BeginTime = TimeSpan.FromSeconds(.05),
                    From = 465,
                    To = 0,
                    Duration = new Duration(TimeSpan.FromSeconds(0.3)),
                    EasingFunction = new QuadraticEase() { EasingMode = EasingMode.EaseInOut }
                };
                DoubleAnimation da2 = new DoubleAnimation()
                {
                    From = 1,
                    To = 0,
                    Duration = new Duration(TimeSpan.FromSeconds(0.3)),
                    EasingFunction = new QuadraticEase() { EasingMode = EasingMode.EaseIn }
                };
                DoubleAnimation dalbl1 = new DoubleAnimation()
                {
                    BeginTime = TimeSpan.FromSeconds(.45),
                    From = 1,
                    To = 0,
                    Duration = new Duration(TimeSpan.FromSeconds(0.3)),
                    EasingFunction = new QuadraticEase() { EasingMode = EasingMode.EaseInOut }
                };
                DoubleAnimation dalbl2 = new DoubleAnimation()
                {
                    BeginTime = TimeSpan.FromSeconds(.55),
                    From = 1,
                    To = 0,
                    Duration = new Duration(TimeSpan.FromSeconds(0.3)),
                    EasingFunction = new QuadraticEase() { EasingMode = EasingMode.EaseInOut }
                };
                DoubleAnimation dalbl3 = new DoubleAnimation()
                {
                    BeginTime = TimeSpan.FromSeconds(.65),
                    From = 1,
                    To = 0,
                    Duration = new Duration(TimeSpan.FromSeconds(0.3)),
                    EasingFunction = new QuadraticEase() { EasingMode = EasingMode.EaseInOut }
                };
                TextNotify.BeginAnimation(OpacityProperty, dalbl1);
                TitleNotify.BeginAnimation(OpacityProperty, dalbl2);
                dalbl3.Completed += (q, e) =>
                {
                    da.Completed += (a, b) =>
                    {
                        Dispatcher.BeginInvoke(new Action(() => { this.Close(); }));
                    };
                    this.BeginAnimation(OpacityProperty, da2);
                    this.BeginAnimation(WidthProperty, da);
                };
                Image.BeginAnimation(OpacityProperty, dalbl3);
            }));
        }
        private void ShowMoveActionStart()
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                DoubleAnimation da = new DoubleAnimation()
                {
                    From = 0,
                    To = 465,
                    Duration = new Duration(TimeSpan.FromSeconds(0.3)),
                    EasingFunction = new QuadraticEase() { EasingMode = EasingMode.EaseInOut }
                };
                DoubleAnimation da2 = new DoubleAnimation()
                {
                    BeginTime = TimeSpan.FromSeconds(.05),
                    From = 0,
                    To = 1,
                    Duration = new Duration(TimeSpan.FromSeconds(0.3)),
                    EasingFunction = new QuadraticEase() { EasingMode = EasingMode.EaseIn }
                };
                DoubleAnimation dalbl0 = new DoubleAnimation()
                {
                    BeginTime = TimeSpan.FromSeconds(.35),
                    From = 0,
                    To = 1,
                    Duration = new Duration(TimeSpan.FromSeconds(0.3)),
                    EasingFunction = new QuadraticEase() { EasingMode = EasingMode.EaseInOut }
                };
                DoubleAnimation dalbl1 = new DoubleAnimation()
                {
                    BeginTime = TimeSpan.FromSeconds(.45),
                    From = 0,
                    To = 1,
                    Duration = new Duration(TimeSpan.FromSeconds(0.3)),
                    EasingFunction = new QuadraticEase() { EasingMode = EasingMode.EaseInOut }
                };
                DoubleAnimation dalbl2 = new DoubleAnimation()
                {
                    BeginTime = TimeSpan.FromSeconds(.55),
                    From = 0,
                    To = 1,
                    Duration = new Duration(TimeSpan.FromSeconds(0.3)),
                    EasingFunction = new QuadraticEase() { EasingMode = EasingMode.EaseInOut }
                };
                this.BeginAnimation(OpacityProperty, da2);
                this.BeginAnimation(WidthProperty, da);
                TextNotify.BeginAnimation(OpacityProperty, dalbl2);
                TitleNotify.BeginAnimation(OpacityProperty, dalbl1);
                Image.BeginAnimation(OpacityProperty, dalbl0);
            }));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CloseMoveActionStart();
        }

        public static bool IsWindowOpen(Type win)
        {
            int count = 0;
            foreach (Window openWin in System.Windows.Application.Current.Windows)
            {
                if (openWin.GetType()==win)
                    count++;
            }
            if (count > 0) return true;
            else
                return false;
        }
        public static MediaPlayer mp = new MediaPlayer();
        private static async void PlayBeep()
        {
            if(Properties.Settings.Default.onMusic==true)
            await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                mp.Open(new Uri(AppDomain.CurrentDomain.BaseDirectory+@"beep.mp3"));
                mp.Volume = 0.05;
                mp.Play();
            }));
        }
        public static async void ShowNotify(string title, string text, string image, int TimeShow, bool isWait,Type get_window,string name_conversation,BitmapImage bitmapimage)
        {
            await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(async () =>
            {
                if (!NotifyWindowCustom.IsWindowOpen(typeof(NotifyWindowCustom)))
                {
                    PlayBeep();
                    var g = new NotifyWindowCustom(title, text, image, TimeShow, bitmapimage);
                    g.ShowActivated = false;
                    g.Show();
                    var panel = g.FindName("_Panel") as StackPanel;
                    panel.PreviewMouseUp += (object sender, MouseButtonEventArgs e) =>
                    {
                        if (e.LeftButton == MouseButtonState.Released)
                        {
                            foreach (Window window in Application.Current.Windows)
                            {
                                if (window.GetType() == get_window)
                                {
                                    (window).Show();
                                    (window).Activate();
                                    (window).WindowState = WindowState.Normal;
                                    if (window.GetType() == typeof(Chat))
                                    {
                                        foreach (var s in (window as Chat).listofusersLB.Items)
                                            if (s.GetType() == typeof(Chat.ConversationsUsersAndGroupsClass))
                                            {
                                                var es = (Chat.ConversationsUsersAndGroupsClass)s;
                                                if (es.name == name_conversation)
                                                    (window as Chat).listofusersLB.SelectedItem = s;
                                            }
                                    }
                                }
                                if (!IsWindowOpen(typeof(Chat)) && get_window == typeof(Chat))
                                {
                                    var g = new Chat();
                                    g.Show();
                                    g.Activate();
                                }
                            }
                            g.CloseMoveActionStart();
                        }
                    };
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(Chat))
                        {
                            g.MouseLeave += (q, e) =>
                            {
                                g.CloseMoveActionStart();
                            };
                        }
                    }
                }
                else
                {
                    if (!isWait)
                    {
                        animationFadeOut();
                        await Task.Delay(900);
                        TimeShow = TimeShow / 2;
                        foreach (Window window in Application.Current.Windows.OfType<NotifyWindowCustom>())
                        {
                            ((NotifyWindowCustom)window).setParameters(title, text, image, TimeShow, bitmapimage);
                        }
                        animationFadeIn();
                    }
                    else
                    {
                        await Task.Delay(TimeShow);
                        ShowNotify(title, text, image, TimeShow, isWait, get_window, name_conversation, bitmapimage);
                    }
                    //if (!isWait)
                    //{
                    //    await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                    //     {
                    //         foreach (Window window in Application.Current.Windows.OfType<NotifyWindowCustom>())
                    //             ((NotifyWindowCustom)window).Close();
                    //     }));
                    //    await Task.Delay(TimeShow);
                    //    ShowNotify(title, text, image, TimeShow, isWait);
                    //}
                    //else
                    //{
                    //    await Task.Delay(TimeShow);
                    //    ShowNotify(title, text, image, TimeShow, isWait);
                    //}
                }
            }));
        }

        public static void animationFadeIn()
        {
            DoubleAnimation dalbl0 = new DoubleAnimation()
            {
                BeginTime = TimeSpan.FromSeconds(.35),
                From = 0,
                To = 1,
                Duration = new Duration(TimeSpan.FromSeconds(0.3)),
                EasingFunction = new QuadraticEase() { EasingMode = EasingMode.EaseInOut }
            };
            DoubleAnimation dalbl1 = new DoubleAnimation()
            {
                BeginTime = TimeSpan.FromSeconds(.45),
                From = 0,
                To = 1,
                Duration = new Duration(TimeSpan.FromSeconds(0.3)),
                EasingFunction = new QuadraticEase() { EasingMode = EasingMode.EaseInOut }
            };
            DoubleAnimation dalbl2 = new DoubleAnimation()
            {
                BeginTime = TimeSpan.FromSeconds(.55),
                From = 0,
                To = 1,
                Duration = new Duration(TimeSpan.FromSeconds(0.3)),
                EasingFunction = new QuadraticEase() { EasingMode = EasingMode.EaseInOut }
            };
            foreach (Window window in Application.Current.Windows.OfType<NotifyWindowCustom>())
            {
                ((NotifyWindowCustom)window).TextNotify.BeginAnimation(OpacityProperty, dalbl2);
                ((NotifyWindowCustom)window).TitleNotify.BeginAnimation(OpacityProperty, dalbl1);
                ((NotifyWindowCustom)window).Image.BeginAnimation(OpacityProperty, dalbl0);
            }
        }

        public static void animationFadeOut()
        {
            DoubleAnimation dalbl1 = new DoubleAnimation()
            {
                BeginTime = TimeSpan.FromSeconds(.45),
                From = 1,
                To = 0,
                Duration = new Duration(TimeSpan.FromSeconds(0.3)),
                EasingFunction = new QuadraticEase() { EasingMode = EasingMode.EaseInOut }
            };
            DoubleAnimation dalbl2 = new DoubleAnimation()
            {
                BeginTime = TimeSpan.FromSeconds(.55),
                From = 1,
                To = 0,
                Duration = new Duration(TimeSpan.FromSeconds(0.3)),
                EasingFunction = new QuadraticEase() { EasingMode = EasingMode.EaseInOut }
            };
            DoubleAnimation dalbl3 = new DoubleAnimation()
            {
                BeginTime = TimeSpan.FromSeconds(.65),
                From = 1,
                To = 0,
                Duration = new Duration(TimeSpan.FromSeconds(0.3)),
                EasingFunction = new QuadraticEase() { EasingMode = EasingMode.EaseInOut }
            };
            foreach (Window window in Application.Current.Windows.OfType<NotifyWindowCustom>())
            {
                ((NotifyWindowCustom)window).TextNotify.BeginAnimation(OpacityProperty, dalbl1);
                ((NotifyWindowCustom)window).TitleNotify.BeginAnimation(OpacityProperty, dalbl2);
                ((NotifyWindowCustom)window).Image.BeginAnimation(OpacityProperty, dalbl3);
            }
        }
    }
}
