using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Dagorlad.classes
{
    class ControlCPUUsage
    {
        private static PerformanceCounter theCPUCounter =
   new PerformanceCounter("Process", "% Processor Time",
   Process.GetCurrentProcess().ProcessName, true);
        public static void CheckCPUUsage(bool forcerestart, int maxusagecpu, int countstack, TimeSpan timebeforestart, TimeSpan timeout)
        {
            Dispatcher.CurrentDispatcher.BeginInvoke(new Action(async () =>
            {
                try
                {
                    int stack = 0;
                    await Task.Delay(timebeforestart);
                    while (true)
                    {
                        await Task.Delay(timeout);
                        var value = Math.Round((double)theCPUCounter.NextValue() / Environment.ProcessorCount, 1);
                        Console.WriteLine("CPU Usage: {0}%", value);
                        Logger.Do(new StringBuilder().AppendFormat("CPU Usage: {0}%", value),
                            Logger.LevelAttention.Information);
                        foreach (var window in Application.Current.Windows)
                            if (window.GetType() == typeof(MainWindow))
                            {
                                var mw = (MainWindow)window;
                                mw.NameApp.Content = value;
                            }

                        if (value > maxusagecpu)
                        {
                            if (forcerestart && stack > countstack)
                            {
                                NotifyWindowCustom.ShowNotify("Внимание!", "Потребление процессора превышено, " +
                                    "ради комфортной работы - программа будет перезапущена.", "notify_critical.png", 11, true, typeof(MainWindow), null, null);
                                await Task.Delay(10000);
                                Update.ExecuteAsAdmin(Assembly.GetEntryAssembly().Location, null);
                                Process.GetCurrentProcess().Kill();
                            }
                            stack++;
                        }
                        else stack = 0;
                    }
                }
                catch (Exception ex)
                {
                    Logger.Do(ex.ToString(),Logger.LevelAttention.Warning);
                }
            }));
        }
    }
}
