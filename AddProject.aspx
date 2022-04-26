<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddProject.aspx.cs" Inherits="ActivityManagementSystem.AddProject" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid borderShadow">
        <div class="row">
            <div class="col-md-4">
                <div class="form-floating mb-2">
                    <asp:TextBox ID="txtProjectName" runat="server" CssClass="form-control" placeholder="Project Name"></asp:TextBox>
                    <label for="txtProjectName" class="form-label">Project Name</label>
                </div>
                <div class="form-floating mb-2">
                    <asp:DropDownList ID="ddlActive" CssClass="form-select" runat="server">
                        <asp:ListItem Selected="True" Value="Y">Yes</asp:ListItem>
                        <asp:ListItem Value="N">No</asp:ListItem>
                    </asp:DropDownList>
                    <label for="ddlActive" class="form-label">Active</label>
                </div>
                <div class="d-flex justify-content-center my-1">
                    <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSubmit_Click" CssClass="btn btn-success btn-sm mx-1" />
                    <asp:Button ID="btnUpdate" runat="server" Text="Update" Visible="false" OnClick="btnUpdate_Click" CssClass="btn btn-warning btn-sm mx-1" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click" CssClass="btn btn-danger btn-sm mx-1" />
                </div>
            </div>
            <div class="col-md-8">
                <div style="overflow-x: auto; width: 100%">
                    <asp:GridView ID="GvProject" runat="server" AutoGenerateColumns="false" OnRowCommand="GvProject_RowCommand" CssClass="table table-sm table-hover table-striped table-bordered"  AllowPaging="True" OnPageIndexChanging="GvProject_PageIndexChanging" PageSize="10" >
                        <Columns>
                            <asp:TemplateField ItemStyle-Width="30">
                                <ItemTemplate>
                                    <asp:Button Text="Edit" runat="server" CommandName="EditC" CommandArgument="<%# Container.DataItemIndex %>"  CssClass="btn btn-primary btn-sm" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="ProjectId" HeaderText="Project Id" ItemStyle-Width="30" />
                            <asp:BoundField DataField="ProjectName" HeaderText="Project Name" ItemStyle-Width="30" />
                            <asp:BoundField DataField="IsActive" HeaderText="Active" ItemStyle-Width="30" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hiddenProjectName" runat="server" />
</asp:Content>
