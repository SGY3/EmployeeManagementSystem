using ActivityManagementSystem.App_Code;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace ActivityManagementSystem
{
    public partial class AddActivityType : System.Web.UI.Page
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
                        lblTitle.Text = "Activity Type";
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
                    sql = "Select ActivityTypeId,ActivityTypeName,IsActive from [MstrActivityType] " +
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
                            GvActivityType.DataSource = dt;
                            GvActivityType.DataBind();
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

                        sql = "insert into [MstrActivityType] (ActivityTypeId,ActivityTypeName,IsActive,OgCode,CreatedBy,CreatedOn) " +
                               "values (@ActivityTypeId,@ActivityTypeName,@IsActive,@OgCode,@CreatedBy,@CreatedOn)";

                        using (SqlCommand cmd = new SqlCommand(sql, con))
                        {
                            cmd.Parameters.AddWithValue("@ActivityTypeId", cc.GetActivityTypeCode(Session["OgCode"].ToString()));
                            cmd.Parameters.AddWithValue("@ActivityTypeName", txtActivityName.Text.ToString());
                            cmd.Parameters.AddWithValue("@IsActive", ddlActive.SelectedValue.ToString());
                            cmd.Parameters.AddWithValue("@OgCode", Session["OgCode"].ToString());
                            cmd.Parameters.AddWithValue("@CreatedBy", Session["UserId"].ToString());
                            cmd.Parameters.AddWithValue("@CreatedOn", cc.TodaysDate());
                            cmd.ExecuteNonQuery();
                        }
                    }
                    ShowMsg("G", "Record saved successfully");
                    BindGrid();
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
            if (txtActivityName.Text.ToString() == "")
            {
                ShowMsg("R", "Activity name can not be blank.");
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
            txtActivityName.Text = "";
            btnUpdate.Visible = false;
            btnSave.Visible = true;
            hiddenActivityTypeId.Value = "";
        }
        public void ShowMsg(string color, string msg)
        {
            Site site = (Site)this.Master;
            site.ShowMessage(color, msg);
        }

        protected void GvActivityType_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "EditC")
                {
                    //Determine the RowIndex of the Row whose Button was clicked.
                    int rowIndex = Convert.ToInt32(e.CommandArgument);

                    //Reference the GridView Row.
                    GridViewRow row = GvActivityType.Rows[rowIndex];

                    //Fetch value.
                    hiddenActivityTypeId.Value = row.Cells[1].Text;
                    txtActivityName.Text = row.Cells[2].Text;
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

                    sql = "update [MstrActivityType] set ActivityTypeName=@ActivityTypeName,IsActive=@IsActive,EditedOn=@EditedOn,EditedBy=@EditedBy where OgCode=@OgCode and ActivityTypeId=@ActivityTypeId";

                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@ActivityTypeName", txtActivityName.Text.ToString());
                        cmd.Parameters.AddWithValue("@IsActive", ddlActive.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@OgCode", Session["OgCode"].ToString());
                        cmd.Parameters.AddWithValue("@EditedBy", Session["UserId"].ToString());
                        cmd.Parameters.AddWithValue("@EditedOn", cc.TodaysDate());
                        cmd.Parameters.AddWithValue("@ActivityTypeId", hiddenActivityTypeId.Value.ToString());
                        cmd.ExecuteNonQuery();
                    }
                }
                ShowMsg("G", "Record update successfully");
                BindGrid();
                Reset();
            }
            catch (Exception ex)
            {
                ShowMsg("R", ex.Message.ToString());
            }
        }

        protected void GvActivityType_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GvActivityType.PageIndex = e.NewPageIndex;
            BindGrid();
        }
    }
}