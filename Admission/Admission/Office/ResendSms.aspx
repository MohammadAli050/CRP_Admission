<%@ Page Title="Resend SMS" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="ResendSms.aspx.cs" Inherits="Admission.Admission.Office.ResendSms" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">

    <link href="../../Content/formStyle.css" rel="stylesheet" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-md-12">

            <div class="panel panel-default">
                <div class="panel-heading">
                    <h4>Resend Candidate SMS</h4>
                </div>
                <div class="panel-body">
                    <asp:Panel ID="messagePanel" runat="server">
                        Message:
                        <asp:Label ID="lblMessage" runat="server"></asp:Label>
                    </asp:Panel>
                    <table class="table_form table_fullwidth">
                        <tr>
                            <td>Mobile Number</td>
                            <td>
                                <asp:TextBox ID="txtMobile" runat="server" placeholer="Format: +8801XXXXXXXXX"></asp:TextBox>
                            </td>
                            <td>Payment ID</td>
                            <td>
                                <asp:TextBox ID="txtPaymentId" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Button ID="btnLoad" runat="server" Text="Load" OnClick="btnLoad_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>

    <div runat="server" id="divSendSms">
        <div class="row">
            <div class="col-lg-3 col-md-3 col-sm-3">

                <asp:Button ID="btnSendSMS" runat="server" Text="Send SMS (Bulk)" OnClick="btnSendSMS_Click" />
            </div>
            <div class="col-lg-3 col-md-3 col-sm-3">
                <asp:Button ID="btnSendEmail" runat="server" Text="Send Email (Bulk)" OnClick="btnSendEmail_Click" />

            </div>
        </div>
    </div>

    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-body">

                    <asp:UpdatePanel ID="updatepanel" runat="server">
                        <ContentTemplate>
                            <asp:Panel ID="listViewPanel" runat="server">
                                <div class="row" style="margin-bottom: 1%;">
                                    Records: &nbsp;
                                    <asp:Label ID="lblCount" runat="server" CssClass="badge"></asp:Label>
                                    &nbsp;&nbsp;
                                    <asp:Label ID="lblMessageLv" runat="server"></asp:Label>
                                </div>
                                <asp:ListView ID="lvSmsList" runat="server"
                                    OnItemDataBound="lvSmsList_ItemDataBound"
                                    OnItemCommand="lvSmsList_ItemCommand"
                                    OnPagePropertiesChanging="lvSmsList_PagePropertiesChanging">
                                    <LayoutTemplate>
                                        <table id="tblFormRequest"
                                            class="table_form"
                                            style="width: 100%; text-align: left">
                                            <tr runat="server" style="background-color: #1387de; color: white; height: 25px; font-size: small">
                                                <th runat="server" style="text-align: center">SL#</th>
                                                <th runat="server" style="text-align: center">Name</th>
                                                <th runat="server" style="text-align: center">Payment Id</th>
                                                <th runat="server" style="text-align: center">Email</th>
                                                <th runat="server" style="text-align: center">Mobile</th>
                                                <%--<th runat="server" style="text-align: center">Date Applied</th>--%>
                                                <th runat="server" style="text-align: center">Paid</th>
                                                <th runat="server" style="text-align: center">SMS Sent</th>
                                                <th runat="server" style="text-align: center">Email Sent</th>
                                                <th runat="server" style="text-align: center">Send</th>
                                            </tr>
                                            <tr runat="server" id="itemPlaceholder" />
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr runat="server" style="font-size: 10pt">
                                            <td valign="middle" align="center" class="">
                                                <asp:Label ID="lblSerial" runat="server" />
                                            </td>
                                            <td valign="middle" align="left" class="">
                                                <asp:Label ID="lblName" runat="server" />
                                            </td>
                                            <td valign="middle" align="center" class="">
                                                <asp:Label ID="lblPaymentId" runat="server" />
                                            </td>
                                            <td valign="middle" align="center" class="">
                                                <asp:Label ID="lblEmail" runat="server" />
                                            </td>
                                            <td valign="middle" align="center" class="">
                                                <asp:Label ID="lblMobile" runat="server" />
                                            </td>
                                            <%--<td valign="middle" align="left" class="">
                                        <asp:Label ID="lblUnit" runat="server" />.
                                    </td>--%>
                                            <%--<td valign="middle" align="center" class="">
                                        <asp:Label ID="lblDateApplied" runat="server" />.
                                    </td>--%>
                                            <td valign="middle" align="center" class="">
                                                <asp:Label ID="lblPaid" runat="server"></asp:Label>
                                            </td>
                                            <td valign="middle" align="center" class="">
                                                <asp:Label ID="lblNoSmsSent" runat="server"></asp:Label>
                                            </td>
                                            <td valign="middle" align="center" class="">
                                                <asp:Label ID="lblNoEmailSent" runat="server"></asp:Label>
                                            </td>
                                            <td valign="middle" align="center" class="">
                                                <asp:LinkButton ID="lnkSendSms" runat="server" Text="SMS"></asp:LinkButton>
                                                | 
                                        <asp:LinkButton ID="lnkSendEmail" runat="server" Text="Email" Visible="true"></asp:LinkButton>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <EmptyDataTemplate>
                                        <div class="alert alert-warning" role="alert" style="text-align: center">No item to display.</div>
                                    </EmptyDataTemplate>
                                </asp:ListView>
                                <div class="pagerStyle">
                                    <br />
                                    <asp:DataPager runat="server" ID="lvDataPager"
                                        PagedControlID="lvSmsList" PageSize="50">
                                        <Fields>
                                            <asp:NextPreviousPagerField PreviousPageText="<<" FirstPageText="First" ShowPreviousPageButton="true"
                                                ShowFirstPageButton="true" ShowNextPageButton="false" ShowLastPageButton="false"
                                                ButtonCssClass="btn btn-default" RenderNonBreakingSpacesBetweenControls="false" RenderDisabledButtonsAsLabels="false" />
                                            <asp:NumericPagerField ButtonType="Link" CurrentPageLabelCssClass="btn btn-primary disabled" RenderNonBreakingSpacesBetweenControls="false"
                                                NumericButtonCssClass="btn btn-default" ButtonCount="10" NextPageText="..." NextPreviousButtonCssClass="btn btn-default" />
                                            <asp:NextPreviousPagerField NextPageText=">>" LastPageText="Last" ShowNextPageButton="true"
                                                ShowLastPageButton="true" ShowPreviousPageButton="false" ShowFirstPageButton="false"
                                                ButtonCssClass="btn btn-default" RenderNonBreakingSpacesBetweenControls="false" RenderDisabledButtonsAsLabels="false" />
                                        </Fields>
                                    </asp:DataPager>
                                </div>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                </div>
            </div>
        </div>
    </div>

</asp:Content>
