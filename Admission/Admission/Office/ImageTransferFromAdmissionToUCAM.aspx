<%@ Page Title="" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="ImageTransferFromAdmissionToUCAM.aspx.cs" Inherits="Admission.Admission.Office.ImageTransferFromAdmissionToUCAM" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    
    <div class="row">
        <div class="col-lg-12">
            <asp:UpdatePanel ID="updatePanelMassage" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="messagePanel" runat="server">
                        <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
  



     <asp:UpdatePanel ID="updatePanelAll" runat="server">
        <ContentTemplate>

            <div class="row">
                <div class="col-sm-12 col-md-12 col-lg-12">
                    <h3>Approve Candidate</h3>
                    <%--<hr style="margin-top: 0px; margin-bottom: 10px;" />--%>
                </div>
            </div>

            <div class="panel panel-default">
                <div class="panel-body">
                    <div class="row">
                        <div class="col-sm-4 col-md-4 col-lg-4">
                            <div class="form-group">
                                <label><strong>Faculty <span style="color: crimson; font-weight: bold;">*</span></strong></label>
                                <asp:DropDownList ID="ddlFaculty" runat="server" Width="100%"></asp:DropDownList>
                                <asp:CompareValidator ID="CompareValidator2" runat="server" Display="Dynamic"
                                    ErrorMessage="Required" ForeColor="Crimson" ControlToValidate="ddlFaculty"
                                    Font-Size="10pt"
                                    ValueToCompare="-1" Operator="NotEqual" ValidationGroup="loadVg"></asp:CompareValidator>
                            </div>
                        </div>
                        <div class="col-sm-4 col-md-4 col-lg-4">
                            <div class="form-group">
                                <label><strong>Session <span style="color: crimson; font-weight: bold;">*</span></strong></label>
                                <asp:DropDownList ID="ddlSession" runat="server" Width="100%"></asp:DropDownList>
                                <asp:CompareValidator ID="ddlSessionComV" runat="server" Display="Dynamic"
                                    ErrorMessage="Required" ForeColor="Crimson" ControlToValidate="ddlSession"
                                    Font-Size="10pt"
                                    ValueToCompare="-1" Operator="NotEqual" ValidationGroup="loadVg"></asp:CompareValidator>
                            </div>
                        </div>
                        <div class="col-sm-4 col-md-4 col-lg-4">
                            <div class="form-group">
                                <label><strong>Document Type <span style="color: crimson; font-weight: bold;">*</span></strong></label>
                                <asp:DropDownList ID="ddlDocumentType" runat="server" Width="100%"></asp:DropDownList>
                                <asp:CompareValidator ID="CompareValidator1" runat="server" Display="Dynamic"
                                    ErrorMessage="Required" ForeColor="Crimson" ControlToValidate="ddlDocumentType"
                                    Font-Size="10pt"
                                    ValueToCompare="-1" Operator="NotEqual" ValidationGroup="loadVg"></asp:CompareValidator>
                            </div>
                        </div>
                    </div>


                    <asp:Button ID="btnTransferDocument" runat="server" Text="Transfer Document" CssClass="btn btn-info"
                        Style="width: 100%; margin-top: 4px;"
                        ValidationGroup="loadVg" OnClick="btnTransferDocument_Click" />

                </div>
            </div>


        </ContentTemplate>
    </asp:UpdatePanel>



</asp:Content>
