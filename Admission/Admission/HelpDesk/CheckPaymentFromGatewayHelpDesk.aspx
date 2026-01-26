<%@ Page Title="Check Payment From Gateway Help Desk" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="CheckPaymentFromGatewayHelpDesk.aspx.cs" Inherits="Admission.Admission.HelpDesk.CheckPaymentFromGatewayHelpDesk" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <link href="../../Content/formStyle.css" rel="stylesheet" />
    <style type="text/css">
        .auto-style1 {
            height: 24px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-md-6">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <strong>Payment Status</strong>
                </div>
                <div class="panel-body">
                    <asp:UpdatePanel ID="updatePanelFilterSSL" runat="server" UpdateMode="Always">
                        <ContentTemplate>

                            <table class="table_form table_fullwidth">
                                <tr>
                                    <td style="width: 15%">Payment ID:</td>
                                    <td style="width: 50%">
                                        <asp:TextBox ID="txtPaymentIdSsl" runat="server" Width="100%"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="txtPaymentIdSslREV" runat="server"
                                            ControlToValidate="txtPaymentIdSsl" ErrorMessage="Required" Font-Size="Small"
                                            ForeColor="Crimson" ValidationGroup="gr1" Display="Dynamic">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                    <td style="width: 35%">
                                        <asp:Button ID="btnLoadSsl" runat="server" Text="Search" ValidationGroup="gr1"
                                            OnClick="btnLoadSsl_Click" />
                                    </td>
                                </tr>
                            </table>
                            <br />
                             <asp:Label ID="lblSearchMsg" runat="server"></asp:Label>
                           <%-- <asp:Panel ID="messagePanel" runat="server" Visible="false">
                                <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                                <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                                    <span aria-hidden="true">&times;</span>
                                </button>
                            </asp:Panel>--%>
                            <table class="table_form table_fullwidth">
                                <tr>
                                    <td style="width: 30%">Payment ID</td>
                                    <td style="width: 70%">
                                        <asp:Label ID="lblTranId" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Candidate Name</td>
                                    <td>
                                        <asp:Label ID="lblCandidateName" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Status</td>
                                    <td>
                                        <asp:Label ID="lblStatus" runat="server" Width="95%"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Transaction Date</td>
                                    <td>
                                        <asp:Label ID="lblTransDate" runat="server" Width="95%"></asp:Label>
                                    </td>
                                </tr>
                                <%--<tr>
                                    <td>Bank</td>
                                    <td>
                                            <asp:Label ID="txtBank" runat="server" Width="95%"></asp:Label>
                                    </td>
                                </tr>--%>
                                <tr>
                                    <td>Bank Transaction ID</td>
                                    <td>
                                        <asp:Label ID="lblBankTranId" runat="server" Width="95%"></asp:Label>
                                    </td>
                                </tr>
                                <tr style="display: none">
                                    <td>Validation ID</td>
                                    <td>
                                        <asp:Label ID="lblValId" runat="server" Width="95%"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Amount</td>
                                    <td>
                                        <asp:Label ID="lblAmount" runat="server" Width="95%"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Receivable</td>
                                    <td>
                                        <asp:Label ID="lblReceivable" runat="server" Width="95%"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Card Type</td>
                                    <td>
                                        <asp:Label ID="lblCardType" runat="server" Width="95%"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Card Number</td>
                                    <td>
                                        <asp:Label ID="lblCardNumber" runat="server" Width="95%"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Issuer Bank Or Country</td>
                                    <td>
                                        <asp:Label ID="lblIssuerBankCountry" runat="server" Width="95%"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Currency</td>
                                    <td>
                                        <asp:Label ID="lblCurrency" runat="server" Text="" Width="95%"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>


        <%--<tr>
                <td style="width: 15%">Payment ID:</td>
                <td style="width: 50%">
                    <asp:TextBox ID="txtPaymentIdBkash" runat="server" Width="100%"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="txtPaymentIdBkashREV" runat="server"
                        ControlToValidate="txtPaymentIdBkash" ErrorMessage="Required" Font-Size="Small"
                        ForeColor="Crimson" ValidationGroup="gr2" Display="Dynamic">
                    </asp:RequiredFieldValidator>
                </td>
                <td style="width: 35%">
                    <asp:Button ID="btnLoadPaymentBkash" runat="server" Text="Search" ValidationGroup="gr2"
                        OnClick="btnLoadPaymentBkash_Click" CssClass="float-left" />
                </td>
            </tr>--%>

        <%--<tr>
            <td>Reversed</td>
            <td>
                <asp:Label ID="lblReveredB" runat="server"></asp:Label>
            </td>
        </tr>--%>

        <%-- <div class="col-md-offset-6" style="">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <strong>bKash</strong>
                </div>
                <div class="panel-body">
                    <asp:UpdatePanel ID="updatePanelFilterBkash" runat="server" UpdateMode="Always">
                        <ContentTemplate>

                            <table class="table_form table_fullwidth">
                                
                                <tr>
                                    <td>Trx ID:</td>
                                    <td>
                                        <asp:TextBox ID="txtTrxIdBkash" runat="server" Width="100%"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="txtTrxIdBkashREV" runat="server"
                                            ControlToValidate="txtTrxIdBkash" ErrorMessage="Required" Font-Size="Small"
                                            ForeColor="Crimson" ValidationGroup="gr3" Display="Dynamic">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnLoadTrxBkash" runat="server" Text="Search" ValidationGroup="gr3"
                                            OnClick="btnLoadTrxBkash_Click" />
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <asp:Panel ID="panel_Message2" runat="server" Visible="false" CssClass="alert alert-info">
                                Message: <asp:Label ID="lblMessageBkash" runat="server"></asp:Label>
                            </asp:Panel>
                            <table class="table_form table_fullwidth">
                                <tr>
                                    <td style="width: 30%">Name</td>
                                    <td style="width: 70%">
                                        <asp:Label ID="lblNameB" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>System Amount</td>
                                    <td>
                                        <asp:Label ID="lblSysAmntB" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Status Code</td>
                                    <td>
                                        <asp:Label ID="lblStatusCodeB" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Status Details</td>
                                    <td>
                                        <asp:Label ID="lblStatusDetailsB" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Trx Id</td>
                                    <td>
                                        <asp:Label ID="lblTrxIdB" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Reference No./Payment Id</td>
                                    <td>
                                        <asp:Label ID="lblReferenceNoB" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Counter</td>
                                    <td>
                                        <asp:Label ID="lblCounterB" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Paid Amount</td>
                                    <td>
                                        <asp:Label ID="lblPaidAmntB" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                
                                <tr>
                                    <td>Sender</td>
                                    <td>
                                        <asp:Label ID="lblSenderB" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Service</td>
                                    <td>
                                        <asp:Label ID="lblServiceB" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Currency</td>
                                    <td>
                                        <asp:Label ID="lblCurrencyB" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Receiver</td>
                                    <td>
                                        <asp:Label ID="lblReceiverB" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>

                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>--%>
    </div>

    <%-- ---------------------------------------------------------------------------------- --%>


    <asp:UpdatePanel ID="upEkPay" runat="server">
        <ContentTemplate>

            <div class="row">
                <div class="col-sm-12 col-md-12 col-lg-12">

                    <div class="panel panel-default">
                        <div class="panel-heading text-center">
                            <h4 style="margin: 0;"><b>EkPay</b></h4>
                        </div>
                        <div class="panel-body">



                            <div class="row">
                                <div class="col-sm-3 col-md-3 col-lg-3">
                                    <div class="form-group">
                                        <label><strong>PaymentId <span style="color: crimson; font-weight: bold;">*</span></strong></label>
                                        <asp:TextBox ID="txtEkPaySearchPaymentId" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="txtPlaceOfBirth_ReqV" runat="server"
                                            ControlToValidate="txtEkPaySearchPaymentId"
                                            ErrorMessage="Required"
                                            ForeColor="Crimson"
                                            Display="Dynamic"
                                            Font-Size="9pt"
                                            Font-Bold="true"
                                            ValidationGroup="gr1">
                                        </asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="col-sm-3 col-md-3 col-lg-3">
                                    <br />
                                    <asp:Button ID="btnSearchByEkPay" runat="server" Text="Search"
                                        CssClass="btn btn-default" ValidationGroup="gr1"
                                        OnClick="btnSearchByEkPay_Click" />
                                </div>
                                <div class="col-sm-3 col-md-3 col-lg-3">
                                    <div class="form-group">
                                        <label><strong>TransactionId </strong></label>
                                        <asp:TextBox ID="txtEkPaySearchTransactionId" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-sm-3 col-md-3 col-lg-3">
                                    <br />
                                    <asp:Button ID="btnSearchByEkPayTransactionId" runat="server" Text="Search"
                                        CssClass="btn btn-default"
                                        OnClick="btnSearchByEkPayTransactionId_Click" />
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-sm-12 col-md-12 col-lg-12">
                                    <asp:Panel ID="messagePanelEkPay" runat="server" Visible="false">
                                        <asp:Label ID="lblMessageEkPay" runat="server" Text=""></asp:Label>
                                    </asp:Panel>
                                </div>
                            </div>

                            <hr />

                            <asp:Panel ID="panelEkPayGetInfoView" runat="server" Visible="false">


                                <div class="row">
                                    <div class="col-sm-6 col-md-6 col-lg-6">
                                        <div class="form-group">
                                            <label><strong>PaymentId </strong></label>
                                            <asp:Label ID="lblEkPayPaymentId" runat="server" CssClass="form-control" Width="100%"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="col-sm-6 col-md-6 col-lg-6">
                                        <div class="form-group">
                                            <label><strong>TransactionId </strong></label>
                                            <asp:Label ID="lblEkPayTransactionId" runat="server" CssClass="form-control" Width="100%"></asp:Label>
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-sm-4 col-md-4 col-lg-4">
                                        <div class="form-group">
                                            <label><strong>Name </strong></label>
                                            <asp:Label ID="lblEkPayName" runat="server" CssClass="form-control" Width="100%"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="col-sm-4 col-md-4 col-lg-4">
                                        <div class="form-group">
                                            <label><strong>Phone </strong></label>
                                            <asp:Label ID="lblEkPayPhone" runat="server" CssClass="form-control" Width="100%"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="col-sm-4 col-md-4 col-lg-4">
                                        <div class="form-group">
                                            <label><strong>Email </strong></label>
                                            <asp:Label ID="lblEkPayEmail" runat="server" CssClass="form-control" Width="100%"></asp:Label>
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-sm-4 col-md-4 col-lg-4">
                                        <div class="form-group">
                                            <label><strong>Payment Date </strong></label>
                                            <asp:Label ID="lblEkPayPaymentDate" runat="server" CssClass="form-control" Width="100%"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="col-sm-4 col-md-4 col-lg-4">
                                        <div class="form-group">
                                            <label><strong>Payment Type </strong></label>
                                            <asp:Label ID="lblEkPayPaymentType" runat="server" CssClass="form-control" Width="100%"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="col-sm-4 col-md-4 col-lg-4">
                                        <div class="form-group">
                                            <label><strong>Payment Gateway </strong></label>
                                            <asp:Label ID="lblEkPayPaymentGateway" runat="server" CssClass="form-control" Width="100%"></asp:Label>
                                        </div>
                                    </div>
                                </div>


                            </asp:Panel>


                        </div>
                    </div>

                </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>




</asp:Content>
