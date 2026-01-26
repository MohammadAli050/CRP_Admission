<%@ Page Title="Campus Setup" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="CampusSetup.aspx.cs" Inherits="Admission.Admission.Office.CampusSetup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">

    <link href="../../Content/formStyle.css" rel="stylesheet" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h4>Campus Setup</h4>
                </div>
                <div class="panel-body">
                    <asp:UpdatePanel ID="updatePnael_Filter" runat="server">
                        <ContentTemplate>
                            <asp:Panel ID="messagePanel" runat="server">
                                <asp:Label ID="lblMessage" runat="server"></asp:Label>
                            </asp:Panel>
                            <table class="table_form table_fullwidth">
                                <tr>
                                    <td class="style_td" style="width: 10%">District</td>
                                    <td>
                                        <asp:DropDownList ID="ddlDistrictSeatPlanSetup" runat="server" Width="100%"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="ReqiredFieldValidator1" runat="server"
                                            Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                            ControlToValidate="ddlDistrictSeatPlanSetup" ErrorMessage="Required"
                                            ValidationGroup="gr1">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td class="style_td" style="width: 10%">Campus Name</td>
                                    <td style="width: 40%">
                                        <asp:TextBox ID="txtCampusName" runat="server" Width="90%"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="txtCampusNameReqV" runat="server" ControlToValidate="txtCampusName"
                                            ErrorMessage="Required" ForeColor="Crimson" Display="Dynamic" ValidationGroup="gr1">
                                        </asp:RequiredFieldValidator>

                                    </td>
                                    <td class="style_td style_td_secondCol" style="width: 10%">Campus Number</td>
                                    <td style="width: 40%">
                                        <asp:TextBox ID="txtCampusNumber" runat="server" TextMode="Number"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style_td">Priority</td>
                                    <td>
                                        <asp:TextBox ID="txtPriority" runat="server"></asp:TextBox>
                                    </td>
                                    <td class="style_td style_td_secondCol">Address Line</td>
                                    <td>
                                        <asp:TextBox ID="txtAddress" runat="server" TextMode="MultiLine" Width="70%"></asp:TextBox>
                                    </td>
                                    <tr>
                                        <td class="style_td">Is Active?</td>
                                        <td>
                                            <asp:CheckBox ID="ckbxIsActive" runat="server" />
                                        </td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                            </table>
                            <asp:Button ID="btnSubmit" Text="Save" runat="server" OnClick="btnSubmit_Click" ValidationGroup="gr1"/>
                            &nbsp;
                            <asp:Label ID="lblMessageSave" runat="server"></asp:Label>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <%-- END PANEL-BODY --%>
            </div>
        </div>
        <%-- END COL-MD-12 --%>
    </div>
    <%-- END ROW 1 --%>

    <asp:UpdatePanel ID="UpdatePanel_Lv" runat="server">
        <ContentTemplate>

            <div class="row">
                <div class="col-md-12">
                    <div class="panel panel-default">
                        <div class="row" style="margin-bottom: 1%; padding-left: 1%">
                            Records: &nbsp;
                                <asp:Label ID="lblCount" runat="server" CssClass="badge"></asp:Label>
                        </div>
                        <div class="panel-body">

                            <asp:ListView ID="lvCampus" runat="server"
                                OnItemDataBound="lvCampus_ItemDataBound"
                                OnItemCommand="lvCampus_ItemCommand"
                                OnItemDeleting="lvCampus_ItemDeleting"
                                OnItemUpdating="lvCampus_ItemUpdating">
                                <LayoutTemplate>
                                    <table id="tbl"
                                        class="table table-hover table-condensed table-striped"
                                        style="width: 100%; text-align: left">
                                        <tr runat="server" style="background-color: #1387de; color: white;">
                                            <th runat="server">SL#</th>
                                            <th runat="server">District Name</th>
                                            <th runat="server">Campus Name</th>
                                            <th runat="server">Number</th>
                                            <th runat="server">Priority</th>
                                            <th runat="server">Address</th>
                                            <th runat="server">Active?</th>
                                            <th></th>
                                        </tr>
                                        <tr runat="server" id="itemPlaceholder" />
                                    </table>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <tr runat="server" style="">
                                        <td valign="middle" align="left" class="">
                                            <asp:Label ID="lblSerial" runat="server" />.
                                        </td>
                                        <td valign="middle" align="left" class="">
                                            <asp:Label ID="lblDistrictName" runat="server" />.
                                        </td>
                                        <td valign="middle" align="left" class="">
                                            <asp:Label ID="lblCampusname" runat="server" />.
                                        </td>
                                        <td valign="middle" align="left" class="">
                                            <asp:Label ID="lblCampusNumber" runat="server" />
                                        </td>
                                        <td valign="middle" align="left" class="">
                                            <asp:Label ID="lblPriority" runat="server" />
                                        </td>
                                        <td valign="middle" align="left" class="">
                                            <asp:Label ID="lblAddress" runat="server" />
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

                        </div>
                        <%-- END PANEL-BODY --%>
                    </div>
                </div>
                <%-- END COL-MD-12 --%>
            </div>
            <%-- END ROW 2 --%>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
