using EmployeeManagementSystem.App_Code;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace EmployeeManagementSystem
{
    public partial class SeeAllProgress : System.Web.UI.Page
    {
        string LoginPageUrl = "Login.aspx";
        Common cc = new Common();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["userId"] != null)
            {
                if (!IsPostBack)
                {
                    if (this.Master.FindControl("lblPageTitle") is Label lblTitle)
                    {
                        lblTitle.Text = "All Progress";
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

                    sql = "Select [ActivityCode] ,[ToDoCode] ,[ProjectName] ,[ShortTitle] ,[Description] ,[ActivityType] ,[PageName] ,[SpName] ,[Priority] ," +
                          "[CompletionStatus] ,[AssignTo] ,[UserId] ,[OgCode] ,[CreatedBy] ,[CreatedOn] ,[EditedBy] ,[EditedOn] FROM [MstrActivity] " +
                           "Where OgCode=@OgCode and [IsActive]='Y' and CreatedBy=@CreatedBy  Order by CreatedOn desc";


                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@OgCode", Session["OgCode"].ToString());
                        cmd.Parameters.AddWithValue("@CreatedBy", Session["UserId"].ToString());

                        using (DataTable dt = new DataTable())
                        {
                            dt.Load(cmd.ExecuteReader());
                            if (dt.Rows.Count > 0)
                            {
                                GvAllProgress.DataSource = dt;
                                GvAllProgress.DataBind();
                                // lblMessage.Text = "";
                            }
                            else
                            {
                                //lblMessage.Text = "No data to show";
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
                    if (ddlValue == "View")
                    {
                        string url = $"ViewProgress.aspx?ActivityId={activityId}";
                        string s = "window.open('" + url + "', 'popup_window', 'width=500,height=400,left=100,top=100,resizable=yes');";
                        ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
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

        protected void GvAllProgress_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GvAllProgress.PageIndex = e.NewPageIndex;   
            BindGrid();
        }
    }
}