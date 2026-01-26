<%@ Page Title="Quota" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="Quota.aspx.cs" Inherits="Admission.Admission.Admin.Quota" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">

    <link href="../../Content/formStyle.css" rel="stylesheet" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <strong>Quota Setup</strong>
                </div>
                <div class="panel-body">
                    <asp:UpdatePanel ID="updatePanelfilter" runat="server">
                        <ContentTemplate>
                            <asp:Panel ID="messagePanel" runat="server">
                                <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                            </asp:Panel>

                            <table class="table_form table_fullwidth">
                                <tr>
                                    <td class="style_td" style="width: 10%">Quota Name</td>
                                    <td>
                                        <asp:TextBox ID="txtQuotaName" runat="server" Width="30%"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="txtQuotaNameReqV" runat="server"
                                            ControlToValidate="txtQuotaName" ErrorMessage="Required" ForeColor="Crimson"
                                            Display="Dynamic" ValidationGroup="gr1"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style_td" style="width: 10%">Remarks</td>
                                    <td>
                                        <asp:TextBox ID="txtRemarks" runat="server" Width="30%"
                                            TextMode="MultiLine" placeholder="max 500 characters"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style_td">Is Active?</td>
                                    <td>
                                        <asp:CheckBox ID="ckbxIsActive" runat="server" />
                                    </td>
                                </tr>
                            </table>
                            <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />
                            <asp:Button ID="btnClear" runat="server" Text="Clear" OnClick="btnClear_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <%-- END PANEL-BODY --%>
            </div>
            <%-- END PANEL-DEFAULT --%>
        </div>
        <%-- END COL-MD-12 --%>
    </div>
    <%-- END ROW --%>
    <%-- ------------------------------------------------------------------------------------------ --%>
    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">

                <div class="panel-body">
                    <asp:UpdatePanel ID="updatePanel_QuotaList" runat="server">
                        <ContentTemplate>

                            <div class="row" style="margin-bottom: 1%; padding-left: 1%">
                                <strong>Quota.</strong> Records: &nbsp;
                                <asp:Label ID="lblCount" runat="server" CssClass="badge"></asp:Label>
                            </div>

                            <asp:ListView ID="lvQuotaList" runat="server"
                                OnItemCommand="lvQuotaList_ItemCommand"
                                OnItemDataBound="lvQuotaList_ItemDataBound"
                                OnItemDeleting="lvQuotaList_ItemDeleting"
                                OnItemUpdating="lvQuotaList_ItemUpdating">
                                <LayoutTemplate>
                                    <table class="table table-responsive table-hover"
                                        style="width: 100%; text-align: left;">
                                        <tr>
                                            <th runat="server">SL#</th>
                                            <th runat="server">Quota Name</th>
                                            <th runat="server">Remarks</th>
                                            <th runat="server">Is Active</th>
                                            <th></th>
                                        </tr>
                                        <tr runat="server" id="itemPlaceholder"></tr>
                                    </table>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <tr runat="server">
                                        <td valign="middle" align="left" class="">
                                            <%#Container.DataItemIndex + 1 %>
                                        </td>
                                        <td valign="middle" align="left" class="">
                                            <asp:Label ID="lblQuota" runat="server"></asp:Label>
                                        </td>
                                        <td valign="middle" align="left" class="">
                                            <asp:Label ID="lblRemarks" runat="server"></asp:Label>
                                        </td>
                                        <td valign="middle" align="left" class="">
                                            <asp:Label ID="lblIsActive" runat="server"></asp:Label>
                                        </td>
                                        <td valign="middle" align="left" class="">
                                            <asp:LinkButton ID="lnkEdit" runat="server">Edit</asp:LinkButton>
                                            |
                                            <asp:LinkButton ID="lnkDelete" runat="server"
                                                OnClientClick="return confirm('Are you sure you want to delete?');">
                                                Delete
                                            </asp:LinkButton>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <EmptyDataTemplate>
                                    <div class="alert alert-warning" role="alert">No item to display...</div>
                                </EmptyDataTemplate>
                            </asp:ListView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <%-- END PANEL-BODY --%>
            </div>
            <%-- END PANEL-DEFAULT --%>
        </div>
        <%-- END COL-MD-6 --%>
    </div>
    <%-- END ROW --%>
</asp:Content>
