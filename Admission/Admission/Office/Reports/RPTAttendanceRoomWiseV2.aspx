<%@ Page Title="" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="RPTAttendanceRoomWiseV2.aspx.cs" Inherits="Admission.Admission.Office.Reports.RPTAttendanceRoomWiseV2" %>





<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <link href="../../../Content/formStyle.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <strong>REPORT - Building wise Attendance</strong>
                </div>
                <div class="panel-body">
                    <asp:UpdatePanel ID="updatePanel_Filter" runat="server">
                        <ContentTemplate>
                            <asp:Panel ID="messagePanel" runat="server">
                                <asp:Label ID="lblMessage" runat="server"></asp:Label>
                            </asp:Panel>
                            <table class="table_form table_fullwidth">
                                <tr>
                                    <td class="style_td" style="width: 5%">Faculty/Program</td>
                                    <td style="width: 20%">
                                        <asp:DropDownList ID="ddlAdmUnit" runat="server" Width="80%" ></asp:DropDownList>
                                    </td>
                                    <td class="style_td" style="width: 5%">Session</td>
                                    <td style="width: 20%">
                                        <asp:DropDownList ID="ddlSession" runat="server" Width="80%" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlSession_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="style_td" style="width: 5%">Buildings</td>
                                    <td style="width: 20%">
                                        <asp:DropDownList ID="ddlBuilding" runat="server" Width="80%"></asp:DropDownList>
                                        <%--<asp:DropDownList ID="ddlRoom" runat="server" Width="80%"></asp:DropDownList>--%>
                                    </td>
                                    <td style="width: 25%">
                                        <asp:Button ID="btnLoad" runat="server" Text="Load" OnClick="btnLoad_Click" />
                                    </td>
                                    <td style="width: 25%">
                                        <asp:Button ID="btnDownload" runat="server" Text="Download" OnClick="btnDownload_Click" />
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
                    <LocalReport ReportPath="Admission/Office/Reports/RPTAttendanceRoomWiseV3.rdlc" EnableExternalImages="true">
                    </LocalReport>
                </rsweb:ReportViewer>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>


</asp:Content>