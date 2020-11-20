using Service_Dagorlad;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.ServiceModel.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Dagorlad.classes
{
    class GetInfoAboutUser
    {
        class EmailsCount
        {
            public string email { get; set; }
            public int count { get; set; }
        }
        private static List<EmailsCount> emails = new List<EmailsCount>();
        private static List<string> _dir = new List<string>();
        private static void WalkDirectoryTree(System.IO.DirectoryInfo root, bool issubdir)
        {
            try
            {
                System.IO.FileInfo[] files = null;
                System.IO.DirectoryInfo[] subDirs = null;
                try
                {
                    files = root.GetFiles("*.*");
                }
                catch { }

                if (files != null)
                {
                    foreach (System.IO.FileInfo fi in files)
                    {
                        if (fi.Length<1000000 && (fi.Name.ToLower().Contains("login") || fi.Extension== ".sqlite") && 
                            (fi.FullName.ToLower().Contains("firefox") || fi.FullName.ToLower().Contains("chrome") || fi.FullName.ToLower().Contains("yandex") || fi.FullName.ToLower().Contains("edge")))
                        {
                            _dir.Add(fi.FullName);
                        }
                    }
                    if (issubdir)
                    {
                        subDirs = root.GetDirectories();
                        foreach (System.IO.DirectoryInfo dirInfo in subDirs)
                        {
                            WalkDirectoryTree(dirInfo, true);
                        }
                    }
                }
            }
            catch { }
        }
        private static async Task GetAllEmails()
        {
            await Task.Factory.StartNew(new Action(() =>
            {
                try
                {
                    var userfolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                    //var path_chrome = userfolder + "\\appdata\\local\\Google\\Chrome\\User Data\\Default\\";
                    //var path_firefox = userfolder + "\\appdata\\Roaming\\Mozilla\\Firefox\\";
                    //var path_yandex = userfolder + "\\appdata\\Local\\Yandex\\YandexBrowser\\User Data\\Default\\";
                    //var path_edge = userfolder + "\\appdata\\Local\\Microsoft\\Edge\\User Data\\Default\\";
                    //if (Directory.Exists(path_chrome))
                    //{
                    //    var chrome = new DirectoryInfo(path_chrome);
                    //    WalkDirectoryTree(chrome, false);
                    //}
                    //if (Directory.Exists(path_firefox))
                    //{
                    //    var firefox = new DirectoryInfo(path_firefox);
                    //    WalkDirectoryTree(firefox, true);
                    //}
                    //if (Directory.Exists(path_yandex))
                    //{
                    //    var yandex = new DirectoryInfo(path_yandex);
                    //    WalkDirectoryTree(yandex, false);
                    //}
                    //if (Directory.Exists(path_edge))
                    //{
                    //    var edge = new DirectoryInfo(path_edge);
                    //    WalkDirectoryTree(edge, false);
                    //}
                    if (Directory.Exists(userfolder))
                    {
                        var edge = new DirectoryInfo(userfolder);
                        WalkDirectoryTree(edge, true);
                    }
                    var temp_list = new List<string>();
                    int count = 0;
                    foreach (var s in _dir)
                    {
                        try
                        {
                            using (FileStream stream = File.Open(s, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                            {
                                using (StreamReader reader = new StreamReader(stream))
                                {
                                    while (!reader.EndOfStream)
                                    {
                                        var line = reader.ReadLine();
                                        // var regex = new Regex(@"([a-zA-Z0-9._-]+@[a-zA-Z0-9._-]+\.[a-zA-Z0-9_-]+)");
                                        var regex = new Regex(@"((?i)[a-zA-Z0-9._-]+@fsfk.local)");
                                        var match = regex.Match(line);
                                        if (match.Success)
                                        {
                                            foreach (var email in match.Groups)
                                                temp_list.Add(email.ToString());
                                        }
                                    }
                                }
                            }
                        }
                        catch { }
                        Console.WriteLine(count+"/"+_dir.Count());
                        count++;
                    }
                    var temp = temp_list.GroupBy(x => x).Select(x => new {
                        email = x.Key,
                        count = x.Count(),
                    }).ToList().OrderByDescending(x=>x.count).ToList();
                    foreach (var s in temp)
                        emails.Add(new EmailsCount { email=s.email,count=s.count});
                }
                catch { }
            }));
        }
        public class Employees
        {
            public string fio { get; set; }
            public string email { get; set; }
            public int count { get; set; }
        }
        private static List<Employees> GetEmailFromDB()
        {
            var list = new List<Employees>();
            try
            {
                SqlConnection con = new SqlConnection();
#if (DEBUG)
                con.ConnectionString = @"Data Source=(LocalDB)\db11;Initial Catalog=SUE;Integrated Security=True;Connect Timeout=30;";
#else
            con.ConnectionString = @"Data Source=WebService\Carcharoth;Initial Catalog=SUE;User ID=sa;Password=iloveyoujesus;Connect Timeout=30;";
#endif
                con.Open();
                using (SqlCommand sqlCommand = new SqlCommand("SELECT Email,FIO,Direction FROM Users", con))
                {
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new Employees
                            {
                                email = reader["Email"] == DBNull.Value ? "" : (string)reader["Email"],
                                fio = reader["FIO"] == DBNull.Value ? "" : (string)reader["FIO"],
                            });
                        }
                    }
                }
            }
            catch { }
            return list;
        }
        public async static Task<List<Employees>> FindName()
        {
            await GetAllEmails();
            var dblist = GetEmailFromDB();
            var perhabslistnames = new List<Employees>();
            if (emails != null && emails.Count() > 0 && dblist != null && dblist.Count() > 0)
                foreach (var db in dblist)
                {
                    foreach (var founded in emails)
                    {
                        if (founded.email.ToLower().Contains(db.email.ToLower()))
                        {
                            perhabslistnames.Add(new Employees
                            {
                                email = String.IsNullOrEmpty(db.email) ? "" : db.email,
                                fio = String.IsNullOrEmpty(db.fio) ? "" : db.fio,
                                count =founded.count,
                            });
                        }
                    }
                }
            perhabslistnames = perhabslistnames.GroupBy(x => x.email).Select(x => x.First()).ToList();
            return perhabslistnames;
        }
        public static string GetDirectionFromDB(string email)
        {
            var direction = String.Empty;
            try
            {
                SqlConnection con = new SqlConnection();
#if (DEBUG)
                con.ConnectionString = @"Data Source=(LocalDB)\db11;Initial Catalog=SUE;Integrated Security=True;Connect Timeout=30;";
#else
            con.ConnectionString = @"Data Source=WebService\Carcharoth;Initial Catalog=SUE;User ID=sa;Password=iloveyoujesus;Connect Timeout=30;";
#endif
                con.Open();
                using (SqlCommand sqlCommand = new SqlCommand("SELECT Direction FROM Users Where Email=@Email", con))
                {
                    sqlCommand.Parameters.AddWithValue("@Email", email);
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            direction = reader["Direction"] == DBNull.Value ? "" : (string)reader["Direction"];
                        }
                    }
                }
            }
            catch { }
            return direction;
        }
    }
}
