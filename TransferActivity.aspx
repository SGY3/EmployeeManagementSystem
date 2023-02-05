<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TransferActivity.aspx.cs" Inherits="EmployeeManagementSystem.TransferActivity" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container borderShadow">
        <div class="row">
            <div class="col-md-4">
                <div>
                    <div class="form-floating mb-2">
                        <asp:TextBox ID="txtActivityId" CssClass="form-control" runat="server" placeholder="Activity Id" Enabled="false"></asp:TextBox>
                        <label for="txtActivityId" class="form-label">Activity Id</label>
                    </div>
                    <div class="form-floating mb-2">
                        <asp:TextBox ID="txtActivityTitle" CssClass="form-control" runat="server" placeholder="Title" Enabled="false"></asp:TextBox>
                        <label for="txtActivityTitle" class="form-label">Title</label>
                    </div>
                    <div class="form-floating mb-2">
                        <asp:DropDownList ID="ddlEmployee" CssClass="form-select" runat="server"></asp:DropDownList>
                        <label for="ddlEmployee" class="form-label">Select Employee</label>
                    </div>
                    <div class="d-flex justify-content-center">
                        <asp:Button ID="btnTransfer" runat="server" Text="Assign" CssClass="btn btn-primary btn-sm mx-1" OnClick="btnTransfer_Click" />
                        <asp:Button ID="btnActivityReset" runat="server" Text="Reset" OnClick="btnActivityReset_Click" CssClass="btn btn-danger btn-sm" />
                    </div>
                </div>
            </div>
            <div class="col-md-8">
                <div class="d-flex justify-content-center">
                    <asp:Label ID="lblMessage" runat="server" CssClass="text-center h2 text-warning" Text=""></asp:Label>
                </div>
                <div style="overflow-x: auto; width: 100%">
                    <asp:GridView ID="GvDashboard" runat="server" AutoGenerateColumns="false" CssClass="table table-sm table-hover table-striped table-bordered"  AllowPaging="True" OnPageIndexChanging="GvDashboard_PageIndexChanging" PageSize="6" >
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlAction" runat="server" OnSelectedIndexChanged="ddlAction_SelectedIndexChanged" AutoPostBack="true" CssClass="form-select" Width="100">
                                        <asp:ListItem Selected="True" Value="00">Action</asp:ListItem>
                                        <asp:ListItem>Transfer</asp:ListItem>
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>
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
    </div>
</asp:Content>
