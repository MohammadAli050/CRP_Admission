<%@ Page Title="Notice" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="Notice.aspx.cs" Inherits="Admission.Admission.Office.Notice" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">

    <link href="../../Content/formStyle.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h4>Notice Setup</h4>
                </div>
                <div class="panel-body" style="margin-bottom: 3px;">


                    <asp:Panel ID="messagePanel" runat="server">
                        <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                    </asp:Panel>
                    <table style="width: 100%" class="table_form">
                        <tr>
                            <td style="width: 10%" class="style_td">Notice Title</td>
                            <td style="width: 90%">
                                <asp:TextBox ID="txtNoticeTitle" runat="server" Width="90%" placeholder="max 1000 characters"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="txtNoticeTitleReqV" runat="server"
                                    ControlToValidate="txtNoticeTitle" ErrorMessage="Required" ForeColor="Crimson"
                                    Font-Size="14pt" Display="Dynamic" ValidationGroup="gr1"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="style_td">Notice Date</td>
                            <td>
                                <asp:TextBox ID="txtNoticeDate" runat="server" Width="30%"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server"
                                    TargetControlID="txtNoticeDate" Format="dd/MM/yyyy" />
                                <asp:RequiredFieldValidator ID="txtNoticeDateReqV" runat="server"
                                    ControlToValidate="txtNoticeDate" ErrorMessage="Required" ForeColor="Crimson"
                                    Font-Size="14pt" Display="Dynamic" ValidationGroup="gr1"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="style_td">Notice Details</td>
                            <td>
                                <asp:TextBox ID="txtNoticeDetails" runat="server" Width="90%" TextMode="MultiLine" placeholder="max 3000 characters"></asp:TextBox>
                                <%--<asp:RequiredFieldValidator ID="txtNoticeDetailsReqV" runat="server"
                                    ControlToValidate="txtNoticeDetails" ErrorMessage="Required" ForeColor="Crimson"
                                    Font-Size="14pt" Display="Dynamic" ValidationGroup="gr1"></asp:RequiredFieldValidator>--%>
                            </td>
                        </tr>
                        <tr style="display: none">
                            <td class="style_td">From Date</td>
                            <td>
                                <asp:TextBox ID="txtFromDate" runat="server"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server"
                                    TargetControlID="txtFromDate" Format="dd/MM/yyyy" />
                            </td>
                        </tr>
                        <tr style="display: none">
                            <td class="style_td">To Date</td>
                            <td>
                                <asp:TextBox ID="txtToDate" runat="server"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender3" runat="server"
                                    TargetControlID="txtToDate" Format="dd/MM/yyyy" />
                            </td>
                        </tr>
                        <tr >
                            <td class="style_td">Upload File</td>
                            <td>
                                <asp:FileUpload ID="fuAttachment" runat="server"/>
                            </td>
                        </tr>
                        <%--<tr>
                            <td class="style_td">External Url</td>
                            <td>
                                <asp:TextBox ID="txtExternalUrl" runat="server" Width="90%" placeholder="max 1500 characters"></asp:TextBox>
                            </td>
                        </tr>--%>
                        <%--<tr style="display: none">
                            <td class="style_td">Extra Attachment</td>
                            <asp:FileUpload ID="fuAttachmentExtra" runat="server" Visible="false" />
                        </tr>--%>
                        <tr>
                            <td class="style_td">Is Active</td>
                            <td>
                                <asp:CheckBox ID="ckbxIsActive" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Button ID="btnSave" runat="server" Text="Save" ValidationGroup="gr1" OnClick="btnSave_Click" />
                                <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btnClearAdmin" OnClick="btnClear_Click" />
                            </td>
                        </tr>
                    </table>


                </div>
                <%-- END PANEL-BODY --%>
            </div>
            <%-- END PANEL-DEFAULT --%>
        </div>
        <%-- END COL-MD-12 --%>
    </div>
    <%-- END ROW 1--%>
    <%-- ---------------------------------------------------------------------------------------------------------------------------------- --%>

    <%--<asp:UpdatePanel ID="updatePanelListView" runat="server">
        <ContentTemplate>--%>
            <div class="row">
                <div class="col-md-12">
                    <div class="panel panel-default">
                        <div class="panel-body" style="margin-bottom: -25px;">
                            <div class="row" style="margin-bottom: 1%;">
                                Records: &nbsp;
                                <asp:Label ID="lblCount" runat="server" CssClass="badge"></asp:Label>
                            </div>
                            <asp:ListView ID="lvNotices" runat="server"
                                OnItemDataBound="lvNotices_ItemDataBound"
                                OnItemCommand="lvNotices_ItemCommand"
                                OnItemDeleting="lvNotices_ItemDeleting"
                                OnItemUpdating="lvNotices_ItemUpdating">
                                <LayoutTemplate>
                                    <table id="tblNotices"
                                        class="table table-hover table-condensed table-striped"
                                        style="width: 100%; text-align: center">
                                        <tr runat="server" style="background-color: #1387de; color: white;">
                                            <th runat="server">SL#</th>
                                            <th runat="server">Notice Title</th>
                                            <th runat="server">Notice Date</th>
                                            <%--<th runat="server">Attachment
                                                <br />
                                                Available?
                                            </th>--%>
                                            <th runat="server">External Is
                                                <br />
                                                Available?</th>
                                            <th runat="server">Is Active</th>
                                            <th></th>
                                        </tr>
                                        <tr runat="server" id="itemPlaceholder" />
                                    </table>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <tr runat="server">
                                        <td valign="middle" align="left" class="">
                                            <asp:Label ID="lblSerial" runat="server" 
                                                Text='<%# Container.DataItemIndex + 1 %>'/>.
                                        </td>
                                        <td valign="middle" align="left" class="">
                                            <asp:Label ID="lblNoticeTitle" runat="server" />
                                        </td>
                                        <td valign="middle" align="left" class="">
                                            <asp:Label ID="lblNoticeDate" runat="server" />
                                        </td>
                                        <%--<td valign="middle" align="left" class="">
                                            <asp:Label ID="lblIsAttachmentAvailable" runat="server" />
                                        </td>--%>
                                        <td valign="middle" align="left" class="">
                                            <asp:Label ID="lblIsExternalUrlAvailable" runat="server" />
                                        </td>
                                        <td valign="middle" align="left" class="">
                                            <asp:Label ID="lblIsActive" runat="server" />
                                        </td>
                                        <td valign="middle" align="right" class="">
                                            <asp:LinkButton CssClass="" ID="lnkEdit" runat="server">Edit</asp:LinkButton>
                                            |
                                            <asp:LinkButton CssClass="" ID="lnkDelete"
                                                OnClientClick="return confirm('Confirm you want to Delete?');"
                                                runat="server">Delete</asp:LinkButton>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <EmptyDataTemplate>
                                    <div class="alert alert-warning" role="alert" style="text-align: center">No item to display.</div>
                                </EmptyDataTemplate>
                            </asp:ListView>
                            <%--</ContentTemplate>
                        </asp:UpdatePanel>--%>
                        </div>
                        <%-- END PANEL-BODY --%>
                    </div>
                    <%-- END PANEL-DAFAULT --%>
                </div>
                <%-- END COL-MD-12 --%>
            </div>
            <%-- END ROW 2 --%>
            <%-- ---------------------------------------------------------------------------------------------------------------------------------- --%>
        <%--</ContentTemplate>
    </asp:UpdatePanel>--%>

</asp:Content>
