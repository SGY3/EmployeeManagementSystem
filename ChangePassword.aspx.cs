using ActivityManagementSystem.App_Code;
using System;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace ActivityManagementSystem
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        string LoginPageUrl = "Login.aspx";
        Common cc = new Common();
        SendMail mail = new SendMail();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["userId"] != null)
            {
                if (!IsPostBack)
                {
                    if (this.Master.FindControl("lblPageTitle") is Label lblTitle)
                    {
                        lblTitle.Text = "Change Password";
                    }
                    txtUserId.Value = Session["UserId"] != null ? Session["UserId"].ToString() : "";
                }
            }
            else
            {
                Response.Redirect(LoginPageUrl);
            }
        }
        private Boolean BlankCheck()
        {
            if (txtUserId.Value.ToString() == "")
            {
                ShowMsg("R", "UserId not found try again!");
                return false;
            }
            if (txtOldPassword.Value.ToString() == "")
            {
                ShowMsg("R", "Enter current password.");
                return false;
            }
            if (txtNewPassword.Value.ToString() == "")
            {
                ShowMsg("R", "Enter new password.");
                return false;
            }
            if (txtConfirmPassword.Value.ToString() == "")
            {
                ShowMsg("R", "Enter confirm password.");
                return false;
            }
            return true;
        }
        private Boolean DbPasswordCheck()
        {
            try
            {
                string dbPassword = "";
                bool passCheck = false;
                using (SqlConnection con = new SqlConnection(cc.GetConnectionString()))
                {
                    con.Open();
                    string sql = "select [Password] from MstrLogin where UserName=@UserName and IsActive='Y' and OgCode=@OgCode";
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@UserName", Session["UserId"].ToString());
                        cmd.Parameters.AddWithValue("@OgCode", Session["OgCode"].ToString());
                        dbPassword = cc.Decrypt(cmd.ExecuteScalar().ToString());
                        if (txtOldPassword.Value.ToString() == dbPassword)
                        {
                            passCheck = true;
                        }
                    }
                }
                return passCheck;
            }
            catch (Exception ex)
            {
                ShowMsg("R", ex.Message.ToString());
                return false;
            }
        }
        private Boolean PasswordCheck()
        {
            if (txtNewPassword.Value.ToString() != txtConfirmPassword.Value.ToString())
            {
                ShowMsg("R", "New password and confirm password not matching try again!.");
                return false;
            }
            return true;
        }
        private Boolean ValidCheck()
        {
            if (!BlankCheck())
            {
                return false;
            }
            if (!PasswordCheck())
            {
                return false;
            }
            if (!DbPasswordCheck())
            {
                ShowMsg("R", "Current password is wrong");
                return false;
            }
            return true;
        }
        public void ShowMsg(string color, string msg)
        {
            Site site = (Site)this.Master;
            site.ShowMessage(color, msg);
        }

        protected void btnSubmit_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (ValidCheck())
                {
                    using (SqlConnection con = new SqlConnection(cc.GetConnectionString()))
                    {
                        con.Open();
                        string sql = "update MstrLogin set password=@password,PasswordResetFlag='N' where UserName=@UserName and IsActive='Y' and OgCode=@OgCode";
                        using (SqlCommand cmd = new SqlCommand(sql, con))
                        {
                            cmd.Parameters.AddWithValue("@UserName", Session["UserId"].ToString());
                            cmd.Parameters.AddWithValue("@OgCode", Session["OgCode"].ToString());
                            cmd.Parameters.AddWithValue("@password", cc.Encrypt(txtNewPassword.Value.ToString()));
                            cmd.ExecuteNonQuery();
                            Session["PassFlag"] = "N";
                        }
                    }
                    ShowMsg("G","Password changed successfully");
                }
            }
            catch (Exception ex)
            {
                ShowMsg("R", ex.Message.ToString());
            }
        }
    }
}