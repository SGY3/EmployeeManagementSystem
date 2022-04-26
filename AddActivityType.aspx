<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddActivityType.aspx.cs" Inherits="ActivityManagementSystem.AddActivityType" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid borderShadow">
        <div class="row">
            <div class="col-md-4">
                <div class="form-floating mb-2">
                    <asp:TextBox ID="txtActivityName" runat="server" CssClass="form-control" placeholder="Activity Name"></asp:TextBox>
                    <label for="txtActivityName" class="form-label">Activity Name</label>
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
                    <asp:GridView ID="GvActivityType" runat="server" AutoGenerateColumns="false" OnRowCommand="GvActivityType_RowCommand" CssClass="table table-sm table-hover table-bordered"  AllowPaging="True" OnPageIndexChanging="GvActivityType_PageIndexChanging" PageSize="10" >
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Button Text="Edit" runat="server" CommandName="EditC" CommandArgument="<%# Container.DataItemIndex %>" CssClass="btn btn-primary btn-sm"/>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="ActivityTypeId" HeaderText="Activity Type Id" />
                            <asp:BoundField DataField="ActivityTypeName" HeaderText="Activity Type Name" />
                            <asp:BoundField DataField="IsActive" HeaderText="Active" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hiddenActivityTypeId" runat="server" />
</asp:Content>
