using EmployeeManagementSystem.App_Code;
using System;
using System.Data.SqlClient;

namespace EmployeeManagementSystem
{
    public partial class Login : System.Web.UI.Page
    {
        Common cc = new Common();
        SendMail mail = new SendMail();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["Logout"] != null && Request.QueryString["Logout"].ToString() == "1")
                {
                    Session.Abandon();
                }
            }
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
                        string sql = "select OgCode,CONCAT(FirstName,' ',LastName) as FullName,UserName,IsAdmin,PasswordResetFlag from MstrLogin where UserName=@UserName and IsActive='Y' and OgCode=@OgCode";
                        using (SqlCommand cmd = new SqlCommand(sql, con))
                        {
                            cmd.Parameters.AddWithValue("@UserName", txtUserName.Value.ToString());
                            cmd.Parameters.AddWithValue("@OgCode", txtOrganizationId.Value.ToString());
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                reader.Read();
                                Session["OgCode"] = reader["OgCode"].ToString();
                                Session["Name"] = reader["FullName"].ToString();
                                Session["UserId"] = reader["UserName"].ToString();
                                Session["Admin"] = reader["IsAdmin"].ToString();
                                Session["PassFlag"] = reader["PasswordResetFlag"].ToString();
                                if (Session["PassFlag"].ToString() != "Y")
                                    Response.Redirect("Dashboard.aspx");
                                else
                                    Response.Redirect("ChangePassword.aspx");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMsg("R", ex.Message.ToString());
            }
        }
        private Boolean BlankCheck()
        {
            if (txtOrganizationId.Value.ToString() == "")
            {
                ShowMsg("R", "Enter Organization Id");
                return false;
            }
            if (txtUserName.Value.ToString() == "")
            {
                ShowMsg("R", "Enter Username");
                return false;
            }
            if (txtPassword.Value.ToString() == "")
            {
                ShowMsg("R", "Enter Password");
                return false;
            }
            return true;
        }
        private int OrganizationCheck()
        {
            int cnt = 0;
            using (SqlConnection con = new SqlConnection(cc.GetConnectionString()))
            {
                con.Open();
                string sql = "select Count(OgCode) from MstrOraganization where OgCode=@OgCode and ProcessCompleteYN='Y' and IsActive='Y'";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@OgCode", txtOrganizationId.Value.ToString());
                    cnt = cc.StoI(cmd.ExecuteScalar().ToString());
                }
            }
            return cnt;
        }
        private int CheckExist()
        {
            int cnt = 0;
            using (SqlConnection con = new SqlConnection(cc.GetConnectionString()))
            {
                con.Open();
                string sql = "select Count(UserName) from MstrLogin where UserName=@UserName and IsActive='Y' and OgCode=@OgCode";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@UserName", txtUserName.Value.ToString());
                    cmd.Parameters.AddWithValue("@OgCode", txtOrganizationId.Value.ToString());
                    cnt = cc.StoI(cmd.ExecuteScalar().ToString());
                }
            }
            return cnt;
        }
        private Boolean PasswordCheck()
        {
            string dbPassword = "";
            bool passCheck = false;
            using (SqlConnection con = new SqlConnection(cc.GetConnectionString()))
            {
                con.Open();
                string sql = "select [Password] from MstrLogin where UserName=@UserName and IsActive='Y' and OgCode=@OgCode";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@UserName", txtUserName.Value.ToString());
                    cmd.Parameters.AddWithValue("@OgCode", txtOrganizationId.Value.ToString());
                    dbPassword = cc.Decrypt(cmd.ExecuteScalar().ToString());
                    if (txtPassword.Value.ToString() == dbPassword)
                    {
                        passCheck = true;
                    }
                }
            }
            return passCheck;
        }
        private Boolean ValidCheck()
        {
            if (!BlankCheck())
            {
                return false;
            }
            if (OrganizationCheck() == 0)
            {
                ShowMsg("R", "Organization not found");
                return false;
            }
            if (CheckExist() == 0)
            {
                ShowMsg("R", "Username does't exists.");
                return false;
            }
            if (!PasswordCheck())
            {
                ShowMsg("R", "Username or Password wrong");
                return false;
            }
            return true;
        }
        public void ShowMsg(string color, string msg)
        {
            home home = (home)this.Master;
            home.ShowMessage(color, msg);
        }
    }
}