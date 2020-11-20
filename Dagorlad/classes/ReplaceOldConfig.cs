using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Dagorlad
{
    class ReplaceOldConfig
    {
        private class PathAndVersionDirectory_fields
        {
            public string Path { get; set; }
            public Version Version { get; set; }
        }
        /// <summary>
        /// AskQuestion - show message or not
        /// </summary>
        /// <param name="AskQuestion"></param>
        public static void CheckAndReplace(bool AskQuestion)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal);//ссылки добавить configuration

            if (!File.Exists(config.FilePath))
            {
                List<PathAndVersionDirectory_fields> PathAndVersionDirectory = new List<PathAndVersionDirectory_fields>();
                try
                {
                    foreach (string d in Directory.GetDirectories(
                        Environment.GetFolderPath(
                            Environment.SpecialFolder.LocalApplicationData) + "\\" +
                            FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location).CompanyName))
                    {
                        if (d.Contains(FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location).ProductName))
                        {
                            foreach (string f in Directory.GetDirectories(d))
                            {
                                foreach (string g in Directory.GetFiles(f))
                                {
                                    PathAndVersionDirectory.Add(new PathAndVersionDirectory_fields
                                    {
                                        Path = g,
                                        Version = new Version(f.Split('\\').LastOrDefault())
                                    });
                                }
                            }
                        }
                    }
                    PathAndVersionDirectory.Sort((x, y) => x.Version.CompareTo(y.Version));//сортировка
                    var Old_ConfigFile = PathAndVersionDirectory.LastOrDefault().Path;//путь к последнему созданому user.config
                    //Получение пути новой директории version
                    var MustToBoCreatedDirectory = config.FilePath.Split('\\');
                    string New_DirectoryVersion = String.Empty;

                    for (int i = 0; i < MustToBoCreatedDirectory.Count() - 1; i++)
                    {
                        New_DirectoryVersion += MustToBoCreatedDirectory[i] + "\\";
                    }
                    Directory.CreateDirectory(New_DirectoryVersion);//создание директории version
                    //
                    if (AskQuestion)
                    {
                        if (File.Exists(Old_ConfigFile) && new FileInfo(Old_ConfigFile).Length != 0)
                        {
                            var answer = MessageBox.Show("Обнаружены настройки из предыдущей версии " + Assembly.GetEntryAssembly().GetName().Name + Environment.NewLine +
                                Old_ConfigFile.Split('\\').ElementAtOrDefault(Old_ConfigFile.Split('\\').Count() - 2) + Environment.NewLine +
                                Assembly.GetExecutingAssembly().GetName().Version.ToString() + " - новая версия" + Environment.NewLine +
                                "Перенести Ваши настройки в новую версию?", "Настройки", MessageBoxButton.YesNo, MessageBoxImage.Question);
                            if (answer == MessageBoxResult.Yes)
                                ReplaceConfig(Old_ConfigFile, config.FilePath);
                        }
                    }
                    else
                        ReplaceConfig(Old_ConfigFile, config.FilePath);

                }
                catch
                {
                    //MessageBox.Show("Ошибка при переносе файла:" + Environment.NewLine + excpt.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private static void ReplaceConfig(string Old_ConfigFile, string FilePath)
        {
            File.Copy(Old_ConfigFile, FilePath + "_", true);
            File.Move(FilePath + "_", FilePath);
            File.Delete(FilePath + "_");

            var ListArgs = new List<string>();
            var args = Environment.GetCommandLineArgs();
            for (int i = 1; i < args.Length; i ++)
                ListArgs.Add(args[i]);
            ListArgs.Add(Update._KeyConfig);
            var newArgs= String.Join(" ", ListArgs.ToArray());
            Process.Start(System.AppDomain.CurrentDomain.BaseDirectory + Assembly.GetEntryAssembly().GetName().Name + ".exe", newArgs);
            Process.GetCurrentProcess().Kill();
        }
    }
}
