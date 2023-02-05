<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Progess.aspx.cs" Inherits="EmployeeManagementSystem.Progess" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid borderShadow">
        <div class="row">
            <div class="col-md-4 border p-2">
                <div class="form-floating mb-2">
                    <asp:TextBox ID="txtActivityId" runat="server" CssClass="form-control" Enabled="false" placeholder="Activity Id"></asp:TextBox>
                    <label for="txtActivityId" class="form-label">Activity Id</label>
                </div>
                <div class="form-floating mb-2">
                    <asp:TextBox ID="txtActivityTitleId" runat="server" CssClass="form-control" Enabled="false" placeholder="Activity Title"></asp:TextBox>
                    <label for="txtActivityTitleId" class="form-label">Activity Title</label>
                </div>
                <div class="form-floating mb-2">
                    <asp:DropDownList ID="ddlHour" CssClass="form-select" runat="server">
                        <asp:ListItem>0</asp:ListItem>
                        <asp:ListItem>1</asp:ListItem>
                        <asp:ListItem>2</asp:ListItem>
                        <asp:ListItem>3</asp:ListItem>
                        <asp:ListItem>4</asp:ListItem>
                        <asp:ListItem>5</asp:ListItem>
                        <asp:ListItem>6</asp:ListItem>
                        <asp:ListItem>7</asp:ListItem>
                        <asp:ListItem>8</asp:ListItem>
                        <asp:ListItem>9</asp:ListItem>
                    </asp:DropDownList>
                    <label for="ddlHour" class="form-label">Hour Spend</label>
                </div>
                <div class="form-floating mb-2">
                    <asp:DropDownList ID="ddlMinute" CssClass="form-select" runat="server">
                        <asp:ListItem>0</asp:ListItem>
                        <asp:ListItem>10</asp:ListItem>
                        <asp:ListItem>15</asp:ListItem>
                        <asp:ListItem>20</asp:ListItem>
                        <asp:ListItem>25</asp:ListItem>
                        <asp:ListItem>30</asp:ListItem>
                        <asp:ListItem>35</asp:ListItem>
                        <asp:ListItem>40</asp:ListItem>
                        <asp:ListItem>45</asp:ListItem>
                        <asp:ListItem>50</asp:ListItem>
                        <asp:ListItem>55</asp:ListItem>
                        <asp:ListItem>60</asp:ListItem>
                    </asp:DropDownList>
                    <label for="ddlMinute" class="form-label">Minutes Spend</label>
                </div>
                <div class="form-floating mb-2">
                    <asp:TextBox ID="txtRemark" CssClass="form-control" runat="server" TextMode="MultiLine" Rows="3" placeholder="Remark"></asp:TextBox>
                    <label for="txtRemark" class="form-label">Remark</label>
                </div>
                <div class="form-floating mb-2">
                    <asp:DropDownList ID="ddlCompleteStatus" CssClass="form-select" runat="server">
                        <asp:ListItem Value="Y">Yes</asp:ListItem>
                        <asp:ListItem Selected="True" Value="N">No</asp:ListItem>
                    </asp:DropDownList>
                    <label for="ddlCompleteStatus" class="form-label">Completetion Status</label>
                </div>
                <div class="form-floating mb-2">
                    <asp:TextBox ID="txtDate" CssClass="form-control" runat="server" TextMode="Date" placeholder="Date"></asp:TextBox>
                    <label for="txtDate" class="form-label">Date</label>
                </div>
                <div class="d-flex justify-content-center my-1">
                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" CssClass="btn btn-success btn-sm mx-1" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click" CssClass="btn btn-danger btn-sm mx-1" />
                </div>
            </div>
            <div class="col-md-8">
                <div style="overflow-x: auto; width: 100%">
                    <asp:GridView ID="GvProgress" runat="server" AutoGenerateColumns="false" CssClass="table table-sm table-hover  table-bordered"  AllowPaging="True" OnPageIndexChanging="GvProgress_PageIndexChanging" PageSize="10" >
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlAction" runat="server" OnSelectedIndexChanged="ddlAction_SelectedIndexChanged" AutoPostBack="true" CssClass="form-select" Width="100">
                                        <asp:ListItem Selected="True" Value="00">Action</asp:ListItem>
                                        <asp:ListItem>Add Progress</asp:ListItem>
                                        <asp:ListItem>Attachment</asp:ListItem>
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="ActivityCode" HeaderText="Activity Code" />
                            <asp:BoundField DataField="ToDoCode" HeaderText="ToDo Code" />
                            <asp:BoundField DataField="Priority" HeaderText="Priority" />
                            <asp:BoundField DataField="ProjectName" HeaderText="Project Name" />
                            <asp:BoundField DataField="ShortTitle" HeaderText="Short Title" />
                            <asp:BoundField DataField="Description" HeaderText="Description" />
                            <asp:BoundField DataField="ActivityType" HeaderText="Activity Type" />
                            <asp:BoundField DataField="PageName" HeaderText="Page Name" />
                            <asp:BoundField DataField="SpName" HeaderText="Sp Name" />
                            <asp:BoundField DataField="CompletionStatus" HeaderText="CompletionStatus" Visible="false" />
                            <asp:BoundField DataField="AssignTo" HeaderText="AssignTo" Visible="false" />
                            <asp:BoundField DataField="UserId" HeaderText="UserId" Visible="false" />
                            <asp:BoundField DataField="CreatedBy" HeaderText="CreatedBy" Visible="false" />
                            <asp:BoundField DataField="CreatedOn" HeaderText="CreatedOn" Visible="false" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
