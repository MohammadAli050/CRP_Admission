<%@ Page Title="Application Form" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ApplicationForm.aspx.cs" Inherits="Admission.Admission.Candidate.ApplicationForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">

    <style type="text/css">
        .nav-tabs > li.active > a,
        .nav-tabs > li.active > a:hover,
        .nav-tabs > li.active > a:focus {
            color: #555555;
            background-color: #edf9f9;
            border: 1px solid #dddddd;
            border-bottom-color: transparent;
            cursor: default;
        }

        .style_td {
            font-weight: bold;
            text-align: left;
            font-size: 9pt;
        }

        .style_td_secondCol {
            border-left: dotted;
            border-color: gray;
            border-width: 1px;
        }

        .spanAsterisk {
            color: crimson;
            font-size: 10pt;
        }

        .style_thead {
            text-align: center;
            /*background-color: lightgrey;*/
            font-family: Calibri;
            font-size: 12pt;
            font-weight: bold;
        }

        .panelBody_edu_marginBottom {
            margin-bottom: -5%;
        }

        /*-----------------------------------------------------------*/
        .TabHeaderCSS {
            font-family: Verdana, Arial, Courier New;
            font-size: 10px;
            text-align: left;
        }
        /*-----------------------------------------------------------*/
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <%--<div class="row">
        <div class="col-md-12">
            message div here
        </div>
    </div>--%>

    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <strong>Application Form</strong>
                </div>
                <div class="panel-body">
                    <div>

                        <!-- Nav tabs -->
                        <ul class="nav nav-tabs" role="tablist" style="font-size: 10pt">
                            <li role="presentation" class="active"><a href="#basicInfo" aria-controls="home" role="tab" data-toggle="tab"><strong>1. </strong>Basic Info</a></li>
                            <li role="presentation"><a href="#educationInfo" aria-controls="profile" role="tab" data-toggle="tab"><strong>2. </strong>Education</a></li>
                            <%--<li role="presentation"><a href="#" aria-controls="profile" role="tab" data-toggle="tab"><strong>2. </strong>Program Priority</a></li>--%>
                            <li role="presentation"><a href="#parentInfo" aria-controls="messages" role="tab" data-toggle="tab"><strong>3. </strong>Parent/Guardian</a></li>
                            <li role="presentation"><a href="#addressInfo" aria-controls="settings" role="tab" data-toggle="tab"><strong>4. </strong>Address</a></li>
                            <li role="presentation"><a href="#finGuarantorInfo" aria-controls="settings" role="tab" data-toggle="tab"><strong>5. </strong>Financial Guarantor</a></li>
                            <li role="presentation"><a href="#additionalInfo" aria-controls="settings" role="tab" data-toggle="tab"><strong>6. </strong>Additional</a></li>
                            <li role="presentation"><a href="#photoSigUpload" aria-controls="settings" role="tab" data-toggle="tab"><strong>5. </strong>Photo/Signature</a></li>
                        </ul>

                        <!-- Tab panes -->
                        <div class="tab-content">
                            <div role="tabpanel" class="tab-pane active" id="basicInfo">
                                <div class="panel panel-default">
                                    <div class="panel-body">

                                        <asp:UpdatePanel ID="updatePanelBasicInfo" runat="server" ChildrenAsTriggers="true">
                                            <ContentTemplate>
                                                <asp:Panel ID="messagePanel_Basic" runat="server">
                                                    <asp:Label ID="lblMessageBasic" runat="server" Text=""></asp:Label>
                                                </asp:Panel>
                                                <table style="width: 100%" class="table table-condensed">
                                                    <tr>
                                                        <td style="width: 15%" class="style_td">First Name <span class="spanAsterisk">*</span></td>
                                                        <td style="width: 35%">
                                                            <asp:TextBox ID="txtFirstName" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                                                ControlToValidate="txtFirstName" ErrorMessage="First name required"
                                                                ForeColor="Crimson" Display="Dynamic" Font-Size="10pt"
                                                                ValidationGroup="basic1"></asp:RequiredFieldValidator>
                                                        </td>
                                                        <td style="width: 15%" class="style_td style_td_secondCol">Middle Name</td>
                                                        <td style="width: 35%">
                                                            <asp:TextBox ID="txtMiddleName" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style_td">Last Name</td>
                                                        <td>
                                                            <asp:TextBox ID="txtLastName" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                        </td>
                                                        <td class="style_td style_td_secondCol">Nick Name</td>
                                                        <td>
                                                            <asp:TextBox ID="txtNickName" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style_td">Date Of Birth <span class="spanAsterisk">*</span></td>
                                                        <td>
                                                            <asp:TextBox ID="txtDateOfBirth" runat="server" Width="50%" CssClass="form-control"></asp:TextBox>
                                                            <ajaxToolkit:CalendarExtender ID="CalenderExtender_DOB" runat="server"
                                                                TargetControlID="txtDateOfBirth" Format="dd/MM/yyyy" />
                                                            <asp:RequiredFieldValidator ID="txtDateOfBirthReqV" runat="server"
                                                                ControlToValidate="txtDateOfBirth" ErrorMessage="Date of Birth required"
                                                                ForeColor="Crimson" Display="Dynamic" Font-Size="10pt"
                                                                ValidationGroup="basic1"></asp:RequiredFieldValidator>
                                                        </td>
                                                        <td class="style_td style_td_secondCol">Place of Birth</td>
                                                        <td>
                                                            <asp:TextBox ID="txtPlaceOfBirth" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style_td">Citizenship/<br />
                                                            Nationality</td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlNationality" runat="server" Width="85%" CssClass="form-control"></asp:DropDownList>
                                                        </td>
                                                        <td class="style_td style_td_secondCol">Mother Tongue</td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlLanguage" runat="server" Width="85%" CssClass="form-control"></asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style_td">Gender <span class="spanAsterisk">*</span></td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlGender" runat="server" Width="85%" CssClass="form-control"></asp:DropDownList>
                                                        </td>
                                                        <td class="style_td style_td_secondCol">Marital Status</td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlMaritalStatus" runat="server" Width="85%" CssClass="form-control"></asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style_td">National ID <span class="spanAsterisk">*</span></td>
                                                        <td>
                                                            <asp:TextBox ID="txtNationalId" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="txtNationalIdReqV" runat="server"
                                                                ControlToValidate="txtNationalId" ErrorMessage="National ID required"
                                                                ForeColor="Crimson" Display="Dynamic" Font-Size="10pt"
                                                                ValidationGroup="basic1"></asp:RequiredFieldValidator>
                                                        </td>
                                                        <td class="style_td style_td_secondCol">Blood Group <span class="spanAsterisk">*</span></td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlBloodGroup" runat="server" Width="85%" CssClass="form-control"></asp:DropDownList>
                                                            <asp:CompareValidator ID="ddlBloodGroupComV" runat="server"
                                                                ControlToValidate="ddlBloodGroup" ErrorMessage="Blood group required"
                                                                ForeColor="Crimson" Display="Dynamic" Font-Size="10pt"
                                                                ValueToCompare="-1" Operator="NotEqual" ValidationGroup="basic1"></asp:CompareValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style_td">Email <span class="spanAsterisk">*</span></td>
                                                        <td>
                                                            <asp:TextBox ID="txtEmail" runat="server" Width="100%" TextMode="Email" CssClass="form-control"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="txtEmailReqV" runat="server"
                                                                ControlToValidate="txtEmail" ErrorMessage="Email required"
                                                                ForeColor="Crimson" Display="Dynamic" Font-Size="10pt"
                                                                ValidationGroup="basic1"></asp:RequiredFieldValidator>
                                                        </td>
                                                        <td class="style_td style_td_secondCol">Phone<br />
                                                            (Res)</td>
                                                        <td>
                                                            <asp:TextBox ID="txtPhoneRes" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style_td">Emergency Phone</td>
                                                        <td>
                                                            <asp:TextBox ID="txtPhoneEmergency" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                        </td>
                                                        <td class="style_td style_td_secondCol">Mobile <span class="spanAsterisk">*</span></td>
                                                        <td>
                                                            <asp:TextBox ID="txtMobile" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                            <span style="color: cornflowerblue">Format: +8801XXXXXXXXX</span>
                                                            <asp:RequiredFieldValidator ID="txtMobileReqV" runat="server"
                                                                ControlToValidate="txtMobile" ErrorMessage="Email required"
                                                                ForeColor="Crimson" Display="Dynamic" Font-Size="10pt"
                                                                ValidationGroup="basic1"></asp:RequiredFieldValidator>
                                                            <asp:RegularExpressionValidator ID="txtMobileRegEx" runat="server"
                                                                ControlToValidate="txtMobile"></asp:RegularExpressionValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style_td">Religion</td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlReligion" runat="server" Width="85%" CssClass="form-control"></asp:DropDownList>
                                                        </td>
                                                        <td></td>
                                                        <td></td>
                                                        <%--<td class="style_td style_td_secondCol">Quota</td>
                                                                    <td>
                                                                        <asp:DropDownList ID="ddlQuota" runat="server" Width="85%" CssClass="form-control"></asp:DropDownList>
                                                                    </td>--%>
                                                    </tr>
                                                </table>

                                            </ContentTemplate>
                                        </asp:UpdatePanel>

                                        <asp:Button ID="btnSave_Basic" runat="server" Text="Save Basic Info"
                                            CssClass="btn btn-primary" OnClick="btnSave_Basic_Click"
                                            ValidationGroup="basic1" />

                                    </div>
                                </div>
                            </div>
                            <%-- *****END DIV BASIC INFO TAB-CONTENT************************************************************************************ --%>
                            <div role="tabpanel" class="tab-pane" id="educationInfo">
                                <div class="panel panel-default">
                                    <div class="panel-body">

                                        <table style="width: 100%">
                                            <tr>
                                                <td>
                                                    <span style="color: darkred">Grade Points Calculation process for <strong>English Medium Students</strong> - Calculate the Average Grade Points using the table considering all subjects.<br />
                                                        <span class="glyphicon glyphicon-ok">&nbsp;</span>Average points required - 15 in 5 subject (O level)<br />
                                                        <span class="glyphicon glyphicon-ok">&nbsp;</span>Average points required - 6 in 2 subject (A level)</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <table style="width: 10%; color: darkred" class="table table-bordered">
                                                        <tr>
                                                            <th>Grade</th>
                                                            <td>A</td>
                                                            <td>B</td>
                                                            <td>C</td>
                                                            <td>D</td>
                                                            <td>E</td>
                                                        </tr>
                                                        <tr>
                                                            <th>GPA</th>
                                                            <td>5</td>
                                                            <td>4</td>
                                                            <td>3</td>
                                                            <td>2</td>
                                                            <td>1</td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>

                                        <asp:Panel ID="messagePanel_Education" runat="server">
                                            <asp:Label ID="lblMessageEducation" runat="server" Text=""></asp:Label>
                                        </asp:Panel>

                                        <asp:UpdatePanel ID="updatePanelEducation" runat="server">
                                            <ContentTemplate>
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <div class="col-md-6">
                                                            <div class="panel panel-default">
                                                                <div class="panel-heading style_thead">
                                                                    Secondary School / O-Level
                                                                </div>
                                                                <div class="panel-body panelBody_edu_marginBottom">
                                                                    <table style="width: 100%" class="table table-condensed">
                                                                        <tr>
                                                                            <td style="width: 30%" class="style_td">Exam Type</td>
                                                                            <td style="width: 70%">
                                                                                <asp:DropDownList ID="ddlSec_ExamType" runat="server" Width="85%" CssClass="form-control"></asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="style_td">Education Board</td>
                                                                            <td>
                                                                                <asp:DropDownList ID="ddlSec_EducationBrd" runat="server" Width="85%" CssClass="form-control"></asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="style_td">Institute</td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtSec_Institute" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="style_td">Roll Number</td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtSec_RollNo" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <%--<tr>
                                                                                <td class="style_td">Registration Number</td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtSec_RegNo" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                                                </td>
                                                                            </tr>--%>
                                                                        <tr>
                                                                            <td class="style_td">Group Or Subject</td>
                                                                            <td>
                                                                                <asp:DropDownList ID="ddlSec_GrpOrSub" runat="server" Width="85%" CssClass="form-control"></asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="style_td">Division/Class</td>
                                                                            <td>
                                                                                <asp:DropDownList ID="ddlSec_DivClass" runat="server" Width="85%" CssClass="form-control"
                                                                                    AutoPostBack="true">
                                                                                </asp:DropDownList>
                                                                                <%--<asp:CompareValidator ID="ddlSec_DivClass_ComV" runat="server" Display="Dynamic"
                                                                                ControlToValidate="ddlSec_DivClass" ErrorMessage="Required" Font-Size="10pt" ForeColor="Crimson"
                                                                                ValueToCompare="-1" Operator="NotEqual" ValidationGroup="education1"></asp:CompareValidator>--%>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="style_td">CGPA/Score</td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtSec_CgpaScore" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="style_td">Marks<br />
                                                                                <span style="color: cornflowerblue; font-size: 9pt; font-weight: normal">For older marking system</span></td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtSec_Marks" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="style_td">Exam Year</td>
                                                                            <td>
                                                                                <asp:DropDownList ID="ddlSec_PassingYear" runat="server" Width="85%" CssClass="form-control"></asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <div class="panel panel-default">
                                                                <div class="panel-heading style_thead">
                                                                    Higher Secondary School / A-Level
                                                                </div>
                                                                <div class="panel-body panelBody_edu_marginBottom">
                                                                    <table style="width: 100%" class="table table-condensed">
                                                                        <tr>
                                                                            <td style="width: 30%" class="style_td">Exam Type</td>
                                                                            <td style="width: 70%">
                                                                                <asp:DropDownList ID="ddlHigherSec_ExamType" runat="server" Width="85%" CssClass="form-control"></asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="style_td">Education Board</td>
                                                                            <td>
                                                                                <asp:DropDownList ID="ddlHigherSec_EducationBrd" runat="server" Width="85%" CssClass="form-control"></asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="style_td">Institute</td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtHigherSec_Institute" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="style_td">Roll Number</td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtHigherSec_RollNo" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <%--<tr>
                                                                                <td class="style_td">Registration Number</td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtHigherSec_RegNo" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                                                </td>
                                                                            </tr>--%>
                                                                        <tr>
                                                                            <td class="style_td">Group Or Subject</td>
                                                                            <td>
                                                                                <asp:DropDownList ID="ddlHigherSec_GrpOrSub" runat="server" Width="85%" CssClass="form-control"></asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="style_td">Division/Class</td>
                                                                            <td>
                                                                                <asp:DropDownList ID="ddlHigherSec_DivClass" runat="server" Width="85%" CssClass="form-control"></asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="style_td">CGPA/Score</td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtHigherSec_CgpaScore" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="style_td">Marks<br />
                                                                                <span style="color: cornflowerblue; font-size: 9pt; font-weight: normal">For older marking system</span></td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtHigherSec_Marks" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="style_td">Exam Year</td>
                                                                            <td>
                                                                                <asp:DropDownList ID="ddlHigherSec_PassingYear" runat="server" Width="85%" CssClass="form-control"></asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <%-- END COL-MD-12 --%>
                                                </div>
                                                <%-- END ROW --%>
                                                <%-- END ROW   SECONDARY & HIGHER SECONDARY --%>
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <div class="col-md-6">
                                                            <div class="panel panel-default">
                                                                <div class="panel-heading style_thead">
                                                                    Undergraduate
                                                                </div>
                                                                <div class="panel-body panelBody_edu_marginBottom">
                                                                    <table style="width: 100%" class="table table-condensed">
                                                                        <tr>
                                                                            <td style="width: 30%" class="style_td">Institute</td>
                                                                            <td style="width: 70%">
                                                                                <asp:TextBox ID="txtUndergrad_Institute" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="style_td">Program/Degree</td>
                                                                            <td>
                                                                                <asp:DropDownList ID="ddlUndergrad_ProgramDegree" runat="server" Width="85%" CssClass="form-control"></asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="style_td">Others</td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtUndergrad_ProgOthers" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <%--<tr>
                                                                                <td class="style_td">Division/Class</td>
                                                                                <td>
                                                                                    <asp:DropDownList ID="ddlUndergrad_DivClass" runat="server" Width="85%" CssClass="form-control"></asp:DropDownList>
                                                                                </td>
                                                                            </tr>--%>
                                                                        <tr>
                                                                            <td class="style_td">CGPA/Score</td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtUndergrad_CgpaScore" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="style_td">Exam Year</td>
                                                                            <td>
                                                                                <asp:DropDownList ID="ddlUndergrad_PassingYear" runat="server" Width="85%" CssClass="form-control"></asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <div class="panel panel-default">
                                                                <div class="panel-heading style_thead">
                                                                    Graduate
                                                                </div>
                                                                <div class="panel-body panelBody_edu_marginBottom">
                                                                    <table style="width: 100%" class="table table-condensed">
                                                                        <tr>
                                                                            <td style="width: 30%" class="style_td">Institute</td>
                                                                            <td style="width: 70%">
                                                                                <asp:TextBox ID="txtGraduate_Institute" runat="server" CssClass="form-control"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="style_td">Program/Degree</td>
                                                                            <td>
                                                                                <asp:DropDownList ID="ddlGraduate_ProgramDegree" runat="server" Width="85%" CssClass="form-control"></asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="style_td">Others</td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtGraduate_ProgOthers" runat="server" CssClass="form-control"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <%--<tr>
                                                                                <td class="style_td">Division/Class</td>
                                                                                <td>
                                                                                    <asp:DropDownList ID="ddlGraduate_DivClass" runat="server" Width="85%" CssClass="form-control"></asp:DropDownList>
                                                                                </td>
                                                                            </tr>--%>
                                                                        <tr>
                                                                            <td class="style_td">CGPA/Score</td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtGraduate_CgpaScore" runat="server" CssClass="form-control"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="style_td">Exam Year</td>
                                                                            <td>
                                                                                <asp:DropDownList ID="ddlGraduate_PassingYear" runat="server" CssClass="form-control"></asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <%-- END COL-MD-12 --%>
                                                </div>
                                                <%-- END ROW  UNDERGRAD & GRAD --%>

                                                <asp:Button ID="btnSave_Education" runat="server" Text="Save Education Info"
                                                    CssClass="btn btn-primary" OnClick="btnSave_Education_Click"
                                                    ValidationGroup="education1" />

                                            </ContentTemplate>
                                        </asp:UpdatePanel>

                                    </div>
                                </div>
                            </div>
                            <%-- *****END DIV EDUCATION INFO TAB-CONTENT******************************************************************************** --%>
                            <div role="tabpanel" class="tab-pane" id="parentInfo">
                                <div class="panel panel-default">
                                    <div class="panel-body">

                                        <asp:Panel ID="messagePanel_Parent" runat="server">
                                            <asp:Label ID="lblMessageParent" runat="server" Text=""></asp:Label>
                                        </asp:Panel>

                                        <asp:UpdatePanel ID="updatePanelParent" runat="server">
                                            <ContentTemplate>

                                                <div class="row">
                                                    <div class="col-md-12">

                                                        <div class="col-md-6">
                                                            <div class="panel panel-default">
                                                                <div class="panel-heading style_thead">
                                                                    Father
                                                                </div>
                                                                <div class="panel-body panelBody_edu_marginBottom">
                                                                    <table style="width: 100%" class="table table-condensed">
                                                                        <tr>
                                                                            <td style="width: 30%" class="style_td">Father's Name <span class="spanAsterisk">*</span></td>
                                                                            <td style="width: 70%">
                                                                                <asp:TextBox ID="txtFatherName" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="style_td">Occupation</td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtFatherOccupation" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="style_td">Business Address</td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtFatherOrgAddress" runat="server" Width="100%" CssClass="form-control"
                                                                                    TextMode="MultiLine"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="style_td">Mobile</td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtFatherMobile" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <div class="panel panel-default">
                                                                <div class="panel-heading style_thead">
                                                                    Mother
                                                                </div>
                                                                <div class="panel-body panelBody_edu_marginBottom">
                                                                    <table style="width: 100%" class="table table-condensed">
                                                                        <tr>
                                                                            <td style="width: 30%" class="style_td">Mother's Name <span class="spanAsterisk">*</span></td>
                                                                            <td style="width: 70%">
                                                                                <asp:TextBox ID="txtMotherName" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="style_td">Occupation</td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtMotherOccupation" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="style_td">Mailing Address<br />
                                                                                (Postal)</td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtMotherMailingAddress" runat="server" Width="100%" CssClass="form-control"
                                                                                    TextMode="MultiLine"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="style_td">Mobile</td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtMotherMobile" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <%-- END COL-MD-12 --%>
                                                </div>
                                                <%-- END ROW   FATHER & MOTHER --%>
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <div class="col-md-6">
                                                            <div class="panel panel-default">
                                                                <div class="panel-heading style_thead">
                                                                    Guardian
                                                                </div>
                                                                <div class="panel-body panelBody_edu_marginBottom">
                                                                    <table style="width: 100%" class="table table-condensed">
                                                                        <tr>
                                                                            <td style="width: 30%" class="style_td">Guardian's Name</td>
                                                                            <td style="width: 70%">
                                                                                <asp:TextBox ID="txtGuardian_Name" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="style_td">Relationship with the applicant</td>
                                                                            <td>
                                                                                <asp:DropDownList ID="ddlGuardianRelation" runat="server" Width="85%" CssClass="form-control"></asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="style_td">Other</td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtGuardianOtherRelation" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="style_td">Mailing Address<br />
                                                                                (Postal)</td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtGuardianMailingAddress" runat="server" Width="100%" CssClass="form-control"
                                                                                    TextMode="MultiLine"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="style_td">Email</td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtGuardianEmail" runat="server" Width="100%" CssClass="form-control"
                                                                                    TextMode="Email"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="style_td">Mobile <span class="spanAsterisk">*</span></td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtGuardianMobile" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="style_td">Phone(Res)</td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtGuardianPhoneRes" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="style_td">Phone(Office)</td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtGuardianPhoneOffice" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-6">
                                                        </div>
                                                    </div>
                                                    <%-- END COL-MD-12 --%>
                                                </div>
                                                <%-- END ROW  Guardian --%>

                                                <asp:Button ID="btnSave_Parent" runat="server" Text="Save Parent/Guardian Info"
                                                    CssClass="btn btn-primary" OnClick="btnSave_Parent_Click"
                                                    ValidationGroup="parent1" />

                                            </ContentTemplate>
                                        </asp:UpdatePanel>

                                    </div>
                                </div>
                            </div>
                            <%-- *****END DIV PARENT/GUARDIAN INFO TAB-CONTENT************************************************************************** --%>
                            <div role="tabpanel" class="tab-pane" id="addressInfo">
                                <div class="panel panel-default">
                                    <div class="panel-body">

                                        <asp:Panel ID="messagePanel_Address" runat="server">
                                            <asp:Label ID="lblMessageAddress" runat="server" Text=""></asp:Label>
                                        </asp:Panel>

                                        <asp:UpdatePanel ID="updatePanelAddress" runat="server">
                                            <ContentTemplate>

                                                <div class="row">
                                                    <div class="col-md-12">

                                                        <div class="col-md-6">
                                                            <div class="panel panel-default">
                                                                <div class="panel-heading style_thead">
                                                                    Present/Mailing Address
                                                                </div>
                                                                <div class="panel-body panelBody_edu_marginBottom">
                                                                    <table style="width: 100%" class="table table-condensed">
                                                                        <tr>
                                                                            <td style="width: 31%" class="style_td">Mailing Address <span class="spanAsterisk">*</span><br />
                                                                                (Postal)</td>
                                                                            <td style="width: 69%">
                                                                                <asp:TextBox ID="txtPresentAddress" runat="server" Width="100%" CssClass="form-control"
                                                                                    TextMode="MultiLine"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="style_td">Divsion</td>
                                                                            <td>
                                                                                <asp:DropDownList ID="ddlPresentDivision" runat="server" Width="85%" CssClass="form-control"></asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="style_td">District</td>
                                                                            <td>
                                                                                <asp:DropDownList ID="ddlPresentDistrict" runat="server" Width="85%" CssClass="form-control"></asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="style_td">Upazila</td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtPresentUpazila" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <%-- PRESENT ADDRESS --%>
                                                        <div class="col-md-6">
                                                            <div class="panel panel-default">
                                                                <div class="panel-heading style_thead">
                                                                    Permanent Address
                                                                </div>
                                                                <div class="panel-body panelBody_edu_marginBottom">
                                                                    <table style="width: 100%" class="table table-condensed">
                                                                        <tr>
                                                                            <td style="width: 31%" class="style_td">Mailing Address <span class="spanAsterisk">*</span><br />
                                                                                (Postal)</td>
                                                                            <td style="width: 69%">
                                                                                <asp:TextBox ID="txtPermanentAddress" runat="server" Width="100%" CssClass="form-control"
                                                                                    TextMode="MultiLine"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="style_td">Divsion</td>
                                                                            <td>
                                                                                <asp:DropDownList ID="ddlPermanentDivision" runat="server" Width="85%" CssClass="form-control"></asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="style_td">District</td>
                                                                            <td>
                                                                                <asp:DropDownList ID="ddlPermanentDistrict" runat="server" Width="85%" CssClass="form-control"></asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="style_td">Upazila</td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtPermanentUpazila" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <%-- PERMANENT ADDRESS --%>
                                                    </div>
                                                    <%-- END COL-MD-12 --%>
                                                </div>
                                                <%-- END ROW --%>

                                                <asp:Button ID="btnSave_Address" runat="server" Text="Save Address Info"
                                                    CssClass="btn btn-primary" OnClick="btnSave_Address_Click"
                                                    ValidationGroup="address1" />

                                            </ContentTemplate>
                                        </asp:UpdatePanel>

                                    </div>
                                </div>
                            </div>
                            <%-- *****END DIV EXTRACURRICULAR/WORKEXPERIENCE INFO TAB-CONTENT*********************************************************** --%>
                            <div role="tabpanel" class="tab-pane" id="finGuarantorInfo">
                                <div class="panel panel-default">
                                    <div class="panel-body">

                                        <asp:Panel ID="messagePanel_FinGuar" runat="server">
                                            <asp:Label ID="lblMessageFinGuar" runat="server" Text=""></asp:Label>
                                        </asp:Panel>

                                        <asp:UpdatePanel ID="updatePanelFinGuarantor" runat="server">
                                            <ContentTemplate>
                                                <table style="width: 100%" class="table table-condensed">
                                                    <tr>
                                                        <td colspan="2"><strong>Name and Business Address of the Financial Guarantor (All correspondence including academic records will be released to this person unless admised otherwise in writing)</strong></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 15%" class="style_td">Name (Full Name)</td>
                                                        <td style="width: 85%">
                                                            <asp:TextBox ID="txtFinGuarantorName" runat="server" Width="50%" CssClass="form-control"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style_td">Relationship with the applicant</td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlRelationWithGuarantor" runat="server" Width="35%" CssClass="form-control"></asp:DropDownList>
                                                            &nbsp; Others
                                                                        <asp:TextBox ID="txtRelationWithGuarantorOthers" runat="server" Width="50%" CssClass="form-control"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style_td">Occupation</td>
                                                        <td>
                                                            <asp:TextBox ID="txtFinGuarantorOccupation" runat="server" Width="50%" CssClass="form-control"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style_td">Organization</td>
                                                        <td>
                                                            <asp:TextBox ID="txtFinGuarantorOrganization" runat="server" Width="50%" CssClass="form-control"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style_td">Position</td>
                                                        <td>
                                                            <asp:TextBox ID="txtFinGuarantorPosition" runat="server" Width="50%" CssClass="form-control"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style_td">Mailing Address
                                                                        <br />
                                                            <strong>(Postal)</strong></td>
                                                        <td>
                                                            <asp:TextBox ID="txtFinGuarantorAddress" runat="server" Width="50%" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style_td">Email (if any)</td>
                                                        <td>
                                                            <asp:TextBox ID="txtFinGuarantorEmail" runat="server" Width="50%" CssClass="form-control" TextMode="Email"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style_td">Phone (Res)</td>
                                                        <td>
                                                            <asp:TextBox ID="txtFinGuarantorPhoneRes" runat="server" Width="50%" CssClass="form-control"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style_td">Phone (Office)</td>
                                                        <td>
                                                            <asp:TextBox ID="txtFinGuarantorPhoneOffice" runat="server" Width="50%" CssClass="form-control"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2"><strong>declare that the information provided in this application is true. I also declare that I will abide by the rules of the university.</strong></td>
                                                    </tr>
                                                </table>

                                                <asp:Button ID="btnSave_Guarantor" runat="server" Text="Save Financial Guarantor Info"
                                                    CssClass="btn btn-primary" OnClick="btnSave_Guarantor_Click"
                                                    ValidationGroup="finguar1" />

                                            </ContentTemplate>
                                        </asp:UpdatePanel>

                                    </div>
                                </div>
                            </div>
                            <%-- *****END DIV FINANCIAL GUARANTOR INFO TAB-CONTENT********************************************************************** --%>
                            <div role="tabpanel" class="tab-pane" id="additionalInfo">
                                <div class="panel panel-default">
                                    <div class="panel-body">

                                        <asp:Panel ID="messagePanel_Additional" runat="server">
                                            <asp:Label ID="lblMessageAdditional" runat="server" Text=""></asp:Label>
                                        </asp:Panel>

                                        <asp:UpdatePanel ID="updatePanelAdditional" runat="server">
                                            <ContentTemplate>
                                                <table style="width: 100%" class="table table-condensed">
                                                    <tr>
                                                        <td style="width: 30%" class="style_td">Have you ever been admitted to 
                                                                        <asp:Label ID="lblUniShortName" runat="server"></asp:Label>? </td>
                                                        <td style="width: 70%">
                                                            <asp:DropDownList ID="ddlAdmittedBefore" runat="server" Width="45%" CssClass="form-control"></asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style_td">If yes, state Student ID No.</td>
                                                        <td>
                                                            <asp:TextBox ID="txtCurrentStudentId" runat="server" Width="50%" CssClass="form-control"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style_td">Have you ever been dismissed, suspended or expelled from any school or college?</td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlDismissedBefore" runat="server" Width="45%" CssClass="form-control"></asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style_td">If yes, describe briefly</td>
                                                        <td>
                                                            <asp:TextBox ID="txtDismissalStatement" runat="server" Width="50%" CssClass="form-control"
                                                                TextMode="MultiLine"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>

                                                <asp:Button ID="btnSave_Additional" runat="server" Text="Save Additional Info"
                                                    CssClass="btn btn-primary" OnClick="btnSave_Additional_Click" />

                                            </ContentTemplate>
                                        </asp:UpdatePanel>

                                    </div>
                                </div>
                            </div>
                            <%-- *****END DIV ADDITIONAL INFO TAB-CONTENT******************************************************************************* --%>
                            <div role="tabpanel" class="tab-pane" id="photoSigUpload">
                                <div class="panel panel-default">
                                    <div class="panel-body">

                                        <asp:Panel ID="messagePanel_Photo" runat="server">
                                            <asp:Label ID="lblMessagePhoto" runat="server"></asp:Label>
                                        </asp:Panel>

                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="col-md-4">
                                                    <div class="panel panel-default">
                                                        <div class="panel-heading style_thead">
                                                            Photo
                                                        </div>
                                                        <div class="panel-body">
                                                            <asp:UpdatePanel ID="updPanelPhotoUpload" runat="server">
                                                                <ContentTemplate>
                                                                    <div style="text-align: center">
                                                                        <asp:Image ID="ImagePhoto" runat="server"
                                                                            ImageUrl="~/Images/AppImg/user7.jpg"
                                                                            Width="154" Height="154" />
                                                                    </div>
                                                                    <div style="color: red;"><sup>File size: 150kb</sup></div>
                                                                    <asp:FileUpload ID="FileUploadPhoto" runat="server" />
                                                                    <asp:Button ID="btnUploadPhoto" runat="server"
                                                                        Text="Upload Photo"
                                                                        CssClass="btn btn-success" />
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="btnUploadPhoto" />
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                    </div>
                                                </div>
                                                <%-- PHOTO --%>
                                                <div class="col-md-4">
                                                    <div class="panel panel-default">
                                                        <div class="panel-heading style_thead">
                                                            Signature
                                                        </div>
                                                        <div class="panel-body">
                                                            <asp:UpdatePanel ID="updPnlSignatureUpload" runat="server">
                                                                <ContentTemplate>
                                                                    <label>Upload signature with full name.</label>
                                                                    <div style="text-align: center">
                                                                        <asp:Image ID="ImageSignature" runat="server"
                                                                            ImageUrl="~/Images/AppImg/sign2.png"
                                                                            Width="256" Height="128" />
                                                                    </div>
                                                                    <div style="color: red;"><sup>File size: 150kb</sup></div>
                                                                    <asp:FileUpload ID="FileUploadSignature" runat="server" />
                                                                    <asp:Button ID="btnUploadSignature" runat="server"
                                                                        Text="Upload signature"
                                                                        CssClass="btn btn-success" />
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="btnUploadSignature" />
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                    </div>
                                                </div>
                                                <%-- SIGNATURE --%>

                                                <div class="col-md-4">
                                                    <div class="panel panel-default">
                                                        <div class="panel-heading style_thead">
                                                            Financial Guarantor's Signature
                                                        </div>
                                                        <div class="panel-body">
                                                            <asp:UpdatePanel ID="updPnlFinGuarSignatureUpload" runat="server">
                                                                <ContentTemplate>
                                                                    <label>Upload signature with full name.</label>
                                                                    <div style="text-align: center">
                                                                        <asp:Image ID="ImageFinGuarSignature" runat="server"
                                                                            ImageUrl="~/Images/AppImg/sign2.png"
                                                                            Width="256" Height="128" />
                                                                    </div>
                                                                    <div style="color: red;"><sup>File size: 150kb</sup></div>
                                                                    <asp:FileUpload ID="FileUploadFinGuarSignature" runat="server" />
                                                                    <asp:Button ID="btnUploadFinGuarSignature" runat="server"
                                                                        Text="Upload signature"
                                                                        CssClass="btn btn-success" />
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="btnUploadFinGuarSignature" />
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                    </div>
                                                </div>
                                                <%-- SIGNATURE --%>
                                            </div>
                                            <%-- END COL-MD-12 --%>

                                            <asp:Button ID="btnSave_Upload" runat="server" Text="Save Photo/Signature"
                                                CssClass="btn btn-primary" />

                                        </div>
                                        <%-- END ROW --%>
                                    </div>
                                </div>
                            </div>
                            <%-- *****END DIV PHOTO/SIGNATURE INFO TAB-CONTENT************************************************************************** --%>
                        </div>

                    </div>
                </div>
                <%-- END PANEL-BODY --%>
            </div>
            <%-- END PANEL-DEFAULT --%>
        </div>
        <%-- END COL-MD- --%>
    </div>
    <%-- END ROW --%>
</asp:Content>
