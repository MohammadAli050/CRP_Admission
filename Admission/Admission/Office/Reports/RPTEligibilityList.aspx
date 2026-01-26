<%@ Page Title="" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="RPTEligibilityList.aspx.cs" Inherits="Admission.Admission.Office.Reports.RPTEligibilityList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <link href="../../../Content/formStyle.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <strong>REPORT - Eligibility List</strong>
                </div>
                <div class="panel-body">
                    <asp:UpdatePanel ID="updatePanelFilter" runat="server">
                        <ContentTemplate>
                            <asp:Panel ID="messagePanel" runat="server">
                                <asp:Label ID="lblMessage" runat="server"></asp:Label>
                            </asp:Panel>
                            <table class="table_form table_fullwidth">
                                <tr>
                                    <td class="style_td" style="width: 10%">Faculty/Program</td>
                                    <td style="width: 40%">
                                        <asp:DropDownList ID="ddlAdmUnit" runat="server" Width="70%"
                                            OnSelectedIndexChanged="ddlAdmUnit_SelectedIndexChanged" AutoPostBack="true">
                                        </asp:DropDownList>
                                        <asp:CompareValidator ID="ddlAdmUnitComV" runat="server" ControlToValidate="ddlAdmUnit"
                                            ValueToCompare="-1" Display="Dynamic" ErrorMessage="Required" ForeColor="Crimson"
                                            Font-Size="Medium" Operator="NotEqual" ValidationGroup="gr1"></asp:CompareValidator>
                                    </td>
                                    <td class="style_td style_td_secondCol" style="width: 10%">Session</td>
                                    <td style="width: 40%">
                                        <asp:DropDownList ID="ddlSession" runat="server" Width="70%"></asp:DropDownList>
                                        <asp:CompareValidator ID="ddlSessionComV" runat="server" ControlToValidate="ddlSession"
                                            ValueToCompare="-1" Display="Dynamic" ErrorMessage="Required" ForeColor="Crimson"
                                            Font-Size="Medium" Operator="NotEqual" ValidationGroup="gr1"></asp:CompareValidator>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <asp:Button ID="btnLoad" runat="server" Text="Load"
                                ValidationGroup="gr1" OnClick="btnLoad_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
    <%-- ---------------------------------------------------------------------------------------------------- --%>

    <asp:UpdatePanel ID="updatePanel_Report" runat="server">
        <ContentTemplate>
            <div class="row">
                <%--<rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana"
                    Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt"
                    AsyncRendering="true" Width="100%" SizeToReportContent="true" Visible="true">
                    <LocalReport ReportPath="Admission/Office/Reports/RptAttendanceRoomWise.rdlc" EnableExternalImages="true">
                    </LocalReport>
                </rsweb:ReportViewer>--%>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
