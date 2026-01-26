<%@ Page Title="Success Payment" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EkPaySuccess.aspx.cs" Inherits="Admission.Admission.EkPaySuccess" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <asp:UpdatePanel ID="up" runat="server">
        <ContentTemplate>


            <div class="row">
                <div class="col-sm-12 col-md-12 col-lg-12">
                    <div class="container" >

                        <asp:Panel ID="messagePanel" runat="server" Visible="false">
                            <asp:Panel ID="pnlSuccessIcon" runat="server" Visible="false" style="text-align: center;">
                                <div style="font-size: 50px;"><i class="far fa-check-circle"></i></div>
                            </asp:Panel>
                            <asp:Panel ID="pnlFailICon" runat="server" Visible="false" style="text-align: center;">
                                <div style="font-size: 50px;"><i class="fas fa-ban"></i></div>
                            </asp:Panel>
                            <h2 style="margin-top: 0px; text-align: center;">
                                <asp:Label ID="lblMsg" runat="server"></asp:Label></h2>

                            <p>
                                Name:
                                <asp:Label ID="lblName" runat="server"></asp:Label>
                            </p>
                            <p>
                                PaymentId:
                        <asp:Label ID="lblPaymentId" runat="server"></asp:Label>
                            </p>

                            <asp:Panel ID="panelSuccessNote" runat="server" Visible="false">

                                <div class="row">
                                    <div class="col-sm-10 col-md-10 col-lg-10">
                                        <%--<h3 style="margin: 8px 0;"></h3>--%>
                                        <p style="text-align:justify;font-size: 22px;margin-top: 10px;">
                                            <b>Please login using your UserId and Password (Sent to your Mobile and Email) and complete your Application Form</b>
                                        </p>
                                    </div>
                                    <div class="col-sm-2 col-md-2 col-lg-2">
                                        <asp:HyperLink ID="HyperLink1" runat="server" CssClass="btn btn-info btn-lg" NavigateUrl="~/Admission/Login" Style="margin-top: 12px; width:100%">Login</asp:HyperLink>
                                    </div>
                                </div>

                                
                            </asp:Panel>
                        </asp:Panel>

                        <asp:Panel ID="panelReloadMessage" runat="server" Visible="false">
                            <div style="display: flex; flex-direction: row; justify-content: center;">
                                <div>
                                    <h3 style="margin: 0;">If payment is not updated, please click refresh after 30 seconds &nbsp;</h3>
                                </div>
                                <div>
                                    <asp:LinkButton ID="btnRefresh" runat="server"
                                        CssClass="btn btn-default"
                                        OnClick="btnRefresh_Click">
                                    <i class="fas fa-redo"></i>&nbsp;Refresh
                                    </asp:LinkButton>
                                </div>
                            </div>
                        </asp:Panel>

                    </div>
                </div>
            </div>


        </ContentTemplate>
    </asp:UpdatePanel>


</asp:Content>
