<%@ Page Title="" Language="C#" MasterPageFile="~/home.Master" AutoEventWireup="true" CodeBehind="EncyDncy.aspx.cs" Inherits="EmployeeManagementSystem.EncyDncy" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container w-25 borderShadow  centerDiv">
        <h2 class="text-center text-primary"><i class="bi bi-person-lines-fill mx-2"></i>EMS</h2>
        <div class="input-group mb-3">
            <span class="input-group-text"><i class="bi bi-card-text"></i></span>
            <input type="text" class="form-control" placeholder="Decrypted Text" id="txtDecrypted" runat="server" />
        </div>
        <div class="input-group mb-3">
            <span class="input-group-text"><i class="bi bi-wrench-adjustable-circle"></i></span>
            <input type="text" class="form-control" placeholder="Encrypted Text" id="txtEncrypted" runat="server" />
        </div>
        <div class="d-flex justify-content-center my-1">
            <asp:Button ID="btnEncrypt" runat="server" Text="Encrypt" OnClick="btnEncrypt_Click" CssClass="btn btn-success btn-sm mx-1" />
            <asp:Button ID="btnDecrypt" runat="server" Text="Decrypt" OnClick="btnDecrypt_Click" CssClass="btn btn-primary btn-sm mx-1" />
        </div>
    </div>
</asp:Content>
