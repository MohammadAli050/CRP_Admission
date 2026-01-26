<%@ Page Title="Building Setup" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="BuildingSetup.aspx.cs" Inherits="Admission.Admission.Office.BuildingSetup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">

    <link href="../../Content/formStyle.css" rel="stylesheet" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h4>Building Setup</h4>
                </div>
                <div class="panel-body">
                    <asp:UpdatePanel ID="updatePnael_Filter" runat="server">
                        <ContentTemplate>
                            <asp:Panel ID="messagePanel" runat="server">
                                <asp:Label ID="lblMessage" runat="server"></asp:Label>
                            </asp:Panel>
                            <table class="table_form table_fullwidth">
                                <tr>
                                    <td class="style_td" style="width: 10%">Building Name</td>
                                    <td style="width: 40%">
                                        <asp:TextBox ID="txtBuildingName" runat="server" Width="90%"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="txtBuildingNameReqV" runat="server" ControlToValidate="txtBuildingName"
                                            ErrorMessage="Required" ForeColor="Crimson" Display="Dynamic" ValidationGroup="gr1">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                    <td class="style_td style_td_secondCol" style="width: 10%">Campus</td>
                                    <td style="width: 40%">
                                        <asp:DropDownList ID="ddlCampus" runat="server" Width="90%"></asp:DropDownList>
                                        <asp:CompareValidator ID="ddlCampusComV" runat="server" ControlToValidate="ddlCampus"
                                            ValueToCompare="-1" Operator="NotEqual" ErrorMessage="Required" ForeColor="Crimson"
                                            Font-Size="Larger" Display="Dynamic" ValidationGroup="gr1"></asp:CompareValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style_td ">Number</td>
                                    <td>
                                        <asp:TextBox ID="txtNumber" runat="server"></asp:TextBox>
                                    </td>
                                    <td class="style_td style_td_secondCol">Priority</td>
                                    <td>
                                        <asp:TextBox ID="txtPriority" runat="server" TextMode="Number"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="txtPriorityReqV" runat="server" ControlToValidate="txtPriority"
                                            ErrorMessage="Required" ForeColor="Crimson" Display="Dynamic" ValidationGroup="gr1">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <%--<tr>
                                    <td class="style_td ">Address Line</td>
                                    <td>
                                        <asp:TextBox ID="txtAddress" runat="server" TextMode="MultiLine" Width="70%"></asp:TextBox>
                                    </td>
                                    <td></td>
                                    <td></td>
                                </tr>--%>
                                <tr>
                                    <td class="style_td">Is Active?</td>
                                    <td>
                                        <asp:CheckBox ID="ckbxIsActive" runat="server" />
                                    </td>
                                    <td></td>
                                    <td></td>
                                </tr>
                            </table>
                            <asp:Button ID="btnSubmit" Text="Save" runat="server" 
                                ValidationGroup="gr1" OnClick="btnSubmit_Click" />
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
                        <div class="row" style="margin-top: 1%; margin-left: 1%">
                            Records: &nbsp;
                                <asp:Label ID="lblCount" runat="server" CssClass="badge"></asp:Label>
                        </div>
                        <div class="panel-body">
                            <div class="row" style="overflow-y:scroll;overflow-x:scroll">

                            <asp:ListView ID="lvBuilding" runat="server"
                                OnItemDataBound="lvBuilding_ItemDataBound"
                                OnItemCommand="lvBuilding_ItemCommand"
                                OnItemDeleting="lvBuilding_ItemDeleting"
                                OnItemUpdating="lvBuilding_ItemUpdating">
                                <LayoutTemplate>
                                    <table id="tbl"
                                        class="table table-hover table-condensed table-striped"
                                        style="width: 100%; text-align: left">
                                        <tr runat="server" style="background-color: #1387de; color: white;">
                                            <th runat="server">SL#</th>
                                            <th runat="server">Building Name</th>
                                            <th runat="server">Campus Name</th>
                                            <th runat="server">Number</th>
                                            <th runat="server">Priority</th>
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
                                            <asp:Label ID="lblBuildingName" runat="server" />.
                                        </td>
                                        <td valign="middle" align="left" class="">
                                            <asp:Label ID="lblCampusName" runat="server" />.
                                        </td>
                                        <td valign="middle" align="left" class="">
                                            <asp:Label ID="lblNumber" runat="server" />
                                        </td>
                                        <td valign="middle" align="left" class="">
                                            <asp:Label ID="lblPriority" runat="server" />
                                        </td>
                                        <%--<td valign="middle" align="left" class="">
                                            <asp:Label ID="lblAddress" runat="server" />
                                        </td>--%>
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
