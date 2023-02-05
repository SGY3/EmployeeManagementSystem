<%@ Page Title="" Language="C#" MasterPageFile="~/Empty.Master" AutoEventWireup="true" CodeBehind="Attachments.aspx.cs" Inherits="EmployeeManagementSystem.Attachments" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container borderShadow bg-white">
        <div class="text-center">
            <h1 class="text-warning">Attachment</h1>
        </div>
        <div class="borderShadow mb-2">
            <div class="container">
                <div class="row mb-2">
                    <div class="col-md-12">
                        <asp:FileUpload ID="fuAttach" runat="server" CssClass="form-control" />
                    </div>
                </div>
                <div class="row mb-2">
                    <div class="col-md-1">
                        Remark
                    </div>
                    <div class="col-md-11">
                        <asp:TextBox ID="txtRemark" runat="server" TextMode="MultiLine" Rows="3" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="d-flex justify-content-center my-2">
                    <button type="button" id="btnUpload" class="btn btn-primary btn-sm" runat="server" onserverclick="btnUpload_Click">Upload <i class="bi bi-cloud-upload"></i></button>
                </div>
            </div>
        </div>
        <div>
            <div class="d-flex justify-content-center">
                <asp:Label ID="lblMessage" runat="server" CssClass="text-center h2 text-warning" Text=""></asp:Label>
            </div>
            <div style="overflow-x: auto; width: 100%">
                <asp:GridView ID="GvAttachment" runat="server" AutoGenerateColumns="false" CssClass="table table-sm table-hover table-striped table-bordered"  AllowPaging="True" OnPageIndexChanging="GvAttachment_PageIndexChanging" PageSize="10" >
                    <Columns>
                        <asp:TemplateField ItemStyle-Width="30">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnDownload" OnClick="btnDownload_ServerClick" runat="server"><i class="bi bi-cloud-download-fill"></i></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Width="30">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnDelete" OnClick="btnDelete_Click" runat="server"><i class="bi bi-trash"></i></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="AttachmentId" HeaderText="AttachmentId" ItemStyle-CssClass="d-none" HeaderStyle-CssClass="d-none" />
                        <asp:BoundField DataField="FileName" HeaderText="FileName" />
                        <asp:BoundField DataField="FilePath" HeaderText="FilePath" ItemStyle-CssClass="d-none" HeaderStyle-CssClass="d-none" />
                        <asp:BoundField DataField="UploadFileName" HeaderText="UploadFileName" ItemStyle-CssClass="d-none" HeaderStyle-CssClass="d-none"/>
                        <asp:BoundField DataField="Remark" HeaderText="Remark" />
                        <asp:BoundField DataField="UploadOn" HeaderText="UploadOn" />
                        <asp:BoundField DataField="UploadedBy" HeaderText="UploadedBy" />
                        <asp:BoundField DataField="ActivityCode" HeaderText="ActivityCode" ItemStyle-CssClass="d-none" HeaderStyle-CssClass="d-none" />
                        <asp:BoundField DataField="TodoCode" HeaderText="TodoCode" ItemStyle-CssClass="d-none" HeaderStyle-CssClass="d-none" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>
