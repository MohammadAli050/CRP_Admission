<%@ Page Title="" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="ApproveCandidateV3.aspx.cs" Inherits="Admission.Admission.Office.ApproveCandidateV3" %>




<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">


    <style type="text/css">
        input[type="checkbox"] {
            width: 20px !important;
            height: 20px !important;
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
                    <h3>Approve Candidate</h3>
                    <%--<hr style="margin-top: 0px; margin-bottom: 10px;" />--%>
                </div>
            </div>

            <div class="panel panel-default">
                <div class="panel-body">
                    <div class="row">
                        <div class="col-sm-3 col-md-3 col-lg-3">
                            <div class="form-group">
                                <label><strong>Faculty <span style="color: crimson; font-weight: bold;">*</span></strong></label>
                                <asp:DropDownList ID="ddlFaculty" runat="server" Width="100%" CssClass="form-control"></asp:DropDownList>
                                <asp:CompareValidator ID="CompareValidator2" runat="server" Display="Dynamic"
                                    ErrorMessage="Required" ForeColor="Crimson" ControlToValidate="ddlFaculty"
                                    Font-Size="10pt"
                                    ValueToCompare="-1" Operator="NotEqual" ValidationGroup="loadVg"></asp:CompareValidator>
                            </div>
                        </div>
                        <div class="col-sm-3 col-md-3 col-lg-3">
                            <div class="form-group">
                                <label><strong>Education Category <span style="color: crimson; font-weight: bold;">*</span></strong></label>
                                <asp:DropDownList ID="ddlEducationCategory" runat="server" Width="100%" CssClass="form-control"></asp:DropDownList>
                                <asp:CompareValidator ID="CompareValidator1" runat="server" Display="Dynamic"
                                    ErrorMessage="Required" ForeColor="Crimson" ControlToValidate="ddlEducationCategory"
                                    Font-Size="10pt"
                                    ValueToCompare="-1" Operator="NotEqual" ValidationGroup="loadVg"></asp:CompareValidator>
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
                            <div class="form-group">
                                <label><strong>SSC/O-Level/Dakhil GPA >=</strong></label>
                                <asp:TextBox ID="txtSSCGpa" runat="server" Text="" Width="100%" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                    </div>

                    <div class="row">

                        <div class="col-sm-3 col-md-3 col-lg-3">
                            <div class="form-group">
                                <label><strong>HSC/A-Level/Alim GPA >=</strong></label>
                                <asp:TextBox ID="txtHSCGpa" runat="server" Text="" Width="100%" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-sm-3 col-md-3 col-lg-3">
                            <div class="form-group">
                                <label><strong>SSC/O-Level/Dakhil Group/Subject</strong></label>
                                <asp:DropDownList ID="ddlSSCGrpSub" runat="server" Width="100%" CssClass="form-control"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-sm-3 col-md-3 col-lg-3">
                            <div class="form-group">
                                <label><strong>HSC/A-Level/Alim Group/Subject</strong></label>
                                <asp:DropDownList ID="ddlHSCGrpSub" runat="server" Width="100%" CssClass="form-control"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-sm-3 col-md-3 col-lg-3">
                            <div class="form-group">
                                <label><strong>Undergrad GPA >=</strong></label>
                                <asp:TextBox ID="txtUndergradGpa" runat="server" Text="" Width="100%" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-sm-3 col-md-3 col-lg-3">
                            <div class="form-group">
                                <label><strong>Final Submit</strong></label>
                                <asp:DropDownList ID="ddlIsFinalSubmit" runat="server" Width="100%" CssClass="form-control"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-sm-3 col-md-3 col-lg-3">
                            <div class="form-group">
                                <label><strong>Approved</strong></label>
                                <asp:DropDownList ID="ddlIsApproved" runat="server" Width="100%" CssClass="form-control"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-sm-3 col-md-3 col-lg-3">
                            <div class="form-group">
                                <label><strong>Quota</strong></label>
                                <asp:DropDownList ID="ddlQuota" runat="server" Width="100%" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-sm-3 col-md-3 col-lg-3" runat="server" id="divStatus">
                            <div class="form-group">
                                <label><strong>Status</strong></label>
                                <asp:DropDownList ID="ddlStatus" runat="server" Width="100%" CssClass="form-control">
                                    <asp:ListItem Value="0" Selected="True">Valid</asp:ListItem>
                                    <asp:ListItem Value="1">In-Valid</asp:ListItem>

                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-sm-3 col-md-3 col-lg-3">
                            <div class="form-group">
                                <br />
                                <asp:Button ID="btnLoad" runat="server" Text="Load" CssClass="btn btn-info"
                                    Style="width: 100%; margin-top: 4px;"
                                    ValidationGroup="loadVg" OnClick="btnLoad_Click" />
                            </div>
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
                                    <div class="col-sm-3 col-md-3 col-lg-3">
                                        <div class="form-group">
                                            <label><strong>User ID</strong></label>
                                            <asp:TextBox ID="txtFilterUserID" runat="server"
                                                placeholder="Enter Candidate User ID"
                                                CssClass="form-control" Width="100%"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-sm-3 col-md-3 col-lg-3">
                                        <br />

                                        <asp:LinkButton ID="btnFilterClear" runat="server" Width="30%"
                                            OnClick="btnFilterClear_Click"
                                            CssClass="btn btn-default btn-sm form-control">
                                   <strong>Clear</strong>
                                        </asp:LinkButton>
                                        &nbsp;
&nbsp;
                                        <asp:HiddenField ID="hfIsFilterClick" runat="server" />
                                        <asp:LinkButton ID="btnFilterData" runat="server" Width="30%"
                                            ValidationGroup="gr1" OnClick="btnFilterData_Click"
                                            CssClass="btn btn-info btn-sm form-control">
            <strong>Filter</strong>
                                        </asp:LinkButton>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-sm-3 col-md-3 col-lg-3">
                                    </div>
                                    <div class="col-sm-3 col-md-3 col-lg-3">
                                    </div>
                                    <div class="col-sm-3 col-md-3 col-lg-3">
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


            <div class="row">
                <div class="col-sm-12 col-md-12 col-lg-12" style="text-align: right;">
                    <asp:Button ID="btnApproveUG" runat="server" Text="Approve For UG" Visible="false"
                        OnClientClick="return confirm('Are you sure, you want to Approved')"
                        CssClass="btn btn-danger" OnClick="btnApproveUG_Click" />
                    <asp:Button ID="btnApprovePG" runat="server" Text="Approve For PG" Visible="false"
                        OnClientClick="return confirm('Are you sure, you want to Approved')"
                        CssClass="btn btn-danger" OnClick="btnApprovePG_Click" />
                </div>
            </div>

            <asp:Panel ID="panelBachelor" runat="server" Visible="false">
                <div class="panel panel-default">
                    <div class="panel-body">
                        <p style="font-size: 15px;">
                            <span class="label label-primary">Records:&nbsp;<asp:Label ID="lblCountBachelor" runat="server"></asp:Label></span>
                        </p>


                        <asp:ListView ID="lvBachelor" runat="server"
                            OnItemDataBound="lvBachelor_ItemDataBound"
                            OnPagePropertiesChanging="lvBachelor_PagePropertiesChanging">
                            <LayoutTemplate>
                                <table id="tblFormRequest"
                                    class="table table-bordered table-hover"
                                    style="width: 100%; text-align: left">
                                    <tr runat="server" style="background-color: #1387de; color: white; height: 25px; font-size: small">
                                        <th runat="server" style="text-align: center">SL#</th>
                                        <th runat="server" style="text-align: center">Candidate</th>
                                        <th runat="server" style="text-align: center">Payment ID</th>
                                        <th runat="server" style="text-align: center">Photo / Signature</th>

                                        <th runat="server" style="text-align: center">SSC / O-Level / Dakhil GPA</th>
                                        <th runat="server" style="text-align: center">SSC / O-Level / Dakhil Exam Type</th>
                                        <th runat="server" style="text-align: center">SSC / O-Level / Dakhil Group</th>
                                        <th runat="server" style="text-align: center">SSC / O-Level / Dakhil Year</th>

                                        <th runat="server" style="text-align: center">HSC / A-Level / Alim GPA</th>
                                        <th runat="server" style="text-align: center">HSC / A-Level / Alim Exam Type</th>
                                        <th runat="server" style="text-align: center">HSC / A-Level / Alim Group</th>
                                        <th runat="server" style="text-align: center">HSC / A-Level / Alim Year</th>

                                        <th runat="server" style="text-align: center">GPA Total</th>

                                        <th runat="server" style="text-align: center">Quota</th>

                                        <th runat="server" style="text-align: center">
                                            <asp:CheckBox runat="server" ID="cbSelectAllBachelor" Font-Bold="true" Text="<b> Select All</b>"
                                                OnCheckedChanged="cbSelectAllBachelor_CheckedChanged" AutoPostBack="true" />
                                        </th>
                                        <th runat="server" style="text-align: center">Photo</th>
                                        <th runat="server" style="text-align: center">Signature</th>
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
                                        <asp:Label ID="lblName" runat="server" Style="font-weight: bold; font-size: 15px;" />
                                        <br />
                                        <asp:Label ID="lblMobile" runat="server" />
                                        <br />
                                        <asp:Label ID="lblEmail" runat="server" />
                                        <br />
                                        <asp:Label ID="lblFaculty" runat="server" />

                                        <asp:HiddenField ID="hfBachelorCandidateId" runat="server" />
                                        <asp:HiddenField ID="hfBachelorAcaCalId" runat="server" />
                                        <asp:HiddenField ID="hfBachelorPaymentId" runat="server" />
                                        <asp:HiddenField ID="hfBachelorFormSerial" runat="server" />
                                        <asp:HiddenField ID="hfBachelorAdmissionSetupId" runat="server" />
                                        <asp:HiddenField ID="hfBachelorAdmissionUnitId" runat="server" />

                                    </td>

                                    <td valign="middle" align="center" class="">
                                        <asp:Label ID="lblPaymentId" runat="server" />
                                    </td>

                                    <td valign="middle" align="center" class="">
                                        <asp:Label ID="lblPhotoSignature" runat="server" />
                                    </td>


                                    <td valign="middle" align="left" class="">
                                        <asp:Label ID="lblSSCOLevelDakhilGPA" runat="server" />
                                    </td>
                                    <td valign="middle" align="left" class="">
                                        <asp:Label ID="lblSSCOLevelDakhilExamType" runat="server" />
                                    </td>
                                    <td valign="middle" align="left" class="">
                                        <asp:Label ID="lblSSCOLevelDakhilGroup" runat="server" />
                                    </td>
                                    <td valign="middle" align="left" class="">
                                        <asp:Label ID="lblSSCOLevelDakhilYear" runat="server" />
                                    </td>

                                    <td valign="middle" align="left" class="">
                                        <asp:Label ID="lblHSCALevelAlimGPA" runat="server" />
                                    </td>
                                    <td valign="middle" align="left" class="">
                                        <asp:Label ID="lblHSCALevelAlimExamType" runat="server" />
                                    </td>
                                    <td valign="middle" align="left" class="">
                                        <asp:Label ID="lblHSCALevelAlimGroup" runat="server" />
                                    </td>
                                    <td valign="middle" align="left" class="">
                                        <asp:Label ID="lblHSCALevelAlimYear" runat="server" />
                                    </td>

                                    <td valign="middle" align="left" class="">
                                        <asp:Label ID="lblTotalGPA" runat="server" />
                                    </td>

                                    <td valign="middle" align="left" class="">
                                        <asp:Label ID="lblQuotaName" runat="server" />
                                    </td>

                                    <td valign="middle" align="center" class="">
                                        <asp:CheckBox ID="cbSingleBachelor" runat="server" />
                                    </td>
                                    <td valign="middle" align="center" class="">
                                        <div style="text-align: center">
                                            <asp:Image ID="Photo1" Height="60px" Width="60px" runat="server" Style="border-radius: 50%" />
                                        </div>
                                    </td>
                                    <td valign="middle" align="center" class="">
                                        <div style="text-align: center">
                                            <asp:Image ID="Sign1" Height="60px" Width="60px" runat="server" Style="border-radius: 50%" />
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
                            <asp:DataPager runat="server" ID="DataPagerBachelor"
                                PagedControlID="lvBachelor" PageSize="1000">
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



            <asp:Panel ID="panelMasters" runat="server" Visible="false">
                <div class="panel panel-default">
                    <div class="panel-body">
                        <p style="font-size: 15px;">
                            <span class="label label-primary">Records:&nbsp;<asp:Label ID="lblCountMasters" runat="server"></asp:Label></span>
                        </p>


                        <asp:ListView ID="lvMasters" runat="server"
                            OnItemDataBound="lvMasters_ItemDataBound"
                            OnPagePropertiesChanging="lvMasters_PagePropertiesChanging">
                            <LayoutTemplate>
                                <table id="tblFormRequest"
                                    class="table table-bordered table-hover"
                                    style="width: 100%; text-align: left">
                                    <tr runat="server" style="background-color: #1387de; color: white; height: 25px; font-size: small">
                                        <th runat="server" style="text-align: center">SL#</th>
                                        <th runat="server" style="text-align: center">Candidate</th>
                                        <th runat="server" style="text-align: center">Payment ID</th>
                                        <th runat="server" style="text-align: center">Photo / Signature</th>

                                        <th runat="server" style="text-align: center; background-color: #0e69ae;">SSC / O-Level / Dakhil GPA, Exam, Group, Year</th>

                                        <th runat="server" style="text-align: center; background-color: #0e69ae;">HSC / A-Level / Alim GPA, Exam, Group, Year</th>

                                        <th runat="server" style="text-align: center">GPA Total</th>

                                        <th runat="server" style="text-align: center">Undergrad CGPA</th>
                                        <th runat="server" style="text-align: center">Undergrad Program</th>
                                        <th runat="server" style="text-align: center">Undergrad Institute</th>
                                        <th runat="server" style="text-align: center">Undergrad Year</th>


                                        <th runat="server" style="text-align: center">Quota</th>

                                        <th runat="server" style="text-align: center">
                                            <asp:CheckBox runat="server" ID="cbSelectAllMasters" Font-Bold="true" Text="<b> Select All</b>"
                                                OnCheckedChanged="cbSelectAllMasters_CheckedChanged" AutoPostBack="true" />
                                        </th>
                                        <th runat="server" style="text-align: center">Photo</th>
                                        <th runat="server" style="text-align: center">Signature</th>
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
                                        <asp:Label ID="lblName" runat="server" Style="font-weight: bold; font-size: 15px;" />
                                        <br />
                                        <asp:Label ID="lblMobile" runat="server" />
                                        <br />
                                        <asp:Label ID="lblEmail" runat="server" />
                                        <br />
                                        <asp:Label ID="lblFaculty" runat="server" />

                                        <asp:HiddenField ID="hfMastersCandidateId" runat="server" />
                                        <asp:HiddenField ID="hfMastersAcaCalId" runat="server" />
                                        <asp:HiddenField ID="hfMastersPaymentId" runat="server" />
                                        <asp:HiddenField ID="hfMastersFormSerial" runat="server" />
                                        <asp:HiddenField ID="hfMastersAdmissionSetupId" runat="server" />
                                        <asp:HiddenField ID="hfMastersAdmissionUnitId" runat="server" />
                                    </td>

                                    <td valign="middle" align="center" class="">
                                        <asp:Label ID="lblPaymentId" runat="server" />
                                    </td>

                                    <td valign="middle" align="center" class="">
                                        <asp:Label ID="lblPhotoSignature" runat="server" />
                                    </td>


                                    <td valign="middle" align="left" class="" style="background-color: #f4f3f3;">
                                        <asp:Label ID="lblSSCOLevelDakhilGPA" runat="server" />,
                                        <br />
                                        <asp:Label ID="lblSSCOLevelDakhilExamType" runat="server" />,
                                        <br />
                                        <asp:Label ID="lblSSCOLevelDakhilGroup" runat="server" />,
                                        <br />
                                        <asp:Label ID="lblSSCOLevelDakhilYear" runat="server" /><br />
                                    </td>

                                    <td valign="middle" align="left" class="" style="background-color: #f4f3f3;">
                                        <asp:Label ID="lblHSCALevelAlimGPA" runat="server" />,
                                        <br />
                                        <asp:Label ID="lblHSCALevelAlimExamType" runat="server" />,
                                        <br />
                                        <asp:Label ID="lblHSCALevelAlimGroup" runat="server" />,
                                        <br />
                                        <asp:Label ID="lblHSCALevelAlimYear" runat="server" />
                                    </td>

                                    <td valign="middle" align="left" class="">
                                        <asp:Label ID="lblTotalGPA" runat="server" />
                                    </td>

                                    <td valign="middle" align="left" class="">
                                        <asp:Label ID="lblUndergradeGPA" runat="server" />
                                    </td>
                                    <td valign="middle" align="left" class="">
                                        <asp:Label ID="lblUndergradeProgram" runat="server" />
                                    </td>
                                    <td valign="middle" align="left" class="">
                                        <asp:Label ID="lblUndergradeInstitute" runat="server" />
                                    </td>
                                    <td valign="middle" align="left" class="">
                                        <asp:Label ID="lblUndergradeYear" runat="server" />
                                    </td>

                                    <td valign="middle" align="left" class="">
                                        <asp:Label ID="lblQuotaName" runat="server" />
                                    </td>

                                    <td valign="middle" align="center" class="">
                                        <asp:CheckBox ID="cbSingleMasters" runat="server" />
                                    </td>
                                    <td valign="middle" align="center" class="">
                                        <div style="text-align: center">
                                            <asp:Image ID="Photo2" Height="60px" Width="60px" runat="server" Style="border-radius: 50%" />
                                        </div>
                                    </td>
                                    <td valign="middle" align="center" class="">
                                        <div style="text-align: center">
                                            <asp:Image ID="Sign2" Height="60px" Width="60px" runat="server" Style="border-radius: 50%" />
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
                            <asp:DataPager runat="server" ID="DataPagerMasters"
                                PagedControlID="lvMasters" PageSize="1000">
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
                    <%--<EnableAction AnimationTarget="btnFilterData" Enabled="false" />--%>
                </Parallel>
            </OnUpdating>
            <OnUpdated>
                <Parallel duration="0">
                    <ScriptAction Script="onComplete();" />
                    <EnableAction   AnimationTarget="btnLoad" Enabled="true" />
                    <%--<EnableAction   AnimationTarget="btnFilterData" Enabled="true" />--%>
                </Parallel>
            </OnUpdated>
        </Animations>
    </ajaxToolkit:UpdatePanelAnimationExtender>

</asp:Content>
