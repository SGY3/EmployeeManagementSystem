<%@ Page Title="" Language="C#" MasterPageFile="~/home.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="ActivityManagementSystem.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container w-25 borderShadow  centerDiv bg-white">
        <h2 class="text-center text-primary"><i class="bi bi-person-lines-fill mx-2"></i>AMS</h2>
        <div class="input-group mb-3">
            <span data-aos="flip-right" class="input-group-text"><i class="bi bi-building"></i></span>
            <input type="text"  class="form-control" placeholder="Organization Id" id="txtOrganizationId" runat="server" />
        </div>
        <div class="input-group mb-3">
            <span  data-aos="flip-left" class="input-group-text"><i class="bi bi-person-circle"></i></span>
            <input type="text" class="form-control" placeholder="Username" id="txtUserName" runat="server" />
        </div>
        <div class="input-group mb-3">
            <span  data-aos="flip-down" class="input-group-text"><i class="bi bi-lock"></i></span>
            <input type="password" class="form-control" placeholder="Password" id="txtPassword" runat="server" />
        </div>
        <div class="text-center">
            <label>Forgot Password?</label><a href="#" class="text-decoration-none mx-1">Change Password</a>
        </div>
        <div class="my-3 text-center">
            <button type="submit" id="btnSubmit" runat="server" class="btn btn-primary" onserverclick="btnSubmit_ServerClick">Login</button>
        </div>
        <div class="text-center">
            <label>Create an account</label><a href="Register.aspx" class="text-decoration-none mx-1">Sign up here</a>
        </div>
    </div>
</asp:Content>
