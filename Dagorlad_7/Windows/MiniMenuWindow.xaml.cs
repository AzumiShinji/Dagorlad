using Dagorlad_7.classes;
using System;
using System.Collections.Generic;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Dagorlad_7.Windows
{
    /// <summary>
    /// Логика взаимодействия для MiniMenuWindow.xaml
    /// </summary>
    public partial class MiniMenuWindow : Window
    {
        double _left = 0;
        Rect main_window_size;
        public MiniMenuWindow()
        {
            InitializeComponent();
            LoadEvents();
        }
        private void LoadEvents()
        {
            //var current_window = System.Windows.Forms.Screen.FromHandle(new WindowInteropHelper(this).Handle);
            main_window_size = System.Windows.SystemParameters.WorkArea;
            //current_window_size = current_window.WorkingArea;
            this.Left = main_window_size.Width - this.Width;
            _left = this.Left;
        }

        private void Head_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
                var left_part = main_window_size.Width / 2;
                if (this.Left > left_part)
                    this.Left = _left;
                else this.Left = 0;
                if ((this.Top+this.Height) > main_window_size.Height)
                    this.Top = main_window_size.Height-this.ActualHeight;
            }
        }

        private void ShowSmartMenu_Click(object sender, RoutedEventArgs e)
        {
            DispatcherControls.ShowSmartMenu();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
        }
    }
}
