<%@ Page Title="Admission Unit" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="AdmissionUnit.aspx.cs"
    Inherits="Admission.Admission.Office.AdmissionUnit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <link href="../../Content/formStyle.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h4>Faculty/Program/Subject Setup</h4>
                </div>
                <div class="panel-body" style="margin-bottom: 0px;">

                    <asp:Panel ID="messagePanel" runat="server">
                        <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                    </asp:Panel>


                    <table style="width: 100%" class="table_form">

                        <tr runat="server" visible="false">
                            <td style="text-align: left; width: 10%; font-weight: bold">Type</td>
                            <td style="text-align: left; width: 45%">
                                 <asp:DropDownList ID="ddlType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlType_SelectedIndexChanged" Width="100%" CssClass="form-control">
                                <asp:ListItem Text="All" Value="0"></asp:ListItem>
                                <asp:ListItem Text="Local" Value="1"  Selected="True"></asp:ListItem>
                                <asp:ListItem Text="Foreign" Value="2"></asp:ListItem>

                            </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 10%; font-weight: bold">Name</td>
                            <td style="text-align: left; width: 45%">
                                <asp:TextBox ID="txtUnitName" runat="server" Width="100%"></asp:TextBox>
                            </td>
                            <td style="text-align: left; width: 45%">
                                <asp:RequiredFieldValidator ID="unitNameRequired" runat="server"
                                    ControlToValidate="txtUnitName" ErrorMessage="Unit name is required"
                                    ForeColor="Crimson" ValidationGroup="gr1"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; font-weight: bold">Short Name</td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtShortName" runat="server" Width="100%"></asp:TextBox>
                            </td>
                            <td style="text-align: left;">
                                <%--<asp:RequiredFieldValidator ID="unitcode1Required" runat="server" 
                                    ControlToValidate="" ErrorMessage="Unit name is required" 
                                    ForeColor="Crimson" ValidationGroup="gr1" Display="Dynamic"
                                    ></asp:RequiredFieldValidator>--%>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; font-weight: bold">Unit Code 1</td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtUnitCode1" runat="server" Width="100%"></asp:TextBox>
                                <span style="color: cadetblue">Used for payment id. [1 digit]</span>
                            </td>
                            <td style="text-align: left;">
                                <%--<asp:RequiredFieldValidator ID="unitcode1Required" runat="server" 
                                    ControlToValidate="" ErrorMessage="Unit name is required" 
                                    ForeColor="Crimson" ValidationGroup="gr1" Display="Dynamic"
                                    ></asp:RequiredFieldValidator>--%>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; font-weight: bold">Unit Code 2</td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtUnitCode2" runat="server" Width="100%"></asp:TextBox>
                                <span style="color: cadetblue">Used for test roll generation. [2 digits]</span>
                            </td>
                            <td style="text-align: left;">
                                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                                    ControlToValidate="" ErrorMessage="Unit name is required" 
                                    ForeColor="Crimson" ValidationGroup="gr1" Display="Dynamic"
                                    ></asp:RequiredFieldValidator>--%>
                            </td>
                        </tr>
                        <tr>

                            <td style="text-align: left; font-weight: bold">Is Active</td>
                            <td style="text-align: left;">
                                <asp:CheckBox ID="ckbxIsActive" runat="server" />
                            </td>
                             
                        </tr>
                        <tr runat="server" visible="false">
                            <td style="text-align: left; font-weight: bold;color:red">Foreign Is Active</td>
                            <td style="text-align: left;">
                                <asp:CheckBox ID="chkboxForeign" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:Button ID="btnSave" runat="server" Text="Save" ValidationGroup="gr1"
                                    OnClick="btnSave_Click" />
                                <asp:Button ID="btnClear" runat="server" Text="Clear"
                                    OnClick="btnClear_Click" CssClass="btnClearAdmin" />
                            </td>
                            <%--<td></td>
                            <td></td>--%>
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
                    <div class="row" style="margin-bottom: 1%; margin-left:1%">
                        Records: &nbsp;
                        <asp:Label ID="lblCount" runat="server" CssClass="badge"></asp:Label>
                    </div>
                    <%--<asp:UpdatePanel ID="updatePanelListView" runat="server">
                        <ContentTemplate>--%>
                    <%--OnItemDeleting="ItemDeleting" 
                                OnItemUpdating="ItemUpdating"  
                                OnItemDataBound="lv_ItemDataBound" 
                                OnItemCommand="lv_ItemCommand" 
                                OnPagePropertiesChanging="lv_PagePropertiesChanging"--%>
                    <div class="row" style="overflow-x:scroll;overflow-y:scroll">
                    <asp:ListView ID="lvAdmissionUnit" runat="server"
                        OnItemDataBound="lvAdmissionUnit_ItemDataBound"
                        OnItemCommand="lvAdmissionUnit_ItemCommand"
                        OnItemDeleting="lvAdmissionUnit_ItemDeleting"
                        OnItemUpdating="lvAdmissionUnit_ItemUpdating">
                        <LayoutTemplate>
                            <table id="tbl"
                                class="table table-hover table-condensed table-striped"
                                style="width: 100%; text-align: left">
                                <tr runat="server" style="background-color: #1387de; color: white;">
                                    <th runat="server">SL#</th>
                                    <th runat="server">Full Name</th>
                                    <th runat="server" style="text-align: center">Short Name</th>
                                    <th runat="server" style="text-align: center">Unit Code 1</th>
                                    <th runat="server" style="text-align: center">Unit Code 2</th>
                                    <th runat="server" style="text-align: center">Active?</th>
                                    <%--<th runat="server" style="text-align: center">Foreign Active?</th>--%>
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
                            <tr runat="server">
                                <td valign="middle" align="left" class="">
                                    <asp:Label ID="lblSerial" runat="server" />.
                                </td>
                                <td valign="middle" align="left" class="">
                                    <asp:Label ID="lblUnitName" runat="server" />
                                </td>
                                <td valign="middle" align="center" class="">
                                    <asp:Label ID="lblShortName" runat="server" />
                                </td>
                                <td valign="middle" align="center" class="">
                                    <asp:Label ID="lblUnitCode1" runat="server" />
                                </td>
                                <td valign="middle" align="center" class="">
                                    <asp:Label ID="lblUnitCode2" runat="server" />
                                </td>
                                <td valign="middle" align="center" class="">
                                    <asp:Label ID="lblIsActive" runat="server" />
                                </td>
                                <%--<td valign="middle" align="center" class="">
                                    <asp:Label ID="lblForeignActive" runat="server" />
                                </td>--%>
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
