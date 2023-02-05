using EmployeeManagementSystem.App_Code;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace EmployeeManagementSystem
{
    public partial class ViewProgress : System.Web.UI.Page
    {
        string LoginPageUrl = "Login.aspx";
        Common cc = new Common();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["userId"] != null)
            {
                if (!IsPostBack)
                {
                    if (Request.QueryString["ActivityId"] != null)
                    {
                        string ActivityId = Request.QueryString["ActivityId"].ToString();
                        txtActivityId.Value = ActivityId;
                        BindGrid(ActivityId);
                    }
                }
            }
            else
            {
                Response.Redirect(LoginPageUrl);
            }
        }
        private void BindGrid(string ActivityId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(cc.GetConnectionString()))
                {
                    con.Open();
                    string sql = "";

                    sql = "Select ActivityId,[ActivityTitle] ,[HourSpend] ,[MinuteSpend] ,[Remark] ,[CompletionYN] ,[Date] ,[IsActive] ,[CreatedOn] ,[CreatedBy] ,[OgCode] from MstrProgress " +
                           "Where OgCode=@OgCode and [IsActive]='Y' and ActivityId=@ActivityId Order by CreatedOn";


                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@OgCode", Session["OgCode"].ToString());
                        cmd.Parameters.AddWithValue("@ActivityId", ActivityId);

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
                            GvViewProgress.DataSource = dt;
                            GvViewProgress.DataBind();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMsg("R", ex.Message.ToString());
            }
        }
        protected void GvViewProgress_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "View")
                {
                    //Determine the RowIndex of the Row whose Button was clicked.
                    int rowIndex = Convert.ToInt32(e.CommandArgument);

                    //Reference the GridView Row.
                    GridViewRow row = GvViewProgress.Rows[rowIndex];

                    //Fetch value.
                    string activityId = row.Cells[7].Text;
                    string url = $"Attachments.aspx?ActivityId={activityId}";
                    string s = "window.open('" + url + "', 'popup_window', 'width=500,height=400,left=100,top=100,resizable=yes');";
                    ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
                }
            }
            catch (Exception ex)
            {
                ShowMsg("R", ex.Message.ToString());
            }
        }
        public void ShowMsg(string color, string msg)
        {
            home home = (home)this.Master;
            home.ShowMessage(color, msg);
        }

        protected void GvViewProgress_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GvViewProgress.PageIndex = e.NewPageIndex;
            if (Request.QueryString["ActivityId"] != null)
            {
                string ActivityId = Request.QueryString["ActivityId"].ToString();
                txtActivityId.Value = ActivityId;
                BindGrid(ActivityId);
            }
        }
    }
}