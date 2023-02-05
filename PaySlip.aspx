<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PaySlip.aspx.cs" Inherits="ActivityManagementSystem.PaySlip" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container borderShadow">
        <button type="button" class="btn btn-info mx-2" id="btnJan" runat="server" onserverclick="btnJan_ServerClick">JAN -2022</button>
        <button type="button" class="btn btn-info mx-2" id="btnFeb" runat="server" onserverclick="btnJan_ServerClick">FEB -2022</button>
        <button type="button" class="btn btn-info mx-2" id="btnMar" runat="server" onserverclick="btnJan_ServerClick">MAR -2022</button>
        <button type="button" class="btn btn-info mx-2" id="btnApr" runat="server" onserverclick="btnJan_ServerClick">APR -2022</button>
        <button type="button" class="btn btn-info mx-2" id="btnMay" runat="server" onserverclick="btnJan_ServerClick">MAY -2022</button>
        <button type="button" class="btn btn-info mx-2" id="btnJun" runat="server" onserverclick="btnJan_ServerClick">JUN -2022</button>
    </div>
</asp:Content>
