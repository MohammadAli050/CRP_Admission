<%@ Page Title="View All Notice" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="NoticeAll.aspx.cs" Inherits="Admission.Admission.NoticeAll" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-info">
                <div class="panel-heading">
                    <strong>All Notices</strong>
                </div>
                <div class="panel-body">
                    <asp:UpdatePanel ID="updatePanel_ListView" runat="server">
                        <ContentTemplate>
                            <asp:ListView ID="lvNotice" runat="server"
                                OnItemDataBound="lvNotice_ItemDataBound"
                                OnItemCommand="lvNotice_ItemCommand"
                                OnPagePropertiesChanging="lvNotice_PagePropertiesChanging">
                                <LayoutTemplate>
                                    <table id="tblNotices"
                                        class="table table-hover table-condensed table-striped table-bordered"
                                        style="width: 100%; text-align: center;">
                                        <th runat="server" style="width: 5%; text-align: center; background-color: #D9F3F7">SL#</th>
                                        <th runat="server" style="width: 10%; text-align: center; background-color: #D9F3F7">Date</th>
                                        <th runat="server" style="background-color: #D9F3F7" class="info"></th>
                                        <th runat="server" style="width: 10%; text-align: center; background-color: #D9F3F7">Link</th>
                                        <tr runat="server" id="itemPlaceholder" />
                                    </table>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <tr runat="server">
                                        <td valign="middle" align="center" style="vertical-align: middle; font-size: small">
                                            <%#Container.DataItemIndex + 1 %>
                                        </td>
                                        <td valign="middle" align="center" style="vertical-align: middle">
                                            <asp:Label ID="lblNoticeDate" runat="server"></asp:Label>
                                        </td>
                                        <td valign="middle" align="left" class="">
                                            <p>
                                                <asp:Label ID="lblNewNotice" runat="server"
                                                    ForeColor="Crimson" Font-Bold="true" Text="NEW " />
                                                <asp:Label ID="lblNoticeTitle" runat="server"
                                                    ForeColor="#3c526f" Font-Underline="false" />
                                            </p>
                                            <p>
                                                <asp:Label ID="lblNoticeDetails" runat="server"
                                                    ForeColor="#3c526f" Font-Underline="false" />
                                            </p>
                                           
                                        </td>
                                        <td>
                                             <asp:HyperLink ID="hrefExternalUrl" runat="server" Target="_blank" Font-Size="Small"></asp:HyperLink>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <EmptyDataTemplate>
                                    <div class="alert alert-warning" role="alert" style="text-align: center">No notices.</div>
                                </EmptyDataTemplate>
                            </asp:ListView>
                            <div class="pagerStyle">
                                <br />
                                <asp:DataPager runat="server" ID="lvDataPager"
                                    PagedControlID="lvNotice" PageSize="15">
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
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
