<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="EmployeeManagementSystem.Profile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container borderShadow w-25">
        <div>
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
            <div class="d-flex justify-content-center my-1">
                <asp:Button ID="btnEdit" runat="server" Text="Edit" OnClick="btnEdit_Click" CssClass="btn btn-success btn-sm mx-1" />
                <asp:Button ID="btnUpdate" runat="server" Text="Update" OnClick="btnUpdate_Click" Visible="false" CssClass="btn btn-warning btn-sm mx-1" />
                <asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click" CssClass="btn btn-danger btn-sm mx-1" />
            </div>
        </div>
    </div>
</asp:Content>
