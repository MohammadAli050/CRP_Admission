<%@ Page Title="" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="HD_PrintCandidateAdmitCard.aspx.cs" Inherits="Admission.Admission.HelpDesk.HD_PrintCandidateAdmitCard" %>





<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">

    <link href="../../../Content/formStyle.css" rel="stylesheet" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-md-12">
            <div class="panel-default">

                <div class="panel-heading">
                    <strong>REPORT - Print Admit Card</strong>
                </div>
                <div class="panel-body">
                    <asp:UpdatePanel ID="updatePanelFilter" runat="server" UpdateMode="Always">
                        <ContentTemplate>
                            <asp:Panel ID="messagePanel" runat="server">
                                <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                            </asp:Panel>
                            <table class="table_form table_fullwidth">
                                <tr>
                                    <td style="width: 10%">Mobile Number</td>
                                    <td>
                                        <asp:TextBox ID="txtMobile" runat="server"></asp:TextBox>
                                        <asp:Button ID="btnMobileSearch" runat="server" Text="Load"
                                            OnClick="btnMobileSearch_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>Payment ID</td>
                                    <td>
                                        <asp:TextBox ID="txtPaymentId" runat="server"></asp:TextBox>
                                        <asp:Button ID="btnPaymentIdSearch" runat="server" Text="Load"
                                            OnClick="btnPaymentIdSearch_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 10%">Faculty</td>
                                    <td>
                                        <asp:DropDownList ID="ddlAdmissionUnit" runat="server" OnSelectedIndexChanged="ddlAdmissionUnit_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>

            </div>
        </div>
    </div>

    <asp:UpdatePanel ID="updatePanelAdmitCard" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <div class="row">
                <div class="col-md-12">
                    <asp:Panel ID="messagePanel1" runat="server">
                        <asp:Label ID="lblMessage1" runat="server" Text=""></asp:Label>
                    </asp:Panel>
                    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana"
                        Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt"
                        AsyncRendering="true" Width="100%" SizeToReportContent="true">
                        <LocalReport ReportPath="Admission/HelpDesk/AdmitCard.rdlc" EnableExternalImages="true">
                        </LocalReport>
                    </rsweb:ReportViewer>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
