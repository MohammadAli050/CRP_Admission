<%@ Page Title="Quota Info" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="FacultyWiseQuota.aspx.cs" Inherits="Admission.Admission.Office.FacultyWiseQuota" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">

    <script type="text/javascript">

        function InProgress() {
            var panelProg = $get('divProgress');
            panelProg.style.display = '';
        }

        function onComplete() {
            var panelProg = $get('divProgress');
            panelProg.style.display = 'none';
        }

    </script>

    <style type="text/css">
        input[type="checkbox"] {
            width: 20px !important;
            height: 20px !important;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


    <div id="divProgress" style="display: none; z-index: 1000; position: fixed; top: 50%; left: 50%; transform: translate(-50%, -50%);">
        <asp:Image ID="LoadingImage" runat="server" ImageUrl="~/Images/AppImg/t1.gif" Height="250px" Width="250px" />
    </div>

    <br />

    <asp:UpdatePanel ID="updatePanelAll" runat="server">
        <ContentTemplate>

            <div class="row">
                <div class="col-sm-12 col-md-12 col-lg-12">
                    <h3>Quota Info</h3>
                </div>
            </div>

            <div class="panel panel-default">
                <div class="panel-body">

                    <div class="row">
                        <div class="col-sm-3 col-md-3 col-lg-3">
                            <div class="form-group">
                                <label><strong>Faculty</strong></label>
                                <asp:DropDownList ID="ddlAdmUnit" runat="server" Width="100%" CssClass="form-control"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-sm-2 col-md-2 col-lg-2">
                            <div class="form-group">
                                <label><strong>Quota</strong></label>
                                <asp:DropDownList ID="ddlQuota" runat="server" Width="100%" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-sm-2 col-md-2 col-lg-2">
                            <div class="form-group">
                                <label><strong>Session <span style="color: crimson; font-weight: bold;">*</span></strong></label>
                                <asp:DropDownList ID="ddlSession" runat="server" Width="100%" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-sm-2 col-md-2 col-lg-2">
                            <div class="form-group">
                                <label><strong>Status</strong></label>
                                <asp:DropDownList ID="ddlStatus" runat="server" Width="100%" CssClass="form-control">
                                    <asp:ListItem Value="-1" Selected="True">All</asp:ListItem>
                                    <asp:ListItem Value="1">Verified</asp:ListItem>
                                    <asp:ListItem Value="0">Not Verified</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-sm-2 col-md-2 col-lg-2">
                            <br />
                            <asp:Button ID="btnLoad" runat="server" Text="Load" Width="100%" CssClass="btn btn-info"
                                OnClick="btnLoad_Click" />
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-sm-12 col-md-12 col-lg-12">
                            <asp:Panel ID="messagePanel" runat="server">
                                <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                            </asp:Panel>
                        </div>
                    </div>

                </div>
            </div>

            <div class="row">
                <div class="col-md-12">
                    <div class="panel panel-default">
                        <div class="panel-body">
                            <p style="font-size: 15px;">
                                <span class="label label-info">Records:&nbsp;<asp:Label ID="lblCount" runat="server" Text="0"></asp:Label></span>
                            </p>

                            <asp:ListView ID="lvQuotaInfo" runat="server"
                                OnItemDataBound="lvQuotaInfo_ItemDataBound"
                                OnPagePropertiesChanging="lvQuotaInfo_PagePropertiesChanging">
                                <LayoutTemplate>
                                    <table id="tbl"
                                        class="table table-hover table-condensed table-striped table-bordered"
                                        style="width: 100%; text-align: left">
                                        <tr runat="server" style="background-color: #1387de; color: white; font-size: small">
                                            <th runat="server" style="text-align: center">SL#</th>
                                            <th runat="server" style="text-align: center">Payment ID</th>
                                            <th runat="server" style="text-align: center; width: 30px;">Candidate</th>
                                            <th runat="server" style="text-align: center; width: 250px;">Faculty</th>
                                            <th runat="server" style="text-align: center; width: 120px;">Quota</th>
                                            <th runat="server" style="text-align: center">Quota Info</th>
                                            <th runat="server" style="text-align: center">Document</th>
                                            <th runat="server" style="text-align: center">Verified</th>
                                        </tr>
                                        <tr runat="server" id="itemPlaceholder" />
                                    </table>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <tr runat="server" style="font-size: smaller">
                                        <td valign="middle" align="center" class="">
                                            <%--<asp:Label ID="lblSerial" runat="server" />.--%>
                                            <span style="line-height: 50px;"><%#Container.DataItemIndex+1 %>.</span>
                                        </td>

                                        <td valign="middle" align="center" class="">
                                            <asp:HiddenField ID="hfQuotaInfoId" runat="server" />
                                            <asp:HiddenField ID="hdnQuotaId" runat="server" />
                                            <asp:Label ID="lblPaymentId" runat="server" Style="line-height: 50px;" />
                                        </td>

                                        <td valign="middle" align="left" class="">
                                            <asp:Label ID="lblCandidateName" runat="server" Style="font-weight: bold; font-size: 15px;" />
                                            <br />
                                            <asp:Label ID="lblCandidatePhone" runat="server" />
                                            <br />
                                            <asp:Label ID="lblCandidateEmail" runat="server" />
                                        </td>

                                        <td valign="middle" align="left" class="">
                                            <asp:Label ID="lblFaculty" runat="server" Style="line-height: 50px;" />
                                        </td>

                                        <td valign="middle" align="center" class="">
                                            <asp:Label ID="lblQuota" runat="server" Style="line-height: 50px;" />
                                        </td>

                                        <td valign="middle" align="left" class="">
                                            <asp:Label ID="lblQuotaInfo" runat="server" />
                                        </td>
                                        <td valign="middle" align="left" class="">
                                            <asp:HyperLink ID="hlDoc1" runat="server" Visible="false" Target="_blank"></asp:HyperLink>
                                            <br />
                                            <br />
                                            <asp:HyperLink ID="hlDoc2" runat="server" Visible="false" Target="_blank"></asp:HyperLink>
                                            <br />
                                            <br />
                                            <asp:HyperLink ID="hlDoc3" runat="server" Visible="false" Target="_blank"></asp:HyperLink>
                                        </td>
                                        <td valign="middle" align="center" class="">
                                            <asp:CheckBox ID="cbSingle" runat="server"
                                                AutoPostBack="true" OnCheckedChanged="cbSingle_CheckedChanged" />
                                            <br />
                                            <asp:Label ID="lblVerifiedDocumentByName" runat="server" Visible="false" />
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
                                    PagedControlID="lvQuotaInfo" PageSize="1000">
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


                        </div>
                    </div>
                </div>
            </div>



        </ContentTemplate>
    </asp:UpdatePanel>



    <ajaxToolkit:UpdatePanelAnimationExtender ID="UpdatePanelAnimationExtender1" TargetControlID="updatePanelAll" runat="server">
        <Animations>
            <OnUpdating>
                <Parallel duration="0">
                    <ScriptAction Script="InProgress();" />
                    <EnableAction AnimationTarget="btnLoad" Enabled="false" />
                </Parallel>
            </OnUpdating>
            <OnUpdated>
                <Parallel duration="0">
                    <ScriptAction Script="onComplete();" />
                    <EnableAction   AnimationTarget="btnLoad" Enabled="true" />
                </Parallel>
            </OnUpdated>
        </Animations>
    </ajaxToolkit:UpdatePanelAnimationExtender>

</asp:Content>
