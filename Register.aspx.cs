using ActivityManagementSystem.App_Code;
using System;
using System.Data.SqlClient;

namespace ActivityManagementSystem
{
    public partial class Register : System.Web.UI.Page
    {
        Common cc = new Common();
        SendMail mail = new SendMail();
        protected void Page_Load(object sender, EventArgs e)
        {

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
                        string sql = "insert into [MstrSignup] ([OgCode] ,[FirstName] ,[LastName] ,[Email] ,[Mobile] ,[ProcessCompleteYN] ,[CreatedOn] ,[CreatedBy]) " +
                            "values (@OgCode,@FirstName,@LastName,@Email,@Mobile,@ProcessCompleteYN,@CreatedOn,@CreatedBy)";
                        using (SqlCommand cmd = new SqlCommand(sql, con))
                        {
                            cmd.Parameters.AddWithValue("@OgCode", txtOrganizationId.Value.ToString());
                            cmd.Parameters.AddWithValue("@FirstName", txtFirstName.Value.ToString());
                            cmd.Parameters.AddWithValue("@LastName", txtLastName.Value.ToString());
                            cmd.Parameters.AddWithValue("@Email", txtEmail.Value.ToString());
                            cmd.Parameters.AddWithValue("@Mobile", txtMobile.Value.ToString());
                            cmd.Parameters.AddWithValue("@ProcessCompleteYN", "N");
                            cmd.Parameters.AddWithValue("@CreatedOn", cc.TodaysDate());
                            cmd.Parameters.AddWithValue("@CreatedBy", txtFirstName.Value.ToString() + txtLastName.Value.ToString());
                            cmd.ExecuteNonQuery();
                        }
                    }
                    mail.SendNewUserVerify(txtEmail.Value.ToString().ToLower(), txtOrganizationId.Value.ToString());
                    ShowMsg("G", "Record saved successfully, Check your mail for verification");
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
            if (txtFirstName.Value.ToString() == "")
            {
                ShowMsg("R", "Enter First Name");
                return false;
            }
            if (txtLastName.Value.ToString() == "")
            {
                ShowMsg("R", "Enter Last Name");
                return false;
            }
            if (txtEmail.Value.ToString() == "")
            {
                ShowMsg("R", "Enter Email Id");
                return false;
            }
            if (txtMobile.Value.ToString() == "")
            {
                ShowMsg("R", "Enter MObile No.");
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
                string sql = "select Count(Email) from MstrLogin where Email=@Email and IsActive='Y'";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@Email", txtEmail.Value.ToString());
                    cnt = cc.StoI(cmd.ExecuteScalar().ToString());
                }
            }
            return cnt;
        }
        private Boolean ValidCheck()
        {
            if (!BlankCheck())
            {
                return false;
            }
            if (OrganizationCheck() == 0)
            {
                ShowMsg("R", "Enter Organization Id Not Found");
                return false;
            }
            if (CheckExist() != 0)
            {
                ShowMsg("R", "Enter Email Id Already Exists");
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