<%@ Page Title="" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="ImageTransferManualAdmissionToFolder.aspx.cs" Inherits="Admission.Admission.Office.ImageTransferManualAdmissionToFolder" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="row">
                <div class="col-lg-12">
                    <asp:Label ID="lblMsg" runat="server" Text="" Font-Bold="true" ForeColor="Red" Font-Size="Medium"></asp:Label>
                </div>
            </div>
            <hr />
            <div class="row">
                <div class="col-lg-12">
                    <asp:Button ID="btnImg" runat="server" Text="Image Trans" OnClick="btnImg_Click" />
                </div>
            </div>
            <br />
            
            <div class="row">
                <div class="col-lg-12">
                    <asp:Button ID="btnSign" runat="server" Text="Sign Trans" OnClick="btnSign_Click" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
