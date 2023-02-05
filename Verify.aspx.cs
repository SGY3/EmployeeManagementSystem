using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;

namespace EmployeeManagementSystem.App_Code
{
    public partial class Verify : System.Web.UI.Page
    {
        Common cc = new Common();
        SendMail mail = new SendMail();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.QueryString["oid"] != null)
                {
                    string FirstName, LastName, Mobile, Email;
                    string email = cc.Decrypt(Request.QueryString["oid"].ToString());
                    if (email != "")
                    {
                        using (SqlConnection con = new SqlConnection(cc.GetConnectionString()))
                        {
                            con.Open();
                            string sql = "";
                            sql = "select [OgEmail] ,[FirstName] ,[LastName] ,[Mobile]  From MstrOraganization where OgEmail=@OgEmail and IsActive='Y' and ProcessCompleteYN='N'";
                            using (SqlCommand cmd = new SqlCommand(sql, con))
                            {
                                cmd.Parameters.AddWithValue("@OgEmail", email);
                                using (SqlDataReader reader = cmd.ExecuteReader())
                                {
                                    if (reader.HasRows)
                                    {
                                        reader.Read();
                                        FirstName = reader["FirstName"].ToString();
                                        LastName = reader["LastName"].ToString();
                                        Mobile = reader["Mobile"].ToString();
                                        Email = reader["OgEmail"].ToString();
                                        string ogCode = cc.GetOgCode();
                                        sql = "update MstrOraganization set OgCode=@OgCode,[ProcessCompleteYN]='Y',[EditedOn]=@EditedOn,[EditedBy]=@EditedBy where OgEmail=@OgEmail";
                                        using (SqlCommand cmdUpdate = new SqlCommand(sql, con))
                                        {
                                            cmdUpdate.Parameters.AddWithValue("@OgCode", ogCode);
                                            cmdUpdate.Parameters.AddWithValue("@EditedOn", cc.TodaysDate());
                                            cmdUpdate.Parameters.AddWithValue("@EditedBy", ogCode);
                                            cmdUpdate.Parameters.AddWithValue("@OgEmail", email);
                                            cmdUpdate.ExecuteNonQuery();
                                        }
                                        LoginInsert(ogCode, FirstName, LastName, email, Mobile, "Y");
                                        ShowMsg("Organization created successfully, check your mail for Login Credentials.");
                                    }
                                    else
                                    {
                                        ShowMsg("Oraganization data may not found or completed the Organixzation creation process");
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        ShowMsg("Something went wrong, please try again after sometime");
                    }

                }
                else if (Request.QueryString["ogCode"] != null && Request.QueryString["regMail"] != null)
                {
                    string FirstName, LastName, Mobile, Email;
                    string ogCode = cc.Decrypt(Request.QueryString["ogCode"].ToString());
                    string email = cc.Decrypt(Request.QueryString["regMail"].ToString());
                    if (ogCode != "" && email != "")
                    {
                        using (SqlConnection con = new SqlConnection(cc.GetConnectionString()))
                        {
                            con.Open();
                            string sql = "";
                            sql = "select top (1) [FirstName] ,[LastName] ,[Mobile],Email from MstrSignup where OgCode=@OgCode and ProcessCompleteYN='N' and Email=@Email order by CreatedOn desc";
                            using (SqlCommand cmd = new SqlCommand(sql, con))
                            {
                                cmd.Parameters.AddWithValue("@OgCode", ogCode);
                                cmd.Parameters.AddWithValue("@Email", email);
                                using (SqlDataReader reader = cmd.ExecuteReader())
                                {
                                    if (reader.HasRows)
                                    {
                                        reader.Read();
                                        FirstName = reader["FirstName"].ToString();
                                        LastName = reader["LastName"].ToString();
                                        Mobile = reader["Mobile"].ToString();
                                        Email = reader["Email"].ToString();
                                        sql = "update MstrSignup set [ProcessCompleteYN]='Y',[EditedOn]=@EditedOn,[EditedBy]=@EditedBy where OgCode=@OgCode and Email=@Email ";
                                        using (SqlCommand cmdUpdate = new SqlCommand(sql, con))
                                        {
                                            cmdUpdate.Parameters.AddWithValue("@OgCode", ogCode);
                                            cmdUpdate.Parameters.AddWithValue("@EditedOn", cc.TodaysDate());
                                            cmdUpdate.Parameters.AddWithValue("@EditedBy", ogCode);
                                            cmdUpdate.Parameters.AddWithValue("@Email", email);
                                            cmdUpdate.ExecuteNonQuery();
                                        }
                                        LoginInsert(ogCode, FirstName, LastName, email, Mobile, "N");
                                        ShowMsg("Sign up process completed, check your mail for Login Credentials.");
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        ShowMsg("Something went wrong, please try again after sometime");
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMsg(ex.Message.ToString());
            }
        }
        private void LoginInsert(string ogCode, string fName, string lName, string email, string mobile, string isAdmin)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(cc.GetConnectionString()))
                {
                    con.Open();
                    string sql = "";
                    sql = "insert into [MstrLogin] ([OgCode] ,[FirstName] ,[LastName] ,[Email] ,[Mobile] ,[UserName] ,[Password] ,[IsAdmin] ,[IsActive] ,[CreatedOn] ,[CreatedBy],PasswordResetFlag) " +
                              "values (@OgCode,@FirstName,@LastName,@Email,@Mobile,@UserName,@Password,@IsAdmin,@IsActive,@CreatedOn,@CreatedBy,'Y')";

                    string empCode = cc.GetEmpCode(ogCode);
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@OgCode", ogCode);
                        cmd.Parameters.AddWithValue("@FirstName", fName);
                        cmd.Parameters.AddWithValue("@LastName", lName);
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@Mobile", mobile);
                        cmd.Parameters.AddWithValue("@UserName", empCode);
                        cmd.Parameters.AddWithValue("@Password", cc.Encrypt(empCode));
                        cmd.Parameters.AddWithValue("@IsAdmin", isAdmin);
                        cmd.Parameters.AddWithValue("@IsActive", "Y");
                        cmd.Parameters.AddWithValue("@CreatedOn", cc.TodaysDate());
                        cmd.Parameters.AddWithValue("@CreatedBy", ogCode);
                        cmd.ExecuteNonQuery();
                    }
                    mail.SendNewEmpCreation(email, ogCode, empCode);
                }
            }
            catch (Exception ex)
            {
                ShowMsg(ex.Message.ToString());
            }
        }
        public void ShowMsg(string msg)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert(`" + msg + "`);window.location.href = \"Login.aspx\";", true);
        }
    }
}