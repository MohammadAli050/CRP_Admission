<%@ Page Title="Candidate Login Credentials" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="CandidateLoginCredentials.aspx.cs" Inherits="Admission.Admission.Office.CandidateLoginCredentials" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <link href="../../Content/formStyle.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h4>Candidate's Login Credentials</h4>
                </div>
                <div class="panel-body">
                    <table class="table_form table_fullwidth">
                        <tr>
                            <td style="width: 10%">Payment ID</td>
                            <td style="width: 25%">
                                <asp:TextBox ID="txtPaymentId" runat="server" CssClass="float-left" Width="100%" TextMode="Number"></asp:TextBox>
                            </td>
                            <%--<td>Mobile</td>
                        <td>
                            <asp:TextBox ID="txtMobileNo" runat="server" Enabled="true"></asp:TextBox>
                        </td>--%>
                            <td>
                                <asp:Button ID="btnSearch" runat="server" Text="Search by Payment Id" OnClick="btnSearch_Click"
                                    CssClass="float-left" />
                            </td>
                        </tr>
                        <tr>
                            <td>Mobile No</td>
                            <td>
                                <asp:TextBox ID="txtMobileNo" runat="server" CssClass="float-left" Width="100%" Text="+88"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Button ID="btnSearchMobile" runat="server" Text="Search by Mobile" OnClick="btnSearchMobile_Click"
                                    CssClass="float-left" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>

    <div class="row">
        <div class="col-md-12">
            <asp:ListView ID="lvCandidateCred" runat="server">
                <LayoutTemplate>
                    <table id="tbl"
                        class="table table-hover table-condensed table-striped"
                        style="width: 100%; text-align: left">
                        <tr runat="server" style="background-color: #1387de; color: white; font-size: small">
                            <th runat="server">SL#</th>
                            <th runat="server">Name</th>
                            <th runat="server">Mobile</th>
                            <th runat="server">Payment ID</th>
                            <th runat="server">Username</th>
                            <th runat="server">Password</th>
                            <th></th>
                        </tr>
                        <tr runat="server" id="itemPlaceholder" />
                    </table>
                </LayoutTemplate>
                <ItemTemplate>
                    <tr runat="server" style="font-size: smaller">
                        <td valign="middle" align="left" class="">
                            <asp:Label ID="lblSerial" runat="server" Text='<%# Container.DataItemIndex + 1 %>' />.
                        </td>
                        <td valign="middle" align="left" class="">
                            <asp:Label ID="lblName" runat="server" Text='<%# Eval("FirstName")%>' />
                        </td>
                        <td valign="middle" align="left" class="">
                            <asp:Label ID="Label1" runat="server" Text='<%# Eval("Mobile") %>' />
                        </td>
                        <td valign="middle" align="left" class="">
                            <asp:Label ID="Label2" runat="server" Text='<%# Eval("PaymentID") %>' />
                        </td>
                        <td valign="middle" align="left" class="">
                            <asp:Label ID="lblUserName" runat="server" Text='<%# Eval("UserName") %>' />
                        </td>
                        <td valign="middle" align="left" class="">
                            <asp:Label ID="lblPassword" runat="server" Text='<%# Eval("Password") %>' />
                        </td>
                        <td valign="middle" align="right" class=""></td>
                    </tr>
                </ItemTemplate>
                <EmptyDataTemplate>
                    <div class="alert alert-warning" role="alert" style="text-align: center">No item to display.</div>
                </EmptyDataTemplate>
            </asp:ListView>
        </div>
    </div>


</asp:Content>
