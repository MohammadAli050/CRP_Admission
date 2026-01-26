<%@ Page Title="" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="Country.aspx.cs" Inherits="Admission.Admission.Admin.Country" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">

    <style type="text/css">
        table {
            width: 100%;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-md-7">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <strong>Country Setup</strong>
                </div>
                <div class="panel-body">
                    <asp:UpdatePanel ID="updatePanel_CountrySetup" runat="server">
                        <ContentTemplate>
                            <asp:Label ID="lblMessage" runat="server" Visible="false" Text=""></asp:Label>
                            <table>
                                <tr>
                                    <td style="width:17%">Country Name &nbsp;</td>
                                    <td style="width:46%">
                                        <asp:TextBox ID="txtCountryName" runat="server" 
                                            Width="100%"></asp:TextBox>&nbsp;
                                    </td>
                                    <td style="width:10%">
                                        <asp:Button ID="btnSave" runat="server" Text="Save" 
                                            ValidationGroup="g1"/>
                                    </td>
                                    <td>
                                        <asp:RequiredFieldValidator ID="req_txtCountryName" runat="server"
                                            ControlToValidate="txtCountryName"
                                            ErrorMessage="Country Name is required" ForeColor="Crimson"
                                            ValidationGroup="g1" Display="Dynamic">
                                        </asp:RequiredFieldValidator>
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
                    Available Countries
                </div>
                <div class="panel-body">
                    <asp:UpdatePanel ID="updatePanel_CountryList" runat="server">
                        <ContentTemplate>
                            <asp:ListView ID="lvCountryList" runat="server">
                                <LayoutTemplate>
                                    <table class="table table-responsive table-hover"
                                        style="width: 100%; text-align: left;">
                                        <tr>
                                            <th runat="server">Country Name</th>
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
                                            <asp:Label ID="lblCountryName" runat="server"></asp:Label>
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
