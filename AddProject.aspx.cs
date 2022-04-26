using ActivityManagementSystem.App_Code;
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
    public partial class AddProject : System.Web.UI.Page
    {
        string LoginPageUrl = "Login.aspx";
        Common cc = new Common();
        SendMail mail = new SendMail();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (this.Master.FindControl("lblPageTitle") is Label lblTitle)
                {
                    lblTitle.Text = "Project";
                }
                BindGrid();
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
                    sql = "Select ProjectId,ProjectName,IsActive from MstrProject " +
                        "Where OgCode=@OgCode Order by CreatedOn";


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
                            GvProject.DataSource = dt;
                            GvProject.DataBind();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMsg("R", ex.Message.ToString());
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (BlankCheck() && Session["UserId"] != null)
                {
                    using (SqlConnection con = new SqlConnection(cc.GetConnectionString()))
                    {
                        con.Open();
                        string sql = "";

                        sql = "insert into MstrProject (Projectid,ProjectName,IsActive,OgCode,CreatedBy,CreatedOn) " +
                               "values (@Projectid,@ProjectName,@IsActive,@OgCode,@CreatedBy,@CreatedOn)";

                        using (SqlCommand cmd = new SqlCommand(sql, con))
                        {
                            cmd.Parameters.AddWithValue("@Projectid", cc.GetProjectCode(Session["OgCode"].ToString()));
                            cmd.Parameters.AddWithValue("@ProjectName", txtProjectName.Text.ToString());
                            cmd.Parameters.AddWithValue("@IsActive", ddlActive.SelectedValue.ToString());
                            cmd.Parameters.AddWithValue("@OgCode", Session["OgCode"].ToString());
                            cmd.Parameters.AddWithValue("@CreatedBy", Session["UserId"].ToString());
                            cmd.Parameters.AddWithValue("@CreatedOn", cc.TodaysDate());
                            cmd.ExecuteNonQuery();
                        }
                    }
                    BindGrid();
                    ShowMsg("G", "Record saved successfully");
                    Reset();
                }
            }
            catch (Exception ex)
            {
                ShowMsg("R", ex.Message.ToString());
            }
        }
        private Boolean BlankCheck()
        {
            if (txtProjectName.Text.ToString() == "")
            {
                ShowMsg("R", "Enter project name");
                return false;
            }
            return true;
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            Reset();
        }
        private void Reset()
        {
            txtProjectName.Text = "";
            btnSave.Visible = true;
            btnUpdate.Visible = false;
            hiddenProjectName.Value = "";
        }
        public void ShowMsg(string color, string msg)
        {
            Site site = (Site)this.Master;
            site.ShowMessage(color, msg);
        }

        protected void GvProject_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "EditC")
                {
                    //Determine the RowIndex of the Row whose Button was clicked.
                    int rowIndex = Convert.ToInt32(e.CommandArgument);

                    //Reference the GridView Row.
                    GridViewRow row = GvProject.Rows[rowIndex];

                    //Fetch value.
                    hiddenProjectName.Value = row.Cells[1].Text;
                    txtProjectName.Text = row.Cells[2].Text;
                    btnSave.Visible = false;
                    btnUpdate.Visible = true;

                }
            }
            catch (Exception ex)
            {
                ShowMsg("R", ex.Message.ToString());
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(cc.GetConnectionString()))
                {
                    con.Open();
                    string sql = "";

                    sql = "update [MstrProject] set [ProjectName]=@ProjectName,IsActive=@IsActive,EditedOn=@EditedOn,EditedBy=@EditedBy where OgCode=@OgCode and ProjectId=@ProjectId";

                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@ProjectName", txtProjectName.Text.ToString());
                        cmd.Parameters.AddWithValue("@IsActive", ddlActive.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@OgCode", Session["OgCode"].ToString());
                        cmd.Parameters.AddWithValue("@EditedBy", Session["UserId"].ToString());
                        cmd.Parameters.AddWithValue("@EditedOn", cc.TodaysDate());
                        cmd.Parameters.AddWithValue("@ProjectId",hiddenProjectName.Value.ToString());
                        cmd.ExecuteNonQuery();
                    }
                }
                ShowMsg("G", "Record saved successfully");
                BindGrid();
                Reset();
            }
            catch (Exception ex)
            {
                ShowMsg("R", ex.Message.ToString());
            }
        }

        protected void GvProject_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GvProject.PageIndex = e.NewPageIndex;
            BindGrid();
        }
    }
}