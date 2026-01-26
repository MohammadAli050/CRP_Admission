<%@ Page Title="Sorry...Site Under Maintenance" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SiteUnderMaintenance.aspx.cs" Inherits="Admission.Admission.SiteUnderMaintenance" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-md-12">
            <div class="alert alert-danger" role="alert">
                <strong>Site is under maintenance.</strong>
                <br />
                <asp:Label ID="lblMaintenanceNotice" runat="server" Visible="false" Text=""></asp:Label>
            </div>
        </div>
    </div>

</asp:Content>
