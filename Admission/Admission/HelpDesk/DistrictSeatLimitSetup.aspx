<%@ Page Title="" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="DistrictSeatLimitSetup.aspx.cs" Inherits="Admission.Admission.HelpDesk.DistrictSeatLimitSetup" %>
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
                                            <th runat="server">District</th>
                                            <th runat="server" style="text-align: center">Seat Limit</th>
                                            <th runat="server" style="text-align: center">Seat Fillup</th>
                                            <%--<th runat="server" style="text-align: center">Active?</th>--%>
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
                                        <%--<td valign="middle" align="right" class="">

                                            <asp:LinkButton CssClass="" ID="lnkEdit" runat="server">Edit</asp:LinkButton>
                                            |                      
                                        <asp:LinkButton CssClass="" ID="lnkDelete"
                                            OnClientClick="return confirm('Are you Confirm you want to Delete?');"
                                            runat="server">Delete</asp:LinkButton>
                                        </td>--%>
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
