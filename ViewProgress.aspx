<%@ Page Title="" Language="C#" MasterPageFile="~/Empty.Master" AutoEventWireup="true" CodeBehind="ViewProgress.aspx.cs" Inherits="EmployeeManagementSystem.ViewProgress" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container borderShadow">
        <div class="input-group mb-3">
            <span class="input-group-text">Activity Id</span>
            <input type="text" class="form-control" placeholder="ActivityId" readonly id="txtActivityId" runat="server" />
        </div>
        <div>
            <div class="d-flex justify-content-center">
                <asp:Label ID="lblMessage" runat="server" CssClass="text-center h2 text-warning" Text=""></asp:Label>
            </div>
             <div style="overflow-x: auto; width: 100%">
                    <asp:GridView ID="GvViewProgress" runat="server" AutoGenerateColumns="false" OnRowCommand="GvViewProgress_RowCommand" CssClass="table table-sm table-hover table-striped table-bordered"  AllowPaging="True" OnPageIndexChanging="GvViewProgress_PageIndexChanging" PageSize="10" >
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Button Text="View Atttachment" runat="server" CommandName="View" CommandArgument="<%# Container.DataItemIndex %>" CssClass="btn btn-primary btn-sm"/>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="ActivityTitle" HeaderText="Activity Title" />
                            <asp:BoundField DataField="HourSpend" HeaderText="Hour Spend" />
                            <asp:BoundField DataField="MinuteSpend" HeaderText="Minute Spend" />
                            <asp:BoundField DataField="Remark" HeaderText="Remark" />
                            <asp:BoundField DataField="CompletionYN" HeaderText="Completion Status" />
                            <asp:BoundField DataField="Date" HeaderText="Date" />
                            <asp:BoundField DataField="ActivityId" HeaderText="ActivityId" ItemStyle-CssClass="d-none"  HeaderStyle-CssClass="d-none" />
                        </Columns>
                    </asp:GridView>
                </div>
        </div>
    </div>
</asp:Content>
