<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="EmployeeManagementSystem.Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/Dashboard.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container border shadow w-75 rounded p-1">
        <div class="d-flex justify-content-evenly flex-wrap">
            <div class="card cardWidth blueGrad">
                <div class="card-body d-flex flex-column justify-content-center">
                    <div class="countIndicator">
                        <asp:Label ID="lblActive" runat="server" Text="0"></asp:Label>
                    </div>
                    <div class="desLabel">
                        Active Activity
                    </div>
                </div>
            </div>
            <div class="card cardWidth yellowGrad">
                <div class="card-body d-flex flex-column justify-content-center">
                    <div class="countIndicator">
                        <asp:Label ID="lblCompleted" runat="server" Text="0"></asp:Label>
                    </div>
                    <div class="desLabel">
                        Finished Activity
                    </div>
                </div>
            </div>
            <div class="card cardWidth greenGrad">
                <div class="card-body d-flex flex-column justify-content-center">
                    <div class="countIndicator">
                        <asp:Label ID="lblTotal" runat="server" Text="0"></asp:Label>
                    </div>
                    <div class="desLabel">
                        Total Activity
                    </div>
                </div>
            </div>
            <div class="card cardWidth pinkGrad">
                <div class="card-body d-flex flex-column justify-content-center">
                    <div class="countIndicator">
                        <asp:Label ID="lblTodo" runat="server" Text="0"></asp:Label>
                    </div>
                    <div class="desLabel">
                        Todo Added
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="container borderShadow my-3">
        <div class="table-responsive">
            <table id="myTable" class="table" style="width: 100%">
                <thead>
                    <tr>
                        <th>Action</th>
                        <th>Activity Code</th>
                        <th>ToDo Code</th>
                        <th>Project Name</th>
                        <th>Short Title</th>
                        <th>Description</th>
                        <th>Activity Type</th>
                        <th>Page Name</th>
                        <th>SP Name</th>
                        <th>Priority</th>
                        <th>Completion Status</th>
                        <th>Assign To</th>
                        <th>User ID</th>
                        <th>Created By</th>
                        <th>Created On</th>
                    </tr>
                </thead>
                <tbody>
                </tbody>
            </table>
        </div>
        <div class="d-flex justify-content-center">
            <asp:Label ID="lblMessage" runat="server" CssClass="text-center h2 text-warning" Text=""></asp:Label>
        </div>
        <div style="overflow-x: auto; width: 100%">
            <%-- <asp:GridView ID="GvDashboard" runat="server" AutoGenerateColumns="false" CssClass="table table-sm table-hover table-striped table-bordered" AllowPaging="True" OnPageIndexChanging="GvDashboard_PageIndexChanging" PageSize="5">
                <Columns>
                    <asp:BoundField DataField="ActivityCode" HeaderText="Activity Code" ItemStyle-Width="30" />
                    <asp:BoundField DataField="ToDoCode" HeaderText="ToDo Code" ItemStyle-Width="30" />
                    <asp:BoundField DataField="ProjectName" HeaderText="Project Name" ItemStyle-Width="30" />
                    <asp:BoundField DataField="ShortTitle" HeaderText="Short Title" ItemStyle-Width="30" />
                    <asp:BoundField DataField="Description" HeaderText="Description" ItemStyle-Width="30" />
                    <asp:BoundField DataField="ActivityType" HeaderText="Activity Type" ItemStyle-Width="30" />
                    <asp:BoundField DataField="PageName" HeaderText="Page Name" ItemStyle-Width="30" />
                    <asp:BoundField DataField="SpName" HeaderText="Sp Name" ItemStyle-Width="30" />
                    <asp:BoundField DataField="Priority" HeaderText="Priority" ItemStyle-Width="30" />
                    <asp:BoundField DataField="CompletionStatus" HeaderText="Completion Status" ItemStyle-Width="30" />
                    <asp:BoundField DataField="AssignTo" HeaderText="AssignTo" ItemStyle-Width="30" />
                    <asp:BoundField DataField="UserId" HeaderText="UserId" ItemStyle-Width="30" />
                    <asp:BoundField DataField="CreatedBy" HeaderText="Created By" ItemStyle-Width="30" />
                    <asp:BoundField DataField="CreatedOn" HeaderText="Created On" ItemStyle-Width="30" />
                </Columns>
            </asp:GridView>--%>
        </div>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#myTable').DataTable({
                data: <%= GetData() %>,
                columns: [
                    {
                        data: null,
                        render: function (data, type, row) {
                            return '<button type="button" class="btn btn-danger btn-sm" onclick="performAction(\'' + row.ActivityCode + '\')">Delete</button>';
                        }
                    },
                    { data: "ActivityCode" },
                    { data: "ToDoCode" },
                    { data: "ProjectName" },
                    { data: "ShortTitle" },
                    { data: "Description" },
                    { data: "ActivityType" },
                    { data: "PageName" },
                    { data: "SpName" },
                    { data: "Priority" },
                    { data: "CompletionStatus" },
                    { data: "AssignTo" },
                    { data: "UserId" },
                    { data: "CreatedBy" },
                    { data: "CreatedOn" }
                ]
            });
        });
        function performAction(activityCode) {
            // Call a server-side function to perform the necessary action
            $.ajax({
                type: "POST",
                url: "MyPage.aspx/PerformAction",
                data: '{activityCode: "' + activityCode + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    // Handle the response from the server
                },
                error: function (response) {
                    // Handle any errors that occurred
                }
            });
        }
    </script>
</asp:Content>
