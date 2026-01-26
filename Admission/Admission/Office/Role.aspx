<%@ Page Title="" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="Role.aspx.cs" Inherits="Admission.Admission.Office.Role" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">

    <style type="text/css">
        .textAlignCenter {
            text-align: center;
        }

        .fontBold {
            font-weight: bold;
        }
    </style>
    <link href="../../Content/formStyle.css" rel="stylesheet" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h4>Role Setup</h4>
                </div>
                <div class="panel-body panelBody_noMarginBottom">
                    <asp:Panel ID="messagePanel" runat="server">
                        <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                    </asp:Panel>
                    <table style="width: 100%" class="table_form">
                        <tr>
                            <td style="width: 10%;" class="fontBold">Name <span class="asteriskColor">*</span></td>
                            <td style="width: 40%">
                                <asp:TextBox ID="txtRoleName" runat="server" Width="100%"></asp:TextBox>
                            </td>
                            <td style="width: 50%">
                                <asp:RequiredFieldValidator ID="txtRoleNameReq" runat="server"
                                    ControlToValidate="txtRoleName" ErrorMessage="Role name is required."
                                    Font-Size="12pt" ForeColor="Crimson" Display="Dynamic"
                                    ValidationGroup="gr1"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="fontBold">Description</td>
                            <td>
                                <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine"
                                    Width="100%"></asp:TextBox>
                            </td>
                            <td></td>
                        </tr>
                        <tr>
                            <td class="fontBold">Is Active</td>
                            <td>
                                <asp:CheckBox ID="chbxIsActive" runat="server" />
                            </td>
                            <td></td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:Button ID="btnSave" runat="server" Text="Save" ValidationGroup="gr1" 
                                    OnClick="btnSave_Click"/>
                                <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btnClearAdmin" 
                                    OnClick="btnClear_Click"/>
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
                <div class="panel-body" style="margin-bottom: -25px;">
                    <div class="row" style="margin-bottom: 1%;">
                        Records: &nbsp;
                        <asp:Label ID="lblCount" runat="server" CssClass="badge"></asp:Label>
                    </div>
                    <asp:ListView ID="lvRoleActive" runat="server"
                        OnItemDataBound="lvRoleActive_ItemDataBound"
                        OnItemCommand="lvRoleActive_ItemCommand"
                        OnItemDeleting="lvRoleActive_ItemDeleting"
                        OnItemUpdating="lvRoleActive_ItemUpdating">
                        <LayoutTemplate>
                            <table id="tblRole"
                                class="table table-hover table-condensed table-striped"
                                style="width: 100%; text-align: left">
                                <tr runat="server" style="background-color: #1387de; color: white;">
                                    <th runat="server">SL#</th>
                                    <th runat="server">Role Name</th>
                                    <th runat="server">Description</th>
                                    <th runat="server">Is Active</th>
                                    <th></th>
                                </tr>
                                <tr runat="server" id="itemPlaceholder" />
                            </table>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr runat="server">
                                <td valign="middle" align="left" class="">
                                    <asp:Label ID="lblSerial" runat="server" />.
                                </td>
                                <td valign="middle" align="left" class="">
                                    <asp:Label ID="lblRoleName" runat="server" />
                                </td>
                                <td valign="middle" align="left" class="">
                                    <asp:Label ID="lblDescription" runat="server" />
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
