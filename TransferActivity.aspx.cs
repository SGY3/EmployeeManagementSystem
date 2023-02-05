using EmployeeManagementSystem.App_Code;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace EmployeeManagementSystem
{
    public partial class TransferActivity : System.Web.UI.Page
    {
        string LoginPageUrl = "Login.aspx";
        Common cc = new Common();
        SendMail mail = new SendMail();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Admin"] != null && Session["Admin"].ToString() == "Y")
            {
                if (!IsPostBack)
                {
                    if (this.Master.FindControl("lblPageTitle") is Label lblTitle)
                    {
                        lblTitle.Text = "Transfer Activity";
                    }
                    BindGrid();
                    BindEmployee();
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
                    sql = "Select [ActivityCode] ,[ProjectName] ,[ShortTitle] ,[Description] ,[ActivityType] ,[PageName] ,[SpName] ,[Priority] ," +
                        "[CompletionStatus] ,[AssignTo] ,[UserId] ,[OgCode] ,[CreatedBy] ,[CreatedOn] ,[EditedBy] ,[EditedOn] FROM [MstrActivity] " +
                        "Where OgCode=@OgCode and [IsActive]='Y' and CompletionStatus='N' Order by CreatedOn desc";

                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@OgCode", Session["OgCode"].ToString());

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
        private void BindEmployee()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(cc.GetConnectionString()))
                {
                    con.Open();
                    string sql = "";
                    sql = $"select '00' as EmpId,'--Select--' as EmpName union select UserName as EmpId,CONCAT(FirstName,' ',LastName) as EmpName from MstrLogin  where OgCode=@OgCode and IsActive='Y'";

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
        public void ShowMsg(string color, string msg)
        {
            Site site = (Site)this.Master;
            site.ShowMessage(color, msg);
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
                    string ActivityCode = gvr.Cells[1].Text;
                    string ActivityTitle = gvr.Cells[3].Text;
                    if (ddlValue == "Transfer")
                    {
                        txtActivityId.Text = ActivityCode;
                        txtActivityTitle.Text = ActivityTitle;
                    }
                    dropdown.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                ShowMsg("R", ex.Message.ToString());
            }
        }
        protected void btnTransfer_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidCheck())
                {
                    using (SqlConnection con = new SqlConnection(cc.GetConnectionString()))
                    {
                        con.Open();
                        string sql = "";
                        sql = "update [MstrActivity] set [AssignTo]=@AssignTo,[UserId]=@UserId,[EditedBy]=@EditedBy,[EditedOn]=@EditedOn,CreatedBy=@CreatedBy Where OgCode=@OgCode and ActivityCode=@ActivityCode";
                        using (SqlCommand cmd = new SqlCommand(sql, con))
                        {
                            cmd.Parameters.AddWithValue("@OgCode", Session["OgCode"].ToString());
                            cmd.Parameters.AddWithValue("@ActivityCode", txtActivityId.Text.ToString());
                            cmd.Parameters.AddWithValue("@UserId", ddlEmployee.SelectedValue.ToString());
                            cmd.Parameters.AddWithValue("@CreatedBy", ddlEmployee.SelectedValue.ToString());
                            cmd.Parameters.AddWithValue("@AssignTo", ddlEmployee.SelectedItem.ToString());
                            cmd.Parameters.AddWithValue("@EditedBy", Session["Userid"].ToString());
                            cmd.Parameters.AddWithValue("@EditedOn", cc.TodaysDate());
                            cmd.ExecuteNonQuery();
                        }
                    }
                    ShowMsg("G", "Activity transffered successfully.");
                    Task.Run(() =>
                    {
                        mail.SendNewActivityAssign(Session["OgCode"].ToString(), ddlEmployee.SelectedValue.ToString(), txtActivityId.Text.ToString());
                    });
                    BindGrid();
                    Reset();
                }
            }
            catch (Exception ex)
            {
                ShowMsg("R", ex.Message.ToString());
            }
        }
        private Boolean ValidCheck()
        {
            if (txtActivityId.Text == "")
            {
                ShowMsg("R", "Select Activity");
                return false;
            }
            if (txtActivityTitle.Text == "")
            {
                ShowMsg("R", "Title can not be blank");
                return false;
            }
            return true;
        }
        protected void btnActivityReset_Click(object sender, EventArgs e)
        {
            Reset();
        }
        private void Reset()
        {
            txtActivityId.Text = "";
            txtActivityTitle.Text = "";
            ddlEmployee.SelectedIndex = 0;
        }

        protected void GvDashboard_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GvDashboard.PageIndex = e.NewPageIndex;
            BindGrid();
        }
    }
}