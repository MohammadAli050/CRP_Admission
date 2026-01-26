<%@ Page Title="District Seat Limit Setup" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="DistrictSeatLimitSetup.aspx.cs" Inherits="Admission.Admission.Office.DistrictSeatLimitSetup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel" runat="server">
        <ContentTemplate>

            <div class="row">
                <div class="col-md-12">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h4>District Seat Limit Setup</h4>
                        </div>
                        <div class="panel-body" style="margin-bottom: 0px;">

                            <asp:Panel ID="messagePanel" runat="server">
                                <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                            </asp:Panel>

                            <table style="width: 100%" class="table_form">
                                <tr>
                                    <td style="text-align: left; width: 10%; font-weight: bold">Session</td>
                                    <td style="text-align: left; width: 45%">
                                        <asp:DropDownList ID="ddlSession" runat="server" CssClass="form-control" Width="100%">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="text-align: left; width: 45%">
                                        <asp:CompareValidator ID="ddlNationality_ComV" runat="server" 
                                    ControlToValidate="ddlSession" 
                                    ErrorMessage="Required"
                                    Font-Bold="true" 
                                    Font-Size="9pt" 
                                    ForeColor="Crimson"
                                    Display="Dynamic" 
                                    ValueToCompare="-1" 
                                    Operator="NotEqual" 
                                    ValidationGroup="gr1"></asp:CompareValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; width: 10%; font-weight: bold">Faculty</td>
                                    <td style="text-align: left; width: 45%">
                                        <asp:DropDownList ID="ddlAdmissionUnit" runat="server" CssClass="form-control" Width="100%">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="text-align: left; width: 45%">
                                        <asp:CompareValidator ID="CompareValidator1" runat="server" 
                                    ControlToValidate="ddlAdmissionUnit" 
                                    ErrorMessage="Required"
                                    Font-Bold="true" 
                                    Font-Size="9pt" 
                                    ForeColor="Crimson"
                                    Display="Dynamic" 
                                    ValueToCompare="-1" 
                                    Operator="NotEqual" 
                                    ValidationGroup="gr1"></asp:CompareValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; width: 10%; font-weight: bold">District</td>
                                    <td style="text-align: left; width: 45%">
                                        <asp:DropDownList ID="ddlDistrict" runat="server" CssClass="form-control" Width="100%">
                                            <%--<asp:ListItem Enabled="true" Text="-- Select District --" Value="-1"></asp:ListItem>
                                            <asp:ListItem Text="Dhaka" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Chittagong" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="Bogura" Value="3"></asp:ListItem>
                                            <asp:ListItem Text="Khulna" Value="4"></asp:ListItem>--%>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="text-align: left; width: 45%">
                                        <asp:CompareValidator ID="CompareValidator2" runat="server" 
                                    ControlToValidate="ddlDistrict" 
                                    ErrorMessage="Required"
                                    Font-Bold="true" 
                                    Font-Size="9pt" 
                                    ForeColor="Crimson"
                                    Display="Dynamic" 
                                    ValueToCompare="-1" 
                                    Operator="NotEqual" 
                                    ValidationGroup="gr1"></asp:CompareValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; font-weight: bold">Seat Limit</td>
                                    <td style="text-align: left;">
                                        <asp:TextBox ID="txtSeatLimit" runat="server" type="number" CssClass="form-control" Width="100%"></asp:TextBox>
                                    </td>
                                    <td style="text-align: left;">
                                        <asp:RequiredFieldValidator runat="server"
                                            ID="RequiredFieldValidator10"
                                            ValidationGroup="gr1"
                                            ControlToValidate="txtSeatLimit"
                                            Display="Dynamic"
                                            Font-Size="9pt"
                                            ForeColor="Crimson"
                                            Font-Bold="false"
                                            ErrorMessage="Required" />
                                        
                                    </td>
                                </tr>

                                <tr>
                                    <td style="text-align: left; font-weight: bold">IsActive</td>
                                    <td style="text-align: left;">
                                        <asp:CheckBox ID="cbIsActive" runat="server" />
                                    </td>
                                    <td style="text-align: left;">
                                       
                                        
                                    </td>
                                </tr>

                                <tr>
                                    <td colspan="3">
                                        <asp:Button ID="btnSave" runat="server" Text="Save" ValidationGroup="gr1"
                                            OnClick="btnSave_Click" />
                                        <asp:Button ID="btnClear" runat="server" Text="Clear"
                                            OnClick="btnClear_Click" CssClass="btnClearAdmin" />

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



            <div class="row">
                <div class="col-md-12">
                    <div class="panel panel-default">
                        <div class="panel-body" style="margin-bottom: -25px;">
                            <div class="row" style="margin-bottom: 1%;">
                                Records: &nbsp;
                        <asp:Label ID="lblCount" runat="server" CssClass="badge"></asp:Label>
                            </div>
                            <asp:ListView ID="lvDistrictSeatLimitSetup" runat="server"
                                OnItemDataBound="lvDistrictSeatLimitSetup_ItemDataBound"
                                OnItemCommand="lvDistrictSeatLimitSetup_ItemCommand"
                                OnItemDeleting="lvDistrictSeatLimitSetup_ItemDeleting"
                                OnItemUpdating="lvDistrictSeatLimitSetup_ItemUpdating">
                                <LayoutTemplate>
                                    <table id="tbl"
                                        class="table table-hover table-condensed table-striped"
                                        style="width: 100%; text-align: left">
                                        <tr runat="server" style="background-color: #1387de; color: white;">
                                            <th runat="server">SL#</th>
                                            <th runat="server">Faculty</th>
                                            <th runat="server">Session</th>
                                            <th runat="server">District</th>
                                            <th runat="server" style="text-align: center">Seat Limit</th>
                                            <th runat="server" style="text-align: center">Seat Fillup</th>
                                            <th runat="server" style="text-align: center">Active?</th>
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
                                            <asp:Label ID="lblFaculty" runat="server" />
                                        </td>
                                        <td valign="middle" align="left" class="">
                                            <asp:Label ID="lblSession" runat="server" />
                                        </td>
                                        <td valign="middle" align="left" class="">
                                            <asp:Label ID="lblDistrictName" runat="server" />
                                        </td>
                                        <td valign="middle" align="center" class="">
                                            <asp:Label ID="lblSeatLimit" runat="server" />
                                        </td>
                                        <td valign="middle" align="center" class="">
                                            <asp:Label ID="lblSeatFillup" runat="server" />
                                        </td>
                                        <td valign="middle" align="center" class="">
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
                    <%-- END PANEL-DAFAULT --%>
                </div>
                <%-- END COL-MD-12 --%>
            </div>



        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
