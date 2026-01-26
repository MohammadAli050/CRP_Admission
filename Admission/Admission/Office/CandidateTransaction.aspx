<%@ Page Title="Candidate Transaction" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="CandidateTransaction.aspx.cs" Inherits="Admission.Admission.Office.CandidateTransaction" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>


<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">

    <%--<link href="../../Content/formStyle.css" rel="stylesheet" />--%>



    <script type="text/javascript">

        function InProgress() {
            var panelProg = $get('divProgress');
            panelProg.style.display = '';
        }

        function onComplete() {
            var panelProg = $get('divProgress');
            panelProg.style.display = 'none';
        }

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


    <div id="divProgress" style="display: none; z-index: 1000; position: fixed; top: 50%; left: 50%; transform: translate(-50%, -50%);">
        <asp:Image ID="LoadingImage" runat="server" ImageUrl="~/Images/AppImg/t1.gif" Height="250px" Width="250px" />
    </div>

    <br />


    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <div class="row">
                <div class="col-md-6">
                    <div class="panel panel-default">

                        <td style="text-align: left;">
                            <td style="text-align: left; font-weight: bold">From Date</td>
                            <asp:TextBox ID="txtFromDate" runat="server" Width="30%"></asp:TextBox>
                            <ajaxToolkit:CalendarExtender ID="CalendarExtender3" runat="server"
                                TargetControlID="txtFromDate" Format="dd/MM/yyyy" />
                            <%--   <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                                ControlToValidate="txtFromDate" ErrorMessage="*" ForeColor="Crimson"
                                Font-Size="14pt" Display="Dynamic" Font-Bold="true"
                                ValidationGroup="gr1"></asp:RequiredFieldValidator>--%>


                            <td style="text-align: left; font-weight: bold">To Date</td>
                            <asp:TextBox ID="txtTodate" runat="server" Width="30%"></asp:TextBox>
                            <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server"
                                TargetControlID="txtTodate" Format="dd/MM/yyyy" />
                            <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                ControlToValidate="txtTodate" ErrorMessage="*" ForeColor="Crimson"
                                Font-Size="14pt" Display="Dynamic" Font-Bold="true"
                                ValidationGroup="gr1"></asp:RequiredFieldValidator>--%>
                            <%--   <asp:Button ID="LoadPaymentfail" runat="server" Text="Load" ValidationGroup="gr1"
                                    CssClass="btn btn-success"
                                    OnClick="LoadPaymentfail_Click" />&nbsp;--%>
                            <asp:Button ID="LoadPaymentfail" runat="server" Text="Load" OnClick="LoadPaymentfail_Click" /></td>
                        </td>
                                               <asp:Panel ID="PnlRegisteredCourse" runat="server" Width="100%" Wrap="False">
                                                   <div class=" panel panel-default pp" style="">
                                                       <div class="table-responsive " style="border-radius: 8px; overflow: scroll">
                                                           <asp:GridView ID="gvRegisteredCourse" runat="server" AutoGenerateColumns="False" TabIndex="6" CssClass="table-bordered"
                                                               Width="100%" CellPadding="4" ShowFooter="True" ForeColor="#333333" GridLines="None">
                                                               <FooterStyle BackColor="#434352" ForeColor="White" Height="30" Font-Bold="True" />
                                                               <HeaderStyle BackColor="#101257" Font-Bold="True" ForeColor="White" Height="40" />
                                                               <AlternatingRowStyle BackColor="#BBD6E6" />
                                                               <%--<PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                                        <RowStyle Height="25px" BackColor="#E3EAEB" />--%>
                                                               <Columns>
                                                                   <asp:TemplateField HeaderText="SL" ItemStyle-HorizontalAlign="Center">
                                                                       <ItemTemplate>
                                                                           <b><%# Container.DataItemIndex + 1 %></b>
                                                                       </ItemTemplate>
                                                                       <ItemStyle HorizontalAlign="Center" />
                                                                   </asp:TemplateField>
                                                                   <%-- <asp:TemplateField HeaderText="Term" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate><asp:Label runat="server" ID="lblCourseStatus" Font-Bold="True" Text='<%#Eval("SemesterNo") %>' /></ItemTemplate>
                                </asp:TemplateField>--%>
                                                                   <asp:TemplateField HeaderText="PaymentId" ItemStyle-HorizontalAlign="Center">
                                                                       <ItemTemplate>
                                                                           <asp:Label runat="server" ID="lblPaymentId" Font-Bold="True" Text='<%#Eval("PaymentId") %>' />
                                                                       </ItemTemplate>
                                                                       <ItemStyle HorizontalAlign="Center" />
                                                                   </asp:TemplateField>
                                                                   <asp:TemplateField HeaderText="Name" ItemStyle-HorizontalAlign="Center">
                                                                       <ItemTemplate>
                                                                           <asp:Label runat="server" ID="lblFirstName" Font-Bold="True" Text='<%#Eval("FirstName") %>' />
                                                                       </ItemTemplate>
                                                                       <ItemStyle HorizontalAlign="Center" />
                                                                   </asp:TemplateField>
                                                                   <asp:TemplateField HeaderText="Mobile" ItemStyle-HorizontalAlign="Left">
                                                                       <ItemTemplate>
                                                                           <asp:Label runat="server" ID="lblMobile" Font-Bold="True" Text='<%#Eval("Mobile") %>' />
                                                                       </ItemTemplate>
                                                                       <ItemStyle HorizontalAlign="Left" />
                                                                   </asp:TemplateField>
                                                                   <asp:TemplateField HeaderText="Email" ItemStyle-HorizontalAlign="Center">
                                                                       <ItemTemplate>
                                                                           <asp:Label runat="server" ID="lblEmail" Font-Bold="True" Text='<%#Eval("Email") %>' />
                                                                       </ItemTemplate>
                                                                       <ItemStyle HorizontalAlign="Center" />
                                                                   </asp:TemplateField>
                                                                   <asp:TemplateField HeaderText="Amount" ItemStyle-HorizontalAlign="Center">
                                                                       <ItemTemplate>
                                                                           <asp:Label runat="server" ID="lblAmount" Font-Bold="True" Text='<%#Eval("Amount") %>' />
                                                                       </ItemTemplate>
                                                                       <ItemStyle HorizontalAlign="Center" />
                                                                   </asp:TemplateField>

                                                               </Columns>
                                                               <EditRowStyle BackColor="#7C6F57" />
                                                               <EmptyDataTemplate>
                                                                   <b>No Data Found !</b>
                                                               </EmptyDataTemplate>
                                                               <%-- <RowStyle CssClass="rowCss" />
                            <HeaderStyle CssClass="tableHead" />--%>
                                                               <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                                                               <SortedAscendingCellStyle BackColor="#F8FAFA" />
                                                               <SortedAscendingHeaderStyle BackColor="#246B61" />
                                                               <SortedDescendingCellStyle BackColor="#D4DFE1" />
                                                               <SortedDescendingHeaderStyle BackColor="#15524A" />
                                                           </asp:GridView>
                                                       </div>
                                                   </div>
                                               </asp:Panel>
                        <div class="panel-heading">
                            <strong>Insert Candidate Transaction (SSL)</strong>
                        </div>
                        <div class="panel-body">
                            <%--<asp:UpdatePanel ID="updatePanel_Form" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>--%>
                            <div>
                                <p style="color: crimson; font-weight: bold">
                                    Please check the name of the candidate, amount and other information before saving.
                                </p>
                            </div>
                            <table class="table_form table_fullwidth">
                                <tr>
                                    <td style="width: 20%">Payment/Transaction ID:</td>
                                    <td style="width: 80%">
                                        <asp:TextBox ID="txtPaymentId" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">Note: Search will only return candidates who haven't paid.
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" /></td>
                                    <td>
                                        <asp:Label ID="lblSearchMsg" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Status</td>
                                    <td>
                                        <asp:TextBox ID="txtStatus" runat="server" Text="VALID" Width="95%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Transaction Date</td>
                                    <td>
                                        <asp:TextBox ID="txtTransactionDate" runat="server" Width="95%"></asp:TextBox>
                                    </td>
                                </tr>
                                <%--<tr>
                            <td>Bank</td>
                            <td>
                                <asp:Textbox ID="txtBank" runat="server" Width="95%"></asp:Textbox>
                            </td>
                        </tr>--%>
                                <tr>
                                    <td>Bank Transaction ID</td>
                                    <td>
                                        <asp:TextBox ID="txtBankTransactionId" runat="server" Width="95%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Validation ID</td>
                                    <td>
                                        <asp:TextBox ID="txtValidationId" runat="server" Width="95%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Amount</td>
                                    <td>
                                        <asp:TextBox ID="txtAmount" runat="server" Width="95%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Receivable</td>
                                    <td>
                                        <asp:TextBox ID="txtStoreAmount" runat="server" Width="95%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Card Type</td>
                                    <td>
                                        <asp:TextBox ID="txtCardType" runat="server" Width="95%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Card Number</td>
                                    <td>
                                        <asp:TextBox ID="txtCardNumber" runat="server" Width="95%"></asp:TextBox>
                                    </td>
                                </tr>
                                <%--<tr>
                            <td>Card Holder Name</td>
                            <td>
                                <asp:TextBox ID="txtCardHolderName" runat="server" Width="95%"></asp:TextBox>
                            </td>
                        </tr>--%>
                                <tr>
                                    <td>Issuer Bank Or Country</td>
                                    <td>
                                        <asp:TextBox ID="txtIssuerBankCountry" runat="server" Width="95%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Currency</td>
                                    <td>
                                        <asp:TextBox ID="txtCurrency" runat="server" Text="BDT" Width="95%"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <asp:Panel ID="panel_Message" runat="server" Visible="false">
                                <asp:Label ID="lblMessage" runat="server"></asp:Label>
                            </asp:Panel>
                            <asp:Button ID="btnSave" runat="server" Text="Confirm Payment SSL"
                                CssClass="btn btn-danger"
                                OnClientClick="return confirm('Are you sure, you want to Update Payment from SSL')" Visible="false" OnClick="btnSave_Click" />

                            <%--</ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnCopy" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>--%>
                        </div>
                    </div>
                </div>
                <%-- END COL-MD-6 --%>
                <div class="col-md-offset-6">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <strong>Candidate Transaction Info From Gateway (SSL)</strong>
                        </div>
                        <div class="panel-body" style="margin-bottom: -5.5%;">
                            <div>
                                <p>
                                    Please check the name of the candidate, amount and other information before saving.
                                </p>
                            </div>
                            <asp:Panel ID="panel_Message1" runat="server" Visible="true">
                                Message:
                        <asp:Label ID="lblMessage1" runat="server"></asp:Label>
                            </asp:Panel>

                            <table class="table_form table_fullwidth">
                                <tr>
                                    <td style="width: 30%">Candidate Name:</td>
                                    <td style="width: 70%">
                                        <asp:Label ID="lblCandidateName" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 30%">Payment/Transaction ID:</td>
                                    <td style="width: 70%">
                                        <asp:Label ID="lblTranId" runat="server"></asp:Label>
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
                                <tr>
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
                                        <asp:Label ID="lblCurrency" runat="server" Text="BDT" Width="95%"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <%--<asp:Button ID="btnCopy" runat="server" Text="Copy And Paste" Visible="false" OnClick="btnCopy_Click" />--%>
                        </div>
                    </div>
                </div>
            </div>



            <div class="row" style="display: none;">

                <div class="col-md-6">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <strong>Insert Candidate Transaction (bKash)</strong>
                        </div>
                        <div class="panel-body">
                            <div>
                                <p style="color: crimson; font-weight: bold">
                                    Please check the name of the candidate, amount and other information before saving.
                                </p>
                            </div>
                            <table class="table_form table_fullwidth">
                                <tr>
                                    <td style="width: 20%">Trx ID:</td>
                                    <td style="width: 70%">
                                        <asp:TextBox ID="txtTrxIdB" runat="server" Width="95%"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnLoadB" runat="server" Text="Load" OnClick="btnLoadB_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 30%">Name</td>
                                    <td style="width: 70%">
                                        <asp:TextBox ID="txtCandidateNameB" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>System Amount</td>
                                    <td>
                                        <asp:TextBox ID="txtSystemAmountB" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Status Code</td>
                                    <td>
                                        <asp:TextBox ID="txtStatusCodeB" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Status Details</td>
                                    <td>
                                        <asp:TextBox ID="txtStatusDetailsB" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Trx Id</td>
                                    <td>
                                        <asp:TextBox ID="txtTrxIdB_1" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Reference No./Payment Id</td>
                                    <td>
                                        <asp:TextBox ID="txtPaymentIdB" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Counter</td>
                                    <td>
                                        <asp:TextBox ID="txtCounterB" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Paid Amount</td>
                                    <td>
                                        <asp:TextBox ID="txtPaidAmountB" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Reversed</td>
                                    <td>
                                        <asp:TextBox ID="txtReversedB" runat="server" Enabled="false"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Sender</td>
                                    <td>
                                        <asp:TextBox ID="txtSenderB" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Service</td>
                                    <td>
                                        <asp:TextBox ID="txtServiceB" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Currency</td>
                                    <td>
                                        <asp:TextBox ID="txtCurrencyB" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Receiver</td>
                                    <td>
                                        <asp:TextBox ID="txtReceiverB" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Trx Timestamp</td>
                                    <td>
                                        <asp:TextBox ID="txtTrxTimestampB" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <asp:Panel ID="panelMessageB" runat="server" Visible="false">
                                <asp:Label ID="lblMessageB" runat="server"></asp:Label>
                            </asp:Panel>
                            <asp:Button ID="btnSaveB" runat="server" Text="Save" Visible="false" OnClick="btnSaveB_Click" />
                        </div>
                    </div>
                </div>

                <div class="col-md-offset-6">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <strong>Candidate Transaction Info From Gateway (bKash)</strong>
                        </div>
                        <div class="panel-body">
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
                                <%--<tr>
                                    <td>Reversed</td>
                                    <td>
                                        <asp:Label ID="lblReveredB" runat="server"></asp:Label>
                                    </td>
                                </tr>--%>
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
                        </div>
                    </div>
                </div>
            </div>



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

                                <div class="row">
                                    <div class="col-sm-12 col-md-12 col-lg-12">
                                        <asp:Button ID="btnUpdatePaymentFromEkPay" runat="server" Text="Confirm Payment EkPay"
                                            CssClass="btn btn-danger"
                                            OnClientClick="return confirm('Are you sure, you want to Update Payment from EkPay')"
                                            OnClick="btnUpdatePaymentFromEkPay_Click" />
                                    </div>
                                </div>

                            </asp:Panel>


                        </div>
                    </div>

                </div>
            </div>



            <div class="row">
                <div class="col-sm-12 col-md-12 col-lg-12">

                    <div class="panel panel-default">
                        <div class="panel-heading text-center"><b>EkPay Unpaid Report</b></div>
                        <div class="panel-body">

                            <div class="row">
                                <div class="col-sm-6 col-md-6 col-lg-6">
                                    <div class="form-group">
                                        <label><strong>Session <span style="color: crimson; font-weight: bold;">*</span></strong></label>
                                        <asp:DropDownList ID="ddlSession" runat="server" Width="100%" CssClass="form-control"></asp:DropDownList>
                                        <asp:CompareValidator ID="ddlSessionComV" runat="server" Display="Dynamic"
                                            ErrorMessage="Required" ForeColor="Crimson" ControlToValidate="ddlSession"
                                            Font-Size="10pt"
                                            ValueToCompare="-1" Operator="NotEqual" ValidationGroup="loadVg"></asp:CompareValidator>
                                    </div>
                                </div>
                                <div class="col-sm-6 col-md-6 col-lg-6">
                                    <br />
                                    <asp:Button ID="btnEkPayUnPaidCandidate" runat="server" Text="Load" CssClass="btn btn-info" Width="100%"
                                        ValidationGroup="loadVg" OnClick="btnEkPayUnPaidCandidate_Click" />
                                </div>
                            </div>

                            <asp:Panel ID="panelCheckEkPayPaymentUpdate" runat="server" Visible="false">
                                <div class="row">
                                    <div class="col-sm-6 col-md-6 col-lg-6">
                                    </div>
                                    <div class="col-sm-6 col-md-6 col-lg-6">
                                        <br />
                                        <asp:Button ID="btnCheckForPaymentUpdateInEkPay" runat="server" Text="Check For Payment Update In EkPay"
                                            CssClass="btn btn-danger" Width="100%"
                                            OnClick="btnCheckForPaymentUpdateInEkPay_Click" />
                                    </div>
                                </div>
                            </asp:Panel>

                            <hr />

                            <div>
                                <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana"
                                    Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt"
                                    AsyncRendering="true" Width="100%" SizeToReportContent="true" Visible="true">
                                    <LocalReport ReportPath="Admission/Office/RPTEkPayUnPaidCandidateInfo.rdlc" EnableExternalImages="true">
                                    </LocalReport>
                                </rsweb:ReportViewer>
                            </div>



                        </div>
                    </div>

                </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>










    <ajaxToolkit:UpdatePanelAnimationExtender ID="UpdatePanelAnimationExtender1" TargetControlID="UpdatePanel1" runat="server">
        <Animations>
            <OnUpdating>
                <Parallel duration="0">
                    <ScriptAction Script="InProgress();" />
                    <EnableAction AnimationTarget="btnLoad" Enabled="false" />
                    <EnableAction AnimationTarget="btnEkPayUnPaidCandidate" Enabled="false" />
                </Parallel>
            </OnUpdating>
            <OnUpdated>
                <Parallel duration="0">
                    <ScriptAction Script="onComplete();" />
                    <EnableAction   AnimationTarget="btnLoad" Enabled="true" />
                    <EnableAction   AnimationTarget="btnEkPayUnPaidCandidate" Enabled="true" />
                </Parallel>
            </OnUpdated>
        </Animations>
    </ajaxToolkit:UpdatePanelAnimationExtender>







</asp:Content>
