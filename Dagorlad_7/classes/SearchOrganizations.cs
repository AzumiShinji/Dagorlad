using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Dagorlad_7.classes
{
    public class OrganizationsClass
    {
        public string statusName { get; set; }
        public string inn { get; set; }
        public string code { get; set; }
        public string ogrn { get; set; }
        public string kpp { get; set; }
        public string okpoCode { get; set; }
        public string pgmu { get; set; }
        public string fullName { get; set; }
        public string fio { get; set; }
        public string recordNum { get; set; }
        public string cityName { get; set; }
        public string phone { get; set; }
        public string mail { get; set; }
        public string orfkCode { get; set; }
        public string orfkName { get; set; }
    }
    class SearchOrganizations
    {
#if (DEBUG)
        private static string ConnectionString = @"Data Source=(LocalDB)\db11;Initial Catalog=runbp;Integrated Security=True;Connect Timeout=30;";
#else
        private static string ConnectionString = @"Data Source=WebService\Carcharoth;Initial Catalog=runbp;User ID=sa;Password=iloveyoujesus";
#endif
        public static long CheckIfStringAsNumberOfOrganizations(string text)
        {
            if (text.Length < 20)
            {
                var _text = text.Trim();
                var isint = long.TryParse(_text, out long result);
                if (isint)
                {
                    return result;
                }
            }
            return 0;
        }
        public async static Task<List<OrganizationsClass>> TryFindOrganizations(long code)
        {
            var list = new List<OrganizationsClass>();
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = ConnectionString;
                con.Open();
                using (SqlCommand sqlCommand = new SqlCommand("SELECT * FROM runbp WHERE @code = inn OR @code=code OR @code=ogrn OR @code=kpp OR @code=pgmu OR @code = okpoCode", con))
                {
                    sqlCommand.Parameters.AddWithValue("@code", code.ToString());
                    using (SqlDataReader reader = await sqlCommand.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            list.Add(new OrganizationsClass
                            {
                                statusName = (string)reader["statusName"],
                                inn = (string)reader["inn"],
                                code = (string)reader["code"],
                                ogrn = (string)reader["ogrn"],
                                kpp = (string)reader["kpp"],
                                okpoCode = (string)reader["okpoCode"],
                                pgmu = (string)reader["pgmu"],
                                fullName = (string)reader["fullName"],
                                fio = ((string)reader["fio"]).FirstWordToUpper(),
                                recordNum = (string)reader["recordNum"],
                                cityName = ((string)reader["cityName"]).FirstWordToUpper(),
                                phone = (string)reader["phone"],
                                mail = (string)reader["mail"],
                                orfkCode = (string)reader["orfkCode"],
                                orfkName = (string)reader["orfkName"],
                            });
                        }
                    }
                }
                con.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return list;
        }
        public async static Task<DateTime?> GetUpdateDate()
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = ConnectionString;
                con.Open();
                using (SqlCommand sqlCommand = new SqlCommand("SELECT code FROM runbp WHERE statusName='last'", con))
                {
                    var result = (await sqlCommand.ExecuteScalarAsync()) as string;
                    var isdt = DateTime.TryParse(result, out DateTime dt);
                    if (isdt)
                        return dt;
                }
                con.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return null;
        }   
    }
    public static class Extensions
    {
        public static string FirstWordToUpper(this string text)
        {
            string result = text;
            if (!String.IsNullOrEmpty(result))
            {
                var lower_text = result.ToLower();
                var s = Regex.Replace(lower_text, @"(^\w)|(\s\w)", m => m.Value.ToUpper());
                result = s;
            }
            return result;
        }
    }
}
