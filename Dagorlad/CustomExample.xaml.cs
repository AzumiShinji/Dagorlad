using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Dagorlad
{
    /// <summary>
    /// Логика взаимодействия для CustomExample.xaml
    /// </summary>
    public partial class CustomExample : Window
    {
        public class CustomNameSpace
        {
            public string Folder { get; set; }
            public string Name { get; set; }
            public string Text { get; set; }
        }

        public CustomExample()
        {
            InitializeComponent();
            LoadFromJson();
            WindowSize();
            SaveButton.Content = "Сохранить";
            if (Properties.Settings.Default.isInvertTheme)
            {
                handimg.Source = new BitmapImage(new Uri(@"/Dagorlad;component/Resources/black/hand.png", UriKind.Relative));
                this.Resources["ColorOne"] = new SolidColorBrush(Colors.Black);
                this.Resources["ColorTwo"] = new SolidColorBrush(Colors.White);
            }
        }

        private void WindowSize()
        {
            this.Height = System.Windows.SystemParameters.PrimaryScreenHeight * 0.7;
            this.Width = System.Windows.SystemParameters.PrimaryScreenWidth * 0.9;
            DataGridJson.FontSize = Math.Round(this.Width * 0.0089, MidpointRounding.AwayFromZero);
            InfoLabel.Content = "{SD} - для вставки номера обращения из буфера обмена.\n" +
                "{time} - для вставки текущего времени."+
                "\nПапки с одинаковыми именами группируются в одну.";
            double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            double windowWidth = this.Width;
            double windowHeight = this.Height;
            this.Left = (screenWidth / 2) - (windowWidth / 2);
            this.Top = (screenHeight / 2) - (windowHeight / 2);
        }

        List<CustomNameSpace> ExampleList = new List<CustomNameSpace>();
        private async void LoadFromJson()
        {
            await Task.Run(() =>
            {
                foreach (var s in JsonProccess.ReadSettings())
                {
                    if (s.Folder == "") s.Folder = null;
                    ExampleList.Add(new CustomNameSpace()
                    {
                        Folder = s.Folder,
                        Name = s.Name,
                        Text = s.Text
                    });
                }
                Dispatcher.Invoke(() =>
                {
                    ListCollectionView collection = new ListCollectionView(ExampleList);
                    collection.GroupDescriptions.Add(new PropertyGroupDescription("Folder"));
                    DataGridJson.ItemsSource = collection;
                    DataGridJson.Items.Refresh();
                }, DispatcherPriority.ContextIdle);
            });
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            await Task.Run(() =>
            {
                Dispatcher.Invoke(() =>
                {
                    SaveButton.Content = "Сохранение...";
                }, DispatcherPriority.ContextIdle);
                JsonProccess.WriteSettings(ExampleList.OrderByDescending(x => x.Folder).ToList());
                Dispatcher.Invoke(() =>
                {
                    SaveButton.Content = "Сохранено!";
                    this.DialogResult = true;
                }, DispatcherPriority.ContextIdle);
            });
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try { this.DragMove(); } catch { }
        }
    }
}
