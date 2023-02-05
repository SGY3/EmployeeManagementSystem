using EmployeeManagementSystem.App_Code;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.Services.Description;
using System.Web.UI.WebControls;

namespace EmployeeManagementSystem
{
    public partial class Attachments : System.Web.UI.Page
    {
        Common cc = new Common();
        public static string ActivityId = "";
        public static string ToDoId = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            ActivityId = "";
            ToDoId = "";
            if (Request.QueryString["ActivityId"] != null)
            {
                ActivityId = Request.QueryString["ActivityId"].ToString();
            }
            else if (Request.QueryString["ToDoCode"] != null)
            {
                ToDoId = Request.QueryString["ToDoCode"].ToString();
            }
            if (!IsPostBack)
            {
                BindGrid(ActivityId, ToDoId);
            }
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                if (fuAttach.HasFile)
                {
                    if (FileSizeCheck())
                    {
                        string filePath = "Upload/" + Session["OgCode"].ToString() + "";
                        string path = Server.MapPath("~/" + filePath + "/");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        Guid guid = Guid.NewGuid();
                        string fileName = fuAttach.FileName;
                        string newFileName = guid + "_" + fileName;
                        string fileSavePath = path + newFileName;
                        fuAttach.SaveAs(fileSavePath);
                        using (SqlConnection con = new SqlConnection(cc.GetConnectionString()))
                        {
                            con.Open();
                            string sql = "";
                            sql = "insert into [MstrAttachments] ([ActivityCode] ,[TodoCode] ,[FileName] ,[UploadFileName] ,[FilePath] ,[Remark] ,[UploadOn] ,[UploadedBy] ,[OgCode],IsActive)" +
                                  " values (@ActivityCode,@TodoCode,@FileName,@UploadFileName,@FilePath,@Remark,@UploadOn,@UploadedBy,@OgCode,'Y')";

                            using (SqlCommand cmd = new SqlCommand(sql, con))
                            {
                                cmd.Parameters.AddWithValue("@ActivityCode", ActivityId);
                                cmd.Parameters.AddWithValue("@TodoCode", ToDoId);
                                cmd.Parameters.AddWithValue("@FileName", fileName);
                                cmd.Parameters.AddWithValue("@UploadFileName", newFileName);
                                cmd.Parameters.AddWithValue("@FilePath", filePath);
                                cmd.Parameters.AddWithValue("@Remark", txtRemark.Text.ToString());
                                cmd.Parameters.AddWithValue("@UploadOn", cc.TodaysDate());
                                cmd.Parameters.AddWithValue("@UploadedBy", Session["UserId"].ToString());
                                cmd.Parameters.AddWithValue("@OgCode", Session["OgCode"].ToString());
                                cmd.ExecuteNonQuery();
                            }
                        }
                        ShowMsg("G", "File Uploaded Successfully");
                        BindGrid(ActivityId, ToDoId);
                    }
                }
                else
                {
                    ShowMsg("R", "Select file to upload");
                }
            }
            catch (Exception ex)
            {
                ShowMsg("R", ex.Message.ToString());
            }
        }
        private Boolean FileSizeCheck()
        {
            int fileSize = fuAttach.PostedFile.ContentLength / 1024000;
            int allowSize = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(cc.GetConnectionString()))
                {
                    con.Open();
                    string sql = "select MaxFileSize from EMS_Parameter";
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        allowSize = cc.StoI(cmd.ExecuteScalar().ToString());
                    }
                }
                if (allowSize <= fileSize)
                {
                    ShowMsg("W", $"Upload file max allowed is {allowSize}MB only");
                    return false;
                }
            }
            catch (Exception ex)
            {
                ShowMsg("R", ex.Message.ToString());
            }
            return true;
        }
        private void BindGrid(string ActivityId = "", string ToDoId = "")
        {
            try
            {
                using (SqlConnection con = new SqlConnection(cc.GetConnectionString()))
                {
                    con.Open();
                    string sql = "";
                    if (ActivityId != "")
                    {
                        sql = "Select AttachmentId,[ActivityCode] ,[TodoCode] ,[FileName] ,[UploadFileName] ,[FilePath] ,[Remark] ,[UploadOn] ,[UploadedBy] ,[OgCode] From MstrAttachments " +
                            "Where OgCode=@OgCode and ActivityCode=@ActivityCode and IsActive='Y' order by UploadOn desc";
                    }
                    else
                    {
                        sql = "Select AttachmentId,[ActivityCode] ,[TodoCode] ,[FileName] ,[UploadFileName] ,[FilePath] ,[Remark] ,[UploadOn] ,[UploadedBy] ,[OgCode] From MstrAttachments " +
                            "Where OgCode=@OgCode and TodoCode=@TodoCode and IsActive='Y' order by UploadOn desc";
                    }

                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@OgCode", Session["OgCode"].ToString());
                        cmd.Parameters.AddWithValue("@ActivityCode", ActivityId);
                        cmd.Parameters.AddWithValue("@TodoCode", ToDoId);

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
                            GvAttachment.DataSource = dt;
                            GvAttachment.DataBind();
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
            Empty home = (Empty)this.Master;
            home.ShowMessage(color, msg);
        }

        protected void btnDownload_ServerClick(object sender, EventArgs e)
        {
            GridViewRow gvr = ((LinkButton)sender).NamingContainer as GridViewRow;
            if (gvr != null)
            {
                string FileName = gvr.Cells[5].Text;
                string FilePath = gvr.Cells[4].Text;
                string fullpath = Server.MapPath("~/" + FilePath + "/" + FileName + "");
                DownloadFile(fullpath);
            }
        }
        private void DownloadFile(string file)
        {
            try
            {
                var fi = new FileInfo(file);
                string FileExtention = fi.Extension.ToString();
                Response.Clear();
                if (FileExtention == ".mp4")
                {
                    Response.ContentType = "video/mp4";
                }
                else
                {
                    Response.ContentType = "application/octet-stream";
                }
                Response.AddHeader("Content-Disposition", "attachment; filename=" + fi.Name);
                Response.WriteFile(file);
                Response.End();
            }
            catch (Exception ex)
            {
                ShowMsg("R", ex.Message.ToString());
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            GridViewRow gvr = ((LinkButton)sender).NamingContainer as GridViewRow;
            if (gvr != null)
            {
                string AttachmentId = gvr.Cells[2].Text;
                string ActivityCode = gvr.Cells[8].Text;
                string TodoCode = gvr.Cells[9].Text;
                using (SqlConnection con = new SqlConnection(cc.GetConnectionString()))
                {
                    con.Open();
                    string sql = "";
                    if (ActivityId != "")
                    {
                        sql = "update MstrAttachments set IsActive='N',EditedBy=@EditedBy,EditedOn=@EditedOn " +
                            "Where OgCode=@OgCode and ActivityCode=@ActivityCode and AttachmentId=@AttachmentId";
                    }
                    else
                    {
                        sql = "update MstrAttachments set IsActive='N',EditedBy=@EditedBy,EditedOn=@EditedOn " +
                           "Where OgCode=@OgCode and TodoCode=@TodoCode and AttachmentId=@AttachmentId";
                    }


                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@OgCode", Session["OgCode"].ToString());
                        cmd.Parameters.AddWithValue("@ActivityCode", ActivityCode);
                        cmd.Parameters.AddWithValue("@TodoCode", TodoCode);
                        cmd.Parameters.AddWithValue("@EditedBy", Session["UserId"].ToString());
                        cmd.Parameters.AddWithValue("@EditedOn", cc.TodaysDate());
                        cmd.Parameters.AddWithValue("@AttachmentId", AttachmentId);
                        cmd.ExecuteNonQuery();
                    }
                }
                ShowMsg("W", "File deleted successfully");
                BindGrid(ActivityId, ToDoId);
            }
        }

        protected void GvAttachment_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GvAttachment.PageIndex = e.NewPageIndex;
            BindGrid(ActivityId, ToDoId);
        }
    }
}