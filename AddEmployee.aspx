<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddEmployee.aspx.cs" Inherits="EmployeeManagementSystem.AddEmployee" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid borderShadow">
        <div class="row">
            <div class="col-md-4">
                <div class="form-floating mb-2">
                    <asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control" placeholder="First Name"></asp:TextBox>
                    <label for="txtFirstName" class="form-label">First Name</label>
                </div>
                <div class="form-floating mb-2">
                    <asp:TextBox ID="txtLastName" runat="server" CssClass="form-control" placeholder="Last Name"></asp:TextBox>
                    <label for="txtLastName" class="form-label">Last Name</label>
                </div>
                <div class="form-floating mb-2">
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="Email"></asp:TextBox>
                    <label for="txtEmail" class="form-label">Email</label>
                </div>
                <div class="form-floating mb-2">
                    <asp:TextBox ID="txtMobile" runat="server" CssClass="form-control" placeholder="Mobile"></asp:TextBox>
                    <label for="txtMobile" class="form-label">Mobile</label>
                </div>
                <div class="form-floating mb-2">
                    <asp:DropDownList ID="ddlActive" CssClass="form-select" runat="server">
                        <asp:ListItem Selected="True" Value="Y">Yes</asp:ListItem>
                        <asp:ListItem Value="N">No</asp:ListItem>
                    </asp:DropDownList>
                    <label for="ddlActive" class="form-label">Active</label>
                </div>
                <div class="d-flex justify-content-center my-1">
                    <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" CssClass="btn btn-success btn-sm mx-1" />
                    <asp:Button ID="btnUpdate" runat="server" Text="Update" OnClick="btnUpdate_Click" Visible="false" CssClass="btn btn-warning btn-sm mx-1" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click" CssClass="btn btn-danger btn-sm mx-1" />
                    <asp:Button ID="btnPassReset" runat="server" Text="Reset Password" OnClick="btnPassReset_Click" CssClass="btn btn-warning btn-sm mx-1" Visible="false" />
                </div>
            </div>
            <div class="col-md-8">
                <div style="overflow-x: auto; width: 100%">
                    <asp:GridView ID="GvEmployee" runat="server" AutoGenerateColumns="false" CssClass="table table-sm table-hover table-striped table-bordered" OnRowCommand="GvEmployee_RowCommand" AllowPaging="True" OnPageIndexChanging="GvEmployee_PageIndexChanging" PageSize="10">
                        <Columns>
                            <asp:TemplateField ItemStyle-Width="30">
                                <ItemTemplate>
                                    <asp:Button Text="Edit" runat="server" CommandName="EditC" CommandArgument="<%# Container.DataItemIndex %>" CssClass="btn btn-primary btn-sm" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="FirstName" HeaderText="FirstName" ItemStyle-Width="30" />
                            <asp:BoundField DataField="LastName" HeaderText="LastName" ItemStyle-Width="30" />
                            <asp:BoundField DataField="Email" HeaderText="Email" ItemStyle-Width="30" />
                            <asp:BoundField DataField="Mobile" HeaderText="Mobile" ItemStyle-Width="30" />
                            <asp:BoundField DataField="UserName" HeaderText="UserName" ItemStyle-Width="30" />
                            <asp:BoundField DataField="IsActive" HeaderText="Active" ItemStyle-Width="30" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hiddenEmp" runat="server" />
</asp:Content>
