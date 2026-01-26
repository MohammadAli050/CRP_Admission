<%@ Page Title="Menu" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true"
    CodeBehind="Menu.aspx.cs" Inherits="Admission.Admission.Admin.Menu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">

    <style type="text/css">
        .textAlignCenter {
            text-align: center;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h4>MENU</h4>
                </div>
                <div class="panel-body" style="margin-bottom: -30px;">
                    <asp:Panel ID="messagePanel" runat="server">
                        <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                    </asp:Panel>
                    <table style="width: 100%" class="table">
                        <tr>
                            <td style="font-weight: bold; width: 10%">Name <span style="color: crimson">*</span></td>
                            <td style="width: 40%">
                                <asp:TextBox ID="txtMenuName" runat="server" Width="100%"></asp:TextBox>
                            </td>
                            <td style="width: 50%">
                                <asp:RequiredFieldValidator ID="txtMenuNameReq" runat="server"
                                    ControlToValidate="txtMenuName" ErrorMessage="Menu name is required"
                                    Font-Size="12pt" ForeColor="Crimson" Display="Dynamic"
                                    ValidationGroup="gr1"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td style="font-weight: bold">URL <span style="color: crimson">*</span></td>
                            <td>
                                <asp:TextBox ID="txtUrl" runat="server" Width="100%"></asp:TextBox>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="txtUrlReq" runat="server"
                                    ControlToValidate="txtUrl" ErrorMessage="URL is required"
                                    Font-Size="12pt" ForeColor="Crimson" Display="Dynamic"
                                    ValidationGroup="gr1"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td style="font-weight: bold">Parent Menu</td>
                            <td>
                                <asp:DropDownList ID="ddlParentMenu" runat="server" Width="50%"></asp:DropDownList>
                            </td>
                            <td></td>
                        </tr>
                        <tr>
                            <td style="font-weight: bold">Tier</td>
                            <td>
                                <asp:TextBox ID="txtTier" runat="server" TextMode="Number" Text="0"
                                    Enabled="false"></asp:TextBox>
                            </td>
                            <td></td>
                        </tr>
                        <tr>
                            <td style="font-weight: bold">Menu Order <span style="color: crimson">*</span></td>
                            <td>
                                <asp:TextBox ID="txtMenuOrder" runat="server" TextMode="Number"></asp:TextBox>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="txtMenuOrderReq" runat="server"
                                    ControlToValidate="txtMenuOrder" ErrorMessage="Menu order required"
                                    Font-Size="12pt" ForeColor="Crimson" Display="Dynamic"
                                    ValidationGroup="gr1"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td style="font-weight: bold">Is Admin? </td>
                            <td>
                                <asp:CheckBox ID="chbxIsAdmin" runat="server" />
                            </td>
                            <td></td>
                            <%--</tr>
                        <tr>
                        </tr>
                        <tr>
                        </tr>--%>
                        <tr>
                            <td colspan="4">
                                <asp:Button ID="btnSave" runat="server" Text="SAVE" ValidationGroup="gr1"
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
            <div class="panel panel-default">
                <div class="panel-body" style="margin-bottom: -10px;">
                    <div class="row" style="margin-bottom: 1%; margin-left:1%">
                        Records: &nbsp;
                        <asp:Label ID="lblCount" runat="server" CssClass="badge"></asp:Label>
                        &nbsp;&nbsp;
                        <asp:DropDownList ID="ddlMenuFilter" runat="server"></asp:DropDownList>
                        <%--<asp:CompareValidator ID="ddlMenuFilterComV" runat="server"
                            ControlToValidate="ddlMenuFilter" ErrorMessage="*"
                            ForeColor="Crimson" ValueToCompare="-1" Operator="NotEqual"
                            ValidationGroup="gr2"></asp:CompareValidator>
                        &nbsp;--%>
                        <asp:Button ID="btnMenuFilter" runat="server" Text="Filter"
                            OnClick="btnMenuFilter_Click" ValidationGroup="gr2" />
                    </div>
                    <%--<asp:UpdatePanel ID="updatePanelListView" runat="server">
                        <ContentTemplate>--%>
                    <%--OnItemDeleting="" 
                                OnItemUpdating=""  
                                OnItemDataBound="" 
                                OnItemCommand="" 
                                OnPagePropertiesChanging=""--%>
                    <asp:ListView ID="lvMenu" runat="server" 
                        OnItemDataBound="lvMenu_ItemDataBound"
                        OnItemCommand="lvMenu_ItemCommand"
                        OnItemDeleting="lvMenu_ItemDeleting"
                        OnItemUpdating="lvMenu_ItemUpdating"
                        OnPagePropertiesChanging="lvMenu_PagePropertiesChanging" >
                        <LayoutTemplate>
                            <table id="tblMenu"
                                class="table table-hover table-condensed table-striped"
                                style="width: 100%; text-align: left;">
                                <tr runat="server" style="background-color: #1387de; color: white;">
                                    <th runat="server" style="text-align: center">Name</th>
                                    <th runat="server" style="text-align: center">Url</th>
                                    <th runat="server">Parent Menu</th>
                                    <th runat="server" style="text-align: center">Order</th>
                                    <th runat="server" style="text-align: center">Is Admin</th>
                                    <th></th>
                                </tr>
                                <tr runat="server" id="itemPlaceholder" />
                            </table>
                            <%--<asp:DataPager runat="server" ID="lvDataPager" 
                                PagedControlID="lvMenu" PageSize="5">
                                        <Fields>
                                            <asp:NumericPagerField
                                                PreviousPageText="<--"
                                                NextPageText="-->" />
                                        </Fields>
                                    </asp:DataPager>--%>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr runat="server">
                                <td valign="middle" align="left" class="">
                                    <asp:Label ID="lblName" runat="server" />.
                                </td>
                                <td valign="middle" align="left" class="">
                                    <asp:Label ID="lblUrl" runat="server" />
                                </td>
                                <td valign="middle" align="left" class="">
                                    <asp:Label ID="lblParentMenu" runat="server" />
                                </td>
                                <td valign="middle" align="center" class="">
                                    <asp:Label ID="lblOrder" runat="server" />
                                </td>
                                <td valign="middle" align="center" class="">
                                    <asp:Label ID="lblIsAdmin" runat="server" />
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
                    <div class="textAlignCenter">
                        <asp:DataPager runat="server" ID="lvDataPager"
                            PagedControlID="lvMenu" PageSize="20" class="btn-group btn-group-sm">
                            <Fields>
                                <%--<asp:NumericPagerField
                                    PreviousPageText="<--"
                                    NextPageText="-->" />--%>
                                <asp:NextPreviousPagerField PreviousPageText="<<" FirstPageText="First" ShowPreviousPageButton="true"
                                    ShowFirstPageButton="true" ShowNextPageButton="false" ShowLastPageButton="false"
                                    ButtonCssClass="btn btn-default" RenderNonBreakingSpacesBetweenControls="false" RenderDisabledButtonsAsLabels="false" />
                                <asp:NumericPagerField ButtonType="Link" CurrentPageLabelCssClass="btn btn-primary disabled" RenderNonBreakingSpacesBetweenControls="false"
                                    NumericButtonCssClass="btn btn-default" ButtonCount="10" NextPageText="..." NextPreviousButtonCssClass="btn btn-default" />
                                <asp:NextPreviousPagerField NextPageText=">>" LastPageText="Last" ShowNextPageButton="true"
                                    ShowLastPageButton="true" ShowPreviousPageButton="false" ShowFirstPageButton="false"
                                    ButtonCssClass="btn btn-default" RenderNonBreakingSpacesBetweenControls="false" RenderDisabledButtonsAsLabels="false" />
                            </Fields>
                        </asp:DataPager>
                    </div>
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
</asp:Content>
