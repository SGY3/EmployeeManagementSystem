using EmployeeManagementSystem;
using EmployeeManagementSystem.App_Code;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net.NetworkInformation;
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
                if (ViewState["ExcelColumnData"] != null)
                {
                    CreateDynamicControl();
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
                        ViewState["ExcelColumnData"] = dt;
                    }
                    ViewState["FilePathExcel"] = fileSavePath;
                    tblDynamicControl.Controls.Clear();
                    CreateDynamicControl();
                    // call the JavaScript function
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "showalert", "openModal();", true);
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
            try
            {
                if (ViewState["ExcelColumnData"] != null)
                {
                    tblDynamicControl.Controls.Clear();
                    string[] Array = { "Project Name", "Short Title", "Description", "Activity Type" };

                    for (int count = 0; count < Array.Length; count++)
                    {
                        TableRow row = new TableRow();
                        TableCell cell1 = new TableCell();
                        TableCell cell2 = new TableCell();

                        DropDownList emsDDL = new DropDownList();
                        emsDDL.ID = "ddlEmsDynamic" + count.ToString();
                        emsDDL.Items.Add(new ListItem(Array[count], Array[count].Replace(" ", "")));
                        emsDDL.CssClass = "form-select disabled";
                        emsDDL.Enabled = false;
                        emsDDL.DataBind();
                        cell1.Controls.Add(emsDDL);
                        row.Cells.Add(cell1);

                        DropDownList excelDDL = new DropDownList();
                        excelDDL.ID = "ddlExcelDynamic" + count.ToString();
                        excelDDL.DataSource = (DataTable)ViewState["ExcelColumnData"];
                        excelDDL.DataValueField = "Value";
                        excelDDL.DataTextField = "Text";
                        excelDDL.CssClass = "form-select ";
                        excelDDL.DataBind();
                        cell2.Controls.Add(excelDDL);
                        row.Cells.Add(cell2);

                        tblDynamicControl.Controls.Add(row);
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

        protected void btnImportData_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(cc.GetConnectionString()))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("Delete from TempTodo where OgCode=@OgCode and AddedBy=@AddedBy", con))
                    {
                        cmd.Parameters.AddWithValue("@OgCode", Session["OgCode"].ToString());
                        cmd.Parameters.AddWithValue("@AddedBy", Session["UserId"].ToString());
                        cmd.ExecuteNonQuery();
                    }
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(con))
                    {
                        bulkCopy.DestinationTableName = "dbo.TempTodo";
                        foreach (TableRow tableRow in tblDynamicControl.Rows)
                        {
                            DropDownList ddlEms = (DropDownList)tableRow.Cells[0].Controls[0];
                            DropDownList ddlExcel = (DropDownList)tableRow.Cells[1].Controls[0];
                            bulkCopy.ColumnMappings.Add(ddlExcel.SelectedValue.ToString(), ddlEms.SelectedValue.ToString());
                        }
                        using (DataTable dt = cc.ConvertExcelToDataTable(ViewState["FilePathExcel"].ToString()))
                        {
                            DataColumn dataColumn = new DataColumn("OgCode");
                            dataColumn.DefaultValue = Session["OgCode"].ToString();
                            dt.Columns.Add(dataColumn);

                            dataColumn = new DataColumn("AddedBy");
                            dataColumn.DefaultValue = Session["UserId"].ToString();
                            dt.Columns.Add(dataColumn);

                            bulkCopy.ColumnMappings.Add("OgCode", "OgCode");
                            bulkCopy.ColumnMappings.Add("AddedBy", "AddedBy");
                            bulkCopy.WriteToServer(dt);
                        }
                    }
                    using (SqlCommand cmd = new SqlCommand("ImportDataTodo", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@OgCode", Session["OgCode"].ToString());
                        cmd.Parameters.AddWithValue("@AddedBy", Session["UserId"].ToString());
                        cmd.Parameters.AddWithValue("@AddedOn", cc.TodaysDate());
                        cmd.ExecuteNonQuery();
                    }

                }
                ShowMsg("G", "Data Imported Successfully");
            }
            catch (Exception ex)
            {
                ShowMsg("R", ex.Message.ToString());
                Page.ClientScript.RegisterStartupScript(this.GetType(), "showalert", "openModal();", true);
            }
        }
    }
}