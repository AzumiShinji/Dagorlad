﻿using Dagorlad_7.classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Dagorlad_7.Windows
{
    /// <summary>
    /// Логика взаимодействия для SmartMenuWindow.xaml
    /// </summary>

    public partial class SmartMenuWindow : Window
    {
        public SmartMenuWindow()
        {
            InitializeComponent();
            LoadDataToSmartAnswersListBox();
        }

        private async void LoadDataToSmartAnswersListBox()
        {
            var temp_head_name = HeaderLabel.Content;
            HeaderLabel.Content = "ЗАГРУЗКА...";
            SmartAnswersGrid.DataContext = MySettings.Settings.SmartMenuList;
            await Task.Delay(100);
            await DispatcherControls.ChangeToStablePositionWindow(this);
            HeaderLabel.Content = temp_head_name;
        }
        private void SmartAnswersListBox_SelectedEvent(object sender, RoutedEventArgs e)
        {
            var obj = (ListBoxItem)sender;
            if (obj.DataContext != null)
            {
                var dc = obj.DataContext as SmartAnswersClass;
                if (dc != null)
                {
                    SmartAnswers_Items_ListBox.ItemsSource = dc.items;
                }
            }
        }

        private void SmartAnswers_NewItemAddButton_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(NewItemTextBox.Text))
            {
                MySettings.Settings.SmartMenuList.Add(new SmartAnswersClass
                {
                    name = NewItemTextBox.Text,
                    items = new ObservableCollection<SmartAnswers_SubClass>(),
                });
                NewItemTextBox.Text = null;
            }
        }

        private void SmartAnswers_NewItemSubAddButton_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(NewItemSubTextBox.Text))
            {
                var obj = SmartAnswersListBox.SelectedItem;
                if (obj != null)
                {
                    var selected = obj as SmartAnswersClass;
                    var items = selected.items;
                    items.Add(new SmartAnswers_SubClass { title = NewItemSubTextBox.Text });
                    NewItemSubTextBox.Text = null;
                }
            }
        }

        private void WindowHideButton_Click(object sender, RoutedEventArgs e)
        {
            HideAndSave();
        }

        private void DragablePanel_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void ControlContent_Click(object sender, RoutedEventArgs e)
        {
            var obj = (Button)sender;
            var tag = (string)obj.Tag;
            var cmd = obj.CommandParameter as string;
            var datacontext = obj.DataContext as SmartAnswers_SubClass;
            switch (tag)
            {
                case ("copy"):
                    {
                        if (!String.IsNullOrEmpty(datacontext.text))
                            try { Clipboard.SetText(datacontext.text); } catch { }
                        break;
                    }
                case ("show"):
                    {
                        var popup = obj.CommandParameter as Popup;
                        popup.IsOpen = true;
                        break;
                    }
                case ("remove"):
                    {
                        var items = SmartAnswers_Items_ListBox.ItemsSource as ObservableCollection<SmartAnswers_SubClass>;
                        items.Remove(datacontext);
                        break;
                    }
            }
            if (cmd != null)
                switch (cmd)
                {
                    case ("up"):
                        {
                            SmartAnswers_Items_ListBox.SelectedIndex= SmartMenuContent.MoveItemSub(-1, datacontext);
                            break;
                        }
                    case ("down"):
                        {
                            SmartAnswers_Items_ListBox.SelectedIndex = SmartMenuContent.MoveItemSub(1, datacontext);
                            break;
                        }
                }
        }
        private void ClosePopup_Click(object sender, RoutedEventArgs e)
        {
            var obj = (Button)sender;
            var popup = obj.CommandParameter as Popup;
            popup.IsOpen = false;
        }
        private void RemoveRootItem_Click(object sender, RoutedEventArgs e)
        {
            var obj = (Button)sender;
            var datacontext = obj.DataContext as SmartAnswersClass;
            MySettings.Settings.SmartMenuList.Remove(datacontext);
        }

        private void ShowControlMenu_Click(object sender, RoutedEventArgs e)
        {
            ShowRemovePanel(sender, true);
        }

        private void CancelRemove_Click(object sender, RoutedEventArgs e)
        {
            ShowRemovePanel(sender, false);
        }
        private void ShowRemovePanel(object sender, bool show)
        {
            var button = (Button)sender;
            var grid = (Grid)button.CommandParameter;
            Grid panel = (Grid)grid.FindName("RemovePanelGrid");
            Button removebutton = (Button)grid.FindName("SwtichRemovePanelButton");
            if (show)
            {
                panel.Visibility = Visibility.Visible;
                button.Visibility = Visibility.Collapsed;
            }
            else
            {
                panel.Visibility = Visibility.Collapsed;
                removebutton.Visibility = Visibility.Visible;
            }
        }

        private void MoveItem(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var datacontext = button.DataContext as SmartAnswersClass;
            var cmd = (string)button.CommandParameter;
            switch (cmd)
            {
                case ("up"):
                    {
                        SmartAnswersListBox.SelectedIndex= SmartMenuContent.MoveItem(-1, datacontext);
                        break;
                    }
                case ("down"):
                    {
                        SmartAnswersListBox.SelectedIndex = SmartMenuContent.MoveItem(1, datacontext);
                        break;
                    }
            }
        }
        private async void HideAndSave()
        {
            var da = new DoubleAnimation()
            {
                Duration = new Duration(TimeSpan.FromMilliseconds(150)),
                From = 1,
                To = 0,
            };
            da.Completed += (q, f) => { this.Hide(); };
            this.BeginAnimation(Window.OpacityProperty, da);
            await MySettings.Save();
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            HideAndSave();
        }
        private void Window_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                HideAndSave();
            }
        }
    }
    public class WidthFixedListViewConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null || parameter == null) return 0;
            var result = (double)value - System.Convert.ToDouble(parameter);
            if (result >= 0)
                return (double)value - System.Convert.ToDouble(parameter);
            else return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return 0;
        }
    }
}