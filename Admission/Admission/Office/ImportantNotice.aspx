<%@ Page Title="Important Notice" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="ImportantNotice.aspx.cs" Inherits="Admission.Admission.Office.ImportantNotice" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <link href="../../Content/formStyle.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="row">
        <div class="col-md-12">
            
            <div class="panel panel-default">
                <div class="panel-heading">
                    <strong>Setup Important Notice</strong>
                </div>
                <div class="panel-body">

                    <asp:Panel ID="messagePanel" runat="server">
                        <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                    </asp:Panel>

                    <table class="table_fullwidth table_form">
                        <tr>
                            <td class="style_td" style="width: 20%">Notice Title</td>
                            <td style="width: 80%">
                                <asp:TextBox ID="txtNoticeTitle" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="txtNoticeTitleReqV" runat="server"
                                    ControlToValidate="txtNoticeTitle" ErrorMessage="*" ForeColor="Crimson"
                                    Font-Size="14pt" Display="Dynamic" ValidationGroup="gr1"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="style_td">Notice Date</td>
                            <td>
                                <asp:TextBox ID="txtNoticeDate" runat="server" placeholder="dd/MM/yyyy"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender ID="CalenderExtender_txtNoticeDate" runat="server"
                                    TargetControlID="txtNoticeDate" Format="dd/MM/yyyy" />
                                <asp:RequiredFieldValidator ID="txtNoticeDate_RequiredFieldValidator" runat="server"
                                    ControlToValidate="txtNoticeDate" ErrorMessage="*" ForeColor="Crimson"
                                    Font-Size="14pt" Display="Dynamic" ValidationGroup="gr1"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="style_td">Notice text</td>
                            <td>
                                <asp:TextBox ID="txtNoticeDetails" runat="server" Width="90%" TextMode="MultiLine"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="txtNoticeDetailsReqV" runat="server"
                                    ControlToValidate="txtNoticeDetails" ErrorMessage="*" ForeColor="Crimson"
                                    Font-Size="14pt" Display="Dynamic" ValidationGroup="gr1"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="style_td">Display date from</td>
                            <td>
                                <asp:TextBox ID="txtDateFrom" runat="server" placeholder="dd/MM/yyyy"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender_txtDateFrom" runat="server"
                                    TargetControlID="txtDateFrom" Format="dd/MM/yyyy" />
                                <asp:RequiredFieldValidator ID="txtDateFrom_RequiredFieldValidator" runat="server"
                                    ControlToValidate="txtDateFrom" ErrorMessage="*" ForeColor="Crimson"
                                    Font-Size="14pt" Display="Dynamic" ValidationGroup="gr1"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="style_td">Display date to</td>
                            <td>
                                <asp:TextBox ID="txtDateTo" runat="server" placeholder="dd/MM/yyyy"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender_txtDateTo" runat="server"
                                    TargetControlID="txtDateTo" Format="dd/MM/yyyy" />
                                <asp:RequiredFieldValidator ID="txtDateTo_RequiredFieldValidator" runat="server"
                                    ControlToValidate="txtDateTo" ErrorMessage="*" ForeColor="Crimson"
                                    Font-Size="14pt" Display="Dynamic" ValidationGroup="gr1"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                    </table>
                    <asp:Button ID="btnSave" Text="Update" runat="server" ValidationGroup="gr1" 
                        OnClick="btnSave_Click"/>
                </div>
                <%-- END PANEL-BODY --%>
            </div>
            <%-- END PANEL-DEFAULT --%>
        </div>
        <%-- END COL-MD_12 --%>
    </div>
    <%-- END ROW --%>

</asp:Content>
