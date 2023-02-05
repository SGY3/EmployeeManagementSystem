<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Attendance.aspx.cs" Inherits="ActivityManagementSystem.Attendance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container borderShadow">
        <div class="d-flex justify-content-center">
            <button type="button" class="btn btn-success" id="btnLogin" runat="server" onserverclick="btnLogin_ServerClick">I am ready to work</button>
            <span class="mx-2"></span>
            <button type="button" class="btn btn-danger" id="btnLogout" runat="server" onserverclick="btnLogout_ServerClick">I am out for work</button>
        </div>
    </div>
</asp:Content>
