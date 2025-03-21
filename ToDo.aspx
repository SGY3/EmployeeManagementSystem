﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ToDo.aspx.cs" Inherits="ActivityManagementSystem.ToDo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid borderShadow">
        <div class="row">
            <div class="col-md-4 border p-2">
                <div id="divTodo" runat="server">
                    <div class="form-floating mb-2">
                        <asp:DropDownList ID="ddlProject" CssClass="form-select" runat="server"></asp:DropDownList>
                        <label for="ddlProject" class="form-label">Project</label>
                    </div>
                    <div class="form-floating mb-2">
                        <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control" placeholder="Project Title"></asp:TextBox>
                        <label for="txtTitle" class="form-label">Title</label>
                    </div>
                    <div class="form-floating mb-2">
                        <asp:TextBox ID="txtDescription" CssClass="form-control" Rows="4" TextMode="MultiLine" placeholder="Description" runat="server" Style="height: 100px"></asp:TextBox>

                        <label for="txtDescription" class="form-label">Description</label>
                    </div>
                    <div class="form-floating mb-2">
                        <asp:DropDownList ID="ddlActivityType" CssClass="form-select" runat="server"></asp:DropDownList>
                        <label for="ddlActivityType" class="form-label">Activity Type</label>
                    </div>
                    <div class="form-floating mb-2">
                        <asp:TextBox ID="txtPageName" CssClass="form-control" runat="server" placeholder="Page Name"></asp:TextBox>
                        <label for="txtPageName" class="form-label">Page Name</label>
                    </div>
                    <div class="form-floating mb-2">
                        <asp:TextBox ID="txtSpName" CssClass="form-control" runat="server" placeholder="SP Name"></asp:TextBox>
                        <label for="txtPageName" class="form-label">SP Name</label>
                    </div>
                    <div class="d-flex justify-content-center">
                        <asp:RadioButtonList ID="radP" runat="server" CssClass="form-check" RepeatDirection="Horizontal" CellPadding="5">
                            <asp:ListItem Value="L" Selected="True">Low</asp:ListItem>
                            <asp:ListItem Value="M">Medium</asp:ListItem>
                            <asp:ListItem Value="H">High</asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                    <div class="d-flex justify-content-center my-1">
                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" CssClass="btn btn-success btn-sm mx-1" />
                        <asp:Button ID="btnUpdate" runat="server" Text="Update" OnClick="btnUpdate_Click" Visible="false" CssClass="btn btn-warning btn-sm mx-1" />
                        <asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click" CssClass="btn btn-danger btn-sm mx-1" />
                    </div>
                </div>
                <div id="divAssign" runat="server" visible="false">
                    <div class="form-floating mb-2">
                        <asp:TextBox ID="txtTdoId" CssClass="form-control" runat="server" placeholder="Todo Id" Enabled="false"></asp:TextBox>
                        <label for="txtTdoId" class="form-label">Todo Id</label>
                    </div>
                    <div class="form-floating mb-2">
                        <asp:TextBox ID="txtTodoTitle" CssClass="form-control" runat="server" placeholder="Title" Enabled="false"></asp:TextBox>
                        <label for="txtTodoTitle" class="form-label">Title</label>
                    </div>
                    <div class="form-floating mb-2">
                        <asp:DropDownList ID="ddlEmployee" CssClass="form-select" runat="server"></asp:DropDownList>
                        <label for="ddlEmployee" class="form-label">Select Employee</label>
                    </div>
                    <div class="d-flex justify-content-center">
                        <asp:Button ID="btnTodoAssign" runat="server" Text="Assign" CssClass="btn btn-primary btn-sm mx-1" OnClick="btnTodoAssign_Click" />
                        <asp:Button ID="btnTodoReset" runat="server" Text="Reset" OnClick="btnTodoReset_Click" CssClass="btn btn-danger btn-sm" />
                    </div>
                </div>
            </div>
            <div class="col-md-8">
                <div class="d-flex justify-content-center">
                    <asp:Label ID="lblMessage" runat="server" CssClass="text-center h2 text-warning" Text=""></asp:Label>
                </div>
                <div style="overflow-x: auto; width: 100%">
                    <asp:GridView ID="GvDashboard" runat="server" AutoGenerateColumns="false" CssClass="table table-sm table-hover table-striped table-bordered"  AllowPaging="True" OnPageIndexChanging="GvDashboard_PageIndexChanging" PageSize="8" >
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlAction" runat="server" OnSelectedIndexChanged="ddlAction_SelectedIndexChanged" AutoPostBack="true" CssClass="form-select" Width="100">
                                        <asp:ListItem Selected="True" Value="00">Action</asp:ListItem>
                                        <asp:ListItem>Edit</asp:ListItem>
                                        <asp:ListItem>Attachment</asp:ListItem>
                                        <asp:ListItem>Delete</asp:ListItem>
                                        <asp:ListItem>Assign</asp:ListItem>
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="ToDoCode" HeaderText="ToDo Code" />
                            <asp:BoundField DataField="ActivityCode" HeaderText="Activity Code" />
                            <asp:BoundField DataField="ProjectName" HeaderText="Project Name" />
                            <asp:BoundField DataField="ShortTitle" HeaderText="Short Title" />
                            <asp:BoundField DataField="Description" HeaderText="Description" />
                            <asp:BoundField DataField="ActivityType" HeaderText="Activity Type" />
                            <asp:BoundField DataField="PageName" HeaderText="Page Name" />
                            <asp:BoundField DataField="SpName" HeaderText="Sp Name" />
                            <asp:BoundField DataField="Priority" HeaderText="Priority" />
                            <asp:BoundField DataField="CompletionStatus" HeaderText="CompletionStatus" Visible="false" />
                            <asp:BoundField DataField="AssignTo" HeaderText="AssignTo" Visible="false" />
                            <asp:BoundField DataField="UserId" HeaderText="UserId" Visible="false" />
                            <asp:BoundField DataField="CreatedBy" HeaderText="Created By" />
                            <asp:BoundField DataField="CreatedOn" HeaderText="Created On" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hiddenToDoId" runat="server" Value="" />
</asp:Content>
