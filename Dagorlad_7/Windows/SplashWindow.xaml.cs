using System.Windows;

namespace Dagorlad_7.Windows
{
    /// <summary>
    /// Логика взаимодействия для SplashWindow.xaml
    /// </summary>
    public partial class SplashWindow : Window
    {
        public SplashWindow()
        {
            InitializeComponent();
            AppNameLabel.Content = "DAGORLAD";
            var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            AppVersionLabel.Content = string.Format("v.{0}.{1}",version.Major,version.Minor);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
        }
    }
}
