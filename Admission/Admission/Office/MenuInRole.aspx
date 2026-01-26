<%@ Page Title="Menu In Role" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="MenuInRole.aspx.cs" Inherits="Admission.Admission.Office.MenuInRole" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">

    <link href="../../Content/formStyle.css" rel="stylesheet" />

    <style type="text/css">
        .modalProgress {
            clear: both;
            float: left;
            margin: 0px;
            z-index: 100;
            position: absolute;
            left: 0px;
            top: 0px;
            width: 100%;
            height: 100%;
            background-color: #848484;
            filter: alpha(opacity=50);
            opacity: 0.5;
            -moz-opacity: 0.5;
            -khtml-opacity: 0.5;
        }
        
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-md-6">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h4>Menu In Role Setup</h4>
                </div>
                <div class="panel-body">
                    <asp:Panel ID="messagePanel" runat="server">
                        <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                    </asp:Panel>
                    <asp:UpdatePanel ID="updatePanelForm" runat="server">
                        <ContentTemplate>
                            <table class="table_form" style="width: 100%">
                                <tr>
                                    <td class="style_td" style="width: 20%">Role <span class="asteriskColor">*</span></td>
                                    <td>
                                        <asp:DropDownList ID="ddlRole" runat="server"
                                            Width="100%">
                                        </asp:DropDownList>
                                        <asp:CompareValidator ID="ddlRoleComV" runat="server"
                                            ControlToValidate="ddlRole" ErrorMessage="Select Role"
                                            ForeColor="Crimson" Font-Size="9pt" Display="Dynamic"
                                            ValueToCompare="-1" Operator="NotEqual" ValidationGroup="gr1"></asp:CompareValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style_td">Parent Menu <span class="asteriskColor">*</span></td>
                                    <td>
                                        <asp:DropDownList ID="ddlParentMenu" runat="server"
                                            Width="100%">
                                        </asp:DropDownList>
                                        <asp:CompareValidator ID="ddlParentMenuComV" runat="server"
                                            ControlToValidate="ddlParentMenu" ErrorMessage="Select Parent Menu"
                                            ForeColor="Crimson" Font-Size="9pt" Display="Dynamic"
                                            ValueToCompare="-1" Operator="NotEqual" ValidationGroup="gr1"></asp:CompareValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Button ID="btnAddParentMenu" runat="server" Text="Load"
                                            ValidationGroup="gr1" OnClick="btnAddParentMenu_Click" />
                                        <asp:Button ID="btnClearParentMenu" runat="server" Text="Clear"
                                            CssClass="btnClearAdmin" OnClick="btnClearParentMenu_Click" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <%-- END PANEL-BODY --%>
            </div>
            <%-- END PANEL-DEFAULT --%>

        </div>
        <%-- END COL-MD-6 --%>

        <div class="col-md-6">

            <div class="panel panel-default">
                <div class="panel-heading">
                    <h5>Parent/Root Menu</h5>
                </div>
                <div class="panel-body">
                    <asp:UpdateProgress ID="updateProgressParent" runat="server"
                        AssociatedUpdatePanelID="updatePanelParentMenu" DynamicLayout="true" DisplayAfter="0">
                        <ProgressTemplate>
                            <div id="IMGDIV" runat="server" class="modalProgress">
                                <img alt="Loading" src="../../Images/AppImg/t1.gif" width="106" height="107" 
                                    style="margin-left: 45%; margin-top: 5%"/>
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                    <asp:UpdatePanel ID="updatePanelParentMenu" runat="server" ChildrenAsTriggers="true">
                        <ContentTemplate>
                            <asp:Panel ID="messagePanelParent" runat="server">
                                <asp:Label ID="lblMessageParent" runat="server" Text=""></asp:Label>
                            </asp:Panel>
                            <asp:Panel ID="listViewPanelParent" runat="server">
                                <asp:ListView ID="lvParentMenu" runat="server"
                                    OnItemDataBound="lvParentMenu_ItemDataBound"
                                    OnItemCommand="lvParentMenu_ItemCommand"
                                    OnItemUpdating="lvParentMenu_ItemUpdating"
                                    OnItemDeleting="lvParentMenu_ItemDeleting">
                                    <LayoutTemplate>
                                        <table id="tblParentMenu"
                                            class="table_form"
                                            style="width: 100%; text-align: left">
                                            <%--<tr runat="server" style="background-color: #1387de; color: white;">
                                            <th runat="server"></th>
                                            <th></th>
                                        </tr>--%>
                                            <tr runat="server" id="itemPlaceholder" />
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr runat="server">
                                            <td valign="middle" align="left" class="">
                                                <asp:Label ID="lblParentMenuName" runat="server" />.
                                            </td>

                                            <td valign="middle" align="right" class="">
                                                <asp:LinkButton CssClass="btn btn-success btn-xs" ID="lnkParentMenuLoad" runat="server">LOAD</asp:LinkButton>
                                                |
                                            <asp:LinkButton CssClass="btn btn-primary btn-xs" ID="lnkParentMenuAdd" runat="server">ADD</asp:LinkButton>
                                                |
                                            <asp:LinkButton CssClass="btn btn-warning btn-xs" ID="lnkParentMenuDelete"
                                                OnClientClick="return confirm('Are you Confirm you want to Delete? This will delete it's child menus.');"
                                                runat="server">DELETE</asp:LinkButton>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <EmptyDataTemplate>
                                        <hr />
                                    </EmptyDataTemplate>
                                </asp:ListView>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <%-- END PANEL-BODY --%>
            </div>
            <%-- END PANEL-DEFAULT --%>
            <%-- **************************************************************** --%>
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h5>Child/Submenu</h5>
                </div>
                <div class="panel-body">
                    <asp:UpdateProgress ID="updateProgressChild" runat="server"
                        AssociatedUpdatePanelID="updatePanelChildMenu" DynamicLayout="true" DisplayAfter="0">
                        <ProgressTemplate>
                            <div id="IMGDIV2" runat="server" class="modalProgress">
                                <img alt="Loading" src="../../Images/AppImg/t1.gif" width="106" height="107" 
                                    style="margin-left: 45%; margin-top: 5%"/>
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                    <asp:UpdatePanel ID="updatePanelChildMenu" runat="server">
                        <ContentTemplate>
                            <asp:Panel ID="messagePanelChild" runat="server">
                                <asp:Label ID="lblMessageChild" runat="server" Text=""></asp:Label>
                            </asp:Panel>
                            <asp:ListView ID="lvChildMenu" runat="server"
                                OnItemDataBound="lvChildMenu_ItemDataBound"
                                OnItemCommand="lvChildMenu_ItemCommand"
                                OnItemDeleting="lvChildMenu_ItemDeleting"
                                OnItemUpdating="lvChildMenu_ItemUpdating">
                                <LayoutTemplate>
                                    <table id="tblChildMenu"
                                        class="table_form"
                                        style="width: 100%; text-align: left">
                                        <%--<tr runat="server" style="background-color: #1387de; color: white;">
                                            <th runat="server"></th>
                                            <th></th>
                                        </tr>--%>
                                        <tr runat="server" id="itemPlaceholder" />
                                    </table>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <tr runat="server">
                                        <td valign="middle" align="left" class="">
                                            <asp:Label ID="lblChildMenuName" runat="server" />.
                                        </td>
                                        <td valign="middle" align="right" class="">

                                            <asp:LinkButton CssClass="btn btn-primary btn-xs" ID="lnkChildMenuAdd" runat="server">ADD</asp:LinkButton>
                                            |
                                            <asp:LinkButton CssClass="btn btn-warning btn-xs" ID="lnkChildMenuDelete"
                                                OnClientClick="return confirm('Are you Confirm you want to Delete?');"
                                                runat="server">Delete</asp:LinkButton>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <EmptyDataTemplate>
                                    <div class="alert alert-info" role="alert">No submenu available</div>
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
    <%-- END ROW 1--%>
    <%-- ---------------------------------------------------------------------------------------------------------------------------------- --%>
</asp:Content>
