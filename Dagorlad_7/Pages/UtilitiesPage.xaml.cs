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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Dagorlad_7.Pages
{
    /// <summary>
    /// Логика взаимодействия для UtilitiesPage.xaml
    /// </summary>
    public partial class UtilitiesPage : Page
    {
        public UtilitiesPage()
        {
            InitializeComponent();
            if (!String.IsNullOrEmpty(MySettings.Settings.ClearingFolder))
                FolderToBeClearedTextBlock.Text = MySettings.Settings.ClearingFolder;
        }
        private void FolderToBeClearedButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                MySettings.Settings.ClearingFolder = dialog.SelectedPath;
                MySettings.Save();
                FolderToBeClearedTextBlock.Text = MySettings.Settings.ClearingFolder;
            }
        }
    }
}
