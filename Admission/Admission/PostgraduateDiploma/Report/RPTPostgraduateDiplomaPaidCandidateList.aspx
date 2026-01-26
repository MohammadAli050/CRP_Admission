<%@ Page Title="" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="RPTPostgraduateDiplomaPaidCandidateList.aspx.cs" Inherits="Admission.Admission.PostgraduateDiploma.Report.RPTPostgraduateDiplomaPaidCandidateList" %>



<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <link href="../../../Content/formStyle.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-heading">Postgraduate Diploma Candidate List</div>
                <%--<div class="panel-body">
                    <asp:UpdatePanel ID="updatePanelFilter" runat="server">
                        <ContentTemplate>
                            <table class="table_form table_fullwidth">
                                <tr>
                                    <td class="style_td" style="width: 10%">School/Faculty</td>
                                    <td style="width: 40%">
                                        <asp:DropDownList ID="ddlAdmUnit" runat="server" Width="70%"></asp:DropDownList>
                                        <asp:CompareValidator ID="ddlAdmUnitComV" runat="server" ControlToValidate="ddlAdmUnit"
                                            ValueToCompare="-1" Operator="NotEqual" ErrorMessage="Required" ForeColor="Crimson"
                                            Font-Size="Small" ValidationGroup="gr1"></asp:CompareValidator>
                                    </td>
                                    <td class="style_td style_td_secondCol" style="width: 10%">Session</td>
                                    <td style="width: 40%">
                                        <asp:DropDownList ID="ddlSession" runat="server" Width="70%"></asp:DropDownList>
                                        <asp:CompareValidator ID="ddlSessionComV" runat="server" ControlToValidate="ddlSession"
                                            ValueToCompare="-1" Operator="NotEqual" ErrorMessage="Required" ForeColor="Crimson"
                                            Font-Size="Small" ValidationGroup="gr1"></asp:CompareValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style_td">Education Category</td>
                                    <td>
                                        <asp:DropDownList ID="ddlEduCat" runat="server" Width="70%"></asp:DropDownList>
                                        <asp:CompareValidator ID="ddlEduCatComV" runat="server" ControlToValidate="ddlEduCat"
                                            ValueToCompare="-1" Operator="NotEqual" ErrorMessage="Required" ForeColor="Crimson"
                                            Font-Size="Small" ValidationGroup="gr1"></asp:CompareValidator>
                                    </td>
                                    <td class="style_td style_td_secondCol"></td>
                                    <td></td>
                                </tr>
                            </table>
                            <asp:Button ID="btnLoad" runat="server" Text="Load" ValidationGroup="gr1" OnClick="btnLoad_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>--%>
            </div>
        </div>
    </div>
    <%-- END ROW 1 --%>

    <asp:UpdatePanel ID="updatePanelReport" runat="server">
        <ContentTemplate>
            <asp:Panel ID="messagePanel" runat="server">
                <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
            </asp:Panel>
            <div>
                <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana"
                    Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt"
                    AsyncRendering="true" Width="100%" SizeToReportContent="true" Visible="true">
                    <LocalReport ReportPath="Admission/PostgraduateDiploma/Report/RPTPostgraduateDiplomaPaidCandidateList.rdlc" EnableExternalImages="true">
                    </LocalReport>
                </rsweb:ReportViewer>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>




