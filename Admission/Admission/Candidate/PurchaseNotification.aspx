<%@ Page Title="Purchase Notification" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PurchaseNotification.aspx.cs" Inherits="Admission.Admission.Candidate.PurchaseNotification" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <style>
        .panel {
            /*border: 1px solid #f4511e;*/
            border-radius: 5px !important;
            transition: box-shadow 0.5s;
        }

            .panel:hover {
                box-shadow: 5px 0px 40px rgba(0,0,0, .2);
            }



        .panel-footer .btn:hover {
            border: 1px solid #f4511e;
            background-color: #fff !important;
            color: #f4511e;
        }

        /*.panel-heading {
    color: #fff !important;
    background-color: #f4511e !important;
    padding: 25px;
    border-bottom: 1px solid transparent;
    border-top-left-radius: 0px;
    border-top-right-radius: 0px;
    border-bottom-left-radius: 0px;
    border-bottom-right-radius: 0px;
  }*/
        .panel-footer {
            background-color: #f4511e !important;
        }

        .panel-default {
            padding: 15px;
        }

        .panel-footer h3 {
            font-size: 32px;
        }

        .panel-footer h4 {
            color: #aaa;
            font-size: 14px;
        }

        .panel-footer .btn {
            margin: 15px 0;
            border-radius: 5px !important;
            background-color: #f4511e;
            color: #fff;
        }

        .application-container {
            background: rgba(255, 255, 255, 0.95);
            border-radius: var(--radius-lg);
            box-shadow: var(--shadow-lg);
            padding: 1rem;
            margin-bottom: 2rem;
            backdrop-filter: blur(10px);
            -webkit-backdrop-filter: blur(10px);
        }
    </style>


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

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="application-container">
                <div class="row">
                    <div class="col-lg-12" style="text-align: center;">
                        <asp:Panel ID="panelGoBackPageMassage" runat="server">
                            <div class="alert alert-warning hoverable" style="margin-bottom: 10px; padding: 8px 15px;">
                                <label id="lblGoBackPageMassage" style="font-weight: bold; color: crimson; text-align: center;">Warning! Do not press browser back button while you are in Confirmation page.</label>
                            </div>
                        </asp:Panel>
                    </div>
                </div>

                <div class="row">
                    <div class="col-sm-12 col-md-12 col-lg-12">
                        <div class="panel panel-default">

                            <div class="panel-body" style="padding: 8px 15px;">

                                <div class="row">
                                    <div class="col-sm-8 col-md-8 col-lg-8">

                                        <div class="alert alert-success" style="margin: 0; padding: 10px 15px; border-radius: 5px !important;">
                                            <div class="row">
                                                <div class="col-sm-4 col-md-4 col-lg-4">
                                                    <h4>Name:</h4>
                                                </div>
                                                <div class="col-sm-8 col-md-8 col-lg-8">
                                                    <h4 style="font-weight: bold; margin: 0;">
                                                        <asp:Label ID="lblCandidateName" runat="server"></asp:Label>
                                                    </h4>
                                                </div>

                                            </div>
                                            <div class="row">
                                                <div class="col-sm-4 col-md-4 col-lg-4">
                                                    <h4>Payment ID</h4>
                                                </div>
                                                <div class="col-sm-8 col-md-8 col-lg-8">
                                                    <h4 style="font-weight: bold; margin: 0;">
                                                        <asp:Label ID="lblPaymentID" runat="server"></asp:Label></h4>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row" style="padding: 10px 15px;">
                                            <div class="col-sm-4 col-md-4 col-lg-4">
                                                <h5 style="margin: 0;">Amount</h5>
                                            </div>
                                            <div class="col-sm-8 col-md-8 col-lg-8">
                                                <h5 style="font-weight: bold; margin: 0;">
                                                    <asp:Label ID="lblAmount" runat="server" Text="0 BDT"></asp:Label>
                                                    <span>+ Charge</span>
                                                </h5>
                                            </div>
                                        </div>
                                        <div class="row" style="padding: 10px 15px;">
                                            <div class="col-sm-4 col-md-4 col-lg-4">
                                                <h5 style="margin: 0;">Faculty</h5>
                                            </div>
                                            <div class="col-sm-8 col-md-8 col-lg-8">
                                                <h5 style="font-weight: bold; margin: 0;">
                                                    <asp:Label ID="lblPrograms" runat="server"></asp:Label>
                                                </h5>
                                            </div>
                                        </div>

                                    </div>
                                    <div class="col-sm-4 col-md-4 col-lg-4">

                                        <p style="font-weight: bolder; color: crimson; font-size: 15pt; text-align: justify; margin: 0;">
                                            Please note down your Payment ID and save this number for future reference. Your email id is :
                                            
                                            <asp:Label ID="lblEmail" runat="server" Style="font-style: italic;"></asp:Label>

                                        </p>

                                    </div>
                                </div>



                                <%--========== Payment Buttons ========--%>
                                <%--<div class="row">
                                    <div class="col-sm-12 col-md-12 col-lg-12">
                                        <div style="display: flex;">
                                            <div>
                                                <asp:Button ID="btnSubmit_Bkash" runat="server" Text="Pay By BKash"
                                                    CssClass="btn btn-primary" OnClick="btnSubmit_Bkash_Click" Visible="true" Style="display: none;" />
                                                <asp:Button ID="btnSubmit_Fpg" runat="server" Text="Pay By Foster" Visible="true"
                                                    CssClass="btn btn-primary" OnClick="btnSubmit_Fpg_Click" />
                                            </div>

                                            <div>
                                                <asp:Button ID="btnSubmit" runat="server" Text="Pay Now"
                                                    CssClass="btn btn-primary" OnClick="btnSubmit_Click" Style="display: none;" />
                                                <asp:Panel ID="PanelOnlineGateway" runat="server" Visible="false">
                                                </asp:Panel>
                                            </div>

                                            <div style="display: none;">
                                                <asp:Panel ID="PanelDBBLRocket" runat="server" Visible="false">
                                                    <div style="margin-left: 20px;">
                                                        <asp:Button ID="btnDBBLRocket" runat="server" Text="Pay By Rocket"
                                                            CssClass="btn btn-primary" OnClick="btnDBBLRocket_Click" Style="background-color: #89288f; display: none;" />
                                                    </div>
                                                </asp:Panel>
                                            </div>

                                            <div style="margin-left: 20px;">
                                                <asp:Button ID="btnPayLater" runat="server" Text="Pay Later" Visible="true"
                                                    CssClass="btn btn-primary" OnClick="btnSubmitPayLater_Click" Style="display: none;" />
                                            </div>

                                        </div>
                                    </div>
                                </div>--%>
                                <%--========== Payment Buttons ========--%>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-sm-12 col-md-12 col-lg-12">
                        <asp:Panel ID="messagePanel" runat="server" Visible="false">
                            <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                        </asp:Panel>
                    </div>
                </div>

                <asp:Panel ID="panelSSL" runat="server" Visible="true">
                    <div class="row">
                        <div class="col-sm-4 col-xs-12">

                            <%--<div class="panel-heading  text-center">
                                <asp:Image ID="imgSSLCommerz" runat="server" ImageUrl="~/Images/AppImg/sslcommerz.png" Style="width: 185px;" />
                            </div>--%>
                            <%--<div class="panel-body">
                                <p class="text-center" style="font-size: 16px;"><b>Payment Charges</b></p>
                                <table>
                                    <tr>
                                        <td style="padding-right: 15px;">Cards
                                        </td>
                                        <td>: <strong>1.7%</strong>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding-right: 15px;">Mobile Banking
                                        </td>
                                        <td>: <strong>1.7%</strong> (bKash: <strong>1.5%</strong>)
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding-right: 15px;">Internet Banking 
                                        </td>
                                        <td>: <strong>1.7%</strong>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding-right: 15px;">Wallets
                                        </td>
                                        <td>: <strong>1.7%</strong>
                                        </td>
                                    </tr>
                                </table>
                            </div>--%>
                            <div class="panel panel-default" style="text-align: center">
                                <div class="panel-body">

                                    <asp:LinkButton ID="btnPayByEkPay" runat="server"
                                        OnClick="btnSubmit_Click"
                                        CssClass="btn btn-lg panel-footer" ForeColor="White">
         Pay Now
                                    </asp:LinkButton>
                                </div>
                            </div>

                        </div>
                    </div>
                </asp:Panel>


                <div class="row" style="display: none;">
                    <div class="col-sm-12 col-md-12 col-lg-12">

                        <!-- Container (Pricing Section) -->
                        <div class="container-fluid">
                            <div class="text-center" style="display: none;">
                                <h2 style="margin-top: 0; font-size: 25px;"><b>Comparison of Different Payment Rates</b></h2>
                                <h4>Choose a payment plan that works for you</h4>
                            </div>
                            <div class="row">
                                <div class="col-sm-2 col-xs-12"></div>
                                <asp:Panel ID="panelEkPay" runat="server" Visible="false">
                                    <div class="col-sm-4 col-xs-12" style="display: none;">
                                        <div class="panel panel-default">
                                            <div class="panel-heading  text-center">
                                                <%--<h1>EkPay</h1>--%>
                                                <asp:Image ID="imgEkPay" runat="server" ImageUrl="~/Images/AppImg/ekpay.png" Style="width: 157px;" />
                                            </div>
                                            <div class="panel-body">
                                                <p class="text-center" style="font-size: 16px;"><b>Payment Charges</b></p>
                                                <table>
                                                    <tr>
                                                        <td style="padding-right: 15px;">Cards
                                                        </td>
                                                        <td>: <strong>0.9%</strong>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="padding-right: 15px;">Mobile Banking
                                                        </td>
                                                        <td>: <strong>0%</strong>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="padding-right: 15px;">Internet Banking 
                                                        </td>
                                                        <td>: <strong>0%</strong>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="padding-right: 15px;">Wallets
                                                        </td>
                                                        <td>: <strong>0%</strong>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <div class="panel-footer  text-center">
                                                <asp:LinkButton ID="btnPayBySSL" runat="server"
                                                    OnClick="btnPayByEkPay_Click"
                                                    CssClass="btn btn-lg">Pay Now</asp:LinkButton>
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>
                                <div class="col-sm-4 col-xs-12">
                                </div>
                                <div class="col-sm-2 col-xs-12"></div>
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
                </Parallel>
            </OnUpdating>
            <OnUpdated>
                <Parallel duration="0">
                    <ScriptAction Script="onComplete();" />
                    <EnableAction   AnimationTarget="btnLoad" Enabled="true" />
                </Parallel>
            </OnUpdated>
        </Animations>
    </ajaxToolkit:UpdatePanelAnimationExtender>



</asp:Content>
