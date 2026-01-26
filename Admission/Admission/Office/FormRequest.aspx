<%@ Page Title="Form Request" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true"
    CodeBehind="FormRequest.aspx.cs" Inherits="Admission.Admission.Office.FormRequest" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">

    <link href="../../Content/formStyle.css" rel="stylesheet" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h4>Unpaid Forms (Form Request)</h4>
                </div>
                <div class="panel-body">

                    <table class="table_form" style="width: 100%">
                        <tr>
                            <td style="width: 30%" class="style_td">Faculty</td>
                            <td style="width: 30%" class="style_td">Education Category</td>
                            <td style="width: 30%" class="style_td">Session<span class="asteriskColor">*</span></td>
                            <td style="width: 10%" class="style_td"></td>
                        </tr>
                        
                        <tr>
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
                            
                        </tr>
                        <tr>
                            
                            <td style="width: 30%" class="style_td">From Date</td>
                            <td style="width: 30%" class="style_td">To Date</td>
                            <td></td>
                            <td></td>
                        </tr>
                        <tr>                                                
                            <td>
                                <asp:TextBox ID="txtFromDate" runat="server" Width="95%"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server"
                                    TargetControlID="txtFromDate" Format="dd/MM/yyyy" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                    ControlToValidate="txtFromDate" ErrorMessage="*" ForeColor="Crimson"
                                    Font-Size="14pt" Display="Dynamic" Font-Bold="true"
                                    ValidationGroup="gr1"></asp:RequiredFieldValidator>
                            </td>
                            <td >
                                <asp:TextBox ID="txtToDate" runat="server" Width="95%"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server"
                                    TargetControlID="txtToDate" Format="dd/MM/yyyy" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                    ControlToValidate="txtToDate" ErrorMessage="*" ForeColor="Crimson"
                                    Font-Size="14pt" Display="Dynamic" Font-Bold="true"
                                    ValidationGroup="gr1"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:Button ID="btnLoad" runat="server" Text="Load" CssClass="float-left" BackColor="DeepSkyBlue" ForeColor="Black" Width="25%" Height="20%"
                                    ValidationGroup="loadVg" OnClick="btnLoad_Click" />
                            </td>
                            <td></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td style="width: 30%" class="style_td">Payment ID / Mobile / Email</td>
                        </tr>
                        
                        <tr>
                            
                            <td style="padding-right: 0%">
                                <asp:TextBox ID="txtSearchText" runat="server" Width="94%" CssClass="float-left" placeholder="Payment ID / Mobile / Email"></asp:TextBox>
                                <span class="asteriskColor">*</span>
                                <asp:RequiredFieldValidator ID="txtSearchTextReqV" runat="server" Display="Dynamic"
                                    ControlToValidate="txtSearchText" ErrorMessage="Required" Font-Size="10pt"
                                    ForeColor="Crimson" ValidationGroup="searchVg"></asp:RequiredFieldValidator>
                            </td>

                            <td>
                                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="float-left" BackColor="ForestGreen" ForeColor="White" Width="25%" 
                                    ValidationGroup="searchVg" OnClick="btnSearch_Click" />
                            </td>
                            <td></td>
                            <td></td>
                        </tr>
                        
                        <tr>
                            
                            <td style="padding-right: 0%" colspan='2' >
                                <span style="color: darkorange; font-size: 9pt;">Please include country code for Mobile Search, e.g.: +8801700000000</span>
                            </td>
                            <td></td>
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
                            <asp:Label ID="lblMessageLv" runat="server"></asp:Label>
                        </div>

                        <%--<td valign="middle" align="center" class="">
                                        
                                        <asp:LinkButton ID="lbPrintPurchaseReciept" runat="server" Text="PR"></asp:LinkButton>&nbsp;
                                        <asp:LinkButton ID="lbPrintMoneyReciept" runat="server" Text="MR"></asp:LinkButton>

                                    </td>--%>

                       <%-- <asp:ListView ID="lvFormRequest" runat="server"
                            OnItemDataBound="lvFormRequest_ItemDataBound"
                            OnPagePropertiesChanging="lvFormRequest_PagePropertiesChanging">
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
                                        <th runat="server" style="text-align: center">Paid</th>
                                        
                                    </tr>
                                    <tr runat="server" id="itemPlaceholder" />
                                </table>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <tr runat="server" style="font-size: 10pt">
                                    <td valign="middle" align="left" class="" style="font-size: 9pt; vertical-align: middle">
                                        
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
                                    <td valign="middle" align="center" class="" style="vertical-align: middle">
                                        <asp:Label ID="lblPaid" runat="server"></asp:Label>
                                    </td>

                                    
                                </tr>
                            </ItemTemplate>
                            <EmptyDataTemplate>
                                <div class="alert alert-warning" role="alert" style="text-align: center">No item to display.</div>
                            </EmptyDataTemplate>
                        </asp:ListView>--%>


                        <asp:ListView ID="lvFormRequest" runat="server"
                            OnItemDataBound="lvFormRequest_ItemDataBound"
                            OnItemCommand="lvFormRequest_ItemCommand"
                            OnPagePropertiesChanging="lvFormRequest_PagePropertiesChanging">
                            <LayoutTemplate>
                                <table id="tblFormRequest"
                                    class="table_form"
                                    style="width: 100%; text-align: left; overflow-x:scroll;overflow-x:scroll">
                                    <tr runat="server" style="background-color: #1387de; color: white; height: 25px; font-size: small">
                                        <th runat="server" style="text-align: center">SL#</th>
                                        <th runat="server" style="text-align: center">Name</th>
                                        <th runat="server" style="text-align: center">Payment Id</th>
                                        <th runat="server" style="text-align: center">Mobile</th>
                                        <th runat="server" style="text-align: center">School</th>
                                        <th runat="server" style="text-align: center">Date Applied</th>
                                        <th runat="server" style="text-align: center">Paid</th>
                                        <th runat="server" style="text-align: center;"></th>
                                        <th runat="server" style="text-align: center;"></th>

                                        <%--display:none;--%>
                                    </tr>
                                    <tr runat="server" id="itemPlaceholder" />
                                </table>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <tr runat="server" style="font-size: 10pt">
                                    <td valign="middle" align="center" class="">
                                        <asp:Label ID="lblSerial" runat="server" />.
                                    </td>
                                    <td valign="middle" align="left" class="">
                                        <asp:Label ID="lblName" runat="server" />
                                    </td>
                                    <td valign="middle" align="center" class="">
                                        <asp:Label ID="lblPaymentId" runat="server" />
                                    </td>
                                    <td valign="middle" align="center" class="">
                                        <asp:Label ID="lblMobile" runat="server" />
                                    </td>
                                    <td valign="middle" align="left" class="">
                                        <asp:Label ID="lblUnit" runat="server" />
                                    </td>
                                    <td valign="middle" align="center" class="">
                                        <asp:Label ID="lblDateApplied" runat="server" />
                                    </td>
                                    <td valign="middle" align="center" class="">
                                        <asp:Label ID="lblPaid" runat="server"></asp:Label>
                                    </td>

                                    <td valign="middle" align="center" class="" >
                                        <%--style="display:none;"--%>
                                        <asp:LinkButton ID="lnkPrintPurchaseReciept" runat="server" Text="Confirm Payment" Visible="True"
                                            OnClientClick="return confirm('Are you sure you want to confirm this payment?');"></asp:LinkButton>&nbsp;
                                        <asp:LinkButton ID="lnkPrintMoneyReciept" runat="server" Text="MR" Visible="false"></asp:LinkButton>

                                    </td>

                                    <td valign="middle" align="center" class="" >
                                        <%--style="display:none;"--%>
                                        <asp:HyperLink ID="lnkFormView" runat="server" Text="Form" Target="_blank" ForeColor="Red"></asp:HyperLink>
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
