using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using ClosedXML.Excel;

namespace EmployeeManagementSystem.App_Code
{
    public class Common
    {
        string EncryptionKey = "ActivityMaster2023";
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
            sqlConnectionStringBuilder.InitialCatalog = "EMSNew";
            sqlConnectionStringBuilder.UserID = "sa";
            sqlConnectionStringBuilder.Password = "admin@123";
            sqlConnectionStringBuilder.MultipleActiveResultSets = true;

            return sqlConnectionStringBuilder.ToString();
        }
        public string Encrypt(string clearText)
        {
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
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff");
        }

        public DataTable ConvertExcelToDataTable(string filePath, string OnlyColumn = "N")
        {
            DataTable dt = new DataTable();
            try
            {
                using (XLWorkbook workbook = new XLWorkbook(filePath))
                {
                    IXLWorksheet worksheet = workbook.Worksheet(1);
                    bool firstRow = true;
                    foreach (IXLRow row in worksheet.Rows())
                    {
                        if (firstRow)
                        {
                            if (OnlyColumn == "N")
                            {
                                foreach (IXLCell cell in row.Cells())
                                {
                                    dt.Columns.Add(cell.Value.ToString());
                                }
                                firstRow = false;
                            }
                            else
                            {
                                dt.Columns.Add("Text", typeof(string));
                                dt.Columns.Add("Value", typeof(string));
                                foreach (IXLCell cell in row.Cells())
                                {
                                    DataRow dr = dt.NewRow();
                                    dr["Text"] = cell.Value.ToString();
                                    dr["Value"] = cell.Value.ToString();
                                    dt.Rows.Add(dr);
                                }
                                return dt;
                            }
                        }
                        else
                        {
                            dt.Rows.Add();
                            int i = 0;
                            foreach (IXLCell cell in row.Cells(1, dt.Columns.Count))
                            {
                                dt.Rows[dt.Rows.Count - 1][i] = cell.Value.ToString();
                                i++;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            dt.Rows.Cast<DataRow>().Where(r => r.ItemArray.All(c => c is DBNull || string.IsNullOrWhiteSpace(c.ToString())))
                            .ToList().ForEach(r => dt.Rows.Remove(r));
            return dt;
        }
    }
}