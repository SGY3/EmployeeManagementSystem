using ActivityManagementSystem.App_Code;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace ActivityManagementSystem
{
    public partial class Progess : System.Web.UI.Page
    {
        string LoginPageUrl = "Login.aspx";
        Common cc = new Common();
        SendMail mail = new SendMail();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["userId"] != null)
            {
                if (!IsPostBack)
                {
                    if (this.Master.FindControl("lblPageTitle") is Label lblTitle)
                    {
                        lblTitle.Text = "Progress";
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
                    sql = sql = "Select [ActivityCode] ,[ToDoCode] ,[ProjectName] ,[ShortTitle] ,[Description] ,[ActivityType] ,[PageName] ,[SpName] ,[Priority] ," +
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
                                //lblMessage.Text = "";
                            }
                            else
                            {
                                // lblMessage.Text = "No data to show";
                            }
                            GvProgress.DataSource = dt;
                            GvProgress.DataBind();
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

                        sql = "insert into [MstrProgress] ([ActivityId],ActivityTitle ,[HourSpend] ,[MinuteSpend] ,[Remark] ,[CompletionYN] ,[Date] ,[CreatedOn] ,[CreatedBy],IsActive,OgCode) " +
                               " Values (@ActivityId,@ActivityTitle,@HourSpend,@MinuteSpend,@Remark,@CompletionYN,@Date,@CreatedOn,@CreatedBy,'Y',@OgCode)";

                        using (SqlCommand cmd = new SqlCommand(sql, con))
                        {
                            cmd.Parameters.AddWithValue("@ActivityId", txtActivityId.Text.ToString());
                            cmd.Parameters.AddWithValue("@ActivityTitle", txtActivityTitleId.Text.ToString());
                            cmd.Parameters.AddWithValue("@HourSpend", ddlHour.SelectedValue.ToString());
                            cmd.Parameters.AddWithValue("@MinuteSpend", ddlMinute.SelectedValue.ToString());
                            cmd.Parameters.AddWithValue("@Remark", txtRemark.Text.ToString());
                            cmd.Parameters.AddWithValue("@CompletionYN", ddlCompleteStatus.SelectedValue.ToString());
                            cmd.Parameters.AddWithValue("@Date", txtDate.Text.ToString());
                            cmd.Parameters.AddWithValue("@CreatedOn", cc.TodaysDate());
                            cmd.Parameters.AddWithValue("@CreatedBy", Session["UserId"].ToString());
                            cmd.Parameters.AddWithValue("@OgCode", Session["OgCode"].ToString());
                            cmd.ExecuteNonQuery();
                        }
                        if (ddlCompleteStatus.SelectedValue.ToString() == "Y")
                        {
                            sql = "update MstrActivity set EditedOn=@EditedOn,EditedBy=@EditedBy,CompletionStatus='Y' where OgCode=@OgCode and ActivityCode=@ActivityCode";
                        }
                        else
                        {
                            sql = "update MstrActivity set EditedOn=@EditedOn,EditedBy=@EditedBy where OgCode=@OgCode and ActivityCode=@ActivityCode";
                        }
                        using (SqlCommand cmd = new SqlCommand(sql, con))
                        {
                            cmd.Parameters.AddWithValue("@EditedOn", cc.TodaysDate());
                            cmd.Parameters.AddWithValue("@EditedBy", Session["UserId"].ToString());
                            cmd.Parameters.AddWithValue("@OgCode", Session["OgCode"].ToString());
                            cmd.Parameters.AddWithValue("@ActivityCode", txtActivityId.Text.ToString());
                            cmd.ExecuteNonQuery();
                        }
                    }
                    if (ddlCompleteStatus.SelectedValue.ToString() == "Y")
                    {

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
        private void CreateNextActivty(string AcivityCode)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(cc.GetConnectionString()))
                {
                    con.Open();
                    string sql = "Select ActivityType from ActivityAutomation where OrderNo =(select OrderNo+1 from ActivityAutomation where ActivityType =(select ActivityType from MstrActivity where OgCode='' and ActivityCode=''))";
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@EditedOn", cc.TodaysDate());
                        cmd.Parameters.AddWithValue("@EditedBy", Session["UserId"].ToString());
                        cmd.Parameters.AddWithValue("@OgCode", Session["OgCode"].ToString());
                        cmd.Parameters.AddWithValue("@ActivityCode", txtActivityId.Text.ToString());
                        cmd.ExecuteNonQuery();
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
            if (txtActivityId.Text.ToString() == "")
            {
                ShowMsg("R", "Select Activity");
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
            txtActivityId.Text = "";
            txtActivityTitleId.Text = "";
            txtRemark.Text = "";
            txtDate.Text = "";
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
                    string shortTitle = gvr.Cells[5].Text;
                    if (ddlValue == "Attachment")
                    {
                        string url = $"Attachments.aspx?ActivityId={activityId}";
                        string s = "window.open('" + url + "', 'popup_window', 'width=500,height=400,left=100,top=100,resizable=yes');";
                        ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
                    }
                    else if (ddlValue == "Add Progress")
                    {
                        txtActivityId.Text = activityId;
                        txtActivityTitleId.Text = shortTitle;
                    }
                    dropdown.SelectedIndex = 0;
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

        protected void GvProgress_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GvProgress.PageIndex=e.NewPageIndex;
            BindGrid();
        }
    }
}