<%@ Page Title="Key" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="Key.aspx.cs" Inherits="Admission.Admission.Admin.Key" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">

    <style type="text/css">
        tr.spaceUnder > td{
            padding-bottom: 5px;
        }

    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">

        <div class="col-md-5">
            <div class="panel panel-default">
                <div class="panel-heading">
                    Keys
                </div>
                <div class="panel-body">
                    <div>
                        <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                    </div>
                    <br />
                    <table style="width: 100%">
                        <tr class="spaceUnder">
                            <td style="width: 20%">Key Name</td>
                            <td style="width: 80%">
                                <asp:TextBox ID="txtKeyName" runat="server" Width="100%"></asp:TextBox>
                                &nbsp;
                                <asp:RequiredFieldValidator ID="reqKeyName" runat="server" ControlToValidate="txtKeyName"
                                    ErrorMessage="*" Display="Dynamic" ForeColor="Red" Font-Bold="true"
                                    Font-Size="12pt" ValidationGroup="gr1"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr class="spaceUnder">
                            <td>Remarks</td>
                            <td><asp:TextBox ID="txtRemarks" runat="server" Width="100%"></asp:TextBox></td>
                        </tr>
                        <tr class="spaceUnder">
                            <td></td>
                            <td style="padding-top: 5px">
                                <asp:Button ID="btnSave" runat="server" CssClass="btn btn-default" Text="Save"
                                    OnClick="btnSave_Click" ValidationGroup="gr1" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
        <div class="col-md-6">
        </div>
    </div>
    <%-- --------------------------------------------------------------------------------------------------------- --%>
    <div class="row">
        <div class="col-md-5">
            <asp:UpdatePanel ID="updatePanel1" runat="server">
                <ContentTemplate>
                    <asp:ListView ID="lvKey" runat="server"
                        OnItemDataBound="lvKey_ItemDataBound"
                        OnItemUpdating="lvKey_ItemUpdating"
                        OnItemDeleting="lvKey_ItemDeleting"
                        OnItemCommand="lvKey_ItemCommand">
                        <LayoutTemplate>
                            <table runat="server" style="width: 100%; text-align: left"
                                class="table table-condensed table-striped">
                                <tr runat="server" class="active">
                                    <th runat="server">ID</th>
                                    <th runat="server">Key Name</th>
                                    <th runat="server">Remarks</th>
                                    <th></th>
                                </tr>
                                <tr runat="server" id="itemPlaceHolder"></tr>
                            </table>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr runat="server">
                                <td style="vertical-align: middle; text-align: left" class="">
                                    <asp:Label ID="lblID" runat="server"></asp:Label>
                                </td>
                                <td style="vertical-align: middle; text-align: left" class="">
                                    <asp:Label ID="lblKeyName" runat="server"></asp:Label>
                                </td>
                                <td style="vertical-align: middle; text-align: left" class="">
                                    <asp:Label ID="lblRemarks" runat="server"></asp:Label>
                                </td>
                                <td style="vertical-align: middle; text-align: left" class="">
                                    <asp:LinkButton ID="lnkEdit" runat="server">Edit</asp:LinkButton>
                                    | 
                                    <asp:LinkButton ID="lnkDelete" runat="server">Delete</asp:LinkButton>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <EmptyDataTemplate>
                            <div class="alert alert-warning" role="alert">
                                No Item to display.
                            </div>
                        </EmptyDataTemplate>
                    </asp:ListView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
