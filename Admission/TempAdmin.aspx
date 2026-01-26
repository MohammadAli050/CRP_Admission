<%@ Page Title="" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="TempAdmin.aspx.cs" Inherits="Admission.TempAdmin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <link href="../../../Content/formStyle.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-md-12">
            <div class="panel-default">
                <div class="panel-heading">
                </div>
                <div class="panel-body">
                    
                    <asp:Panel ID="messagePanel" runat="server">
                        <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                    </asp:Panel>

                    <asp:UpdatePanel ID="updatePanelFilter" runat="server" UpdateMode="Always">
                        <ContentTemplate>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                </div>
            </div>
        </div>
    </div>


    <asp:UpdatePanel ID="updatePanelDynamicViews" runat="server" UpdateMode="Always">
        <ContentTemplate>

            <div class="row">
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
