<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SeeAllProgress.aspx.cs" Inherits="EmployeeManagementSystem.SeeAllProgress" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container borderShadow">
        <div style="overflow-x: auto; width: 100%">
            <asp:GridView ID="GvAllProgress" runat="server" Width="100%" AutoGenerateColumns="false" CssClass="table table-sm table-hover table-striped table-bordered"  AllowPaging="True" OnPageIndexChanging="GvAllProgress_PageIndexChanging" PageSize="10" >
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlAction" runat="server" OnSelectedIndexChanged="ddlAction_SelectedIndexChanged" AutoPostBack="true" CssClass="form-select">
                                <asp:ListItem Selected="True" Value="00">Action</asp:ListItem>
                                <asp:ListItem>View</asp:ListItem>
                            </asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="ActivityCode" HeaderText="Activity Code" />
                    <asp:BoundField DataField="ToDoCode" HeaderText="ToDo Code" />
                    <asp:BoundField DataField="ProjectName" HeaderText="Project Name" />
                    <asp:BoundField DataField="ShortTitle" HeaderText="Short Title" />
                    <asp:BoundField DataField="Description" HeaderText="Description" />
                    <asp:BoundField DataField="ActivityType" HeaderText="Activity Type" />
                    <asp:BoundField DataField="PageName" HeaderText="Page Name" />
                    <asp:BoundField DataField="SpName" HeaderText="Sp Name" />
                    <asp:BoundField DataField="CompletionStatus" HeaderText="Completion Status" />
                    <asp:BoundField DataField="EditedOn" HeaderText="Edited On" Visible="false" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
