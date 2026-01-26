<%@ Page Title="" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="PostgraduateDiplomaFormRequest.aspx.cs" Inherits="Admission.Admission.Office.PostgraduateDiplomaFormRequest" %>




<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">

    <link href="../../Content/formStyle.css" rel="stylesheet" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h4>Postgraduate Diploma Forms (Form Request)</h4>
                </div>
                <div class="panel-body">

                    <table class="table_form" style="width: 100%">
                        <tr>
                            <%--<td style="width: 30%" class="style_td">Faculty</td>
                            <td style="width: 30%" class="style_td">Education Category</td>
                            <td style="width: 30%" class="style_td">Session<span class="asteriskColor">*</span></td>--%>
                            <td style="width: 10%" class="style_td"></td>
                        </tr>
                        <%--<tr>
                            <td>
                                <asp:DropDownList ID="ddlUnitProgram" runat="server" Width="95%"></asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlEducationCategory" runat="server" Width="95%"></asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlSession" runat="server" Width="95%"></asp:DropDownList>
                                <asp:CompareValidator ID="ddlSessionComV" runat="server" Display="Dynamic"
                                    ErrorMessage="Required" ForeColor="Crimson" ControlToValidate="ddlSession"
                                    Font-Size="10pt"
                                    ValueToCompare="-1" Operator="NotEqual" ValidationGroup="loadVg"></asp:CompareValidator>
                            </td>
                            <td>
                                <asp:Button ID="btnLoad" runat="server" Text="Load" CssClass="float-left"
                                    ValidationGroup="loadVg" OnClick="btnLoad_Click" />
                            </td>
                        </tr>--%>
                        <tr>
                            <td></td>
                            <td></td>
                            <td style="padding-right: 0%">
                                <asp:TextBox ID="txtSearchText" runat="server" CssClass="float-left" placeholder="Payment ID / Mobile / Email" style="width:35%;margin-left: 60%;"></asp:TextBox>
                                <span class="asteriskColor">*</span>
                                <asp:RequiredFieldValidator ID="txtSearchTextReqV" runat="server" Display="Dynamic"
                                    ControlToValidate="txtSearchText" ErrorMessage="Required" Font-Size="10pt"
                                    ForeColor="Crimson" ValidationGroup="searchVg"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="float-left"
                                    ValidationGroup="searchVg" OnClick="btnSearch_Click" />
                            </td>
                        </tr>
                    </table>

                </div>
                <%-- END PANEL BODY --%>
            </div>
        </div>
        <%-- END COL-MD-12 --%>
    </div>
    <%-- END ROW TOP CONTROLS --%>

    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-info">
                <div class="panel-body">


                    <asp:Panel ID="listViewPanel" runat="server">
                        <div class="row" style="margin-bottom: 1%;">
                            Records: &nbsp;
                                    <asp:Label ID="lblCount" runat="server" CssClass="badge"></asp:Label>
                        </div>
                        <asp:ListView ID="lvFormRequest" runat="server"
                            OnItemDataBound="lvFormRequest_ItemDataBound"
                            OnPagePropertiesChanging="lvFormRequest_PagePropertiesChanging"
                            OnItemCommand="lvFormRequest_ItemCommand">
                            <LayoutTemplate>
                                <table id="tblFormRequest"
                                    class="table table-striped table-condensed table-bordered table-hover"
                                    style="width: 100%; text-align: left">
                                    <tr runat="server" style="background-color: #1387de; color: white; height: 25px; font-size: small">
                                        <th runat="server" style="text-align: center">SL#</th>
                                        <th runat="server" style="text-align: center">Name</th>
                                        <th runat="server" style="text-align: center; display: none">Form Serial</th>
                                        <th runat="server" style="text-align: center">Payment Id</th>
                                        <th runat="server" style="text-align: center">Mobile & Email</th>
                                        <th runat="server" style="text-align: center">Faculty</th>
                                        <th runat="server" style="text-align: center">Date Applied</th>
                                        <th runat="server" style="text-align: center; display: none;">Paid</th>
                                        <th runat="server" style="text-align: center">Edit Form</th>
                                    </tr>
                                    <tr runat="server" id="itemPlaceholder" />
                                </table>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <tr runat="server" style="font-size: 10pt">
                                    <td valign="middle" align="left" class="" style="font-size: 9pt; vertical-align: middle">
                                        <%--<asp:Label ID="lblSerial" runat="server" />--%>
                                        <%#Container.DataItemIndex+1 %>
                                    </td>
                                    <td valign="middle" align="left" class="" style="vertical-align: middle">
                                        <asp:Label ID="lblName" runat="server" />
                                    </td>
                                    <td valign="middle" align="center" class="" style="vertical-align: middle; display: none">
                                        <asp:Label ID="lblFormSerial" runat="server" Visible="false"/>
                                    </td>
                                    <td valign="middle" align="center" class="" style="vertical-align: middle">
                                        <asp:Label ID="lblPaymentId" runat="server" />
                                    </td>
                                    <td valign="middle" align="center" class="">
                                        <asp:Label ID="lblMobile" runat="server" /><br />
                                        <asp:Label ID="lblEmail" runat="server"></asp:Label>
                                    </td>
                                    <td valign="middle" align="center" class="" style="vertical-align: middle">
                                        <asp:Label ID="lblUnit" runat="server" />
                                    </td>
                                    <td valign="middle" align="center" class="" style="vertical-align: middle">
                                        <asp:Label ID="lblDateApplied" runat="server" />
                                    </td>
                                    <td valign="middle" align="center" class="" style="vertical-align: middle; display: none;">
                                        <asp:Label ID="lblPaid" runat="server"></asp:Label>
                                    </td>

                                    <td valign="middle" align="center" class="">

                                        <asp:LinkButton ID="lbForm" runat="server" Text="Form"></asp:LinkButton>

                                    </td>

                                    <%--<td valign="middle" align="center" class="">
                                        
                                        <asp:LinkButton ID="lbPrintPurchaseReciept" runat="server" Text="PR"></asp:LinkButton>&nbsp;
                                        <asp:LinkButton ID="lbPrintMoneyReciept" runat="server" Text="MR"></asp:LinkButton>

                                    </td>--%>
                                </tr>
                            </ItemTemplate>
                            <EmptyDataTemplate>
                                <div class="alert alert-warning" role="alert" style="text-align: center">No item to display.</div>
                            </EmptyDataTemplate>
                        </asp:ListView>
                        <div class="pagerStyle">
                            <br />
                            <asp:DataPager runat="server" ID="lvDataPager"
                                PagedControlID="lvFormRequest" PageSize="50">
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


                </div>
                <%-- END PANEL-BODY --%>
            </div>
        </div>
        <%-- END COL-MD-12 --%>
    </div>

</asp:Content>

