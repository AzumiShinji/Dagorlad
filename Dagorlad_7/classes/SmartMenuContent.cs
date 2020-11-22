using Dagorlad_7.Windows;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dagorlad_7.classes
{
    public class SmartAnswersClass
    {
        public string name { get; set; }
        public ObservableCollection<SmartAnswers_SubClass> items { get; set; }
    }
    public class SmartAnswers_SubClass
    {
        public string title { get; set; }
        public string text { get; set; }
    }
    class SmartMenuContent
    {
        public static int MoveItem(int step, SmartAnswersClass obj)
        {
            var was_index= MySettings.Settings.SmartMenuList.IndexOf(obj);
            if (step < 0 && was_index == 0)
                return 0;
            if ((was_index+1) == MySettings.Settings.SmartMenuList.Count() && step > 0)
                return was_index;
            MySettings.Settings.SmartMenuList.Remove(obj);
            var new_index = was_index + step;
            MySettings.Settings.SmartMenuList.Insert(new_index, obj);
            return new_index;
        }
        public static int MoveItemSub(int step, SmartAnswers_SubClass obj)
        {
            ObservableCollection<SmartAnswers_SubClass> items = new ObservableCollection<SmartAnswers_SubClass>();
            foreach (var s in MySettings.Settings.SmartMenuList)
            {
                bool isfound = false;
                foreach (var i in s.items)
                    if (i == obj)
                    {
                        items = s.items;
                        isfound = true;
                        break;
                    }
                if (isfound) break;
            }
            if (items.Count() > 1)
            {
                var was_index = items.IndexOf(obj);
                if (step < 0 && was_index == 0)
                    return 0;
                if ((was_index + 1) == items.Count() && step > 0)
                    return was_index;
                items.Remove(obj);
                var new_index = was_index + step;
                items.Insert(new_index, obj);
                return new_index;
            }
            return 0;
        }
    }
}
