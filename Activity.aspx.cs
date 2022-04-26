using ActivityManagementSystem.App_Code;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace ActivityManagementSystem
{
    public partial class Activity : System.Web.UI.Page
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
                        lblTitle.Text = "Activity";
                    }
                    FillDropDown(ddlProject, "ProjectId", "ProjectName", "MstrProject");
                    FillDropDown(ddlActivityType, "ActivityTypeId", "ActivityTypeName", "MstrActivityType");
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

                    sql = "Select [ActivityCode] ,[ToDoCode] ,[ProjectName] ,[ShortTitle] ,[Description] ,[ActivityType] ,[PageName] ,[SpName] ,[Priority] ," +
                          "[CompletionStatus] ,[AssignTo] ,[UserId] ,[OgCode] ,[CreatedBy] ,[CreatedOn] ,[EditedBy] ,[EditedOn] FROM [MstrActivity] " +
                           "Where OgCode=@OgCode and [IsActive]='Y' and CreatedBy=@CreatedBy and CompletionStatus='N'  Order by CreatedOn desc";


                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@OgCode", Session["OgCode"].ToString());
                        cmd.Parameters.AddWithValue("@CreatedBy", Session["UserId"].ToString());

                        using (DataTable dt = new DataTable())
                        {
                            dt.Load(cmd.ExecuteReader());
                            if (dt.Rows.Count > 0)
                            {
                                lblMessage.Text = "";
                            }
                            else
                            {
                                lblMessage.Text = "No data to show";
                            }
                            GvDashboard.DataSource = dt;
                            GvDashboard.DataBind();
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

                        sql = "insert into [MstrActivity] ([ActivityCode] ,[ProjectName] ,[ShortTitle] ,[Description] ,[ActivityType] ,[PageName] ,[SpName] ,[Priority] ," +
                              "[CompletionStatus] ,[AssignTo] ,[UserId] ,[OgCode] ,[CreatedBy] ,[CreatedOn],IsActive) values " +
                               "(@ActivityCode,@ProjectName,@ShortTitle,@Description,@ActivityType,@PageName,@SpName,@Priority,@CompletionStatus,@AssignTo,@UserId,@OgCode,@CreatedBy,@CreatedOn,'Y')";


                        using (SqlCommand cmd = new SqlCommand(sql, con))
                        {
                            cmd.Parameters.AddWithValue("@ActivityCode", cc.GetActivityCode(Session["OgCode"].ToString()));
                            cmd.Parameters.AddWithValue("@ProjectName", ddlProject.SelectedItem.Text.ToString());
                            cmd.Parameters.AddWithValue("@ShortTitle", txtTitle.Text.ToString());
                            cmd.Parameters.AddWithValue("@Description", txtDescription.Text.ToString());
                            cmd.Parameters.AddWithValue("@ActivityType", ddlActivityType.SelectedItem.ToString());
                            cmd.Parameters.AddWithValue("@PageName", txtPageName.Text.ToString());
                            cmd.Parameters.AddWithValue("@SpName", txtSpName.Text.ToString());
                            cmd.Parameters.AddWithValue("@Priority", radP.SelectedItem.ToString());
                            cmd.Parameters.AddWithValue("@CompletionStatus", "N");
                            cmd.Parameters.AddWithValue("@AssignTo", Session["Name"].ToString());
                            cmd.Parameters.AddWithValue("@UserId", Session["UserId"].ToString());
                            cmd.Parameters.AddWithValue("@OgCode", Session["OgCode"].ToString());
                            cmd.Parameters.AddWithValue("@CreatedBy", Session["UserId"].ToString());
                            cmd.Parameters.AddWithValue("@CreatedOn", cc.TodaysDate());
                            cmd.ExecuteNonQuery();
                        }
                    }
                    BindGrid();
                    Reset();
                    ShowMsg("G", "Record save successfully.");
                }
            }
            catch (Exception ex)
            {
                ShowMsg("R", ex.Message.ToString());
            }
        }
        private Boolean BlankCheck()
        {
            if (ddlProject.SelectedValue.ToString() == "00")
            {
                ShowMsg("R", "Select Project.");
                return false;
            }
            if (txtTitle.Text.ToString() == "")
            {
                ShowMsg("R", "Enter Title");
                return false;
            }
            if (txtDescription.Text.ToString() == "")
            {
                ShowMsg("G", "Enter Description");
                return false;
            }
            if (ddlActivityType.SelectedValue.ToString() == "00")
            {
                ShowMsg("R", "Select Activity Type.");
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
            ddlProject.SelectedIndex = 0;
            txtTitle.Text = "";
            txtDescription.Text = "";
            ddlActivityType.SelectedIndex = 0;
            txtPageName.Text = "";
            txtSpName.Text = "";
            hiddenActivityId.Value = "";
            btnSubmit.Visible = true;
            btnUpdate.Visible = false;
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (BlankCheck() && hiddenActivityId.Value.ToString() != "" && Session["UserId"] != null)
                {
                    using (SqlConnection con = new SqlConnection(cc.GetConnectionString()))
                    {
                        con.Open();
                        string sql = "";

                        sql = "update [MstrActivity] set [ProjectName]=@ProjectName ,[ShortTitle]=@ShortTitle ,[Description]=@Description ,[ActivityType]=@ActivityType ," +
                            "[PageName]=@PageName ,[SpName]=@SpName ,[Priority]=@Priority,EditedOn=@EditedOn,EditedBy=@EditedBy where OgCode=@OgCode and ActivityCode=@ActivityCode and UserId=@UserId ";

                        using (SqlCommand cmd = new SqlCommand(sql, con))
                        {
                            cmd.Parameters.AddWithValue("@ActivityCode", hiddenActivityId.Value.ToString());
                            cmd.Parameters.AddWithValue("@ProjectName", ddlProject.SelectedItem.Text.ToString());
                            cmd.Parameters.AddWithValue("@ShortTitle", txtTitle.Text.ToString());
                            cmd.Parameters.AddWithValue("@Description", txtDescription.Text.ToString());
                            cmd.Parameters.AddWithValue("@ActivityType", ddlActivityType.SelectedItem.ToString());
                            cmd.Parameters.AddWithValue("@PageName", txtPageName.Text.ToString());
                            cmd.Parameters.AddWithValue("@SpName", txtSpName.Text.ToString());
                            cmd.Parameters.AddWithValue("@Priority", radP.SelectedItem.ToString());
                            cmd.Parameters.AddWithValue("@UserId", Session["UserId"].ToString());
                            cmd.Parameters.AddWithValue("@OgCode", Session["OgCode"].ToString());
                            cmd.Parameters.AddWithValue("@EditedBy", Session["UserId"].ToString());
                            cmd.Parameters.AddWithValue("@EditedOn", cc.TodaysDate());
                            cmd.ExecuteNonQuery();
                        }
                    }
                    Reset();
                    BindGrid();
                    ShowMsg("G", "Record updated successfully.");
                }
            }
            catch (Exception ex)
            {
                ShowMsg("R", ex.Message.ToString());
            }
        }
        private void FillDropDown(DropDownList ddl, string value, string text, string tableName)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(cc.GetConnectionString()))
                {
                    con.Open();
                    string sql = "";
                    sql = $"select '00' as {value},'--Select--' as {text} union Select {value},{text} from {tableName} where OgCode=@OgCode and IsActive='Y'";
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@OgCode", Session["OgCode"].ToString());

                        using (DataTable dt = new DataTable())
                        {
                            dt.Load(cmd.ExecuteReader());
                            ddl.DataTextField = text;
                            ddl.DataValueField = value;
                            ddl.DataSource = dt;
                            ddl.DataBind();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMsg("R", ex.Message.ToString());
            }
        }


        protected void ddlAction_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow gvr = ((DropDownList)sender).NamingContainer as GridViewRow;
                if (gvr != null)
                {
                    var dropdown = gvr.FindControl("ddlAction") as DropDownList;
                    string ddlValue = dropdown.SelectedItem.ToString();
                    string activityId = gvr.Cells[1].Text;
                    if (ddlValue == "Edit")
                    {
                        using (SqlConnection con = new SqlConnection(cc.GetConnectionString()))
                        {
                            con.Open();
                            string sql = "";

                            sql = "Select [ProjectName] ,[ShortTitle] ,[Description] ,[ActivityType] ,[PageName] ,[SpName] ,[Priority] " +
                                  " FROM [MstrActivity] Where OgCode=@OgCode and [IsActive]='Y' and CreatedBy=@CreatedBy and ActivityCode=@ActivityCode";

                            using (SqlCommand cmd = new SqlCommand(sql, con))
                            {
                                cmd.Parameters.AddWithValue("@OgCode", Session["OgCode"].ToString());
                                cmd.Parameters.AddWithValue("@CreatedBy", Session["UserId"].ToString());
                                cmd.Parameters.AddWithValue("@ActivityCode", activityId);
                                using (SqlDataReader reader = cmd.ExecuteReader())
                                {
                                    if (reader.HasRows)
                                    {
                                        reader.Read();
                                        string projectName = reader["ProjectName"].ToString();
                                        string shortTitle = reader["ShortTitle"].ToString();
                                        string description = reader["Description"].ToString();
                                        string activityType = reader["ActivityType"].ToString();
                                        string pageName = reader["PageName"].ToString();
                                        string spName = reader["SpName"].ToString();
                                        string priority = reader["Priority"].ToString();
                                        hiddenActivityId.Value = activityId;
                                        DropDownSelection(ddlProject, projectName);
                                        txtTitle.Text = shortTitle;
                                        txtDescription.Text = description;
                                        DropDownSelection(ddlActivityType, activityType);
                                        txtPageName.Text = pageName;
                                        txtSpName.Text = spName;
                                        radP.Items.FindByText(priority).Selected = true;
                                        btnSubmit.Visible = false;
                                        btnUpdate.Visible = true;
                                    }
                                }
                            }
                        }
                    }
                    else if (ddlValue == "Attachment")
                    {
                        string url = $"Attachments.aspx?ActivityId={activityId}";
                        string s = "window.open('" + url + "', 'popup_window', 'width=600,height=500,left=100,top=100,resizable=yes');";
                        ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
                    }
                    else if (ddlValue == "Delete")
                    {
                        using (SqlConnection con = new SqlConnection(cc.GetConnectionString()))
                        {
                            con.Open();
                            string sql = "";
                            sql = "update [MstrActivity] set IsActive='N' Where OgCode=@OgCode and CreatedBy=@CreatedBy and ActivityCode=@ActivityCode";
                            using (SqlCommand cmd = new SqlCommand(sql, con))
                            {
                                cmd.Parameters.AddWithValue("@OgCode", Session["OgCode"].ToString());
                                cmd.Parameters.AddWithValue("@CreatedBy", Session["UserId"].ToString());
                                cmd.Parameters.AddWithValue("@ActivityCode", activityId);
                                cmd.ExecuteNonQuery();
                            }
                        }
                        ShowMsg("G", "Record deleted successfully.");
                        BindGrid();
                    }
                    dropdown.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                ShowMsg("R", ex.Message.ToString());
            }
        }
        private void DropDownSelection(DropDownList ddl, string Value)
        {
            ddl.ClearSelection();
            ddl.Items.FindByText(Value).Selected = true;
        }
        public void ShowMsg(string color, string msg)
        {
            Site site = (Site)this.Master;
            site.ShowMessage(color, msg);
        }

        protected void GvDashboard_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GvDashboard.PageIndex = e.NewPageIndex;
            BindGrid();
        }
    }
}