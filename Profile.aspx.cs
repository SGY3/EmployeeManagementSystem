using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ActivityManagementSystem.App_Code;

namespace ActivityManagementSystem
{
    public partial class Profile : System.Web.UI.Page
    {
        string LoginPageUrl = "Login.aspx";
        Common cc = new Common();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] != null)
            {
                if (!IsPostBack)
                {
                    Label lblTitle = this.Master.FindControl("lblPageTitle") as Label;
                    if (lblTitle != null)
                    {
                        lblTitle.Text = "Profile";
                    }
                    BindGrid();
                }
            }
            else
            {
                Response.Redirect(LoginPageUrl);
            }
        }
        private void BindGrid()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(cc.GetConnectionString()))
                {
                    con.Open();
                    string sql = "";
                    sql = "Select [FirstName] ,[LastName] ,[Email] ,[Mobile]  FROM [MstrLogin] " +
                        "Where OgCode=@OgCode and Username=@Username";

                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@OgCode", Session["OgCode"].ToString());
                        cmd.Parameters.AddWithValue("@Username", Session["UserId"].ToString());
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();
                                txtFirstName.Text = reader.GetString(0);
                                txtLastName.Text = reader.GetString(1);
                                txtEmail.Text = reader.GetString(2);
                                txtMobile.Text = reader.GetString(3);
                                EnableDisableControl(false);
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
        public void ShowMsg(string color, string msg)
        {
            Site site = (Site)this.Master;
            site.ShowMessage(color, msg);
        }
        private void EnableDisableControl(bool Val)
        {
            txtFirstName.Enabled = Val;
            txtLastName.Enabled = Val;
            txtMobile.Enabled = Val;
            txtEmail.Enabled = Val;
            if (Val)
            {
                btnEdit.Visible = false;
                btnUpdate.Visible = true;
            }
            else
            {
                btnEdit.Visible = true;
                btnUpdate.Visible = false;
            }
        }
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            EnableDisableControl(true);
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidCheck())
                {
                    using (SqlConnection con = new SqlConnection(cc.GetConnectionString()))
                    {
                        con.Open();
                        string sql = "";

                        sql = "update [MstrLogin] set [FirstName]=@FirstName ,[LastName]=@LastName ,[Email]=@Email ,[Mobile]=@Mobile,EditedOn=@EditedOn,EditedBy=@EditedBy where OgCode=@OgCode and Username=@Username";

                        using (SqlCommand cmd = new SqlCommand(sql, con))
                        {
                            cmd.Parameters.AddWithValue("@FirstName", txtFirstName.Text.ToString());
                            cmd.Parameters.AddWithValue("@LastName", txtLastName.Text.ToString());
                            cmd.Parameters.AddWithValue("@Email", txtEmail.Text.ToString());
                            cmd.Parameters.AddWithValue("@Mobile", txtMobile.Text.ToString());
                            cmd.Parameters.AddWithValue("@OgCode", Session["OgCode"].ToString());
                            cmd.Parameters.AddWithValue("@EditedBy", Session["UserId"].ToString());
                            cmd.Parameters.AddWithValue("@EditedOn", cc.TodaysDate());
                            cmd.Parameters.AddWithValue("@Username", Session["UserId"].ToString());
                            cmd.ExecuteNonQuery();
                        }
                    }
                    ShowMsg("G", "Record update successfully");
                    BindGrid();
                    EnableDisableControl(false);
                }

            }
            catch (Exception ex)
            {
                ShowMsg("R", ex.Message.ToString());
            }
        }
        private Boolean BlankCheck()
        {

            if (txtFirstName.Text.ToString() == "")
            {
                ShowMsg("R", "Enter first name");
                return false;
            }
            if (txtLastName.Text.ToString() == "")
            {
                ShowMsg("R", "Enter last name");
                return false;
            }
            if (txtEmail.Text.ToString() == "")
            {
                ShowMsg("R", "Enter email");
                return false;
            }
            if (txtMobile.Text.ToString() == "")
            {
                ShowMsg("R", "Enter mobile");
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
                string sql = "select Count(Email) from MstrLogin where Email=@Email and IsActive='Y' and Username!=@Username";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@Email", txtEmail.Text.ToString());
                    cmd.Parameters.AddWithValue("@Username", Session["UserId"].ToString());
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
            if (CheckExist() != 0)
            {
                ShowMsg("R", "Email id already exists");
                return false;
            }
            return true;
        }
        protected void btnReset_Click(object sender, EventArgs e)
        {
            BindGrid();
        }
    }
}