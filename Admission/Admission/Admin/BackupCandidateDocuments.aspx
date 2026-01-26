<%@ Page Title="Back Up Candidate Documents" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="BackupCandidateDocuments.aspx.cs" Inherits="Admission.Admission.Office.BackupCandidateDocuments" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">

    <link href="../../Content/formStyle.css" rel="stylesheet" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <strong>Back Up Candidate Documents</strong>
                </div>
                <div class="panel-body">
                    <div style="color:orange">
                        Note: If date range is not selected, all the files in the directory will be compressed and downloaded. 
                        If date range is selected, both the dates (from - to) must be provided.
                    </div>
                    <table class="table_fullwidth table_form">
                        <tr>
                            <td>
                                Type:&nbsp;&nbsp;
                                <asp:DropDownList ID="ddlSelectDir" runat="server"></asp:DropDownList>
                                <asp:CompareValidator ID="ddlSelectDir_ComV" runat="server"
                                    ControlToValidate="ddlSelectDir" ErrorMessage="Required" Font-Size="14pt" Font-Bold="true"
                                    ForeColor="Crimson" Display="Dynamic" ValueToCompare="-1" Operator="NotEqual"
                                    ValidationGroup="gr1"></asp:CompareValidator>
                                &nbsp;&nbsp;
                            </td>
                            <td>
                                Date From:&nbsp;&nbsp;
                                <asp:TextBox ID="txtDateFrom" runat="server"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server"
                                    TargetControlID="txtDateFrom"/>
                                &nbsp;&nbsp;
                            </td>
                            <td>
                                Date To:&nbsp;&nbsp;
                                <asp:TextBox ID="txtDateTo" runat="server"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server"
                                    TargetControlID="txtDateTo"/>
                                &nbsp;&nbsp;
                            </td>
                            <td>
                                <asp:Button ID="btnSave" Text="Download" runat="server" ValidationGroup="gr1"
                                    OnClick="btnSave_Click"/>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblMessage" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
                <%-- END PANEL-BODY --%>
            </div>
            <%-- END PANEL-DEFAULT --%>
        </div>
        <%-- END COL-MD_12 --%>
    </div>
    <%-- END ROW --%>
</asp:Content>
