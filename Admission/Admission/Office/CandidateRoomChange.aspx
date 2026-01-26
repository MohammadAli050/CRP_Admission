<%@ Page Title="Candidate Room Change" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="CandidateRoomChange.aspx.cs" Inherits="Admission.Admission.Office.CandidateRoomChange" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">

    <style type="text/css">
        input[type="checkbox"] {
            width: 20px;
            height: 20px;
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
                    <h3>Candidate Room Change</h3>
                </div>
            </div>

            <div class="panel panel-default">
                <div class="panel-body">

                    <div class="row">
                        <div class="col-sm-5 col-md-5 col-lg-5">
                            <div class="form-group">
                                <label><strong>Faculty <span style="color: crimson; font-weight: bold;">*</span></strong></label>
                                <asp:DropDownList ID="ddlAdmUnit" runat="server" Width="100%" CssClass="form-control">
                                </asp:DropDownList>
                                <%--AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlAdmUnit_SelectedIndexChanged"--%>
                                <asp:CompareValidator ID="ddlNationality_ComV" runat="server"
                                    ControlToValidate="ddlAdmUnit"
                                    ErrorMessage="Required"
                                    Font-Bold="true"
                                    Font-Size="9pt"
                                    ForeColor="Crimson"
                                    Display="Dynamic"
                                    ValueToCompare="-1"
                                    Operator="NotEqual"
                                    ValidationGroup="gr1"></asp:CompareValidator>
                            </div>
                        </div>
                        <div class="col-sm-4 col-md-4 col-lg-4">
                            <div class="form-group">
                                <label><strong>Session <span style="color: crimson; font-weight: bold;">*</span></strong></label>
                                <asp:DropDownList ID="ddlSession" runat="server" Width="100%" CssClass="form-control">
                                </asp:DropDownList>
                                <asp:CompareValidator ID="CompareValidator1" runat="server"
                                    ControlToValidate="ddlSession"
                                    ErrorMessage="Required"
                                    Font-Bold="true"
                                    Font-Size="9pt"
                                    ForeColor="Crimson"
                                    Display="Dynamic"
                                    ValueToCompare="-1"
                                    Operator="NotEqual"
                                    ValidationGroup="gr1"></asp:CompareValidator>
                                <%--AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlSession_SelectedIndexChanged"--%>
                            </div>
                        </div>
                        <div class="col-sm-3 col-md-3 col-lg-3">
                            <br />
                            <%--<asp:Button ID="btnClear" runat="server" Text="Clear" Width="100%" CssClass="btn btn-default" 
                                    OnClick="btnClear_Click"/>--%>

                            <asp:Button ID="btnLoad" runat="server" Text="Load Report" Width="100%" CssClass="btn btn-default"
                                ValidationGroup="gr1"
                                OnClick="btnLoad_Click" />
                        </div>
                    </div>

                    <asp:Panel ID="panelFilterLoad" runat="server" Visible="false">
                        <div class="row">
                            <div class="col-sm-2 col-md-2 col-lg-2">
                                <div class="form-group">
                                    <label><strong>Venue</strong></label>
                                    <asp:DropDownList ID="ddlVenue" runat="server" Width="100%" CssClass="form-control"
                                        AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlVenue_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:CompareValidator ID="CompareValidator2" runat="server"
                                        ControlToValidate="ddlVenue"
                                        ErrorMessage="Required"
                                        Font-Bold="true"
                                        Font-Size="9pt"
                                        ForeColor="Crimson"
                                        Display="Dynamic"
                                        ValueToCompare="-1"
                                        Operator="NotEqual"
                                        ValidationGroup="gr2"></asp:CompareValidator>
                                </div>
                            </div>
                            <div class="col-sm-3 col-md-3 col-lg-3">
                                <div class="form-group">
                                    <label><strong>Campus</strong></label>
                                    <asp:DropDownList ID="ddlCampus" runat="server" Width="100%" CssClass="form-control"
                                        AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlCampus_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:CompareValidator ID="CompareValidator3" runat="server"
                                        ControlToValidate="ddlCampus"
                                        ErrorMessage="Required"
                                        Font-Bold="true"
                                        Font-Size="9pt"
                                        ForeColor="Crimson"
                                        Display="Dynamic"
                                        ValueToCompare="-1"
                                        Operator="NotEqual"
                                        ValidationGroup="gr2"></asp:CompareValidator>
                                </div>
                            </div>
                            <div class="col-sm-3 col-md-3 col-lg-3">
                                <div class="form-group">
                                    <label><strong>Building</strong></label>
                                    <asp:DropDownList ID="ddlBuilding" runat="server" Width="100%" CssClass="form-control"
                                        AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlBuilding_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:CompareValidator ID="CompareValidator4" runat="server"
                                        ControlToValidate="ddlBuilding"
                                        ErrorMessage="Required"
                                        Font-Bold="true"
                                        Font-Size="9pt"
                                        ForeColor="Crimson"
                                        Display="Dynamic"
                                        ValueToCompare="-1"
                                        Operator="NotEqual"
                                        ValidationGroup="gr2"></asp:CompareValidator>
                                </div>
                            </div>
                            <div class="col-sm-2 col-md-2 col-lg-2">
                                <div class="form-group">
                                    <label><strong>Room</strong></label>
                                    <asp:DropDownList ID="ddlRoom" runat="server" Width="100%" CssClass="form-control">
                                    </asp:DropDownList>
                                    <asp:CompareValidator ID="CompareValidator5" runat="server"
                                        ControlToValidate="ddlRoom"
                                        ErrorMessage="Required"
                                        Font-Bold="true"
                                        Font-Size="9pt"
                                        ForeColor="Crimson"
                                        Display="Dynamic"
                                        ValueToCompare="-1"
                                        Operator="NotEqual"
                                        ValidationGroup="gr2"></asp:CompareValidator>
                                </div>
                            </div>
                            <div class="col-sm-2 col-md-2 col-lg-2">
                                <br />
                                <asp:Button ID="btnChangeRoom" runat="server" Text="Make Change" Width="100%" CssClass="btn btn-danger"
                                    ValidationGroup="gr2"
                                    OnClick="btnChangeRoom_Click" />
                            </div>
                        </div>
                    </asp:Panel>

                </div>
            </div>


            <div class="row">
                <div class="col-sm-12 col-md-12 col-lg-12">
                    <asp:Panel ID="messagePanel" runat="server">
                        <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                    </asp:Panel>
                </div>
            </div>

            <hr />

            <asp:Panel ID="listViewPanel" runat="server">

                <div class="panel panel-default">
                    <div class="panel-body">

                        <table>
                            <tr>
                                <th>
                                    <div class="row" style="margin-bottom: 1%;">
                                        Records: &nbsp;
                                                <asp:Label ID="lblCount" runat="server" CssClass="badge"></asp:Label>
                                    </div>
                                </th>
                            </tr>
                        </table>

                        <asp:ListView ID="lvRoomInfo" runat="server"
                            OnItemDataBound="lvRoomInfo_ItemDataBound"
                            OnPagePropertiesChanging="lvRoomInfo_PagePropertiesChanging"
                            OnItemCommand="lvRoomInfo_ItemCommand">
                            <LayoutTemplate>
                                <table id="tblFormRequest"
                                    class="table_form table-bordered"
                                    style="width: 100%; text-align: left">
                                    <tr runat="server" style="background-color: #1387de; color: white; height: 25px; font-size: small">
                                        <th runat="server" style="text-align: center">SL#</th>
                                        <td style="display: none;"></td>
                                        <td runat="server" style="text-align: center">
                                            <asp:CheckBox ID="cbCheckAll" runat="server" Text="All" AutoPostBack="true" OnCheckedChanged="cbCheckAll_CheckedChanged" />
                                        </td>
                                        <th runat="server" style="text-align: center">Test Roll</th>
                                        <th runat="server" style="text-align: center">Candidate</th>
                                        <th runat="server" style="text-align: center">Room Info</th>
                                    </tr>
                                    <tr runat="server" id="itemPlaceholder" />
                                </table>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <tr runat="server" style="font-size: 10pt">
                                    <td valign="middle" align="center" class="">
                                        <%--<asp:Label ID="lblSerial" runat="server" />.--%>
                                        <%#Container.DataItemIndex+1 %>
                                            </td>
                                    <td valign="middle" align="left" class="" style="display: none;">
                                        <asp:HiddenField ID="hfPaymentID" runat="server" />
                                        <asp:HiddenField ID="hfCandidateID" runat="server" />
                                        <asp:HiddenField ID="hfAcaCalID" runat="server" />
                                        <asp:HiddenField ID="hfAdmissionUnitID" runat="server" />
                                        <asp:HiddenField ID="hfAdmissionTestRollID" runat="server" />
                                        <asp:HiddenField ID="hfAdmissionTestRollProgramRoomPriorityID" runat="server" />
                                        <asp:HiddenField ID="hfVenueID" runat="server" />
                                    </td>
                                    <td valign="middle" align="center" class="">
                                        <asp:CheckBox ID="cbSingle" runat="server" />
                                    </td>
                                    <td valign="middle" align="center" class="">
                                        <asp:Label ID="lblTestRoll" runat="server" />
                                    </td>
                                    <td valign="middle" align="left" class="">
                                        <strong>Name: </strong>
                                        <asp:Label ID="lblCandidateName" runat="server" />
                                        <br />
                                        <strong>PaymentId: </strong>
                                        <asp:Label ID="lblCandidatePaymentId" runat="server" />
                                        <br />
                                        <strong>Phone: </strong>
                                        <asp:Label ID="lblPhone" runat="server" />
                                        <br />
                                        <strong>Email: </strong>
                                        <asp:Label ID="lblEmail" runat="server" />
                                    </td>
                                    <td valign="middle" align="left" class="">
                                        <strong>Venue: </strong>
                                        <asp:Label ID="lblVenue" runat="server" />
                                        <br />
                                        <strong>Campus: </strong>
                                        <asp:Label ID="lblCampus" runat="server" />
                                        <br />
                                        <strong>Building: </strong>
                                        <asp:Label ID="lblBuilding" runat="server" />
                                        <br />
                                        <strong>Room: </strong>
                                        <asp:Label ID="lblRoom" runat="server" />
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
                                PagedControlID="lvRoomInfo" PageSize="50">
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
            </asp:Panel>




        </ContentTemplate>
    </asp:UpdatePanel>



    <ajaxToolkit:UpdatePanelAnimationExtender ID="UpdatePanelAnimationExtender1" TargetControlID="updatePanelAll" runat="server">
        <Animations>
            <OnUpdating>
                <Parallel duration="0">
                    <ScriptAction Script="InProgress();" />
                    <EnableAction AnimationTarget="btnLoad" Enabled="false" />
                    <EnableAction AnimationTarget="btnChangeRoom" Enabled="false" />
                </Parallel>
            </OnUpdating>
            <OnUpdated>
                <Parallel duration="0">
                    <ScriptAction Script="onComplete();" />
                    <EnableAction   AnimationTarget="btnLoad" Enabled="true" />
                    <EnableAction   AnimationTarget="btnChangeRoom" Enabled="true" />
                </Parallel>
            </OnUpdated>
        </Animations>
    </ajaxToolkit:UpdatePanelAnimationExtender>



</asp:Content>
