using Dagorlad_7.classes;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
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
    /// Логика взаимодействия для AuthenticationWebServiceWindow.xaml
    /// </summary>
    public partial class AuthenticationWebServiceWindow : Window
    {
        public AuthenticationWebServiceWindow()
        {
            InitializeComponent();
            LoginTextBox.Focus();
        }

        private async void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            StatusLabel.Foreground = Brushes.OrangeRed;
            StatusLabel.Content = null;
            var btn = (Button)sender;
            btn.IsEnabled = false;
            if (!String.IsNullOrEmpty(PasswordPasswordBox.Password) && !String.IsNullOrEmpty(LoginTextBox.Text))
            {
                string login = LoginTextBox.Text.Trim();
                string password = PasswordPasswordBox.Password.Trim();
                if (await Authentication(login, password))
                {
                    this.DialogResult = true;
                }
            } else StatusLabel.Content = "Не указан логин/пароль.";
            btn.IsEnabled = true;
        }
        private async Task<bool> Authentication(string login, string password)
        {
            object result = null;
            try
            {
                using (SHA256 sha256Hash = SHA256.Create())
                {
                    string hash = Hash.GetHash(sha256Hash, password);
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = DispatcherControls.ConnectionString_SUE;
                    con.Open();
                    using (SqlCommand sqlCommand = new SqlCommand("SELECT Level from UsersCarcharoth where Login like @username AND Password like @password", con))
                    {
                        sqlCommand.Parameters.AddWithValue("@username", login);
                        sqlCommand.Parameters.AddWithValue("@password", hash);
                        using (SqlDataReader reader = await sqlCommand.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                result = reader["Level"];
                            }
                        }
                    }
                    con.Close();
                }
                if (result != null && result != DBNull.Value && result.GetType() == typeof(int))
                {
                    StatusLabel.Content = "Авторизация прошла успешно.";
                    int level = (int)result;
                    if (level >= 3)
                    {
                        StatusLabel.Foreground=(this.FindResource("Background.Highlight") as SolidColorBrush);
                        StatusLabel.Content = "Успешно.";
                        await Task.Delay(500);
                        return true;
                    }
                    else
                    {
                        StatusLabel.Content = "Не хватает полномочий.";
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            StatusLabel.Content = "Неверный логин/пароль.";
            return false;
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                this.DialogResult = false;
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            if(this.DialogResult==null)
            this.Close();
        }
    }
}
