<%@ Page Title="" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="StoreSetup.aspx.cs" Inherits="Admission.Admission.Admin.StoreSetup" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h4>Store Setup</h4>
                </div>
                <div class="panel-body" style="margin-bottom: -30px;">

                    <asp:Panel ID="messagePanel" runat="server">
                        <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                    </asp:Panel>

                    <table style="width: 100%" class="table table-hover table-condensed">
                        <tr>
                            <td style="text-align: left; width: 15%; font-weight: bold">Store Name</td>
                            <td style="text-align: left; width: 35%">
                                <asp:TextBox ID="txtStoreName" runat="server" Width="90%"></asp:TextBox>
                            </td>
                            <td style="text-align: left; width: 50%">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                    ControlToValidate="txtStoreName" ErrorMessage="Store name is required." ForeColor="Crimson"
                                    Display="Dynamic" ValidationGroup="gr1"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; font-weight: bold">Store Id</td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtStoreId" runat="server" Width="90%"></asp:TextBox>
                            </td>
                            <td style="text-align: left;">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                    ControlToValidate="txtStoreId" ErrorMessage="Store id is required" ForeColor="Crimson"
                                    Display="Dynamic" ValidationGroup="gr1"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; font-weight: bold">Store Password</td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtStorePassword" runat="server" Width="90%"></asp:TextBox>
                            </td>
                            <td style="text-align: left;">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                                    ControlToValidate="txtStorePassword" ErrorMessage="Password is required" ForeColor="Crimson"
                                    Display="Dynamic" ValidationGroup="gr1"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; font-weight: bold">Is Active</td>
                            <td style="text-align: left;">
                                <asp:CheckBox ID="ckbxIsActive" runat="server" />
                            </td>
                            <td style="text-align: left;"></td>
                        </tr>
                        <tr>
                            <td style="text-align: left; font-weight: bold">Is Multiple Allowed</td>
                            <td style="text-align: left;">
                                <asp:CheckBox ID="ckbxIsMultiple" runat="server" />
                            </td>
                            <td style="text-align: left;"></td>
                        </tr>
                        <tr>
                            <td style="text-align: left; font-weight: bold">URL</td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtUrl" runat="server" Width="90%"></asp:TextBox>
                            </td>
                            <td style="text-align: left;">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server"
                                    ControlToValidate="txtUrl" ErrorMessage="URL is required" ForeColor="Crimson"
                                    Display="Dynamic" ValidationGroup="gr1"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; font-weight: bold">Success Url</td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtSuccessUrl" runat="server" Width="90%"></asp:TextBox>
                            </td>
                            <td style="text-align: left;">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server"
                                    ControlToValidate="txtSuccessUrl" ErrorMessage="Success url is required" ForeColor="Crimson"
                                    Display="Dynamic" ValidationGroup="gr1"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; font-weight: bold">Failed Url</td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtFailedUrl" runat="server" Width="90%"></asp:TextBox>
                            </td>
                            <td style="text-align: left;">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server"
                                    ControlToValidate="txtFailedUrl" ErrorMessage="Failed url is required" ForeColor="Crimson"
                                    Display="Dynamic" ValidationGroup="gr1"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; font-weight: bold">Cancelled Url</td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtCancelledUrl" runat="server" Width="90%"></asp:TextBox>
                            </td>
                            <td style="text-align: left;">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server"
                                    ControlToValidate="txtCancelledUrl" ErrorMessage="Cancelled url is required" ForeColor="Crimson"
                                    Display="Dynamic" ValidationGroup="gr1"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:Button ID="btnSave" runat="server" Text="Save" ValidationGroup="gr1"
                                    OnClick="btnSave_Click" />
                                <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btnClearAdmin"
                                    OnClick="btnClear_Click" />
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

    <div class="row">
        <div class="col-md-12">
            <%--<div class="panel panel-default">
                <div class="panel-body" style="margin-bottom: -25px;">--%>
                <%--<asp:UpdatePanel ID="updatePanelListView" runat="server">
                        <ContentTemplate>--%>
                            <div class="row" style="margin-bottom: 1%; margin-left: 1%">
                                Records: &nbsp;
                                <asp:Label ID="lblCount" runat="server" CssClass="badge"></asp:Label>
                            </div>

                            <%--OnItemDeleting="" 
                                OnItemUpdating=""  
                                OnItemDataBound="" 
                                OnItemCommand="" 
                                OnPagePropertiesChanging=""--%>
                            <asp:ListView ID="lvStores" runat="server"
                                OnItemDataBound="lvStores_ItemDataBound"
                                OnItemCommand="lvStores_ItemCommand"
                                OnItemDeleting="lvStores_ItemDeleting"
                                OnItemUpdating="lvStores_ItemUpdating">
                                <LayoutTemplate>
                                    <table id="tbl"
                                        class="table table-hover table-condensed table-striped table-responsive"
                                        style="width: 100%; text-align: left">
                                        <tr runat="server" style="background-color: #1387de; color: white; font-size:small">
                                            <th runat="server">SL#</th>
                                            <th runat="server">Name</th>
                                            <th runat="server">Id</th>
                                            <th runat="server">URL</th>
                                            <th runat="server">Success URL</th>
                                            <th runat="server">Failed URL</th>
                                            <th runat="server">Cancelled URL</th>
                                            <th runat="server">Multiple Allowed?</th>
                                            <th runat="server">Active?</th>
                                            <th></th>
                                        </tr>
                                        <tr runat="server" id="itemPlaceholder" />
                                    </table>
                                    <%--<asp:DataPager runat="server" ID="DataPager" PageSize="20">
                                        <Fields>
                                            <asp:NumericPagerField
                                                PreviousPageText="<--"
                                                NextPageText="-->" />
                                        </Fields>
                                    </asp:DataPager>--%>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <tr runat="server" style="font-size:smaller">
                                        <td valign="middle" align="left" class="">
                                            <asp:Label ID="lblSerial" runat="server" />.
                                        </td>
                                        <td valign="middle" align="left" class="">
                                            <asp:Label ID="lblStoreName" runat="server" />.
                                        </td>
                                        <td valign="middle" align="left" class="">
                                            <asp:Label ID="lblStoreId" runat="server" />
                                        </td>
                                        <td valign="middle" align="left" class="">
                                            <asp:Label ID="lblUrl" runat="server" />
                                        </td>
                                        <td valign="middle" align="left" class="">
                                            <asp:Label ID="lblSuccessUrl" runat="server" />
                                        </td>
                                        <td valign="middle" align="left" class="">
                                            <asp:Label ID="lblFailedUrl" runat="server" />
                                        </td>
                                        <td valign="middle" align="left" class="">
                                            <asp:Label ID="lblCancelledUrl" runat="server" />
                                        </td>
                                        <td valign="middle" align="left" class="">
                                            <asp:Label ID="lblIsMultiple" runat="server" />
                                        </td>
                                        <td valign="middle" align="left" class="">
                                            <asp:Label ID="lblIsActive" runat="server" />
                                        </td>
                                        <td valign="middle" align="right" class="">

                                            <asp:LinkButton CssClass="" ID="lnkEdit" runat="server">Edit</asp:LinkButton>
                                            |                      
                                        <asp:LinkButton CssClass="" ID="lnkDelete"
                                            OnClientClick="return confirm('Are you Confirm you want to Delete?');"
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
                <%--</div>--%>
                <%-- END PANEL-BODY --%>
            <%--</div>--%>
            <%-- END PANEL-DAFAULT --%>
        </div>
        <%-- END COL-MD-12 --%>
    </div>
    <%-- END ROW 2 --%>
    <%-- ---------------------------------------------------------------------------------------------------------------------------------- --%>

</asp:Content>
