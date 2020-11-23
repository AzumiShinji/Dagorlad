using Dagorlad_7.classes;
using Dagorlad_7.Windows;
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
        private void LoadEvents()
        {
            ShowMiniMenu();
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
            var g = new MySettingsWindow();
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
            Application.Current.Shutdown();
        }
    }
}
