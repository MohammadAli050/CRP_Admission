<%@ Page Title="Office Home" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="OfficeHome.aspx.cs" Inherits="Admission.Admission.Office.OfficeHome" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">

    <script src="../../Scripts/jquery-3.1.1.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $(document).ready(function () {
                $('.fadeInDiv1').fadeIn(250);
            });
            $(document).ready(function () {
                //$('.fadeInDiv2').delay(400*1).fadeIn(3000);
                $('.fadeInDiv2').fadeIn(300);
            });
            $(document).ready(function () {
                $('.fadeInDiv3').fadeIn(350);
            });
            $(document).ready(function () {
                $('.fadeInDivLast').fadeIn(1000);
            });

        });
    </script>
    <link href="../../Content/formStyle.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h4>Office Home</h4>
                </div>
                <div class="panel-body">


                    <div class="row">
                        <%-- LEFT COLUMN --%>
                        <%-- ------------------------------------------------------------------------------------------------------------------------------ --%>
                        <div class="col-md-6 col-sm-6 col-xs-12 fadeInDiv1" style="display: none">

                            <%-- ** TODAY SECTION ** --%>
                            <div class="panel panel-info">
                                <div class="panel-heading">
                                    <strong>Today</strong>
                                </div>
                                <div class="panel-body">
                                    <table class="table_fullwidth table_form">
                                        <tr>
                                            <td class="style_td">No. of Candidates Applied Today:
                                            </td>
                                            <td>
                                                <asp:Label ID="lblNoAppliedToday" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style_td">No. of Payments Today:
                                            </td>
                                            <td>
                                                <asp:Label ID="lblNoPaidToday" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <%--END PANEL-BODY  --%>
                            </div>
                            <%-- END PANEL-INFO ** TODAY SECTION **--%>

                            <%-- ** THIS MONTH ** --%>
                            <div class="panel panel-info">
                                <div class="panel-heading">
                                    <strong>This Month</strong>
                                </div>
                                <div class="panel-body">
                                    <table class="table_fullwidth table_form">
                                        <tr>
                                            <td class="style_td">No. of Candidates Applied This Month (Paid + Unpaid):
                                            </td>
                                            <td>
                                                <asp:Label ID="lblNoAppliedThisMonth" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style_td">No. of Transactions (PaymentID) This Month:
                                            </td>
                                            <td>
                                                <asp:Label ID="lblNoPaidThisMonth" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <%--END PANEL-BODY  --%>
                            </div>
                            <%-- END PANEL-INFO ** THIS MONTH **--%>

                            <%-- SCHOOL WISE PAID CANDIDATE COUNT --%>
                            <div class="panel panel-info" style="width: 125%;">
                                <div class="panel-heading">
                                    <strong>Faculty wise Paid This Month (Total)</strong>
                                </div>
                                <div class="panel-body">
                                    <asp:Chart ID="Chart1" runat="server" Visible="false">
                                        <Series>
                                            <asp:Series Name="Series1"></asp:Series>
                                        </Series>
                                        <ChartAreas>
                                            <asp:ChartArea Name="ChartArea1">
                                            </asp:ChartArea>
                                        </ChartAreas>
                                    </asp:Chart>
                                    <%--<hr />--%>
                                    <div>

                                        <div class="row">
                                            <div class="col-sm-6 col-md-6 col-lg-6">
                                                <label>Session:</label>
                                                <br />
                                                <asp:DropDownList ID="ddlSession" runat="server" Width="100%"
                                                    OnSelectedIndexChanged="ddlSession_SelectedIndexChanged" AutoPostBack="true">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-sm-6 col-md-6 col-lg-6">
                                                <label>Education:</label>
                                                <br />
                                                <asp:DropDownList ID="ddlEducation" runat="server" Width="100%"
                                                    OnSelectedIndexChanged="ddlSession_SelectedIndexChanged" AutoPostBack="true">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <br />
                                        <asp:UpdatePanel ID="updatePanel_lvCandidatePaidCountSchoolWise" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:ListView ID="lvCandidatePaidCountSchoolWise" runat="server"
                                                    OnItemDataBound="lvCandidatePaidCountSchoolWise_ItemDataBound">
                                                    <LayoutTemplate>
                                                        <table id="tbl"
                                                            class="table table-hover table-condensed table-striped"
                                                            style="width: 100%; text-align: left">
                                                            <tr runat="server" style="background-color: #c6e8f9; color: black;">
                                                                <th runat="server">Faculty/Program Name</th>
                                                                <th runat="server" style="text-align: center;">Paid</th>
                                                                <th runat="server" style="text-align: center;">Application Completed</th>
                                                                <th runat="server" style="text-align: center;">Approved</th>
                                                                <th runat="server" style="text-align: center;">Appeared</th>
                                                            </tr>
                                                            <tr runat="server" id="itemPlaceholder" />
                                                        </table>
                                                    </LayoutTemplate>
                                                    <ItemTemplate>
                                                        <tr runat="server">
                                                            <td valign="middle" align="left" class="" style="width:42%">
                                                                <asp:Label ID="lblUnitName" runat="server" />
                                                            </td>
                                                            <td valign="middle" align="left" class="" style="text-align: center;">
                                                                <asp:Label ID="lblTotalPaidCount" runat="server" />
                                                            </td>
                                                            <td valign="middle" align="left" class="" style="text-align: center;">
                                                                <asp:Label ID="lblPaidCandidateCount" runat="server" />
                                                            </td>
                                                            <td valign="middle" align="left" class="" style="text-align: center;">
                                                                <asp:Label ID="lblApprevedCandidateCount" runat="server" />
                                                            </td>
                                                            <td valign="middle" align="left" class="" style="text-align: center;">
                                                                <asp:Label ID="lblAppearedCandidateCount" runat="server" />
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <EmptyDataTemplate>
                                                        <div class="alert alert-warning" role="alert" style="text-align: center">No item to display.</div>
                                                    </EmptyDataTemplate>
                                                </asp:ListView>
                                                <table id="tbl1"
                                                      class="table table-hover table-condensed table-striped"
                                                            style="width: 100%; text-align: left">
                                                    <tr>
                                                        <td valign="middle" align="left" class="" style="width:40%">Total for session <strong>
                                                            <asp:Label ID="lblSessionName" runat="server"></asp:Label></strong> :
                                                        </td>

                                                        <td valign="middle" align="left" class="" style="text-align: center;width:10%">
                                                            <b>
                                                                <asp:Label ID="lbltotalCandidate" runat="server" /></b>
                                                        </td>
                                                        <td valign="middle" align="left" class="" style="text-align: center;width:23%">
                                                            <b>
                                                                <asp:Label ID="lblTotalCandidateFinal" runat="server" /></b>
                                                        </td>
                                                        <td valign="middle" align="left" class="" style="text-align: center;">
                                                            <b>
                                                                <asp:Label ID="lblTotalApprevedCandidateCount" runat="server" /></b>
                                                        </td>
                                                        <td valign="middle" align="left" class="" style="text-align: center;">
                                                            <b>
                                                                <asp:Label ID="lblTotalAppearedCandidateCount" runat="server" /></b>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <asp:Panel ID="panelShowSessionTotal" runat="server" Visible="false">
                                                    <asp:Label ID="lblSessionTotal" runat="server" Visible="false"></asp:Label>
                                                </asp:Panel>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="ddlSession" EventName="SelectedIndexChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                                <%-- END PANEL-BODY --%>
                            </div>
                            <%-- END PANEL_INFO ** SCHOOL WISE PAID CANDIDATE COUNT ** --%>
                        </div>
                        <%-- END LEFT COLUMN --%>



                        <%-- RIGHT COLUMN --%>
                        <%-- ------------------------------------------------------------------------------------------------------------------------------ --%>
                        <div class="col-md-6 col-sm-6 col-xs-12 fadeInDiv2" style="display: none">

                            <%-- ** ADMISSION STATUS ** --%>
                            <div class="panel panel-info">
                                <div class="panel-heading">
                                    <strong>Admission Status</strong>
                                </div>
                                <div class="panel-body">
                                    <table class="table_fullwidth table_form">
                                        <tr>
                                            <td class="style_td">No. of School Opened:
                                            </td>
                                            <td>
                                                <asp:Label ID="lblNoSchoolsOpen" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style_td">No. of Programs Available:
                                            </td>
                                            <td>
                                                <asp:Label ID="lblNoProgAvailable" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style_td">No. of Admission Opened:
                                            </td>
                                            <td>
                                                <asp:Label ID="lblNoAdmissionOpen" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style_td">No. of Active (But Expired) Admission:
                                            </td>
                                            <td>
                                                <asp:Label ID="lblActiveExpiredAdmission" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <%-- END PANEL-BODY --%>
                            </div>
                            <%-- END PANEL-INFO  ** ADMISSION STATUS **--%>
                        </div>
                        <%-- END RIGHT COLUMN --%>
                        <%-- ------------------------------------------------------------------------------------------------------------------------------ --%>
                    </div>


                </div>
            </div>
        </div>
    </div>



</asp:Content>
