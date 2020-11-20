using Dagorlad_7.classes;
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

namespace Dagorlad_7.Windows
{
    /// <summary>
    /// Логика взаимодействия для ContextMenuCustom.xaml
    /// </summary>
    public class SmartAnswersClass
    {
        public string name { get; set; }
        public ObservableCollection<SmartAnswers_SubClass> items { get; set; }
    }
    public class SmartAnswers_SubClass
    {
        public string name { get; set; }
    }
    public partial class ContextMenuCustom : Window
    {
        public ContextMenuCustom()
        {
            InitializeComponent();
            LoadDataToSmartAnswersListBox();
        }
        
        private void LoadDataToSmartAnswersListBox()
        {
            //SmartAnswersListBox.Cursor = Cursors.Hand;
            if (smartanswers.list == null || smartanswers.list.Count == 0)
            {
                smartanswers.list.Add(new SmartAnswersClass { name = "dgfgfd", items = new ObservableCollection<SmartAnswers_SubClass> { new SmartAnswers_SubClass { name = "test1" } } });
                smartanswers.list.Add(new SmartAnswersClass { name = "dgfgfd1", items = new ObservableCollection<SmartAnswers_SubClass> { new SmartAnswers_SubClass { name = "test2" } } });
                smartanswers.list.Add(new SmartAnswersClass { name = "dgfgfd2", items = new ObservableCollection<SmartAnswers_SubClass> { new SmartAnswers_SubClass { name = "test3" } } });
                smartanswers.list.Add(new SmartAnswersClass { name = "dgfgfd3", items = new ObservableCollection<SmartAnswers_SubClass> { new SmartAnswers_SubClass { name = "test4" } } });
            }
            SmartAnswersGrid.DataContext = smartanswers.list;
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
                smartanswers.list.Add(new SmartAnswersClass
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
                    items.Add(new SmartAnswers_SubClass { name = NewItemSubTextBox.Text });
                    NewItemSubTextBox.Text = null;
                }
            }
        }

        private void PopupContextMenu_CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
    }
}
