<%@ Page Title="" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="RPTCampusWiseSeatPlanAllCampus.aspx.cs" Inherits="Admission.Admission.Office.Reports.RPTCampusWiseSeatPlanAllCampus" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
        <link href="../../Content/formStyle.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    
    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h4>Campus Wise Seat Plan All Campus</h4>
                </div>
                <div class="panel-body">
                    <asp:UpdatePanel ID="updatePanel_Filter" runat="server" UpdateMode="Always">
                        <ContentTemplate>

                            <asp:Panel ID="messagePanel" runat="server">
                                <asp:Label ID="lblMessage" runat="server"></asp:Label>
                            </asp:Panel>
                            <table class="table_form table_fullwidth">
                                <tr>
                                    <td class="style_td" style="width: 10%">Faculty/Program</td>
                                    <td style="width: 10%">
                                        <asp:DropDownList ID="ddlAdmUnit" runat="server"></asp:DropDownList>
                                        <asp:CompareValidator ID="ddlAdmissionUnitCompare" runat="server"
                                            ControlToValidate="ddlAdmUnit" ErrorMessage="Required" Font-Size="10pt" Font-Bold="true"
                                            ForeColor="Crimson" Display="Dynamic" ValueToCompare="-1" Operator="NotEqual"
                                            ValidationGroup="gr1"></asp:CompareValidator>
                                    </td>
                                    <td class="style_td" style="width: 10%">Session</td>
                                    <td style="width: 15%">
                                        <asp:DropDownList ID="ddlSession" runat="server"></asp:DropDownList>
                                        <asp:CompareValidator ID="ddlSessionComV" runat="server"
                                            ControlToValidate="ddlSession" ErrorMessage="Required" Font-Size="10pt" Font-Bold="true"
                                            ForeColor="Crimson" Display="Dynamic" ValueToCompare="-1" Operator="NotEqual"
                                            ValidationGroup="gr1"></asp:CompareValidator>
                                    </td>
                                    <td class="style_td" style="width: 10%">Campus</td>
                                    <td style="width: 20%">
                                        <asp:DropDownList ID="ddlCampus" runat="server"></asp:DropDownList>
                                        <asp:CompareValidator ID="ddlCampusComV" runat="server"
                                            ControlToValidate="ddlCampus" ErrorMessage="Required" Font-Size="10pt" Font-Bold="true"
                                            ForeColor="Crimson" Display="Dynamic" ValueToCompare="-1" Operator="NotEqual"
                                            ValidationGroup="gr1"></asp:CompareValidator>
                                    </td>
                                    <td style="width: 10%; text-align: right">
                                        <asp:Button ID="btnLoad" runat="server" Text="Load"
                                            ValidationGroup="gr1"
                                            OnClick="btnLoad_Click" />&nbsp;&nbsp;
                                    </td>
                                </tr>
                            </table>

                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <asp:UpdatePanel ID="updatePanel_Report" runat="server">
        <ContentTemplate>
            <div class="row">
                <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana"
                    Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt"
                    AsyncRendering="true" Width="100%" SizeToReportContent="true" Visible="true">
                    <LocalReport ReportPath="Admission/Office/Reports/RPTCampusWiseSeatPlanAllCampus.rdlc" EnableExternalImages="true">
                    </LocalReport>
                </rsweb:ReportViewer>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>


</asp:Content>
