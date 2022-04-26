using ActivityManagementSystem.App_Code;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace ActivityManagementSystem
{
    public partial class Dashboard : System.Web.UI.Page
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
                    Label lblTitle = this.Master.FindControl("lblPageTitle") as Label;
                    if (lblTitle != null)
                    {
                        lblTitle.Text = "Dashboard";
                    }
                    BindGrid();
                    BindIndicator();
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
                    if (Session["Admin"].ToString() == "Y")
                    {
                        sql = "Select [ActivityCode] ,[ToDoCode] ,[ProjectName] ,[ShortTitle] ,[Description] ,[ActivityType] ,[PageName] ,[SpName] ,[Priority] ," +
                            "[CompletionStatus] ,[AssignTo] ,[UserId] ,[OgCode] ,[CreatedBy] ,[CreatedOn] ,[EditedBy] ,[EditedOn] FROM [MstrActivity] " +
                            "Where OgCode=@OgCode and [IsActive]='Y' Order by CreatedOn desc";
                    }
                    else
                    {
                        sql = "Select [ActivityCode] ,[ToDoCode] ,[ProjectName] ,[ShortTitle] ,[Description] ,[ActivityType] ,[PageName] ,[SpName] ,[Priority] ," +
                              "[CompletionStatus] ,[AssignTo] ,[UserId] ,[OgCode] ,[CreatedBy] ,[CreatedOn] ,[EditedBy] ,[EditedOn] FROM [MstrActivity] " +
                               "Where OgCode=@OgCode and [IsActive]='Y' and AssignTo=@AssignTo  Order by CreatedOn desc";
                    }

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
        private void BindIndicator()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(cc.GetConnectionString()))
                {
                    con.Open();
                    string sql = "";

                    sql = "select (Select Count(ActivityCode) from MstrActivity where OgCode=@OgCode and UserId=@UserId and CompletionStatus='N' and IsActive='Y') as Active," +
                          "(Select Count(ActivityCode) from MstrActivity where OgCode=@OgCode and UserId=@UserId and CompletionStatus='Y' and IsActive='Y') as Completed, " +
                           "(Select Count(ActivityCode) from MstrActivity where OgCode=@OgCode and UserId=@UserId and IsActive='Y') as Total," +
                           "(select Count(ToDoCode) from MstrTodo where OgCode=@OgCode and CreatedBy=@UserId and Isactive='Y') as TodoAdded";


                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@OgCode", Session["OgCode"].ToString());
                        cmd.Parameters.AddWithValue("@UserId", Session["UserId"].ToString());
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();
                                lblActive.Text = reader["Active"].ToString();
                                lblCompleted.Text = reader["Completed"].ToString();
                                lblTotal.Text = reader["Total"].ToString();
                                lblTodo.Text = reader["TodoAdded"].ToString();
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

        protected void GvDashboard_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GvDashboard.PageIndex = e.NewPageIndex;
            BindGrid();
        }
    }
}