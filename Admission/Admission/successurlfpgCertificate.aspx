<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="successurlfpgCertificate.aspx.cs" Inherits="Admission.Admission.successurlfpgCertificate" %>




<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-body">
                    <div>
                        Payment <strong style="color: limegreen;">SUCCESSFUL</strong>.
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
