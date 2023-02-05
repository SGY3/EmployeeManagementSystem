<%@ Page Title="" Language="C#" MasterPageFile="~/home.Master" AutoEventWireup="true" CodeBehind="CreateOrganization.aspx.cs" Inherits="EmployeeManagementSystem.CreateOrganization" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container w-25 borderShadow p-4 centerDiv bg-white">
       <h2 class="text-center text-primary"><i class="bi bi-person-lines-fill mx-2"></i>EMS</h2>
        <div class="input-group mb-3">
            <span class="input-group-text"><i class="bi bi-building"></i></span>
            <input type="text" class="form-control" placeholder="Organization Name" id="txtOrganizationName" runat="server" />
        </div>
        <div class="input-group mb-3">
            <span class="input-group-text"><i class="bi bi-envelope"></i></span>
            <input type="text" class="form-control" placeholder="Organization Email" id="txtOrganizationEmail" runat="server" />
        </div>
        <div class="input-group mb-3">
            <span class="input-group-text"><i class="bi bi-people"></i></span>
            <input type="text" id="txtFirstName" runat="server" class="form-control" placeholder="First Name" />
        </div>
        <div class="input-group mb-3">
            <span class="input-group-text"><i class="bi bi-people"></i></span>
            <input type="text" id="txtLastName" runat="server" class="form-control" placeholder="Last Name" />
        </div>
        <div class="input-group mb-3">
            <span class="input-group-text"><i class="bi bi-phone-fill"></i></span>
            <input type="text" id="txtMobile" runat="server" class="form-control" placeholder="Mobile" />
        </div>
        <div class="my-3 text-center">
            <button type="submit" id="btnSubmit" runat="server" class="btn btn-primary" onserverclick="btnSubmit_ServerClick">Create An Account</button>
        </div>
    </div>
</asp:Content>
