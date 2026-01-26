<%@ Page Title="" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="User.aspx.cs" Inherits="Admission.Admission.Office.User" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <%--<style type="text/css">

        .style_td{
            font-weight: bold;
        }

        .table_form td{
            padding-top: 2px;
            padding-bottom: 2px;
            padding-left: 1px;
            padding-right: 1px;
        }

        .table_form tbody tr:nth-child(odd){
            background-color: #f9f9f9;
        }

    </style>--%>
    <link href="../../Content/formStyle.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-md-6">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h4>USER</h4>
                </div>
                <div class="panel-body" style="margin-bottom: 0px;">
                    <asp:Panel ID="messagePanelUser" runat="server">
                        <asp:Label ID="lblMessageUser" runat="server" Text=""></asp:Label>
                    </asp:Panel>
                    <table style="width: 100%" class="table_form">
                        <tr>
                            <td style="width: 10%" class="style_td">Username <span class="asteriskColor">*</span></td>
                            <td style="width: 40%">
                                <asp:TextBox ID="txtUsername" runat="server" Width="100%"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="txtUsernameReqV" runat="server"
                                    ControlToValidate="txtUsername" ErrorMessage="Username required"
                                    ForeColor="Crimson" Font-Size="10pt" Display="Dynamic"
                                    ValidationGroup="gr1"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="style_td">Password <span class="asteriskColor">*</span></td>
                            <td>
                                <asp:TextBox ID="txtPassword" runat="server" Width="100%" TextMode="Password"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="txtPasswordReqV" runat="server"
                                    ControlToValidate="txtPassword" ErrorMessage="Password required"
                                    ForeColor="Crimson" Font-Size="10pt" Display="Dynamic"
                                    ValidationGroup="gr1"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="style_td">Is Active</td>
                            <td>
                                <asp:CheckBox ID="chbxIsActive" runat="server" />
                            </td>
                        </tr>
                        <%--<tr>
                            <td class="style_td">Mobile</td>
                            <td>
                                <asp:TextBox ID="txtMobile" runat="server" Width="100%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="style_td">Email</td>
                            <td>
                                <asp:TextBox ID="txtEmail" runat="server" Width="100%" TextMode="Email"></asp:TextBox>
                            </td>
                        </tr>--%>
                        <tr>
                            <td colspan="2">
                                <asp:Button ID="btnSave" runat="server" Text="Create User" ValidationGroup="gr1" OnClick="btnSave_Click"/>
                                <asp:Button ID="btnClear" runat="server" Text="Clear" Visible="false"
                                    CssClass="btnClearAdmin" OnClick="btnClear_Click"/>
                            </td>
                        </tr>
                    </table>
                </div>
                <%-- END PANEL-BODY --%>
            </div>
            <%-- END PANEL-DEFAULT --%>
        </div>
        <%-- END COL-MD-6 --%>

        <div class="col-md-6">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h4>User In Role</h4>
                </div>
                <div class="panel-body" style="margin-bottom: 0px;">
                    <asp:Panel ID="messagePanelUserInRole" runat="server">
                        <asp:Label ID="lblMessageUserInRole" runat="server" Text=""></asp:Label>
                    </asp:Panel>
                    <table style="width: 100%" class="table_form">
                        <tr>
                            <td class="style_td">User Name</td>
                            <td>
                                <asp:Label ID="lblUsername" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="style_td">Role <span class="asteriskColor">*</span></td>
                            <td>
                                <asp:DropDownList ID="ddlRole" runat="server" Width="100%"></asp:DropDownList>
                                <asp:CompareValidator ID="ddlRoleComV" runat="server"
                                    ControlToValidate="ddlRole" ErrorMessage="Role required" ForeColor="Crimson"
                                    Font-Size="10pt" Display="Dynamic" ValueToCompare="-1" Operator="NotEqual"
                                    ValidationGroup="gr2"></asp:CompareValidator>
                            </td>
                        </tr>
                       <%--<tr>
                            <td class="style_td">Start Date</td>
                            <td>
                                <asp:TextBox ID="txtStartDate" runat="server" Width="100%"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender ID="CeStartDate" runat="server"
                                    TargetControlID="txtStartDate" Format="dd/MM/yyyy" />
                            </td>
                        </tr>
                        <tr>
                            <td class="style_td">End Date</td>
                            <td>
                                <asp:TextBox ID="txtEndDate" runat="server" Width="100%"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender ID="CeEndDate" runat="server"
                                    TargetControlID="txtEndDate" Format="dd/MM/yyyy" />
                            </td>
                        </tr>--%>

                        <tr>
                            <td colspan="2">
                                <asp:Button ID="btnSaveUserInRole" runat="server" Text="Save" 
                                    ValidationGroup="gr2" OnClick="btnSaveUserInRole_Click"/>
                                <asp:Button ID="btnClearUserInRole" runat="server" Text="Clear"  Visible="false"
                                    CssClass="btnClearAdmin" OnClick="btnClearUserInRole_Click"/>
                            </td>
                        </tr>
                    </table>
                </div>
                <%-- END PANEL-BODY --%>
            </div>
            <%-- END PANEL-DEFAULT --%>
        </div>
        <%-- END COL-MD-6 --%>

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
                    <asp:ListView ID="lvUser" runat="server"
                        OnItemDataBound="lvUser_ItemDataBound"
                        OnItemCommand="lvUser_ItemCommand"
                        OnItemDeleting="lvUser_ItemDeleting"
                        OnItemUpdating="lvUser_ItemUpdating">
                        <LayoutTemplate>
                            <table id="tbl"
                                class="table table-hover table-condensed table-striped"
                                style="width: 100%; text-align: center">
                                <tr runat="server" style="background-color: #1387de; color: white;">
                                    <th runat="server" style="width: 5%">SL#</th>
                                    <th runat="server" style="width: 25%">Username</th>
                                    <th runat="server" style="width: 20%">Role</th>
                                    <th></th>
                                    <th runat="server" style="width: 10%">Is Active</th>
                                    <th runat="server"></th>
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
                                    <asp:Label ID="lblUsername" runat="server" />
                                </td>
                                <td valign="middle" align="left" class="">
                                    <asp:Label ID="lblRole" runat="server" />
                                </td>
                                <td valign="middle" align="center" class="">

                                    <asp:LinkButton CssClass="" ID="lnkAddRole" runat="server">Add Role</asp:LinkButton>
                                    
                                    <asp:LinkButton CssClass="" ID="lnkEditRole" runat="server">Edit Role</asp:LinkButton>
                                    |
                                    <asp:LinkButton CssClass="" ID="lnkDeleteRole"
                                            OnClientClick="return confirm('Are you Confirm you want to Delete?');"
                                            runat="server">Delete Role</asp:LinkButton>
                                </td>
                                <td valign="middle" align="left" class="">
                                    <asp:Label ID="lblIsActive" runat="server" />
                                </td>
                                <td valign="middle" align="left" class="">
                                    <asp:Label ID="lbl5" runat="server" />
                                </td>
                                <td valign="middle" align="right" class="">
                                    <asp:LinkButton CssClass="" ID="lnkEdit" runat="server">Edit User</asp:LinkButton>
                                    |
                                        <asp:LinkButton CssClass="" ID="lnkDelete"
                                            OnClientClick="return confirm('Are you Confirm you want to Delete?');"
                                            runat="server">Delete User</asp:LinkButton>
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
