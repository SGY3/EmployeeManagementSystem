using EmployeeManagementSystem;
using EmployeeManagementSystem.App_Code;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ActivityManagementSystem
{
    public partial class Attendance : System.Web.UI.Page
    {
        Common cc = new Common();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Master.FindControl("lblPageTitle") is Label lblTitle)
            {
                lblTitle.Text = "Attendance";
            }
        }

        protected void btnLogout_ServerClick(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(cc.GetConnectionString()))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_LogsInsert", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@LogType", "Logout");
                        cmd.Parameters.AddWithValue("@AddedBy", Session["UserId"].ToString());
                        cmd.Parameters.AddWithValue("@OgCode", Session["OgCode"].ToString());
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMsg("R", ex.Message.ToString());
            }
        }

        protected void btnLogin_ServerClick(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(cc.GetConnectionString()))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_LogsInsert", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@LogType", "Login");
                        cmd.Parameters.AddWithValue("@AddedBy", Session["UserId"].ToString());
                        cmd.Parameters.AddWithValue("@OgCode", Session["OgCode"].ToString());
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMsg("R", ex.Message.ToString());
            }
        }
        public void ShowMsg(string color, string msg)
        {
            Site site = (Site)this.Master;
            site.ShowMessage(color, msg);
        }
    }
}