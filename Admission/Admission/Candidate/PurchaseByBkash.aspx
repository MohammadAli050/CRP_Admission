<%@ Page Title="Pay By bKash" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PurchaseByBkash.aspx.cs" Inherits="Admission.Admission.Candidate.PurchaseByBkash" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <strong>Pay by bKash</strong>
                </div>
                <div class="panel-body">
                    <asp:Panel ID="contentPanel" runat="server">
                        <table>
                            <tr>
                                <td style="width: 50%; vertical-align: top">
                                    <strong style="color: crimson">Important</strong><br />
                                    <ul>
                                        <li>Transactions can only be done using Personal bKash Account.</li>
                                        <li>The transaction has to be made using the Payment option from bKash menu.</li>
                                        <li>10 digits Payment ID numbers must be used in the Reference section. No symbols, space or punctuation marks can be used.</li>
                                        <li>For counter number, individuals always has to input “1” in the designated section.</li>
                                    </ul>
                                    <br />
                                    <div class="panel panel-warning">
                                        <div class="panel-heading">
                                            <strong>How to Pay:</strong>
                                        </div>
                                        <div class="panel-body">
                                            <ol>
                                                <li>Dial *247#</li>
                                                <li>Select 'Payment' option.</li>
                                                <li>Enter Merchant bKash Account No: <strong style="font-size: large; color: teal">01769028780</strong></li>
                                                <li>Enter amount:
                                                <asp:Label ID="lblAmount" runat="server" Text="" Font-Bold="true" Font-Size="Large" ForeColor="Teal"></asp:Label></li>
                                                <li>Enter reference:
                                                <asp:Label ID="lblPaymentId" runat="server" Font-Bold="true" Font-Size="Large" ForeColor="Teal"></asp:Label></li>
                                                <li>Enter counter number: <strong style="font-size: large; color: teal">1</strong></li>
                                                <li>Enter your PIN number.</li>
                                            </ol>
                                            <ul>
                                                <li>Once you have successfully made the payment, you will receive an SMS with your payment details.
                                                </li>
                                                <li>Go to <a href="../Home.aspx" target="_blank">Home Page</a> and click <a href="../VerifyPayment.aspx" target="_blank">Verify/Complete Payment</a>.
                                                </li>
                                                <li>Enter your <strong>TrxID</strong>, that you have received via SMS, in the 
                                                <strong>Verify or Complete Payment Made By <span style="color: crimson">bKash</span></strong> section on the right.
                                                </li>
                                                <li>Click <strong>Verify bKash Transaction</strong> to verify your payment.
                                                </li>
                                            </ul>
                                            <br />
                                            <span style="color:crimson;">Please make sure to enter the information correctly while making payment. BUP will not take any responsibility for any mistake.</span>
                                        </div>
                                    </div>

                                </td>
                                <td style="width: 50%; padding-left: 1%; vertical-align: top">
                                    <img src="../../ApplicationDocs/HowToPayByBkash.PNG" style="width: 95%; border: 1px solid lightgray;" />
                                </td>
                            </tr>

                        </table>
                    </asp:Panel>
                    <asp:Panel ID="messagePanel" runat="server">
                        <asp:Label ID="lblMessage" runat="server"></asp:Label>
                    </asp:Panel>

                </div>
            </div>
        </div>
    </div>

</asp:Content>
