<%@ Page Title="Verify/Complete Payment" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="VerifyPayment.aspx.cs" Inherits="Admission.Admission.VerifyPayment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">

    <link href="../Content/formStyle.css" rel="stylesheet" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">

        <div class="col-md-6">
            <div class="panel panel-info">
                <div class="panel-heading">
                    <strong>Complete Payment Process</strong>
                </div>
                <div class="panel-body">
                    <table class="table_form table_fullwidth">
                        <tr>
                            <td style="width: 20%">Payment ID</td>
                            <td>
                                <asp:TextBox ID="txtPaymentId" runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" Display="Dynamic"
                                    ControlToValidate="txtPaymentId" ErrorMessage="Required" ForeColor="Crimson" ValidationGroup="gr1"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>Mobile</td>
                            <td>
                                <asp:TextBox ID="txtMobile" runat="server" CssClass="form-control" placeholder="Format: +88017XXXXXXXX"></asp:TextBox>
                                <span style="color: darkorange; font-size: 9pt; display: none">Please include country code, eg: +8801700000000. 
                                    Candidate will not recieve Username and Password 
                                    if number is in wrong format.
                                </span>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2"
                                    ControlToValidate="txtMobile" Display="Dynamic" ForeColor="Crimson"
                                    ErrorMessage="Required" ValidationGroup="gr1"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator runat="server" ID="mobileReg"
                                    ValidationGroup="gr1"
                                    Font-Size="9pt"
                                    ForeColor="Crimson"
                                    Display="Dynamic"
                                    Font-Bold="False"
                                    ErrorMessage="Invalid format. "
                                    ControlToValidate="txtMobile"
                                    ValidationExpression="^(\+88)\d{11}$"></asp:RegularExpressionValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>Email</td>
                            <td>
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" Display="Dynamic" ForeColor="Crimson"
                                    ControlToValidate="txtEmail" ErrorMessage="Required" ValidationGroup="gr1"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="lblMessage" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <asp:Button ID="btnNext" runat="server" Text="Next" ValidationGroup="gr1" 
                        CssClass="btn btn-primary"
                        OnClick="btnNext_Click" />
                </div>
            </div>
        </div>

        <%--<div class="col-md-6" style="">
            <div class="panel panel-warning">
                <div class="panel-heading">
                    <strong>Verify bKash Payment</strong>
                </div>
                <div class="panel-body">
                    <table class="table_form table_fullwidth" style="margin-bottom: 1%">
                        <tr>
                            <td style="width: 30%">Transaction Id or TrxId</td>
                            <td>
                                <asp:TextBox ID="txtTrxId" runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="txtTrxIdReqV" runat="server" ControlToValidate="txtTrxId"
                                    Display="Dynamic" ErrorMessage="Required" ForeColor="Crimson" ValidationGroup="gr2"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                    </table>
                    <asp:Button ID="btnVerifyBkashTrans" runat="server" Text="Verify bKash Transaction" ValidationGroup="gr2" 
                        CssClass="btn btn-primary" OnClick="btnVerifyBkashTrans_Click" Enabled="false"/>
                    <br />
                    <asp:Panel ID="panelBkashMsg" runat="server" Visible="false">
                        Message : <asp:Label ID="lblbKashMsg" runat="server"></asp:Label>
                    </asp:Panel>
                </div>
            </div>
        </div>--%>

    </div>

</asp:Content>
