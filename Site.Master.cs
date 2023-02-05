using System;
using System.IO;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Web;

namespace EmployeeManagementSystem
{
    public partial class Site : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["Name"] != null)
                {
                    lblUsername.Text = Session["Name"].ToString();
                }
            }
            if (Session["Admin"] != null && Session["Admin"].ToString() == "Y")
            {
                AdminMenu.Visible = true;
            }
            else
            {
                AdminMenu.Visible = false;
            }
            divMsg.InnerHtml = "";
        }
        public void ShowMessage(string color, string msg)
        {
            string colorType = "";
            string icon = "";
            if (color == "R")
            {
                colorType = "alert-danger";
                icon = "<i class=\"bi bi-exclamation-triangle-fill\"></i>";
            }
            else if (color == "W")
            {
                colorType = "alert-warning";
                icon = "<i class=\"bi bi-exclamation-triangle-fill\"></i>";
            }
            else if (color == "G")
            {
                colorType = "alert-success";
                icon = "<i class=\"bi bi-check-circle-fill\"></i>";
            }
            string divInner = $"<div class=\"alert {colorType} alert-dismissible fade show\" role=\"alert\">" +
                          $" {icon} {msg}" +
                          "<button type=\"button\" class=\"btn-close\" data-bs-dismiss=\"alert\" aria-label=\"Close\"></button></div>";
            divMsg.InnerHtml = divInner;
        }
    }
}