<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ImportData.aspx.cs" Inherits="ActivityManagementSystem.ImportData" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="borderShadow">
                <div class="row mb-2">
                    <div class="col-md-2">Select File to Upload</div>
                    <div class="col-md-4">
                        <asp:FileUpload ID="fuFile" accept=".xlsx" runat="server" CssClass="form-control" />
                    </div>
                </div>
                <div class="centerItem">
                    <asp:Button ID="btnUploadFile" runat="server" Text="Upload File" OnClick="btnUploadFile_Click" CssClass="btn btn-success btn-sm" />
                </div>
            </div>


            <!-- Button trigger modal -->
            <button type="button" id="btnModal" class="d-none" data-bs-toggle="modal" data-bs-target="#exampleModal">
                Launch demo modal
            </button>

            <!-- Modal -->
            <div class="modal fade" id="exampleModal" data-bs-backdrop="static" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title" id="exampleModalLabel">Modal title</h5>
                            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                        </div>
                        <div class="modal-body">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <div>
                                        <div class="borderShadow">
                                            <asp:Table ID="tblDynamicControl" runat="server" CssClass="table"></asp:Table>
                                        </div>
                                        <div class="d-flex justify-content-end my-2">
                                            <asp:Button ID="btnImportData" runat="server" Text="Import Data" OnClick="btnImportData_Click" CssClass="btn btn-primary" />
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                        </div>
                    </div>
                </div>
            </div>

        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnUploadFile" />
        </Triggers>
    </asp:UpdatePanel>
    <script>
        function openModal() {
            let button = document.getElementById("btnModal");
            if (button != null) {
                button.click();
            }

        }
    </script>
</asp:Content>
