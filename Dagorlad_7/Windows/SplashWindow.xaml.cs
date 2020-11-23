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
            AppVersionLabel.Content = "v."+System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
        }
    }
}
