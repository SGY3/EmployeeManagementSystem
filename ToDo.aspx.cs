using ActivityManagementSystem.App_Code;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace ActivityManagementSystem
{
    public partial class ToDo : System.Web.UI.Page
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
                        lblTitle.Text = "To-Do";
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
                        "[CompletionStatus] ,[AssignTo] ,[UserId] ,[OgCode] ,[CreatedBy] ,[CreatedOn] ,[EditedBy] ,[EditedOn] FROM [MstrToDo] " +
                        "Where OgCode=@OgCode and [IsActive]='Y' and (AssignTo='' or AssignTo is Null) Order by CreatedOn desc";

                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@OgCode", Session["OgCode"].ToString());
                        cmd.Parameters.AddWithValue("@AssignTo", Session["UserId"].ToString());

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

                        sql = "insert into [MstrToDo] ([ToDoCode] ,[ProjectName] ,[ShortTitle] ,[Description] ,[ActivityType] ,[PageName] ,[SpName] ,[Priority] ," +
                              "[CompletionStatus] ,[OgCode] ,[CreatedBy] ,[CreatedOn],IsActive) values " +
                               "(@ToDoCode,@ProjectName,@ShortTitle,@Description,@ActivityType,@PageName,@SpName,@Priority,@CompletionStatus,@OgCode,@CreatedBy,@CreatedOn,'Y')";


                        using (SqlCommand cmd = new SqlCommand(sql, con))
                        {
                            cmd.Parameters.AddWithValue("@ToDoCode", cc.GetToDoCode(Session["OgCode"].ToString()));
                            cmd.Parameters.AddWithValue("@ProjectName", ddlProject.SelectedItem.Text.ToString());
                            cmd.Parameters.AddWithValue("@ShortTitle", txtTitle.Text.ToString());
                            cmd.Parameters.AddWithValue("@Description", txtDescription.Text.ToString());
                            cmd.Parameters.AddWithValue("@ActivityType", ddlActivityType.SelectedItem.ToString());
                            cmd.Parameters.AddWithValue("@PageName", txtPageName.Text.ToString());
                            cmd.Parameters.AddWithValue("@SpName", txtSpName.Text.ToString());
                            cmd.Parameters.AddWithValue("@Priority", radP.SelectedItem.ToString());
                            cmd.Parameters.AddWithValue("@CompletionStatus", "N");
                            cmd.Parameters.AddWithValue("@OgCode", Session["OgCode"].ToString());
                            cmd.Parameters.AddWithValue("@CreatedBy", Session["UserId"].ToString());
                            cmd.Parameters.AddWithValue("@CreatedOn", cc.TodaysDate());
                            cmd.ExecuteNonQuery();
                        }
                    }
                    BindGrid();
                    ShowMsg("G", "Record saved successfully.");
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
            if (ddlProject.SelectedValue.ToString() == "00")
            {
                ShowMsg("R", "Select Project");
                return false;
            }
            if (txtTitle.Text.ToString() == "")
            {
                ShowMsg("R", "Enter Title");
                return false;
            }
            if (txtDescription.Text.ToString() == "")
            {
                ShowMsg("R", "Enter Description");
                return false;
            }
            if (ddlActivityType.SelectedValue.ToString() == "00")
            {
                ShowMsg("R", "Select Activity Type");
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
            hiddenToDoId.Value = "";
            btnSubmit.Visible = true;
            btnUpdate.Visible = false;
            divAssign.Visible = false;
            divTodo.Visible = true;
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (BlankCheck() && hiddenToDoId.Value.ToString() != "" && Session["UserId"] != null)
                {
                    using (SqlConnection con = new SqlConnection(cc.GetConnectionString()))
                    {
                        con.Open();
                        string sql = "";

                        sql = "update [MstrToDo] set [ProjectName]=@ProjectName ,[ShortTitle]=@ShortTitle ,[Description]=@Description ,[ActivityType]=@ActivityType ," +
                            "[PageName]=@PageName ,[SpName]=@SpName ,[Priority]=@Priority,EditedOn=@EditedOn,EditedBy=@EditedBy where OgCode=@OgCode and ToDoCode=@ToDoCode ";

                        using (SqlCommand cmd = new SqlCommand(sql, con))
                        {
                            cmd.Parameters.AddWithValue("@ToDoCode", hiddenToDoId.Value.ToString());
                            cmd.Parameters.AddWithValue("@ProjectName", ddlProject.SelectedItem.Text.ToString());
                            cmd.Parameters.AddWithValue("@ShortTitle", txtTitle.Text.ToString());
                            cmd.Parameters.AddWithValue("@Description", txtDescription.Text.ToString());
                            cmd.Parameters.AddWithValue("@ActivityType", ddlActivityType.SelectedItem.ToString());
                            cmd.Parameters.AddWithValue("@PageName", txtPageName.Text.ToString());
                            cmd.Parameters.AddWithValue("@SpName", txtSpName.Text.ToString());
                            cmd.Parameters.AddWithValue("@Priority", radP.SelectedItem.ToString());
                            cmd.Parameters.AddWithValue("@OgCode", Session["OgCode"].ToString());
                            cmd.Parameters.AddWithValue("@EditedBy", Session["UserId"].ToString());
                            cmd.Parameters.AddWithValue("@EditedOn", cc.TodaysDate());
                            cmd.ExecuteNonQuery();
                        }
                    }
                    ShowMsg("G", "Record updated successfully.");
                    Reset();
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
                    string ToDoCode = gvr.Cells[1].Text;
                    string ToDoTitle = gvr.Cells[4].Text;
                    if (ddlValue == "Edit")
                    {
                        using (SqlConnection con = new SqlConnection(cc.GetConnectionString()))
                        {
                            con.Open();
                            string sql = "";

                            sql = "Select [ToDoCode] ,[ProjectName] ,[ShortTitle] ,[Description] ,[ActivityType] ,[PageName] ,[SpName] ,[Priority] " +
                                  " FROM [MstrTodo] Where OgCode=@OgCode and [IsActive]='Y' and ToDoCode=@ToDoCode";

                            using (SqlCommand cmd = new SqlCommand(sql, con))
                            {
                                cmd.Parameters.AddWithValue("@OgCode", Session["OgCode"].ToString());
                                cmd.Parameters.AddWithValue("@ToDoCode", ToDoCode);
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
                                        hiddenToDoId.Value = ToDoCode;
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
                        string url = $"Attachments.aspx?ToDoCode={ToDoCode}";
                        string s = "window.open('" + url + "', 'popup_window', 'width=600,height=500,left=100,top=100,resizable=yes');";
                        ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
                    }
                    else if (ddlValue == "Delete")
                    {
                        using (SqlConnection con = new SqlConnection(cc.GetConnectionString()))
                        {
                            con.Open();
                            string sql = "";
                            sql = "update [MstrToDo] set IsActive='N' Where OgCode=@OgCode and ToDoCode=@ToDoCode";
                            using (SqlCommand cmd = new SqlCommand(sql, con))
                            {
                                cmd.Parameters.AddWithValue("@OgCode", Session["OgCode"].ToString());
                                cmd.Parameters.AddWithValue("@ToDoCode", ToDoCode);
                                cmd.ExecuteNonQuery();
                            }
                        }
                        ShowMsg("G", "Record deleted successfully.");
                        BindGrid();
                    }
                    else if (ddlValue == "Assign")
                    {
                        divTodo.Visible = false;
                        divAssign.Visible = true;
                        txtTdoId.Text = ToDoCode;
                        txtTodoTitle.Text = ToDoTitle;
                        try
                        {
                            using (SqlConnection con = new SqlConnection(cc.GetConnectionString()))
                            {
                                con.Open();
                                string sql = "";
                                if (Session["Admin"] != null && Session["Admin"].ToString() == "Y")
                                {
                                    sql = $"select '00' as EmpId,'--Select--' as EmpName union select UserName as EmpId,CONCAT(FirstName,' ',LastName) as EmpName from MstrLogin  where OgCode=@OgCode and IsActive='Y'";
                                }
                                else
                                {
                                    sql = $"select '00' as EmpId,'--Select--' as EmpName union select UserName as EmpId,CONCAT(FirstName,' ',LastName) as EmpName from MstrLogin  where OgCode=@OgCode and IsActive='Y' and Username=@Username";
                                }
                                using (SqlCommand cmd = new SqlCommand(sql, con))
                                {
                                    cmd.Parameters.AddWithValue("@OgCode", Session["OgCode"].ToString());
                                    cmd.Parameters.AddWithValue("@Username", Session["UserId"].ToString());

                                    using (DataTable dt = new DataTable())
                                    {
                                        dt.Load(cmd.ExecuteReader());
                                        ddlEmployee.DataTextField = "EmpName";
                                        ddlEmployee.DataValueField = "EmpId";
                                        ddlEmployee.DataSource = dt;
                                        ddlEmployee.DataBind();
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            ShowMsg("R", ex.Message.ToString());
                        }
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

        protected void btnTodoAssign_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(cc.GetConnectionString()))
                {
                    con.Open();
                    string ActivityId = cc.GetActivityCode(Session["OgCode"].ToString());
                    //string sql = "";

                    //sql = "insert into [MstrActivity] ([ToDoCode],[ActivityCode] ,[ProjectName] ,[ShortTitle] ,[Description] ,[ActivityType] ,[PageName] ,[SpName] ,[Priority] ," +
                    //      "[CompletionStatus] ,[AssignTo] ,[UserId] ,[OgCode] ,[CreatedBy] ,[CreatedOn],IsActive,EditedOn,EditedBy) " +
                    //       "select ToDoCode,@ActivityCode,ProjectName,ShortTitle,Description,ActivityType,PageName,SpName,Priority,CompletionStatus,@AssignTo,@UserId,@OgCode,@CreatedBy,@CreatedOn,'Y'," +
                    //       "@EditedOn,@EditedBy from MstrTodo " +
                    //       "where OgCode=@OgCode and ToDoCode=@ToDoCode; " +
                    //       "update MstrTodo set ActivityCode=@ActivityCode, [AssignTo]=@AssignTo ,[UserId]=@UserId,EditedOn=@EditedOn,EditedBy=@EditedBy where OgCode=@OgCode and ToDoCode=@ToDoCode";
                    //
                    //using (SqlCommand cmd = new SqlCommand(sql, con))
                    //{
                    //    cmd.Parameters.AddWithValue("@ActivityCode", ActivityId);
                    //    cmd.Parameters.AddWithValue("@AssignTo", ddlEmployee.SelectedItem.ToString());
                    //    cmd.Parameters.AddWithValue("@UserId", ddlEmployee.SelectedValue.ToString());
                    //    cmd.Parameters.AddWithValue("@OgCode", Session["OgCode"].ToString());
                    //    cmd.Parameters.AddWithValue("@CreatedBy", ddlEmployee.SelectedValue.ToString());
                    //    cmd.Parameters.AddWithValue("@CreatedOn", cc.TodaysDate());
                    //    cmd.Parameters.AddWithValue("@EditedOn", cc.TodaysDate());
                    //    cmd.Parameters.AddWithValue("@EditedBy", Session["UserId"].ToString());
                    //    cmd.Parameters.AddWithValue("@ToDoCode", txtTdoId.Text.ToString());
                    //    cmd.ExecuteNonQuery();
                    //}
                    //sql = "update MstrAttachments set ActivityCode=@ActivityCode,EditedOn=@EditedOn,EditedBy=@EditedBy where TodoCode=@TodoCode and OgCode=@OgCode and IsActive='Y' ";
                    //using (SqlCommand cmd = new SqlCommand(sql, con))
                    //{
                    //    cmd.Parameters.AddWithValue("@ActivityCode", ActivityId);
                    //    cmd.Parameters.AddWithValue("@OgCode", Session["OgCode"].ToString());
                    //    cmd.Parameters.AddWithValue("@EditedBy", Session["UserId"].ToString());
                    //    cmd.Parameters.AddWithValue("@EditedOn", cc.TodaysDate());
                    //    cmd.Parameters.AddWithValue("@ToDoCode", txtTdoId.Text.ToString());
                    //    cmd.ExecuteNonQuery();
                    //}

                    using (SqlCommand cmd = new SqlCommand("sp_ToDoAssign", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ActivityCode", ActivityId);
                        cmd.Parameters.AddWithValue("@AssignTo", ddlEmployee.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@UserId", ddlEmployee.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@OgCode", Session["OgCode"].ToString());
                        cmd.Parameters.AddWithValue("@CreatedBy", ddlEmployee.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@CreatedOn", cc.TodaysDate());
                        cmd.Parameters.AddWithValue("@EditedOn", cc.TodaysDate());
                        cmd.Parameters.AddWithValue("@EditedBy", Session["UserId"].ToString());
                        cmd.Parameters.AddWithValue("@ToDoCode", txtTdoId.Text.ToString());
                        cmd.ExecuteNonQuery();
                    }
                }
                BindGrid();
                ShowMsg("G", "Todo assign successfully.");
                Reset();
            }
            catch (Exception ex)
            {
                ShowMsg("R", ex.Message.ToString());
            }
        }

        protected void btnTodoReset_Click(object sender, EventArgs e)
        {
            Reset();
        }

        protected void GvDashboard_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GvDashboard.PageIndex = e.NewPageIndex;
            BindGrid();
        }
    }
}