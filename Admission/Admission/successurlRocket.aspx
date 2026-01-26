<%@ Page Title="Payment Status" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="successurlRocket.aspx.cs" Inherits="Admission.Admission.successurlRocket" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <style>
        .card {
            box-shadow: 0 4px 8px 0 rgba(0,0,0,0.2);
            transition: 0.3s;
            width: 40%;
            margin: 10% auto;
        }


            .card:hover {
                box-shadow: 0 8px 16px 0 rgba(0,0,0,0.2);
            }

        .container {
            padding: 2px 16px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="card" style="text-align: center;">
                <div class="container">
                    <asp:Label ID="lblPaymentStatus" runat="server" Font-Bold="true" Font-Size="Medium" ForeColor="Green"></asp:Label>
                    <br />
                    <br />
                    <label>Name:</label><asp:Label ID="lblName" runat="server" Font-Bold="true" Font-Size="Medium" Text=""></asp:Label>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
