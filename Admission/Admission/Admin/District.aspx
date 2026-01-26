<%@ Page Title="" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="District.aspx.cs" Inherits="Admission.Admission.Admin.District" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">

    <style type="text/css">
        table {
            width: 100%;
        }

        td {
            vertical-align: top;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <strong>District Setup</strong>
                </div>
                <div class="panel-body">
                    <asp:UpdatePanel ID="updatePanel_DistrictSetup" runat="server">
                        <ContentTemplate>
                            <table>
                                <tr>
                                    <td></td>
                                    <td>
                                        <asp:RequiredFieldValidator ID="req_txtDistrictName" runat="server"
                                            ControlToValidate="txtDistrictName" Font-Size="Smaller"
                                            ErrorMessage="District Name is required" ForeColor="Crimson"
                                            ValidationGroup="g1" Display="Dynamic">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                    <td></td>
                                    <td>
                                        <asp:CompareValidator ID="comV_ddlDivision" runat="server"
                                            ControlToValidate="ddlDivision" ErrorMessage="Select a Division"
                                            ForeColor="Crimson" Font-Size="Smaller" ValueToCompare="-1"
                                            Operator="NotEqual" Display="Dynamic" ValidationGroup="g1"></asp:CompareValidator>
                                    </td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td style="width: 8%">District Name</td>
                                    <td style="width: 26%">
                                        <asp:TextBox ID="txtDistrictName" runat="server"
                                            Width="100%"></asp:TextBox>
                                    </td>
                                    <td style="width: 5%">Division</td>
                                    <td style="width: 26%">
                                        <asp:DropDownList ID="ddlDivision" runat="server"
                                            Width="100%">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 6%">Remarks</td>
                                    <td style="width: 29%">
                                        <asp:TextBox ID="txtDescription" runat="server"
                                            TextMode="MultiLine" Rows="2" Width="100%">
                                        </asp:TextBox>
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
        <%-- END COL-MD-12 --%>
    </div>
    <%-- END ROW --%>
    <%-- ------------------------------------------------------------------------------------------ --%>
    <div class="row">
        <div class="col-md-6">
            <div class="panel panel-default">
                <div class="panel-heading">
                    Available Districts
                </div>
                <div class="panel-body">
                    <asp:UpdatePanel ID="updatePanel_DistrictList" runat="server">
                        <ContentTemplate>
                            <asp:ListView ID="lvDistrictList" runat="server">
                                <LayoutTemplate>
                                    <table class="table table-responsive table-hover"
                                        style="width: 100%; text-align: left;">
                                        <tr>
                                            <th runat="server">District</th>
                                            <th runat="server">Division</th>
                                            <th runat="server">Remarks</th>
                                            <th></th>
                                        </tr>
                                        <tr runat="server" id="itemPlaceholder"></tr>
                                    </table>
                                    <asp:DataPager runat="server" ID="dataPager" PageSize="50">
                                        <Fields>
                                            <asp:NumericPagerField PreviousPageText="<--"
                                                NextPageText="-->" />
                                        </Fields>
                                    </asp:DataPager>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <tr runat="server">
                                        <td valign="middle" align="left"
                                            class="">
                                            <asp:Label ID="lblDistrictName" runat="server"></asp:Label>
                                        </td>
                                        <td valign="middle" align="left"
                                            class="">
                                            <asp:Label ID="lblDivision" runat="server"></asp:Label>
                                        </td>
                                        <td valign="middle" align="left"
                                            class="">
                                            <asp:Label ID="lblDescription" runat="server"></asp:Label>
                                        </td>
                                        <td valign="middle" align="left"
                                            class="">
                                            <asp:LinkButton ID="lnkEdit" runat="server">Edit</asp:LinkButton>
                                            |
                                            <asp:LinkButton ID="lnkDelete" runat="server" 
                                                OnClientClick="return confirm('Are you sure you want to delete?');">
                                                Delete
                                            </asp:LinkButton>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <EmptyDataTemplate>
                                    <div class="alert alert-warning" role="alert">
                                        No item to display...
                                    </div>
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
    <%-- END ROW --%>

</asp:Content>
