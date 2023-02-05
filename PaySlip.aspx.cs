using EmployeeManagementSystem.App_Code;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI.WebControls;

namespace ActivityManagementSystem
{
    public partial class PaySlip : System.Web.UI.Page
    {
        Common cc = new Common();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (this.Master.FindControl("lblPageTitle") is Label lblTitle)
                {
                    lblTitle.Text = "Pay Slip";
                }
                if (Session["Name"] != null)
                {

                }
                else
                {
                    Response.Redirect("Login.aspx");
                }
            }
        }
        private void GeneratePaySlip()
        {
            try
            {
                string sourceFile = Server.MapPath("~/TempPaySlip/tempSlip.html");
                string fileName = "Slip" + Session["Name"].ToString() + DateTime.Now.ToString("yy_MM_dd_HH_mm");
                string destinationFile = Server.MapPath("~/TempPaySlip/" + fileName + ".html");
                try
                {
                    File.Copy(sourceFile, destinationFile, true);

                    string OgName = "";
                    using (SqlConnection con = new SqlConnection(cc.GetConnectionString()))
                    {
                        con.Open();
                        string sql = "";

                        sql = "Select [OgName] from [MstrOraganization] Where OgCode=@OgCode and [IsActive]='Y' ";

                        using (SqlCommand cmd = new SqlCommand(sql, con))
                        {
                            cmd.Parameters.AddWithValue("@OgCode", Session["OgCode"].ToString());
                            OgName = cmd.ExecuteScalar().ToString();
                        }
                    }

                    string text = File.ReadAllText(destinationFile);
                    text = text.Replace("{OgName}", OgName);
                    text = text.Replace("{Name}", Session["Name"].ToString());
                    text = text.Replace("{Basic Salary}", "21000");
                    text = text.Replace("{BasicPay}", "21000");
                    text = text.Replace("{TotalPay}", "21000");
                    File.WriteAllText(destinationFile, text);

                    // open a pop up window at the center of the page.

                    string url = $"TempPaySlip/" + fileName + ".html";
                    string s = "window.open('" + url + "', 'popup_window', 'width=600,height=500,left=100,top=100,resizable=yes');";
                    ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
                    //ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "var Mleft = (screen.width/2)-(760/2);var Mtop = (screen.height/2)-(700/2);window.open( 'TempPaySlip/" + fileName + ".html', null, 'height=700,width=760,status=yes,toolbar=no,scrollbars=yes,menubar=no,location=no,top=\'+Mtop+\', left=\'+Mleft+\'' );", true);
                }
                catch (Exception ex)
                {

                }
                finally
                {
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void btnJan_ServerClick(object sender, EventArgs e)
        {
            GeneratePaySlip();
        }
    }
}