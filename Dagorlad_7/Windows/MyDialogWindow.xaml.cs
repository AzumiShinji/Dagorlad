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
using System.Windows.Shapes;

namespace Dagorlad_7.Windows
{
    /// <summary>
    /// Логика взаимодействия для MyDialogWindow.xaml
    /// </summary>
    public partial class MyDialogWindow : Window
    {
        public enum ResultMyDialog
        {
            Undefined,
            Cancel = 1,
            Ok = 2,
            Yes = 3,
            No = 4,
        }
        public enum TypeMyDialog
        {
            Ok=0,
            YesNo=1,
        }
        TypeMyDialog _type;
        public MyDialogWindow(string title,string text,TypeMyDialog type)
        {
            InitializeComponent();
            _type=type;
            TitleLabel.Content = title;
            this.Title = title;
            TextTextBox.Text = text;
            switch(type)
            {
                case (TypeMyDialog.Ok):
                    {
                        YesButton.Visibility = Visibility.Collapsed;
                        YesButton.IsEnabled = false;
                        NoButton.Visibility = Visibility.Collapsed;
                        NoButton.IsEnabled = false;
                        GridButtons.ColumnDefinitions[0].Width = new GridLength(1-0, GridUnitType.Star);
                        break;
                    }
                case (TypeMyDialog.YesNo):
                    {
                        OkButton.Visibility = Visibility.Collapsed;
                        OkButton.IsEnabled = false;
                        GridButtons.ColumnDefinitions[1].Width = new GridLength(10, GridUnitType.Star);
                        GridButtons.ColumnDefinitions[2].Width = new GridLength(10, GridUnitType.Star);
                        break;
                    }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = false;
        }
        public ResultMyDialog result=ResultMyDialog.Undefined;
        private void EventClick(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var cmd = (string)button.CommandParameter;
            switch (cmd)
            {
                case ("ok"):
                    {
                        result = ResultMyDialog.Ok;
                        this.DialogResult = true;
                        break;
                    }
                case ("yes"):
                    {
                        result = ResultMyDialog.Yes;
                        this.DialogResult = true;
                        break;
                    }
                case ("no"):
                    {

                        result = ResultMyDialog.No;
                        this.DialogResult = true;
                        break;
                    }
                default:
                    {
                        result = ResultMyDialog.Cancel;
                        this.DialogResult = false;
                        break;
                    }
            }
        }
    }
}
