using ActivityManagementSystem.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ActivityManagementSystem
{
    public partial class AddEmployee : System.Web.UI.Page
    {
        string LoginPageUrl = "Login.aspx";
        Common cc = new Common();
        SendMail mail = new SendMail();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] != null)
            {
                if (!IsPostBack)
                {
                    if (this.Master.FindControl("lblPageTitle") is Label lblTitle)
                    {
                        lblTitle.Text = "Employee";
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
                    sql = "Select [FirstName] ,[LastName] ,[Email] ,[Mobile] ,[UserName] ,[IsActive]  FROM [MstrLogin] " +
                        "Where OgCode=@OgCode";

                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@OgCode", Session["OgCode"].ToString());

                        using (DataTable dt = new DataTable())
                        {
                            dt.Load(cmd.ExecuteReader());
                            if (dt.Rows.Count > 0)
                            {

                                //lblMessage.Text = "";
                            }
                            else
                            {
                                // lblMessage.Text = "No data to show";
                            }
                            GvEmployee.DataSource = dt;
                            GvEmployee.DataBind();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMsg("R", ex.Message.ToString());
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidCheck() && Session["UserId"] != null)
                {
                    using (SqlConnection con = new SqlConnection(cc.GetConnectionString()))
                    {
                        con.Open();
                        string sql = "";

                        sql = "insert into [MstrLogin] ([OgCode] ,[FirstName] ,[LastName] ,[Email] ,[Mobile] ,[UserName] ,[Password] ,[IsAdmin] ,[IsActive] ,[CreatedOn] ,[CreatedBy]) " +
                              "values (@OgCode,@FirstName,@LastName,@Email,@Mobile,@UserName,@Password,@IsAdmin,@IsActive,@CreatedOn,@CreatedBy)";

                        string empCode = cc.GetEmpCode(Session["OgCode"].ToString());
                        using (SqlCommand cmd = new SqlCommand(sql, con))
                        {
                            cmd.Parameters.AddWithValue("@OgCode", Session["OgCode"].ToString());
                            cmd.Parameters.AddWithValue("@FirstName", txtFirstName.Text.ToString());
                            cmd.Parameters.AddWithValue("@LastName", txtLastName.Text.ToString());
                            cmd.Parameters.AddWithValue("@Email", txtEmail.Text.ToString());
                            cmd.Parameters.AddWithValue("@Mobile", txtMobile.Text.ToString());
                            cmd.Parameters.AddWithValue("@UserName", empCode);
                            cmd.Parameters.AddWithValue("@Password", cc.Encrypt(empCode));
                            cmd.Parameters.AddWithValue("@IsAdmin", "N");
                            cmd.Parameters.AddWithValue("@IsActive", ddlActive.SelectedValue.ToString());
                            cmd.Parameters.AddWithValue("@CreatedOn", cc.TodaysDate());
                            cmd.Parameters.AddWithValue("@CreatedBy", Session["UserId"].ToString());
                            cmd.ExecuteNonQuery();
                        }
                        mail.SendNewEmpCreation(txtEmail.Text.ToLower(), Session["OgCode"].ToString(), empCode);
                        ShowMsg("G", "Employee created successfully");
                    }
                    BindGrid();
                    Reset();
                }
            }
            catch (Exception ex)
            {
                ShowMsg("R", ex.Message.ToString());
            }

        }
        private void Reset()
        {
            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtEmail.Text = "";
            txtMobile.Text = "";
            btnSave.Visible = true;
            btnUpdate.Visible = false;
            hiddenEmp.Value = "";
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
                string sql = "select Count(Email) from MstrLogin where Email=@Email and IsActive='Y'";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@Email", txtEmail.Text.ToString());
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
        protected void GvEmployee_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "EditC")
                {
                    //Determine the RowIndex of the Row whose Button was clicked.
                    int rowIndex = Convert.ToInt32(e.CommandArgument);

                    //Reference the GridView Row.
                    GridViewRow row = GvEmployee.Rows[rowIndex];

                    //Fetch value.
                    hiddenEmp.Value = row.Cells[5].Text;
                    using (SqlConnection con = new SqlConnection(cc.GetConnectionString()))
                    {
                        con.Open();
                        string sql = "";
                        sql = "Select [FirstName] ,[LastName] ,[Email] ,[Mobile] ,[UserName] ,[IsActive]  FROM [MstrLogin] " +
                            "Where OgCode=@OgCode and Username=@Username";

                        using (SqlCommand cmd = new SqlCommand(sql, con))
                        {
                            cmd.Parameters.AddWithValue("@OgCode", Session["OgCode"].ToString());
                            cmd.Parameters.AddWithValue("@Username", hiddenEmp.Value.ToString());
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    reader.Read();
                                    txtFirstName.Text = reader["FirstName"].ToString();
                                    txtLastName.Text = reader["LastName"].ToString();
                                    txtEmail.Text = reader["Email"].ToString();
                                    txtMobile.Text = reader["Mobile"].ToString();
                                    ddlActive.ClearSelection();
                                    ddlActive.Items.FindByValue(reader["IsActive"].ToString()).Selected = true;
                                }
                            }
                        }
                    }
                    btnSave.Visible = false;
                    btnUpdate.Visible = true;
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

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (BlankCheck())
                {
                    if (UpdateCheckExist() == 0)
                    {
                        using (SqlConnection con = new SqlConnection(cc.GetConnectionString()))
                        {
                            con.Open();
                            string sql = "";

                            sql = "update [MstrLogin] set [FirstName]=@FirstName ,[LastName]=@LastName ,[Email]=@Email ,[Mobile]=@Mobile,IsActive=@IsActive,EditedOn=@EditedOn,EditedBy=@EditedBy where OgCode=@OgCode and Username=@Username";

                            using (SqlCommand cmd = new SqlCommand(sql, con))
                            {
                                cmd.Parameters.AddWithValue("@FirstName", txtFirstName.Text.ToString());
                                cmd.Parameters.AddWithValue("@LastName", txtLastName.Text.ToString());
                                cmd.Parameters.AddWithValue("@Email", txtEmail.Text.ToString());
                                cmd.Parameters.AddWithValue("@Mobile", txtMobile.Text.ToString());
                                cmd.Parameters.AddWithValue("@IsActive", ddlActive.SelectedValue.ToString());
                                cmd.Parameters.AddWithValue("@OgCode", Session["OgCode"].ToString());
                                cmd.Parameters.AddWithValue("@EditedBy", Session["UserId"].ToString());
                                cmd.Parameters.AddWithValue("@EditedOn", cc.TodaysDate());
                                cmd.Parameters.AddWithValue("@Username", hiddenEmp.Value.ToString());
                                cmd.ExecuteNonQuery();
                            }
                        }
                        ShowMsg("G", "Record update successfully");
                        BindGrid();
                        Reset();
                    }
                    else
                    {
                        ShowMsg("R", "Email id already exists.");
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMsg("R", ex.Message.ToString());
            }
        }
        private int UpdateCheckExist()
        {
            int cnt = 0;
            using (SqlConnection con = new SqlConnection(cc.GetConnectionString()))
            {
                con.Open();
                string sql = "select Count(Email) from MstrLogin where Username!=@Username and Email=@Email and IsActive='Y'";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@Email", txtEmail.Text.ToString());
                    cmd.Parameters.AddWithValue("@Username", hiddenEmp.Value.ToString());
                    cnt = cc.StoI(cmd.ExecuteScalar().ToString());
                }
            }
            return cnt;
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            Reset();
        }

        protected void btnPassReset_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(cc.GetConnectionString()))
                {
                    con.Open();
                    string sql = "update MstrLogin set password=@password,PasswordResetFlag='Y' where UserName=@UserName and IsActive='Y' and OgCode=@OgCode";
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@UserName", hiddenEmp.Value.ToString());
                        cmd.Parameters.AddWithValue("@OgCode", Session["OgCode"].ToString());
                        cmd.Parameters.AddWithValue("@password", cc.Encrypt(hiddenEmp.Value.ToString()));
                        cmd.ExecuteNonQuery();
                    }
                }
                ShowMsg("G", "Password reset successfully");
                mail.SendNewEmpCreation(txtEmail.Text.ToString(), Session["OgCode"].ToString(), hiddenEmp.Value.ToString());
                BindGrid();
                Reset();
            }
            catch (Exception ex)
            {
                ShowMsg("R", ex.Message.ToString());
            }
        }

        protected void GvEmployee_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GvEmployee.PageIndex = e.NewPageIndex;
            BindGrid();
        }
    }
}