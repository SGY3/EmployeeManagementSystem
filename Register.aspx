<%@ Page Title="" Language="C#" MasterPageFile="~/home.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="ActivityManagementSystem.Register" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container w-25 borderShadow p-4 centerDiv bg-white">
         <h2 class="text-center text-primary"><i class="bi bi-person-lines-fill mx-2"></i>AMS</h2>
        <div class="input-group mb-3">
            <span class="input-group-text"><i class="bi bi-building"></i></span>
            <input type="text" class="form-control" placeholder="Organization Id" id="txtOrganizationId" runat="server" />
        </div>
        <div class="form-floating mb-3">
            <input type="text" id="txtFirstName" runat="server" class="form-control" placeholder="First Name" />
            <label for="txtFirstName">First Name</label>
        </div>
        <div class="form-floating mb-3">
            <input type="text" id="txtLastName" runat="server" class="form-control" placeholder="Last Name" />
            <label for="txtLastName">Last Name</label>
        </div>
        <div class="form-floating mb-3">
            <input type="text" id="txtEmail" runat="server" class="form-control" placeholder="Email" />
            <label for="txtEmail">Email</label>
        </div>
        <div class="form-floating mb-3">
            <input type="text" id="txtMobile" runat="server" class="form-control" placeholder="Mobile" />
            <label for="txtMobile">Mobile</label>
        </div>
        <div class="my-3 text-center">
            <button type="submit" id="btnSubmit" runat="server" class="btn btn-primary" onserverclick="btnSubmit_ServerClick">Sign Up</button>
        </div>
        <div class="text-center">
            <label>Already have account</label><a href="Login.aspx" class="text-decoration-none">Login here</a>
        </div>
    </div>
</asp:Content>
