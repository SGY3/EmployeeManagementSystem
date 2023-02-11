using EmployeeManagementSystem;
using EmployeeManagementSystem.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ActivityManagementSystem
{
    public partial class ImportData : System.Web.UI.Page
    {
        string LoginPageUrl = "Login.aspx";
        Common cc = new Common();
        protected void Page_PreInit(object sender, EventArgs e)
        {
            CreateDynamicControl();
        }
            protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] != null)
            {
                if (!IsPostBack)
                {
                    if (this.Master.FindControl("lblPageTitle") is Label lblTitle)
                    {
                        lblTitle.Text = "Import Data";
                    }
                }
            }
            else
            {
                Response.Redirect(LoginPageUrl);
            }
        }

        protected void btnUploadFile_Click(object sender, EventArgs e)
        {
            try
            {
                if (FileCheck())
                {
                    string filePath = "Upload/" + Session["OgCode"].ToString() + "";
                    string path = Server.MapPath("~/" + filePath + "/");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    Guid guid = Guid.NewGuid();
                    string fileName = fuFile.FileName;
                    string newFileName = guid + "_" + fileName;
                    string fileSavePath = path + newFileName;
                    fuFile.SaveAs(fileSavePath);

                    using (DataTable dt = cc.ConvertExcelToDataTable(fileSavePath, "Y"))
                    {
                        Session["ExcelColumnData"] = dt;
                    }
                    tblDynamicControl.Controls.Clear();
                    CreateDynamicControl();
                    // call the JavaScript function
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "showalert", "openModal();", true);
                }
            }
            catch (Exception ex)
            {
                ShowMsg("R", ex.Message.ToString());
            }
        }
        protected Boolean FileCheck()
        {
            return true;
        }
        private void CreateDynamicControl()
        {
            if (Session["ExcelColumnData"] != null)
            {
                string[] Array = { "Project Name", "Short Title", "Description", "Activity Type" };

                for (int count = 0; count < Array.Length; count++)
                {
                    TableRow row = new TableRow();
                    TableCell cell1 = new TableCell();
                    TableCell cell2 = new TableCell();

                    DropDownList emsDDL = new DropDownList();
                    emsDDL.ID = "ddlEmsDynamic" + count.ToString();
                    emsDDL.Items.Add(new ListItem(Array[count], Array[count].Replace(" ", "")));
                    emsDDL.CssClass = "form-select";
                    emsDDL.Enabled = false;
                    emsDDL.DataBind();
                    cell1.Controls.Add(emsDDL);
                    row.Cells.Add(cell1);

                    DropDownList excelDDL = new DropDownList();
                    excelDDL.ID = "ddlExcelDynamic" + count.ToString();
                    excelDDL.DataSource = (DataTable)Session["ExcelColumnData"];
                    excelDDL.DataValueField = "Value";
                    excelDDL.DataTextField = "Text";
                    excelDDL.CssClass = "form-select";
                    excelDDL.DataBind();
                    cell2.Controls.Add(excelDDL);
                    row.Cells.Add(cell2);

                    tblDynamicControl.Controls.Add(row);
                }
            }
        }
        public void ShowMsg(string color, string msg)
        {
            Site site = (Site)this.Master;
            site.ShowMessage(color, msg);
        }

        protected void btnImportData_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "showalert", "openModal();", true);
        }
    }
}