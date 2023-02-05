<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="EmployeeManagementSystem.ChangePassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container w-25 borderShadow p-4 centerDiv">
        <h2 class="text-center text-primary"><i class="bi bi-person-lines-fill mx-2"></i>EMS</h2>
        <div class="input-group mb-3">
            <span class="input-group-text"><i class="bi bi-person-circle"></i></span>
            <input type="text" class="form-control" placeholder="User Id" id="txtUserId" runat="server" readonly />
        </div>
        <div class="input-group mb-3">
            <span class="input-group-text"><i class="bi bi-unlock"></i></span>
            <input type="password" id="txtOldPassword" runat="server" class="form-control" placeholder="Current Password" />
        </div>
        <div class="input-group mb-3">
            <span class="input-group-text"><i class="bi bi-lock"></i></span>
            <input type="password" id="txtNewPassword" runat="server" class="form-control" placeholder="New Password" />
        </div>
        <div class="input-group mb-3">
            <span class="input-group-text"><i class="bi bi-lock-fill"></i></span>
            <input type="password" id="txtConfirmPassword" runat="server" class="form-control" placeholder="Confirm Password" />
        </div>
        <div class="my-3 text-center">
            <button type="submit" id="btnSubmit" runat="server" class="btn btn-primary" onserverclick="btnSubmit_ServerClick">Change Password</button>
        </div>
    </div>
</asp:Content>
