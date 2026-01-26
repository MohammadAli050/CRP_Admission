<%@ Page Title="Success - FPG" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="successurlfpg.aspx.cs" Inherits="Admission.Admission.successurlfpg" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-body">
                    <div>
                        Payment <strong style="color: limegreen;">SUCCESSFUL</strong>.
                     You will recieve an SMS shortly with Username and Password.
                    </div>
                    <p>
                        Your Name:
                        <asp:Label runat="server" ID="lblName"></asp:Label>
                        <br />
                        PaymentID:
                        <asp:Label runat="server" ID="lblPaymentId"></asp:Label>
                    </p>
                    <p>
                        Thank you.
                    </p>
                    <p style="color: brown">
                    </p>
                    <p>
                        <asp:HiddenField ID="hfData" runat="server"/>
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

</asp:Content>
