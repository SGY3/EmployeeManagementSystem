using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Data.SqlClient;
using static System.Net.WebRequestMethods;

namespace ActivityManagementSystem.App_Code
{
    public class Common
    {
        public string GetWepAppLink()
        {
            string link = " http://localhost:63122/";
            return link;
        }
        public string GetConnectionString()
        {
            SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder();
            sqlConnectionStringBuilder.Clear();
            sqlConnectionStringBuilder.DataSource = "MSI";
            sqlConnectionStringBuilder.InitialCatalog = "AMS2022";
            sqlConnectionStringBuilder.UserID = "sa";
            sqlConnectionStringBuilder.Password = "admin@123";
            sqlConnectionStringBuilder.MultipleActiveResultSets = true;

            return sqlConnectionStringBuilder.ToString();
        }
        public string Encrypt(string clearText)
        {
            string EncryptionKey = "AMS2022";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }
        public string Decrypt(string cipherText)
        {
            try
            {
                string EncryptionKey = "AMS2022";
                cipherText = cipherText.Replace(" ", "+");
                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(cipherBytes, 0, cipherBytes.Length);
                            cs.Close();
                        }
                        cipherText = Encoding.Unicode.GetString(ms.ToArray());
                    }
                }
                return cipherText;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        public int StoI(string value)
        {
            int valueFinal = Convert.ToInt32(value);
            return valueFinal;
        }
        public string GetActivityCode(string ogCode)
        {
            string finalId = "";
            string start = "A" + DateTime.Now.ToString("yyMMdd");
            int count = 100;
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                con.Open();
                string sql = "";
                sql = "select count(ActivityCode)+1 from MstrActivity where OgCode=@OgCode";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@OgCode", ogCode);
                    count += StoI(cmd.ExecuteScalar().ToString());
                }
            }
            finalId = start + count;
            return finalId;
        }
        public string GetOgCode()
        {
            string finalId = "";
            string start = "O";
            int count = 100;
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                con.Open();
                string sql = "";

                sql = "select count(OgCode)+1 from MstrOraganization";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    count += StoI(cmd.ExecuteScalar().ToString());
                }
            }
            finalId = start + count;
            return finalId;
        }
        public string GetToDoCode(string ogCode)
        {
            string finalId = "";
            string start = "T" + DateTime.Now.ToString("yyMMdd");
            int count = 100;
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                con.Open();
                string sql = "";
                sql = "select count(ToDoCode)+1 from MstrTodo where OgCode=@OgCode";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@OgCode", ogCode);
                    count += StoI(cmd.ExecuteScalar().ToString());
                }
            }
            finalId = start + count;
            return finalId;
        }
        public string GetEmpCode(string ogCode)
        {
            string finalId = "";
            string start = "E";
            int count = 100;
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                con.Open();
                string sql = "";
                sql = "select count(UserName)+1 from MstrLogin where OgCode=@OgCode";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@OgCode", ogCode);
                    count += StoI(cmd.ExecuteScalar().ToString());
                }
            }
            finalId = start + count;
            return finalId;
        }
        public string GetProjectCode(string ogCode)
        {
            string finalId = "";
            string start = "P";
            int count = 0;
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                con.Open();
                string sql = "";

                sql = "select count(ProjectId)+1 from MstrProject where OgCode=@OgCode";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@OgCode", ogCode);
                    count += StoI(cmd.ExecuteScalar().ToString());
                }
            }
            finalId = start + count;
            return finalId;
        }
        public string GetActivityTypeCode(string ogCode)
        {
            string finalId = "";
            string start = "A";
            int count = 0;
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                con.Open();
                string sql = "";

                sql = "select count(ActivityTypeId)+1 from [MstrActivityType] where OgCode=@OgCode";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@OgCode", ogCode);
                    count += StoI(cmd.ExecuteScalar().ToString());
                }
            }
            finalId = start + count;
            return finalId;
        }
        public string TodaysDate()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm");
        }
    }
}