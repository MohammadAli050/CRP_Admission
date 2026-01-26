<%@ Page Title="Receive Form" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true"
    CodeBehind="ReceiveFormForeign.aspx.cs" Inherits="Admission.Admission.Office.ReceiveFormForeign" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <%--<link href="../../Content/formStyle.css" rel="stylesheet" />--%>

    <style type="text/css">
        .myFlexVerticleHorizentalCenter_FlexDirectionRow {
            display: flex;
            flex-direction: row;
            flex-wrap: wrap;
            justify-content: center;
            align-items: center;
            align-content: stretch;
            height: 55px;
        }

        .myFlexVerticleHorizentalCenter_FlexDirectionColumn {
            display: flex;
            flex-direction: column;
            flex-wrap: wrap;
            justify-content: center;
            align-items: center;
            align-content: stretch;
            height: 55px;
        }
    </style>

    
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
                    <h3>Paid Form</h3>
                    <%--<hr style="margin-top: 0px; margin-bottom: 10px;" />--%>
                </div>
            </div>

            <div class="panel panel-default">
                <div class="panel-body">
                    <div class="row">
                        <div class="col-sm-3 col-md-3 col-lg-3">
                            <div class="form-group">
                                <label><strong>Faculty</strong></label>
                                <asp:DropDownList ID="ddlUnitProgram" runat="server" Width="100%" CssClass="form-control"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-sm-3 col-md-3 col-lg-3">
                            <div class="form-group">
                                <label><strong>Education Category</strong></label>
                                <asp:DropDownList ID="ddlEducationCategory" runat="server" Width="100%" CssClass="form-control"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-sm-3 col-md-3 col-lg-3">
                            <div class="form-group">
                                <label><strong>Session <span style="color: crimson; font-weight: bold;">*</span></strong></label>
                                <asp:DropDownList ID="ddlSession" runat="server" Width="100%" CssClass="form-control"></asp:DropDownList>
                                <asp:CompareValidator ID="ddlSessionComV" runat="server" Display="Dynamic"
                                    ErrorMessage="Required" ForeColor="Crimson" ControlToValidate="ddlSession"
                                    Font-Size="10pt"
                                    ValueToCompare="-1" Operator="NotEqual" ValidationGroup="loadVg"></asp:CompareValidator>
                            </div>
                        </div>
                        <div class="col-sm-3 col-md-3 col-lg-3">
                            <br />
                            <asp:Button ID="btnLoad" runat="server" Text="Load" CssClass="btn btn-info"
                                Style="width: 100%; margin-top: 4px;"
                                ValidationGroup="loadVg" OnClick="btnLoad_Click" />
                        </div>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-sm-12 col-md-12 col-lg-12">
                    <div class="panel panel-primary">
                        <a id="clickCollaps" data-toggle="collapse" href="#collapse1">
                            <div class="panel-heading bg-info" style="border-bottom: 1px solid #337ab7;">
                                <h4 class="panel-title"><i class="fas fa-angle-double-down"></i>&nbsp;<strong>Filters</strong></h4>
                            </div>
                        </a>
                        <div id="collapse1" class="panel-collapse collapse">
                            <div class="panel-body">

                                <div class="row">
                                    <div class="col-sm-3 col-md-3 col-lg-3">
                                        <div class="form-group">
                                            <label><strong>Candidate Name</strong></label>
                                            <asp:TextBox ID="txtFilterName" runat="server"
                                                placeholder="Enter Candidate Name"
                                                CssClass="form-control" Width="100%"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-sm-3 col-md-3 col-lg-3">
                                        <div class="form-group">
                                            <label><strong>Mobile</strong></label>
                                            <asp:TextBox ID="txtFilterMobile" runat="server"
                                                placeholder="Enter Candidate Mobile"
                                                CssClass="form-control" Width="100%"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-sm-3 col-md-3 col-lg-3">
                                        <div class="form-group">
                                            <label><strong>Email</strong></label>
                                            <asp:TextBox ID="txtFilterEmail" runat="server"
                                                placeholder="Enter Candidate Email"
                                                CssClass="form-control" Width="100%"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-sm-3 col-md-3 col-lg-3">
                                        <div class="form-group">
                                            <label><strong>Payment ID</strong></label>
                                            <asp:TextBox ID="txtFilterPaymentID" runat="server"
                                                placeholder="Enter Candidate Payment ID"
                                                CssClass="form-control" Width="100%"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-sm-3 col-md-3 col-lg-3">
                                    </div>
                                    <div class="col-sm-3 col-md-3 col-lg-3">
                                    </div>
                                    <div class="col-sm-3 col-md-3 col-lg-3">
                                    </div>
                                    <div class="col-sm-3 col-md-3 col-lg-3">
                                        <asp:LinkButton ID="btnFilterClear" runat="server" Width="30%"
                                            OnClick="btnFilterClear_Click"
                                            CssClass="btn btn-default btn-sm">
                                            <strong>Clear</strong>
                                        </asp:LinkButton>
                                        &nbsp;
                                        &nbsp;
                                        <asp:HiddenField ID="hfIsFilterClick" runat="server" />
                                        <asp:LinkButton ID="btnFilterData" runat="server" Width="30%"
                                            ValidationGroup="gr1" OnClick="btnFilterData_Click"
                                            CssClass="btn btn-info btn-sm">
                                                    <strong>Filter</strong>
                                        </asp:LinkButton>
                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>
                </div>
            </div>



            <div class="row">
                <div class="col-sm-12 col-md-12 col-lg-12">
                    <asp:Panel ID="messagePanel" runat="server">
                        <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                    </asp:Panel>
                </div>
            </div>

            <div class="panel panel-default">
                <div class="panel-body">
                    <p style="font-size: 15px;">
                        <span class="label label-primary">Records:&nbsp;<asp:Label ID="lblCount" runat="server"></asp:Label></span>
                    </p>

                    <%--OnItemCommand="lvFormRequest_ItemCommand"--%>
                    <asp:ListView ID="lvFormRequest" runat="server"
                        OnItemDataBound="lvFormRequest_ItemDataBound"
                        OnPagePropertiesChanging="lvFormRequest_PagePropertiesChanging">
                        <LayoutTemplate>
                            <table id="tblFormRequest"
                                class="table table-bordered table-hover"
                                style="width: 100%; text-align: left">
                                <tr runat="server" style="background-color: #1387de; color: white; height: 25px; font-size: small">
                                    <th runat="server" style="text-align: center">SL#</th>
                                    <th runat="server" style="text-align: center">Candidate</th>
                                    <th runat="server" style="text-align: center">Payment ID</th>
                                    <th runat="server" style="text-align: center">Faculty</th>
                                    <th runat="server" style="text-align: center">Education<br />
                                        Session</th>
                                    <th runat="server" style="text-align: center">Date Applied</th>
                                    <th runat="server" style="text-align: center">Paid</th>
                                    <th runat="server" style="text-align: center">Final<br />Submited</th>
                                    <th runat="server" style="text-align: center">Action</th>
                                </tr>
                                <tr runat="server" id="itemPlaceholder" />
                            </table>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr runat="server" style="font-size: 10pt">
                                <td valign="middle" align="center" class="">
                                    <div class="myFlexVerticleHorizentalCenter_FlexDirectionRow">
                                        <%#Container.DataItemIndex+1 %>
                                    </div>
                                </td>
                                <td valign="middle" align="left" class="">
                                    <asp:Label ID="lblName" runat="server" style="font-weight:bold;font-size: 15px;" />
                                    <br />
                                    <asp:Label ID="lblMobile" runat="server" />
                                    <br />
                                    <asp:Label ID="lblEmail" runat="server" />
                                </td>
                                <td valign="middle" align="center" class="">
                                    <div class="myFlexVerticleHorizentalCenter_FlexDirectionRow">
                                        <asp:Label ID="lblPaymentId" runat="server" />
                                    </div>
                                </td>
                                <td valign="middle" align="left" class="">
                                    <%--style="padding-top: 15px; padding-bottom: 15px;"--%>
                                    <div class="myFlexVerticleHorizentalCenter_FlexDirectionRow">
                                        <asp:Label ID="lblUnit" runat="server" />
                                    </div>
                                </td>
                                <td valign="middle" align="center" class="">
                                    <div class="myFlexVerticleHorizentalCenter_FlexDirectionColumn">
                                        <div>
                                            <asp:Label ID="lblEducationCategory" runat="server" />
                                        </div>
                                        <div>
                                            <asp:Label ID="lblSession" runat="server" style="font-weight: bold;color: #b0f;"/>
                                        </div>
                                    </div>
                                </td>
                                <td valign="middle" align="center" class="">
                                    <div class="myFlexVerticleHorizentalCenter_FlexDirectionRow">
                                        <asp:Label ID="lblDateApplied" runat="server" />
                                    </div>
                                </td>
                                <td valign="middle" align="center" class="">
                                    <div class="myFlexVerticleHorizentalCenter_FlexDirectionRow">
                                        <asp:Label ID="lblPaid" runat="server"></asp:Label>
                                    </div>
                                </td>

                                <td valign="middle" align="center" class="">
                                    <div class="myFlexVerticleHorizentalCenter_FlexDirectionRow">
                                        <asp:Label ID="lblfinalSubmited" runat="server"></asp:Label>
                                    </div>
                                </td>

                                <td valign="middle" align="center" class="">
                                    <div class="myFlexVerticleHorizentalCenter_FlexDirectionRow">
                                        <div class="dropdown">
                                            <button class="btn btn-default btn-sm dropdown-toggle" type="button" data-toggle="dropdown">
                                                Select Action <span class="caret"></span>
                                            </button>
                                            <ul class="dropdown-menu">
                                                <li>
                                                    <%--<asp:LinkButton ID="lblLoginCredential" runat="server" Text="Login Cred"></asp:LinkButton>--%>
                                                    <asp:HyperLink ID="hlLoginCredential" runat="server" Text="Login Cred" Target="_blank"></asp:HyperLink>
                                                </li>
                                                <li>
                                                    <%--<asp:LinkButton ID="lbForm" runat="server" Text="Form"></asp:LinkButton>--%>
                                                    <asp:HyperLink ID="hlForm" runat="server" Text="Form" Target="_blank"></asp:HyperLink>
                                                </li>
                                                <li style="display:none;">
                                                    <asp:HyperLink ID="hlAdmitCard" runat="server" Text="Admit Card" Target="_blank"></asp:HyperLink>
                                                </li>
                                            </ul>
                                        </div>
                                    </div>
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
                    <EnableAction AnimationTarget="btnFilterData" Enabled="false" />
                </Parallel>
            </OnUpdating>
            <OnUpdated>
                <Parallel duration="0">
                    <ScriptAction Script="onComplete();" />
                    <EnableAction   AnimationTarget="btnLoad" Enabled="true" />
                    <EnableAction   AnimationTarget="btnFilterData" Enabled="true" />
                </Parallel>
            </OnUpdated>
        </Animations>
    </ajaxToolkit:UpdatePanelAnimationExtender>



    <%--<div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h4>Paid Forms (Received Form)</h4>
                </div>
                <div class="panel-body">

                    <table class="table_form" style="width: 100%">
                        <tr>
                            <td style="width: 30%" class="style_td">Faculty</td>
                            <td style="width: 30%" class="style_td"></td>
                            <td style="width: 30%" class="style_td">Session<span class="asteriskColor">*</span></td>
                            <td style="width: 10%" class="style_td"></td>
                        </tr>
                        <tr>
                            <td>
                                
                            </td>
                            <td>
                                
                            </td>
                            <td>
                                
                            </td>
                            <td>
                                
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td></td>
                            <td style="padding-right: 0%">
                                <asp:TextBox ID="txtSearchText" runat="server" Width="94%" CssClass="float-left" placeholder="Payment ID / Mobile / Email"></asp:TextBox>
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
                        <tr>
                            <td></td>
                            <td></td>
                            <td style="padding-right: 0%" colspan='2' >
                                <span style="color: darkorange; font-size: 9pt;">Please include country code for Mobile Search, e.g.: +8801700000000</span>
                            </td>
                            <td></td>
                        </tr>
                    </table>

                </div>
            </div>
        </div>
    </div>

    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-info">
                <div class="panel-body">


                    <asp:Panel ID="listViewPanel" runat="server">
                        <div class="row" style="margin-bottom: 1%;">
                            Records: &nbsp;
                                    
                        </div>

                        

                        
                    </asp:Panel>


                </div>
            </div>
        </div>
    </div>--%>
</asp:Content>
