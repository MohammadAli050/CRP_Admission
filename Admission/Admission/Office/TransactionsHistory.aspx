<%@ Page Title="Transaction History" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="TransactionsHistory.aspx.cs" Inherits="Admission.Admission.Office.TransactionsHistory" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <link href="../../Content/formStyle.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h4>Transaction History</h4>
                </div>
                <div class="panel-body">
                    <table class="table_form table_fullwidth">
                        <tr>
                            <td style="width: 10%">Payment ID</td>
                            <td style="width: 90%">
                                <asp:TextBox ID="txtPaymentId" runat="server" Width="25%"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                    ErrorMessage="Required" ControlToValidate="txtPaymentId" Display="Dynamic"
                                    ValidationGroup="gr1"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td>
                                <asp:Button ID="btnSearch" runat="server" Text="Search" ValidationGroup="gr1"
                                    OnClick="btnSearch_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>


    <div class="row">
        <div class="panel panel-default">
            <div class="panel-heading">
                <asp:Label ID="lblPanelHeading" runat="server" Font-Bold="true"></asp:Label><br />
                Records: <asp:Label ID="lblCount" runat="server" CssClass="badge"></asp:Label>
            </div>
            <div class="panel-body">
                
                <asp:Panel ID="panel_transactionHistory" runat="server">

                    <asp:ListView ID="lvTransHistory" runat="server"
                        OnItemDataBound="lvTransHistory_ItemDataBound">
                        <LayoutTemplate>
                            <table id="tbl"
                                class="table table-hover table-condensed table-striped"
                                style="width: 120%; text-align: left">
                                <tr runat="server" style="background-color: #1387de; color: white;">
                                    <th runat="server">SL#</th>
                                    <th runat="server">Candidate Name</th>
                                    <th runat="server">Status</th>
                                    <th runat="server">Trans. Date</th>
                                    <th runat="server">TransactionID/<br />PaymentID</th>
                                    <th runat="server">Amount</th>
                                    <th runat="server">Bank TransactionID</th>
                                    <th runat="server">Card Type</th>
                                    <th runat="server">Card Number</th>
                                    <th runat="server">Card Brand</th>
                                    <th runat="server">Card Issuer<br />Country</th>
                                    <th runat="server">Manually<br />Inserted?</th>
                                </tr>
                                <tr runat="server" id="itemPlaceholder" />
                            </table>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr runat="server">
                                <td valign="middle" align="left" class="">
                                    <asp:Label ID="lblSerial" runat="server" /> .
                                </td>
                                <td valign="middle" align="left" class="">
                                    <asp:Label ID="lblCandidateName" runat="server"/>
                                </td>
                                <td valign="middle" align="left" class="">
                                    <asp:Label ID="lblStatus" runat="server" Font-Size="Smaller"/>
                                </td>
                                <td valign="middle" align="left" class="">
                                    <asp:Label ID="lblTransDate" runat="server" Font-Size="Small"/>
                                </td>
                                <td valign="middle" align="left" class="">
                                    <asp:Label ID="lblTransactionId" runat="server" Font-Size="Smaller"/>
                                </td>
                                <td valign="middle" align="left" class="">
                                    <asp:Label ID="lblAmount" runat="server" Font-Size="Smaller"/>
                                </td>
                                <td valign="middle" align="left" class="">
                                    <asp:Label ID="lblBankTransId" runat="server" Font-Size="Smaller"/>
                                </td>
                                <td valign="middle" align="left" class="">
                                    <asp:Label ID="lblCardType" runat="server" Font-Size="Smaller"/>
                                </td>
                                <td valign="middle" align="left" class="">
                                    <asp:Label ID="lblCardNumber" runat="server" Font-Size="Smaller"/>
                                </td>
                                <td valign="middle" align="left" class="">
                                    <asp:Label ID="lblCardBrand" runat="server" Font-Size="Smaller"/>
                                </td>
                                <td valign="middle" align="left" class="">
                                    <asp:Label ID="lblCardIssuerCountry" runat="server" Font-Size="Smaller"/>
                                </td>
                                <td valign="middle" align="left" class="">
                                    <asp:Label ID="lblIsManualInsert" runat="server" Font-Size="Small"/>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <EmptyDataTemplate>
                            <div class="alert alert-warning" role="alert" style="text-align: center">No item to display.</div>
                        </EmptyDataTemplate>
                    </asp:ListView>

                </asp:Panel>

            </div>
        </div>
    </div>
</asp:Content>
