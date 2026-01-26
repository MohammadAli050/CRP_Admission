<%@ Page Title="Candidate Form Serial" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="CandidateFormSl.aspx.cs" Inherits="Admission.Admission.Admin.CandidateFormSl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <link href="../../Content/formStyle.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-md-12">
            <asp:Panel ID="messagePanel" runat="server">
                <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
            </asp:Panel>
            <div class="panel panel-default">
                <div class="panel-body">
                    <table class="table_form table_fullwidth">
                        <tr>
                            <td class="style_td" style="width: 10%">CandidateID:</td>
                            <td style="width: 35%">
                                <asp:TextBox ID="txtCandidateId" runat="server" Width="50%"></asp:TextBox>
                            </td>
                            <td class="style_td style_td_secondCol" style="width: 10%">FormSl:</td>
                            <td style="width: 35%">
                                <asp:TextBox ID="txtFormSl" runat="server" Width="50%"></asp:TextBox>
                            </td>
                            <td style="width: 10%">
                                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="float-right" 
                                    OnClick="btnSearch_Click"/>
                            </td>
                        </tr>
                    </table>
                </div>
                <%-- END PANEL-BODY --%>
            </div>
            <%-- END PANEL-DEFAULT --%>
        </div>
        <%-- END COL-MD-12 --%>
    </div>
    <%-- END ROW --%>

    <div class="row">
        <div class="col-md-12">
            <asp:ListView ID="lvFormSl" runat="server"
                OnItemDataBound="lvFormSl_ItemDataBound"
                OnItemEditing="lvFormSl_ItemEditing"
                OnItemUpdating="lvFormSl_ItemUpdating"
                OnItemCanceling="lvFormSl_ItemCanceling">
                <LayoutTemplate>
                    <table id="tbl" class="table table-hover table-condensed table-striped"
                        style="width: 100%; text-align: left">
                        <tr runat="server" style="background-color: #1387de; color: white; font-size: small">
                            <th runat="server">SL#</th>
                            <th runat="server">Name</th>
                            <th runat="server">CandidateId</th>
                            <th runat="server">AcaCal</th>
                            <th runat="server">FormSerial</th>
                            <th runat="server">PaymentId</th>
                            <th></th>
                        </tr>
                        <tr runat="server" id="itemPlaceholder" />
                    </table>
                </LayoutTemplate>
                <ItemTemplate>
                    <tr runat="server" style="font-size: smaller">
                        <td valign="middle" align="left" class="">
                            <asp:Label ID="lblSerial" runat="server" />.
                            <asp:HiddenField ID="hfFsLId" runat="server" />
                        </td>
                        <td valign="middle" align="left" class="">
                            <asp:Label ID="lblName" runat="server" />
                        </td>
                        <td valign="middle" align="left" class="">
                            <asp:Label ID="lblCandidateId" runat="server" />
                            <asp:HiddenField ID="hfCandidateId" runat="server" />
                        </td>
                        <td valign="middle" align="left" class="">
                            <asp:Label ID="lblAcaCal" runat="server" />
                        </td>
                        <td valign="middle" align="left" class="">
                            <asp:Label ID="lblFormSerial" runat="server" />
                        </td>
                        <td valign="middle" align="left" class="">
                            <asp:Label ID="lblPaymentId" runat="server" />
                            <asp:HiddenField ID="hfPaymentId" runat="server" />
                        </td>
                        <td valign="middle" align="right" class="">

                            <asp:LinkButton CssClass="" ID="lnkEdit" runat="server" CommandName="Edit">Edit</asp:LinkButton>

                        </td>
                    </tr>
                </ItemTemplate>
                <EditItemTemplate>
                    <tr>
                        <td valign="middle" align="left" class="">
                            <asp:Label ID="lblSerial" runat="server" />.
                            <asp:HiddenField ID="hfFsLId" runat="server" />
                        </td>
                        <td valign="middle" align="left" class="">
                            <asp:Label ID="txtName" runat="server" />
                        </td>
                        <td valign="middle" align="left" class="">
                            <asp:Label ID="lblCandidateId" runat="server" />
                        </td>
                        <td valign="middle" align="left" class="">
                            <asp:Label ID="lblAcaCal" runat="server" />
                        </td>
                        <td valign="middle" align="left" class="">
                            <asp:TextBox ID="txtFormSerial" runat="server" />
                        </td>
                        <td valign="middle" align="left" class="">
                            <asp:Label ID="lblPaymentId" runat="server" />
                        </td>
                        <td valign="middle" align="right" class="">
                            <asp:LinkButton CssClass="" ID="lnkUpdate" runat="server" CommandName="Update">Update</asp:LinkButton>||
                            <asp:LinkButton CssClass="" ID="lnkCancel" runat="server" CommandName="Cancel">Cancel</asp:LinkButton>
                        </td>
                    </tr>
                </EditItemTemplate>
                <EmptyDataTemplate>
                    <div class="alert alert-warning" role="alert" style="text-align: center">No item to display.</div>
                </EmptyDataTemplate>
            </asp:ListView>
        </div>
        <%-- END COL-MD-12 --%>
    </div>
    <%-- END ROW --%>
</asp:Content>
