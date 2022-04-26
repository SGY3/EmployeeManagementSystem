using System;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
namespace ActivityManagementSystem.App_Code
{

    public class SendMail
    {
        Common cc = new Common();
        public void SendOraganizationVerify(string toMail)
        {
            string subject = "Verify your email";
            string body = "Click on Verification link to  activate your AMS Organization account <a href=\"" + cc.GetWepAppLink() + "Verify.aspx?oid=" + cc.Encrypt(toMail) + "\">";
            MailSend(subject, body, toMail);
        }
        public void SendNewUserVerify(string toMail, string ogCode)
        {
            string subject = "Verify your email";
            string body = "Click on Verification link to  activate your AMS account <a href=\"" + cc.GetWepAppLink() + "Verify.aspx?ogCode=" + cc.Encrypt(ogCode) + "&regMail=" + cc.Encrypt(toMail) + "\">";
            MailSend(subject, body, toMail);
        }
        public void SendNewEmpCreation(string toMail, string ogCode, string EmpId)
        {
            string subject = "AMS Login Credential";
            string body = "Your Organization id=" + ogCode + ", Username=" + EmpId + ", Your Password=" + EmpId + " </br> This is default password kindly change your password after login in";
            MailSend(subject, body, toMail);
        }
        public void SendNewActivityAssign(string ogCode, string empId, string ActivityId)
        {
            string subject = "New Activity Assign";
            string toMail = "";
            string body = "The Activity Id:" + ActivityId + " is assiged";
            using (SqlConnection con = new SqlConnection(cc.GetConnectionString()))
            {
                con.Open();
                string sql = "";
                sql = "Select [Email] FROM [MstrLogin] Where OgCode=@OgCode and UserName=@EmpId";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@OgCode", ogCode);
                    cmd.Parameters.AddWithValue("@EmpId", empId);
                    toMail = cmd.ExecuteScalar().ToString();
                }
            }
            MailSend(subject, body, toMail);
        }
        public void MailSend(string subject, string body, string toMail)
        {
            try
            {
                string fromAddress = "";
                string toAddress = toMail;
                string fromPassword = "";
                string host = "";
                int port = 0;
                using (SqlConnection con = new SqlConnection(cc.GetConnectionString()))
                {
                    con.Open();
                    string sql = "select [EmailId],[EmailPassword] ,[EmailPort],[EmailHost] from AMS_Parameter";
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();
                                fromAddress = reader["EmailId"].ToString().Trim();
                                fromPassword = reader["EmailPassword"].ToString().Trim();
                                host = reader["EmailHost"].ToString().Trim();
                                port = cc.StoI(reader["EmailPort"].ToString().Trim());
                            }
                        }
                    }
                }

                var smtp = new SmtpClient
                {
                    Host = host,
                    Port = port,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress, fromPassword)
                };
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                })
                {
                    smtp.Send(message);
                }
            }
            catch (Exception ex)
            {

            }
        }

    }
}