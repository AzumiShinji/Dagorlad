using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
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
using System.Windows.Threading;

namespace Dagorlad
{
    /// <summary>
    /// Логика взаимодействия для Employees.xaml
    /// </summary>
    public partial class Employees : Window
    {
        private class EmployeesClass
        {
            public int ID { get; set; }
            public string Code { get; set; }
            public string FIO { get; set; }
            public string Email { get; set; }
            public string Direction { get; set; }
            public string Position { get; set; }
            public string Phone { get; set; }
            public string BirthDate { get; set; }
        }

        public Employees()
        {
                InitializeComponent();
                StartUp();
                WindowSize();
            if(Properties.Settings.Default.isInvertTheme)
            {
                employeesimg.Source= new BitmapImage(new Uri(@"/Dagorlad;component/Resources/black/employees.png", UriKind.Relative));
                this.Resources["ColorOne"] = new SolidColorBrush(Colors.Black);
                this.Resources["ColorTwo"] = new SolidColorBrush(Colors.White);
            }
        }

        public async void StartUp()
        {
            await Task.Run(async() =>
            {
                Dispatcher.Invoke(() =>
                {
                    BirthMonthLabel.Text = "Ближайшие дни рождения:";
                    BirthDayLabel.Text = "Сегодня празднуют свой день рождения:";
                }, DispatcherPriority.ContextIdle);
                FillListEmployees().Wait();
                ListRead().Wait();
                Dispatcher.Invoke(() =>
                {
                    DataGrid_Employees.CanUserAddRows = false;
                    DataGrid_Employees.CanUserDeleteRows = false;
                    DataGrid_Employees.CanUserSortColumns = false;
                    DataGrid_Employees.CanUserReorderColumns = false;
                }, DispatcherPriority.ContextIdle);
                await DetectedBirthDay();
            });
        }

        static List<EmployeesClass> ListEmployees = new List<EmployeesClass>();
        private static async Task FillListEmployees()
        {
            ListEmployees.Clear();
            if (MainWindow.CheckNeededIP())
                await System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    try
                    {
                        SqlConnection con = new SqlConnection();
#if (DEBUG)
                    con.ConnectionString = @"Data Source=(LocalDB)\db11;Initial Catalog=SUE;Integrated Security=True;Connect Timeout=30;";
#else
                        con.ConnectionString = @"Data Source=WebService\Carcharoth;Initial Catalog=SUE;User ID=sa;Password=iloveyoujesus;Connect Timeout=30;";
#endif
                        con.Open();
                        using (SqlCommand sqlCommand = new SqlCommand("SELECT * FROM Users", con))
                        {
                            using (SqlDataReader reader = sqlCommand.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    DateTime.TryParse((string)reader["BirthDate"], out DateTime date);
                                    ListEmployees.Add(new EmployeesClass
                                    {
                                        ID = (int)reader["ID"],
                                        Code = (string)reader["Code"],
                                        FIO = (string)reader["FIO"],
                                        Email = (string)reader["Email"],
                                        Direction = (string)reader["Direction"],
                                        Position = (string)reader["Position"],
                                        Phone = (string)reader["Phone"],
                                        BirthDate = date.ToString("dd MMMM")
                                    });
                                }
                            }
                        }
                        con.Close();
                    }
                    catch (Exception ex) { MessageBox.Show(ex.ToString()); }
                }));
            else
            {
                ListEmployees.Add(new EmployeesClass
                {
                    ID = 0,
                    Code = "Не та подсеть",
                    FIO = "Нет доступа",
                    Direction="none"
                });
            }
        }
        private async Task ListRead()
        {
            try
            {
                await Task.Run(() =>
                {
                     ListCollectionView collection = new ListCollectionView(ListEmployees);
                    collection.GroupDescriptions.Add(new PropertyGroupDescription("Direction"));
                    Dispatcher.Invoke(() =>
                    {
                        DataGrid_Employees.ItemsSource = collection;
                        DataGrid_Employees.Items.Refresh();
                    }, DispatcherPriority.ContextIdle);
                });
            }
            catch (Exception ex) {
                Dispatcher.Invoke(() =>
                {
                    var data = new EmployeesClass { FIO = ex.ToString() }; DataGrid_Employees.Items.Add(data);
                    DataGrid_Employees.IsReadOnly = true;
                }, DispatcherPriority.ContextIdle);
            }
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void DataGrid_Employees_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        public class _DateTime
        {
            public DateTime? BirthDate {get;set;}
            public DateTime? CompareDayMonth { get; set; }
            public string FIO { get; set; }
        }

        private async Task DetectedBirthDay()
        {
            await Task.Run(() =>
            {
                Dispatcher.Invoke(() =>
                {
                    BirthMonthLabel.Text = "Ближайшие дни рождения: ";
                }, DispatcherPriority.ContextIdle);
                List<_DateTime> _Month = new List<_DateTime>();
                foreach (var s in ListEmployees)
                {
                    var date = DateTime.TryParse(s.BirthDate, out DateTime result);
                    if (date)
                    {
                        _Month.Add(new _DateTime
                        {
                            BirthDate = result,
                            CompareDayMonth = new DateTime(1900, result.Month, result.Day, 0, 0, 0),
                            FIO = s.FIO
                        });
                        if (result.Day == DateTime.Now.Day && result.Month == DateTime.Now.Month)
                        {
                            //var old = DateTime.Now.Year - result.Year;
                            Dispatcher.Invoke(() =>
                            {
                                BirthDayLabel.Text += "\n" + s.FIO + " ( " + s.BirthDate + " )";
                            }, DispatcherPriority.ContextIdle);
                        }
                    }
                }

                _Month.Sort((a, b) => a.CompareDayMonth.Value.CompareTo(b.CompareDayMonth.Value));

                int i = 0;
                foreach (var s in _Month)
                {
                    if (s.BirthDate.Value.Month == DateTime.Now.Month && s.BirthDate.Value.Day > DateTime.Now.Day)
                    {
                        i++;
                        Dispatcher.Invoke(() =>
                        {
                            BirthMonthLabel.Inlines.Add(new LineBreak());
                            //var old = DateTime.Now.Year - s.BirthDate.Value.Year;
                            Run run = new Run(s.FIO + " ( " + s.BirthDate.Value.ToString("dd MMMM")+" )");
                            BirthMonthLabel.Inlines.Add(run);
                        }, DispatcherPriority.ContextIdle);
                    }
                    else
                    if (s.BirthDate.Value.Month > DateTime.Now.Month)
                    {
                        i++;
                        Dispatcher.Invoke(() =>
                        {
                            BirthMonthLabel.Inlines.Add(new LineBreak());
                            //var old = DateTime.Now.Year - s.BirthDate.Value.Year;
                            Run run = new Run(s.FIO + " (" + s.BirthDate.Value.ToString("dd MMMM") + " )");
                            BirthMonthLabel.Inlines.Add(run);
                        }, DispatcherPriority.ContextIdle);
                    }
                    if (i == 5)
                        break;
                }
            });
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try { this.DragMove(); } catch { }
        }

        public static string FIO_BirthDay="";
        public static async void JsonReadNotify()
        {
            try
            {
                string BFIO = "";
                await Task.Run(async() =>
                {
                    if (ListEmployees.Count == 0)
                        await FillListEmployees();

                    foreach (var s in ListEmployees)
                    {
                        var date = DateTime.TryParse(s.BirthDate, out DateTime result);
                        if (date)
                        {
                            if (result.Day == DateTime.Now.Day && result.Month == DateTime.Now.Month)
                            {
                                NotifyWindowCustom.ShowNotify("Сегодня день рождение!", s.FIO, "Notify_birthday.png", 13, true, typeof(MainWindow),null,null);
                                BFIO += s.FIO + "; ";
                            }
                        }
                    }
                    if (BFIO != "")
                        FIO_BirthDay = "Сегодня день рождение у: " + BFIO;
                    else FIO_BirthDay = BFIO;
                });
            }
            catch { }
        }

        private void WindowSize()
        {
            this.Height = System.Windows.SystemParameters.PrimaryScreenHeight * 0.7;
            this.Width = System.Windows.SystemParameters.PrimaryScreenWidth * 0.9;
            DataGrid_Employees.FontSize = Math.Round(this.Width * 0.0089, MidpointRounding.AwayFromZero);
            double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            double windowWidth = this.Width;
            double windowHeight = this.Height;
            this.Left = (screenWidth / 2) - (windowWidth / 2);
            this.Top = (screenHeight / 2) - (windowHeight / 2);
        }
    }
}
