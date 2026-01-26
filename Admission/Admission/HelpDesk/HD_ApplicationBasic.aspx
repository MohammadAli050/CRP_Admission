<%@ Page Title="" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="HD_ApplicationBasic.aspx.cs" Inherits="Admission.Admission.HelpDesk.HD_ApplicationBasic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <link href="../../../Content/ApplicationForm.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-heading panelHeaderHeight">
                    <ol class="breadcrumb">
                        <%--<li class="active">Basic</li>--%>
                        <li class="active"><asp:HyperLink ID="hrefAppBasic" runat="server">Basic Info</asp:HyperLink></li>
                        <li><asp:HyperLink ID="hrefAppPriority" runat="server">Program Priority</asp:HyperLink></li>
                        <li><asp:HyperLink ID="hrefAppEducation" runat="server">Education</asp:HyperLink></li>
                        <li><asp:HyperLink ID="hrefAppRelation" runat="server">Parent/Guardian</asp:HyperLink></li>
                        <li><asp:HyperLink ID="hrefAppAddress" runat="server">Address</asp:HyperLink></li>
                        <li><asp:HyperLink ID="hrefAppAdditional" runat="server">Additional/Work Experience</asp:HyperLink></li>
                        <%--<li><asp:HyperLink ID="hrefAppFinGuar" runat="server">Financial Guarantor</asp:HyperLink></li>--%>
                        <li><asp:HyperLink ID="hrefAppAttachment" runat="server">Upload Photo</asp:HyperLink></li>
                    </ol>
                </div>
                <%-- end panel heading --%>
                <div class="panel-body">
                    
                    <table style="width: 100%" class="table table-condensed table-striped">
                        <tr>
                            <td colspan="4" class="style_td">
                                <span class="spanAsterisk">*</span> indicate required fields.<br />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 15%" class="style_td">Name in FULL <span class="spanAsterisk">*</span></td>
                            <td style="width: 35%" colspan="3">
                                <asp:TextBox ID="txtFirstName" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                            </td>
                            <%--<td style="width: 15%" class="style_td style_td_secondCol">Middle Name</td>
                            <td style="width: 35%">
                                <asp:TextBox ID="txtMiddleName" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                            </td>--%>
                        </tr>
                        <%--<tr>
                            <td class="style_td">Last Name</td>
                            <td>
                                <asp:TextBox ID="txtLastName" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                            </td>
                            <td class="style_td style_td_secondCol">Nick Name</td>
                            <td>
                                <asp:TextBox ID="txtNickName" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                            </td>
                        </tr>--%>
                        <tr>
                            <td class="style_td" style="width: 15%">Date Of Birth <span class="spanAsterisk">*</span></td>
                            <td style="width: 35%">
                                <asp:TextBox ID="txtDateOfBirth" runat="server" Width="50%" CssClass="form-control" placeholder="dd/MM/yyyy"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender ID="CalenderExtender_DOB" runat="server"
                                    TargetControlID="txtDateOfBirth" Format="dd/MM/yyyy" />
                            </td>
                            <td class="style_td style_td_secondCol" style="width: 15%">Place of Birth<span class="spanAsterisk">*</span></td>
                            <td style="width: 35%">
                                <asp:TextBox ID="txtPlaceOfBirth" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="style_td">Nationality<span class="spanAsterisk">*</span></td>
                            <td>
                                <asp:DropDownList ID="ddlNationality" runat="server" Width="85%" CssClass="form-control"></asp:DropDownList>
                            </td>
                            <td class="style_td style_td_secondCol">Mother Tongue<span class="spanAsterisk">*</span></td>
                            <td>
                                <asp:DropDownList ID="ddlLanguage" runat="server" Width="85%" CssClass="form-control"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="style_td">Gender <span class="spanAsterisk">*</span></td>
                            <td>
                                <asp:DropDownList ID="ddlGender" runat="server" Width="85%" CssClass="form-control"></asp:DropDownList>
                            </td>
                            <td class="style_td style_td_secondCol">Marital Status<span class="spanAsterisk">*</span></td>
                            <td>
                                <asp:DropDownList ID="ddlMaritalStatus" runat="server" Width="85%" CssClass="form-control"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="style_td">National ID No.</td>
                            <td>
                                <asp:TextBox ID="txtNationalId" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                            </td>
                            <%--<td class="style_td style_td_secondCol">Birth Registration No.</td>
                            <td>
                                <asp:TextBox ID="txtBirthRegNo" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                            </td>--%>
                            <td class="style_td style_td_secondCol">Religion <span class="spanAsterisk">*</span></td>
                            <td>
                                
                                <asp:DropDownList ID="ddlReligion" runat="server" Width="85%" CssClass="form-control"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="style_td">Blood Group <span class="spanAsterisk">*</span></td>
                            <td>
                                <asp:DropDownList ID="ddlBloodGroup" runat="server" Width="85%" CssClass="form-control"></asp:DropDownList>
                            </td>
                            <td class="style_td style_td_secondCol">Email <span class="spanAsterisk">*</span></td>
                            <td>
                                <asp:TextBox ID="txtEmail" runat="server" Width="100%" TextMode="Email" CssClass="form-control"></asp:TextBox>
                            </td>
                        </tr>
                        <%--<td class="style_td style_td_secondCol">Phone<br />
                                (Res)</td>
                            <td>
                                <asp:TextBox ID="txtPhoneRes" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="style_td">Emergency Phone</td>
                            <td>
                                <asp:TextBox ID="txtPhoneEmergency" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                            </td>--%>
                        <tr>
                            <td class="style_td">Mobile <span class="spanAsterisk">*</span></td>
                            <td>
                                <asp:TextBox ID="txtMobile" runat="server" Width="100%" CssClass="form-control" 
                                    placeholer="Format: +8801XXXXXXXXX"></asp:TextBox>
                                <asp:RegularExpressionValidator runat="server" ID="mobileReg"
                                    ValidationGroup="basic1"
                                    ForeColor="Crimson"
                                    ErrorMessage="Invalid format."
                                    ControlToValidate="txtMobile"
                                    ValidationExpression="^(\+88)\d{11}$"></asp:RegularExpressionValidator>
                            </td>
                            <td class="style_td style_td_secondCol">Quota<span class="spanAsterisk">*</span></td>
                            <td>
                                <asp:DropDownList ID="ddlQuota" runat="server" Width="85%" CssClass="form-control"></asp:DropDownList>
                            </td>
                            <td></td>
                            <td></td>
                        </tr>
                    </table>

                    <%--=========== Quota Info ===========--%>
                    <asp:UpdatePanel ID="UpdatePanelQuotaNote" runat="server">
                        <ContentTemplate>
                            <asp:Panel ID="panelQuotaNote" runat="server" Visible="false">
                                <div class="alert alert-info">
                                    <strong>Quota Note!</strong>
                                    <br />
                                    <asp:Panel ID="panelQuotaNoteSpecialQuota" runat="server" Visible="false">
                                        <ul>
                                            <li>Children of Military Personnel (Serving and Retired)</li>
                                            <li>Children of BUP Permanent Teacher, Officers, and Staffs (Serving and Retired)</li>
                                            <li>Children of Sitting members of BUP Governing Bodies (Senate, Syndicate, Academic Council and Finance Committee)</li>
                                        </ul>
                                    </asp:Panel>
                                    <asp:Panel ID="panelQuotaNoteFreedomFighter" runat="server" Visible="false">
                                        <ul>
                                            <li>Children/Grand Children of Freedom Fighter</li>
                                        </ul>
                                    </asp:Panel>
                                    <asp:Panel ID="panelQuotaNotePersonWithDisability" runat="server" Visible="false">
                                        <ul>
                                            <li>Person with Disability (Physical) except Deaf, Mute, Blind or Multiple</li>
                                        </ul>
                                    </asp:Panel>
                                </div>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <asp:UpdatePanel ID="UpdatePanelQuotaInfo" runat="server">
                        <ContentTemplate>
                            <asp:Panel ID="panelQuotaInfo" runat="server" Visible="false">
                                <div class="panel panel-info">
                                    <div class="panel-heading text-center"><span style="font-size: 19px;"><strong>Quota Information</strong></span></div>
                                    <div class="panel-body">
                                        <div class="row" style="margin-bottom: 5px;">
                                            <div class="col-sm-6 col-md-6 col-lg-6">
                                                <div class="form-group">
                                                    <label>Type of Special Quota<span class="spanAsterisk">*</span></label>
                                                    <asp:DropDownList ID="ddlSQQuotaType" runat="server" Width="100%" CssClass="form-control"></asp:DropDownList>
                                                    <asp:CompareValidator ID="CompareValidator10" runat="server"
                                                        ControlToValidate="ddlSQQuotaType"
                                                        ErrorMessage="Required"
                                                        Display="Dynamic"
                                                        Font-Size="9pt"
                                                        ForeColor="Crimson"
                                                        ValueToCompare="-1"
                                                        Operator="NotEqual"
                                                        ValidationGroup="basic1">
                                                    </asp:CompareValidator>
                                                </div>
                                            </div>
                                            <div class="col-sm-6 col-md-6 col-lg-6">
                                                <div class="form-group">
                                                    <label>Father/Mother’s Name<span class="spanAsterisk">*</span></label>
                                                    <asp:TextBox ID="txtSQFatherOrMotherName" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                    <asp:RequiredFieldValidator runat="server"
                                                        ID="RequiredFieldValidator8"
                                                        ValidationGroup="basic1"
                                                        ControlToValidate="txtSQFatherOrMotherName"
                                                        Display="Dynamic"
                                                        Font-Size="9pt"
                                                        ForeColor="Crimson"
                                                        Font-Bold="false"
                                                        ErrorMessage="Required" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row" style="margin-bottom: 5px;">
                                            <div class="col-sm-6 col-md-6 col-lg-6">
                                                <div class="form-group">
                                                    <label>Rank/Designation<span class="spanAsterisk">*</span></label>
                                                    <asp:TextBox ID="txtSQRankOrDesignation" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                    <asp:RequiredFieldValidator runat="server"
                                                        ID="RequiredFieldValidator1"
                                                        ValidationGroup="basic1"
                                                        ControlToValidate="txtSQRankOrDesignation"
                                                        Display="Dynamic"
                                                        Font-Size="9pt"
                                                        ForeColor="Crimson"
                                                        Font-Bold="false"
                                                        ErrorMessage="Required" />
                                                </div>
                                            </div>
                                            <div class="col-sm-6 col-md-6 col-lg-6">
                                                <div class="form-group">
                                                    <label>Personal No./Sena No./BUP No.<span class="spanAsterisk">*</span></label>
                                                    <asp:TextBox ID="txtSQSenaNoOrBUPNo" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                    <asp:RequiredFieldValidator runat="server"
                                                        ID="RequiredFieldValidator2"
                                                        ValidationGroup="basic1"
                                                        ControlToValidate="txtSQSenaNoOrBUPNo"
                                                        Display="Dynamic"
                                                        Font-Size="9pt"
                                                        ForeColor="Crimson"
                                                        Font-Bold="false"
                                                        ErrorMessage="Required" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-6 col-md-6 col-lg-6">
                                                <div class="form-group">
                                                    <label>Serving/Retired<span class="spanAsterisk">*</span></label>
                                                    <%--<asp:TextBox ID="txtServingOrRetired" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                    <asp:RequiredFieldValidator runat="server"
                                                        ID="RequiredFieldValidator3"
                                                        ValidationGroup="basic1"
                                                        ControlToValidate="txtServingOrRetired"
                                                        Display="Dynamic"
                                                        Font-Size="9pt"
                                                        ForeColor="Crimson"
                                                        Font-Bold="false"
                                                        ErrorMessage="Required" />--%>
                                                    <asp:DropDownList ID="ddlSQServingOrRetired" runat="server" CssClass="form-control" Width="100%">
                                                        <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                                        <asp:ListItem Value="1">Serving</asp:ListItem>
                                                        <asp:ListItem Value="2">Retired</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:CompareValidator ID="CompareValidator1" runat="server"
                                                        ControlToValidate="ddlSQServingOrRetired"
                                                        ErrorMessage="Required"
                                                        Display="Dynamic"
                                                        Font-Size="9pt"
                                                        ForeColor="Crimson"
                                                        ValueToCompare="-1"
                                                        Operator="NotEqual"
                                                        ValidationGroup="basic1">
                                                    </asp:CompareValidator>
                                                </div>
                                            </div>
                                            <div class="col-sm-6 col-md-6 col-lg-6">
                                                <div class="form-group">
                                                    <label>Job Location<span class="spanAsterisk">*</span></label>
                                                    <asp:TextBox ID="txtSQJobLocation" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                    <asp:RequiredFieldValidator runat="server"
                                                        ID="RequiredFieldValidator4"
                                                        ValidationGroup="basic1"
                                                        ControlToValidate="txtSQJobLocation"
                                                        Display="Dynamic"
                                                        Font-Size="9pt"
                                                        ForeColor="Crimson"
                                                        Font-Bold="false"
                                                        ErrorMessage="Required" />
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                </div>
                            </asp:Panel>

                            <asp:Panel ID="panelFreedomFighterInfo" runat="server" Visible="false">
                                <div class="panel panel-info">
                                    <div class="panel-heading text-center"><span style="font-size: 19px;"><strong>Quota Information</strong></span></div>
                                    <div class="panel-body">

                                        <div class="row" style="margin-bottom: 5px;">
                                            <div class="col-sm-12 col-md-12 col-lg-12">
                                                <div class="form-group">
                                                    <label>Name of Freedom Fighter<span class="spanAsterisk">*</span></label>
                                                    <asp:TextBox ID="txtFFName" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                    <asp:RequiredFieldValidator runat="server"
                                                        ID="RequiredFieldValidator3"
                                                        ValidationGroup="basic1"
                                                        ControlToValidate="txtFFName"
                                                        Display="Dynamic"
                                                        Font-Size="9pt"
                                                        ForeColor="Crimson"
                                                        Font-Bold="false"
                                                        ErrorMessage="Required" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row" style="margin-bottom: 5px;">
                                            <div class="col-sm-12 col-md-12 col-lg-12">
                                                <div class="form-group">
                                                    <label>Relation With Applicant<span class="spanAsterisk">*</span></label>
                                                    <asp:DropDownList ID="ddlFFQuotaType" runat="server" Width="100%" CssClass="form-control"></asp:DropDownList>
                                                    <asp:CompareValidator ID="CompareValidator3" runat="server"
                                                        ControlToValidate="ddlFFQuotaType"
                                                        ErrorMessage="Required"
                                                        Display="Dynamic"
                                                        Font-Size="9pt"
                                                        ForeColor="Crimson"
                                                        ValueToCompare="-1"
                                                        Operator="NotEqual"
                                                        ValidationGroup="basic1">
                                                    </asp:CompareValidator>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>

                            <asp:Panel ID="panelPersonWithDisabilityInfo" runat="server" Visible="false">
                                <div class="panel panel-info">
                                    <div class="panel-heading text-center"><span style="font-size: 19px;"><strong>Quota Information</strong></span></div>
                                    <div class="panel-body">
                                        <%--<div class="row" style="margin-bottom: 5px;">
                                            <div class="col-sm-12 col-md-12 col-lg-12">
                                                <div class="form-group">
                                                    <label>Type of Quota<span class="spanAsterisk">*</span></label>
                                                    <asp:DropDownList ID="ddlPWDQuotaType" runat="server" Width="100%" CssClass="form-control"></asp:DropDownList>
                                                    <asp:CompareValidator ID="CompareValidator2" runat="server"
                                                        ControlToValidate="ddlPWDQuotaType"
                                                        ErrorMessage="Required"
                                                        Display="Dynamic"
                                                        Font-Size="9pt"
                                                        ForeColor="Crimson"
                                                        ValueToCompare="-1"
                                                        Operator="NotEqual"
                                                        ValidationGroup="basic1">
                                                    </asp:CompareValidator>
                                                </div>
                                            </div>
                                        </div>--%>
                                        <div class="row" style="margin-bottom: 5px;">
                                            <div class="col-sm-12 col-md-12 col-lg-12">
                                                <div class="form-group">
                                                    <label>Type of Physical Disability<span class="spanAsterisk">*</span></label>
                                                    <asp:TextBox ID="txtPWDDisabilityName" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                    <asp:RequiredFieldValidator runat="server"
                                                        ID="RequiredFieldValidator9"
                                                        ValidationGroup="basic1"
                                                        ControlToValidate="txtPWDDisabilityName"
                                                        Display="Dynamic"
                                                        Font-Size="9pt"
                                                        ForeColor="Crimson"
                                                        Font-Bold="false"
                                                        ErrorMessage="Required" />
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                </div>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <%--=========== END Quota Info ===========--%>

                    <%--=========== Exam Venue Selection ===========--%>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>

                            <div class="row">
                                <div class="col-sm-12 col-md-12 col-lg-12">
                                    <asp:Panel ID="PanelExamSeatInformation" runat="server" Visible="false">
                                        <div class="panel panel-warning">
                                            <div class="panel-heading text-center"><span style="font-size: 19px;"><strong>Exam Venue Selection</strong></span></div>
                                            <div class="panel-body">

                                                <asp:Panel ID="panel_Massage" runat="server" Visible="false">
                                                    <asp:Label ID="districtMassage" runat="server" Font-Bold="true" Font-Size="Large"></asp:Label>
                                                </asp:Panel>

                                                <asp:GridView ID="gvFacultyList" runat="server" CssClass="table table-responsive table-hover"
                                                    AutoGenerateColumns="false" GridLines="none" Width="100%"
                                                    OnRowDataBound="gvFacultyList_RowDataBound">
                                                    <%--OnRowCommand="gvFacultyList_RowCommand"--%>
                                                    <HeaderStyle BackColor="#1387de" ForeColor="White" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="SL" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%">
                                                            <ItemTemplate><%# Container.DataItemIndex + 1 %>.</ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="CandidateFacultyWiseDistrictSeatId" HeaderStyle-Width="40%" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblCandidateFacultyWiseDistrictSeatId"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="AdmissionUnitId" HeaderStyle-Width="40%" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblAdmissionUnitId"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="AdmissionSetupId" HeaderStyle-Width="40%" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblAdmissionSetupId"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Faculty Name" HeaderStyle-Width="40%">
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblFacultyName"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="District" HeaderStyle-Width="30%">
                                                            <ItemTemplate>
                                                                <asp:DropDownList ID="ddlDistrict" runat="server" Width="50%" AutoPostBack="true" OnSelectedIndexChanged="ddlDistrict_SelectedIndexChanged"></asp:DropDownList>
                                                                <asp:CompareValidator ID="ddlDistrictComV" runat="server"
                                                                    ControlToValidate="ddlDistrict" ErrorMessage="Required" ForeColor="Crimson"
                                                                    ValueToCompare="-1" Operator="NotEqual" ValidationGroup="basic1"></asp:CompareValidator>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                </div>
                            </div>

                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <%--=========== Exam Venue Selection ===========--%>


                    <asp:Panel ID="messagePanel_Basic" runat="server">
                        <asp:Label ID="lblMessageBasic" runat="server" Text=""></asp:Label>
                    </asp:Panel>

                    <%--<asp:Button ID="btnSave_Basic" runat="server" Text="Save"
                        CssClass="btn btn-primary" ValidationGroup="basic1"
                        OnClick="btnSave_Basic_Click" />

                    <asp:Button ID="btnNext" runat="server" Text="Next" Visible="false"
                        CssClass="btn btn-primary" />--%>

                    <span id="validationMsg" class="validationErrorMsg"></span>

                </div>
                <%-- end panel body --%>
            </div>
            <%-- end panel default --%>
        </div>
        <%-- end col-md-12 --%>
    </div>
    <%-- end row --%>


</asp:Content>
