using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Dagorlad_7.classes
{
    public class Hash
    {
        public static string GetHash(HashAlgorithm hashAlgorithm, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            var sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        // Verify a hash against a string.
        public static bool VerifyHash(HashAlgorithm hashAlgorithm, string inputNOTHashed, string hash)
        {
            // Hash the input.
            var hashOfInput = GetHash(hashAlgorithm, inputNOTHashed);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            return comparer.Compare(hashOfInput, hash) == 0;
        }

        public static async Task<KeyValuePair<bool, string>> GetHashFromWebServiceEmployees(string Email)
        {
            object result = null;
            try
            {
                using (SHA256 sha256Hash = SHA256.Create())
                {
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = DispatcherControls.ConnectionString_SUE;
                    con.Open();
                    using (SqlCommand sqlCommand = new SqlCommand("SELECT Identityhashmap from Users where Email like @Email", con))
                    {
                        sqlCommand.Parameters.AddWithValue("@Email", Email.Trim());
                        result = await sqlCommand.ExecuteScalarAsync();
                    }
                    con.Close();
                    if (result == DBNull.Value)
                        return new KeyValuePair<bool, string>(true, null);
                    else
                    {
                        if (VerifyHash(sha256Hash, GetUniqueIdentityOfCurrentComputer(Email), (string)result))
                            return new KeyValuePair<bool, string>(true, (string)result);
                        return new KeyValuePair<bool, string>(false, null);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return new KeyValuePair<bool, string>(false, null);
        }
        public static async Task SetHashToWebServiceEmployees(string Email)
        {
            try
            {
                using (SHA256 sha256Hash = SHA256.Create())
                {
                    string Identityhashmap = Hash.GetHash(sha256Hash, GetUniqueIdentityOfCurrentComputer(Email));
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = DispatcherControls.ConnectionString_SUE;
                    con.Open();
                    using (SqlCommand sqlCommand = new SqlCommand("UPDATE Users SET Identityhashmap=@Identityhashmap WHERE Email like @Email", con))
                    {
                        sqlCommand.Parameters.AddWithValue("@Email", Email.Trim());
                        sqlCommand.Parameters.AddWithValue("@Identityhashmap", Identityhashmap);
                        await sqlCommand.ExecuteNonQueryAsync();
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        private static string GetUniqueIdentityOfCurrentComputer(string Email)
        {
            var username = Environment.UserName;
            var WindowsIdentity = System.Security.Principal.WindowsIdentity.GetCurrent().User.Value;
            var machinename = Environment.MachineName;
            var result = String.Format("{0}-{1}-{2}-{3}",username,WindowsIdentity,machinename,Email);
            return result;
        }
        public async static Task<bool> CheckAllowingEmail(string Email)
        {
#if(DEBUG)
            return true;
#endif
            var result = await Hash.GetHashFromWebServiceEmployees(Email);
            if (result.Key == true)
            {
                var remote_hash = result.Value;
                if (!String.IsNullOrEmpty(remote_hash))
                {
                    using (SHA256 sha256Hash = SHA256.Create())
                    {
                        return VerifyHash(sha256Hash, GetUniqueIdentityOfCurrentComputer(Email), remote_hash);
                    }
                }
                else
                {
                    await SetHashToWebServiceEmployees(Email);
                    return true;
                }
            }
            return false;
        }
    }
}
