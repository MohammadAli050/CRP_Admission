<%@ Page Title="Success - SSL" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="successurl.aspx.cs" Inherits="Admission.Admission.successurl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <asp:UpdatePanel ID="UpdatePanel" runat="server">
        <ContentTemplate>

            <asp:Panel ID="PanelSuccess" runat="server" Visible="false">
                <div class="row">
                    <div class="col-md-12">
                        <div class="panel panel-default">
                            <div class="panel-body">
                                <div>
                                    Payment <strong style="color: limegreen;">SUCCESSFUL</strong>.
                     You will recieve an SMS shortly with Username and Password.
                        Please login using Username and Password.
                                </div>
                                <p>
                                    Your Name:
                        <asp:Label runat="server" ID="lblName"></asp:Label>
                                    <br />
                                    PaymentID:
                        <asp:Label runat="server" ID="lblPaymentId"></asp:Label>
                                    <br />
                                    <p style="color:red;font-size:18px"><strong>User Id:</strong>                                                                       
                        <asp:Label runat="server" ID="lblUserId"></asp:Label>
                                    <br />
                                   <p style="color:red;font-size:18px"><strong>Password:</strong>                                   
                        <asp:Label runat="server" ID="lblPassword"></asp:Label>
                                </p>
                                <p>
                                    Thank you.
                                </p>
                                <p style="color: brown">
                                </p>
                                <hr />
                                <%--<div>
                        <asp:Button runat="server" ID="btnGoHome"
                            Text="Home"
                            CssClass="btn btn-info btn-large"
                            OnClick="btnGoHome_Click" />
                    </div>--%>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>

            <asp:Panel ID="PanelAccessDenied" runat="server" Visible="false">
                <div class="row">
                    <div class="col-sm-12 col-md-12 col-lg-12">
                        <div class="container">
                            <div class="jumbotron" style="text-align: center;">
                                <h1>Forbidden Access</h1>
                                <h2>✋ You don't have permission to access 🚧 this area ✋</h2>
                            </div>
                        </div>
                    </div>
                </div>

            </asp:Panel>

        </ContentTemplate>
    </asp:UpdatePanel>



</asp:Content>
