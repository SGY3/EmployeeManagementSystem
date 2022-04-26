using ActivityManagementSystem.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

namespace ActivityManagementSystem
{
    public partial class CreateOrganization : System.Web.UI.Page
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
                        string sql = "insert into [MstrOraganization] ([OgName] ,[OgEmail] ,[FirstName] ,[LastName] ,[Mobile] ,[ProcessCompleteYN] ,[IsActive] ,[CreatedOn] ,[CreatedBy]) " +
                            "values (@OgName,@OgEmail,@FirstName,@LastName,@Mobile,@ProcessCompleteYN,@IsActive,@CreatedOn,@CreatedBy)";
                        using (SqlCommand cmd = new SqlCommand(sql, con))
                        {
                            cmd.Parameters.AddWithValue("@OgName", txtOrganizationName.Value.ToString());
                            cmd.Parameters.AddWithValue("@OgEmail", txtOrganizationEmail.Value.ToString());
                            cmd.Parameters.AddWithValue("@FirstName", txtFirstName.Value.ToString());
                            cmd.Parameters.AddWithValue("@LastName", txtLastName.Value.ToString());
                            cmd.Parameters.AddWithValue("@Mobile", txtMobile.Value.ToString());
                            cmd.Parameters.AddWithValue("@ProcessCompleteYN", "N");
                            cmd.Parameters.AddWithValue("@IsActive", "Y");
                            cmd.Parameters.AddWithValue("@CreatedOn", cc.TodaysDate());
                            cmd.Parameters.AddWithValue("@CreatedBy", txtFirstName.Value.ToString() + txtLastName.Value.ToString());
                            cmd.ExecuteNonQuery();
                        }
                    }
                    mail.SendOraganizationVerify(txtOrganizationEmail.Value.ToString().ToLower());
                    ShowMsg("G", "Record saved successfully, Check your mail for verification");
                }
                else
                {
                    ShowMsg("R", "Something went wrong");
                }
            }
            catch (Exception ex)
            {
                ShowMsg("R", ex.Message.ToString());
            }
        }
        private Boolean BlankCheck()
        {
            if (txtOrganizationName.Value.ToString() == "")
            {
                ShowMsg("R", "Enter Organization Mail");
                return false;
            }
            if (txtOrganizationEmail.Value.ToString() == "")
            {
                ShowMsg("R", "Enter Organization Mail");
                return false;
            }
            if (txtFirstName.Value.ToString() == "")
            {
                ShowMsg("R", "Enter FIrst Name");
                return false;
            }
            if (txtLastName.Value.ToString() == "")
            {
                ShowMsg("R", "Enter Last Name");
                return false;
            }
            if (txtMobile.Value.ToString() == "")
            {
                ShowMsg("R", "Enter Mobile No.");
                return false;
            }
            return true;
        }
        private int CheckExist()
        {
            int cnt = 0;
            using (SqlConnection con = new SqlConnection(cc.GetConnectionString()))
            {
                con.Open();
                string sql = "select Count(OgEmail) from MstrOraganization where OgEmail=@OgEmail and ProcessCompleteYN='Y' and IsActive='Y'";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@OgEmail", txtOrganizationEmail.Value.ToString());
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
            if (CheckExist() == 0)
            {
                ShowMsg("R", "Enter Email Id Already exists");
                return true;
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