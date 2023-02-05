using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EmployeeManagementSystem
{
    public partial class home : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
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