<%@ Page Title="" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="RPTDetailsForVivaUndergraduate.aspx.cs" Inherits="Admission.Admission.Office.Reports.RPTDetailsForVivaUndergraduate" %>



<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <link href="../../../Content/formStyle.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <strong>REPORT - Details for Viva (Bachelors)</strong>
                </div>
                <div class="panel-body">
                    <asp:Panel ID="messagePanel" runat="server">
                        <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                    </asp:Panel>

                    <table class="table_form">
                        <tr>
                            <td style="width: 10%">Program</td>
                            <td style="width: 40%">
                                <asp:DropDownList ID="ddlAdmUnit" runat="server"></asp:DropDownList>
                                <asp:CompareValidator ID="ddlAdmUnitComV" runat="server" ControlToValidate="ddlAdmUnit"
                                    ValueToCompare="-1" Operator="NotEqual" ErrorMessage="Required" ForeColor="Crimson" Display="Dynamic"
                                    ValidationGroup="gr1"></asp:CompareValidator>
                            </td>
                            <td style="width: 10%">Session</td>
                            <td style="width: 40%">
                                <asp:DropDownList ID="ddlSession" runat="server"></asp:DropDownList>
                                <asp:CompareValidator ID="ddlSessionComV" runat="server" ControlToValidate="ddlSession"
                                    ValueToCompare="-1" Operator="NotEqual" ErrorMessage="Required" ForeColor="Crimson" Display="Dynamic"
                                    ValidationGroup="gr1"></asp:CompareValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Total Marks: <asp:TextBox ID="txtTotalMarks" runat="server" TextMode="Number"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="txtTotalMarksReqV" runat="server"
                                    ControlToValidate="txtTotalMarks" ErrorMessage="Required" ForeColor="Crimson" Display="Dynamic"
                                    ValidationGroup="gr1"></asp:RequiredFieldValidator>
                            </td>
                            <td>Upload File (Excel)<asp:FileUpload runat="server" ID="fuExcel" />
                                &nbsp; &nbsp;
                            </td>
                            <td>
                                <asp:Button runat="server" ID="btnUpload" Text="Upload" ValidationGroup="gr1"
                                    OnClick="btnUpload_Click"/>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:Label ID="lblUpl" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:DropDownList ID="ddlDownloadMode" runat="server" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlDownloadMode_SelectedIndexChanged"></asp:DropDownList>
                            </td>
                            <td>
                                Total number: <asp:TextBox ID="txtTotalNumber" runat="server" TextMode="Number"></asp:TextBox>
                            </td>
                            <td></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button runat="server" ID="btnDownload" Text="View" OnClick="btnDownload_Click" /><br />
                                <asp:Button runat="server" ID="btnExportToExcel" Text="Direct Excel Export" Visible="true"
                                    OnClick="btnExportToExcel_Click" />
                            </td>
                            <td></td>
                            <td></td>
                            <td></td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>

    <div class="row">
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana"
            Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt"
            AsyncRendering="true" Width="100%" SizeToReportContent="true" Visible="false">
            <LocalReport ReportPath="Admission/Office/Reports/RptDetailsForVivaUndergraduate.rdlc" EnableExternalImages="true"></LocalReport>
        </rsweb:ReportViewer>
    </div>
    

</asp:Content>
