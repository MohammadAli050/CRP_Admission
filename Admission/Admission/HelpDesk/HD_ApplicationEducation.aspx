<%@ Page Title="" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="HD_ApplicationEducation.aspx.cs" Inherits="Admission.Admission.HelpDesk.HD_ApplicationEducation" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <link href="../../Content/ApplicationForm.css" rel="stylesheet" />

    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/sweetalert/1.1.0/sweetalert.min.js"></script>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/sweetalert/1.1.0/sweetalert.min.css" rel="stylesheet" type="text/css" />

    <style>
         .modalBackground {
            background-color: Gray;
            filter: alpha(opacity=80);
            opacity: 0.8;
            z-index: 10000;
        }

        .blink {
            animation: blinker 0.6s linear infinite;
            color: #1c87c9;
            font-size: 12px;
            font-weight: bold;
            font-family: sans-serif;
        }

        @keyframes blinker {
            50% {
                opacity: 0;
            }
        }

    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
        <div id="divProgress" style="display: none; z-index: 1000000; position: fixed; top: 50%; left: 50%; transform: translate(-50%, -50%);">
        <asp:Image ID="LoadingImage" runat="server" ImageUrl="~/Images/AppImg/t1.gif" Height="250px" Width="250px" />
    </div>

    <br />

    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-heading panelHeaderHeight">
                    <ol class="breadcrumb">
                        <li><asp:HyperLink ID="hrefAppBasic" runat="server">Basic Info</asp:HyperLink></li>
                        <li><asp:HyperLink ID="hrefAppPriority" runat="server">Program Priority</asp:HyperLink></li>
                        <li class="active"><asp:HyperLink ID="hrefAppEducation" runat="server">Education</asp:HyperLink></li>
                        <li><asp:HyperLink ID="hrefAppRelation" runat="server">Parent/Guardian</asp:HyperLink></li>
                        <li><asp:HyperLink ID="hrefAppAddress" runat="server">Address</asp:HyperLink></li>
                        <li><asp:HyperLink ID="hrefAppAdditional" runat="server">Additional/Work Experience</asp:HyperLink></li>
                        <%--<li><asp:HyperLink ID="hrefAppFinGuar" runat="server">Financial Guarantor</asp:HyperLink></li>--%>
                        <li><asp:HyperLink ID="hrefAppAttachment" runat="server">Upload Photo</asp:HyperLink></li>
                    </ol>
                </div>
                <%-- end panel heading --%>
                <div class="panel-body">

                    <%--<table style="width: 100%">
                        <tr>
                            <td>
                                <span style="color: darkred">Grade Points Calculation process for <strong>English Medium Students</strong> - Calculate the Average Grade Points using the table considering all subjects.<br />
                                    <span class="glyphicon glyphicon-ok">&nbsp;</span>Average points required - 15 in 5 subject (O level)<br />
                                    <span class="glyphicon glyphicon-ok">&nbsp;</span>Average points required - 6 in 2 subject (A level)</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table style="width: 30%; color: darkred" class="table table-bordered">
                                    <tr>
                                        <th>Grade</th>
                                        <td>A*/A</td>
                                        <td>B</td>
                                        <td>C</td>
                                        <td>D</td>
                                    </tr>
                                    <tr>
                                        <th>GPA</th>
                                        <td>5.00</td>
                                        <td>4.00</td>
                                        <td>3.50</td>
                                        <td>3.00</td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td style="color: darkred">
                                Students of International Baccalaureate (IB) must pass in minimum 06 (six) subjects 
                                having minimum 30 points based on rating scale used in their curriculum (7, 6, 5, 4, 3). 
                                Rating of 1 and 2 will not be considered in point calculation.
                            </td>
                        </tr>
                    </table>--%>
                    <asp:HiddenField ID="hfEduCat" runat="server" />

                    <%--<asp:Panel ID="messagePanel_Education" runat="server">
                        <asp:Label ID="lblMessageEducation" runat="server" Text=""></asp:Label>
                    </asp:Panel>--%>

                    <asp:UpdatePanel ID="updatePanelEducation" runat="server" ChildrenAsTriggers="true">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-md-12">

                                    <div class="col-md-6">
                                        <div class="panel panel-default">
                                            <div class="panel-heading style_thead">
                                                Secondary School / O-Level
                                            </div>
                                            <div class="panel-body panelBody_edu_marginBottom">
                                                <table style="width: 100%" class="table table-condensed table-striped">
                                                    <tr>
                                                        <td style="width: 30%" class="style_td">Exam Type <span class="spanAsterisk">*</span></td>
                                                        <td style="width: 70%">
                                                            <asp:DropDownList ID="ddlSec_ExamType" runat="server" Width="85%" CssClass="form-control"></asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style_td">Education Board <span class="spanAsterisk">*</span></td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlSec_EducationBrd" runat="server" Width="85%" CssClass="form-control"></asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style_td">Institute <span class="spanAsterisk">*</span></td>
                                                        <td>
                                                            <asp:TextBox ID="txtSec_Institute" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style_td">Roll Number <span class="spanAsterisk">*</span></td>
                                                        <td>
                                                            <asp:TextBox ID="txtSec_RollNo" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr style="display: none">
                                                        <td class="style_td">Registration Number</td>
                                                        <td>
                                                            <asp:TextBox ID="txtSec_RegNo" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style_td">Group Or Subject <span class="spanAsterisk">*</span></td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlSec_GrpOrSub" runat="server" Width="85%" CssClass="form-control"></asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style_td">Division/Class <span class="spanAsterisk">*</span></td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlSec_DivClass" runat="server" Width="85%" CssClass="form-control"
                                                                AutoPostBack="true" OnSelectedIndexChanged="ddlSec_DivClass_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                            <%--<asp:CompareValidator ID="ddlSec_DivClass_ComV" runat="server" Display="Dynamic"
                                                                                ControlToValidate="ddlSec_DivClass" ErrorMessage="Required" Font-Size="10pt" ForeColor="Crimson"
                                                                                ValueToCompare="-1" Operator="NotEqual" ValidationGroup="education1"></asp:CompareValidator>--%>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style_td">GPA/Score <span class="spanAsterisk">*</span></td>
                                                        <td>
                                                            <asp:TextBox ID="txtSec_CgpaScore" runat="server" Width="100%" CssClass="form-control" Enabled="false"></asp:TextBox>
                                                            <asp:RangeValidator ID="txtSec_CgpaScore_RV" runat="server" ControlToValidate="txtSec_CgpaScore"
                                                                ErrorMessage="GPA must be between 1.0 - 5.0" ForeColor="Crimson" Display="Dynamic" 
                                                                ValidationGroup="education1" MaximumValue="5" MinimumValue="0" Type="Double"></asp:RangeValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style_td">GPA Without 4th Subject </td>
                                                        <td>
                                                            <asp:TextBox ID="txtSec_CgpaW4S" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                            <asp:RangeValidator ID="txtSec_CgpaW4S_RV" runat="server" ControlToValidate="txtSec_CgpaW4S"
                                                                ErrorMessage="GPA (without 4th subject) must be between 1.0 - 5.0" ForeColor="Crimson" Display="Dynamic" 
                                                                ValidationGroup="education1" MaximumValue="5" MinimumValue="0" Type="Double"></asp:RangeValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style_td">Total Obtained Marks <span class="spanAsterisk"></span><br />
                                                            <span style="color: cornflowerblue; font-size: 9pt; font-weight: normal; display: none">For older marking system</span></td>
                                                        <td>
                                                            <asp:TextBox ID="txtSec_Marks" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                      <tr id="trOutofMarksForOLevelSSC" runat="server">
                                                        <td class="style_td">Out of<br />
                                                            <span style="color: cornflowerblue; font-size: 9pt; font-weight: normal; display: none">For older marking system</span></td>
                                                        <td>
                                                            <asp:TextBox ID="txtOutofSec_Marks" runat="server" Width="100%" CssClass="form-control" placeholer="Required for Masters candidates"></asp:TextBox>

                                                            <%--<asp:LinkButton ID="lnkAddResult" runat="server" OnClick="lnkAddResult_Click" CssClass="blink">Add Result</asp:LinkButton>--%>

                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style_td">Exam Year <span class="spanAsterisk">*</span></td>
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
                                                <table style="width: 100%" class="table table-condensed table-striped">
                                                    <tr>
                                                        <td style="width: 30%" class="style_td">Exam Type <span class="spanAsterisk">*</span></td>
                                                        <td style="width: 70%">
                                                            <asp:DropDownList ID="ddlHigherSec_ExamType" runat="server" Width="85%" CssClass="form-control"></asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style_td">Education Board <span class="spanAsterisk">*</span></td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlHigherSec_EducationBrd" runat="server" Width="85%" CssClass="form-control"></asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style_td">Institute <span class="spanAsterisk">*</span></td>
                                                        <td>
                                                            <asp:TextBox ID="txtHigherSec_Institute" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style_td">Roll Number <span class="spanAsterisk">*</span></td>
                                                        <td>
                                                            <asp:TextBox ID="txtHigherSec_RollNo" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr style="display: none">
                                                        <td class="style_td">Registration Number</td>
                                                        <td>
                                                            <asp:TextBox ID="txtHigherSec_RegNo" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style_td">Group Or Subject <span class="spanAsterisk">*</span></td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlHigherSec_GrpOrSub" runat="server" Width="85%" CssClass="form-control"></asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style_td">Division/Class <span class="spanAsterisk">*</span></td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlHigherSec_DivClass" runat="server" Width="85%" CssClass="form-control"
                                                                AutoPostBack="true" OnSelectedIndexChanged="ddlHigherSec_DivClass_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style_td">GPA/Score <span class="spanAsterisk">*</span></td>
                                                        <td>
                                                            <asp:TextBox ID="txtHigherSec_CgpaScore" runat="server" Width="100%" CssClass="form-control" Enabled="false"></asp:TextBox>
                                                            <asp:RangeValidator ID="txtHigherSec_CgpaScore_RV" runat="server" ControlToValidate="txtHigherSec_CgpaScore"
                                                                ErrorMessage="GPA must be between 1.0 - 5.0" ForeColor="Crimson" Display="Dynamic" 
                                                                ValidationGroup="education1" MaximumValue="5" MinimumValue="0" Type="Double"></asp:RangeValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style_td">GPA Without 4th Subject </td>
                                                        <td>
                                                            <asp:TextBox ID="txtHigherSec_GpaW4S" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                            <asp:RangeValidator ID="txtHigherSec_GpaW4S_RV" runat="server" ControlToValidate="txtHigherSec_GpaW4S"
                                                                ErrorMessage="GPA (without 4th subject) must be between 1.0 - 5.0" ForeColor="Crimson" Display="Dynamic" 
                                                                ValidationGroup="education1" MaximumValue="5" MinimumValue="0" Type="Double"></asp:RangeValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style_td">Total Obtained Marks <span class="spanAsterisk"></span><br />
                                                            <span style="color: cornflowerblue; font-size: 9pt; font-weight: normal; display: none">For older marking system</span></td>
                                                        <td>
                                                            <asp:TextBox ID="txtHigherSec_Marks" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr id="trOutofMarksForOLevelHSC" runat="server">
                                                        <td class="style_td">Out of<br />
                                                            <span style="color: cornflowerblue; font-size: 9pt; font-weight: normal; display: none">For older marking system</span></td>
                                                        <td>
                                                            <asp:TextBox ID="txtOutofHigherSec_Marks" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                            <%--<asp:LinkButton ID="lnkAddALevelResult" runat="server" OnClick="lnkAddALevelResult_Click" CssClass="blink">Add Result</asp:LinkButton>--%>
                                                        
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style_td">Exam Year <span class="spanAsterisk">*</span></td>
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
                            <asp:HiddenField ID="hfIsUndergrad" runat="server" />
                            <asp:Panel ID="panel_isUndergrad" runat="server">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="col-md-6">
                                            <div class="panel panel-default">
                                                <div class="panel-heading style_thead">
                                                    Bachelor<br />
                                                    <small>(Only for Candidates applying for Masters Program)</small>
                                                </div>
                                                <div class="panel-body panelBody_edu_marginBottom">
                                                    <table style="width: 100%" class="table table-condensed table-striped">
                                                        <tr>
                                                            <td style="width: 30%" class="style_td">Institute <span class="spanAsterisk">*</span></td>
                                                            <td style="width: 70%">
                                                                <asp:TextBox ID="txtUndergrad_Institute" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="style_td">Program/Degree <span class="spanAsterisk">*</span></td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlUndergrad_ProgramDegree" runat="server" Width="85%" CssClass="form-control"
                                                                    AutoPostBack="true" OnSelectedIndexChanged="ddlUndergrad_ProgramDegree_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="style_td">Others <span class="spanAsterisk">*</span></td>
                                                            <td>
                                                                <asp:TextBox ID="txtUndergrad_ProgOthers" runat="server" Width="100%" CssClass="form-control" Enabled="false"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr style="display: none">
                                                            <td class="style_td">Group Or Subject <span class="spanAsterisk">*</span></td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlUndergrad_GrpOrSub" runat="server" Width="85%" CssClass="form-control"></asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="style_td">Division/Class <span class="spanAsterisk">*</span></td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlUndergrad_DivClass" runat="server" Width="85%" CssClass="form-control"
                                                                    AutoPostBack="true" OnSelectedIndexChanged="ddlUndergrad_DivClass_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="style_td">CGPA/Score</td>
                                                            <td>
                                                                <asp:TextBox ID="txtUndergrad_CgpaScore" runat="server" Width="100%" CssClass="form-control" Enabled="false"></asp:TextBox>
                                                                <asp:RangeValidator ID="txtUndergrad_CgpaScore_RV" runat="server" ControlToValidate="txtUndergrad_CgpaScore"
                                                                    MinimumValue="1" MaximumValue="4" Display="Dynamic" ErrorMessage="Invalid number"
                                                                    ForeColor="Crimson" Type="Double" ValidationGroup="education1"></asp:RangeValidator>

                                                            </td>
                                                        </tr>
                                                        <%--<tr>
                                                            <td class="style_td">Grade <span class="spanAsterisk">*</span></td>
                                                            <td>
                                                                <asp:TextBox ID="txtUndergrad_Grade" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                            </td>
                                                        </tr>--%>
                                                        <tr>
                                                            <td class="style_td">Exam Year <span class="spanAsterisk">*</span></td>
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
                                                    Masters<br />
                                                    <small>(Only for Candidates applying for Masters Program)</small>
                                                </div>
                                                <div class="panel-body panelBody_edu_marginBottom">
                                                    <table style="width: 100%" class="table table-condensed table-striped">
                                                        <tr>
                                                            <td style="width: 30%" class="style_td">Institute <span class="spanAsterisk">*</span></td>
                                                            <td style="width: 70%">
                                                                <asp:TextBox ID="txtGraduate_Institute" runat="server" CssClass="form-control"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="style_td">Program/Degree <span class="spanAsterisk">*</span></td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlGraduate_ProgramDegree" runat="server" Width="85%" CssClass="form-control"
                                                                    AutoPostBack="true" OnSelectedIndexChanged="ddlGraduate_ProgramDegree_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="style_td">Others <span class="spanAsterisk">*</span></td>
                                                            <td>
                                                                <asp:TextBox ID="txtGraduate_ProgOthers" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr style="display: none">
                                                            <td class="style_td">Group Or Subject <span class="spanAsterisk">*</span></td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlGraduate_GrpOrSub" runat="server" Width="85%" CssClass="form-control"></asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="style_td">Division/Class <span class="spanAsterisk">*</span></td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlGraduate_DivClass" runat="server" Width="85%" CssClass="form-control"
                                                                    AutoPostBack="true" OnSelectedIndexChanged="ddlGraduate_DivClass_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="style_td">CGPA/Score</td>
                                                            <td>
                                                                <asp:TextBox ID="txtGraduate_CgpaScore" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                                                <asp:RangeValidator ID="txtGraduate_CgpaScore_RV" runat="server" ControlToValidate="txtGraduate_CgpaScore"
                                                                    MinimumValue="1" MaximumValue="4" Display="Dynamic" ErrorMessage="Invalid number"
                                                                    ForeColor="Crimson" Type="Double" ValidationGroup="education1"></asp:RangeValidator>
                                                            </td>
                                                        </tr>
                                                        <%--<tr>
                                                            <td class="style_td">Grade <span class="spanAsterisk">*</span></td>
                                                            <td>
                                                                <asp:TextBox ID="txtGraduate_Grade" runat="server" CssClass="form-control"></asp:TextBox>
                                                            </td>
                                                        </tr>--%>
                                                        <tr>
                                                            <td class="style_td">Exam Year <span class="spanAsterisk">*</span></td>
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
                            </asp:Panel>

                            <asp:Panel ID="messagePanel_Education" runat="server">
                                <asp:Label ID="lblMessageEducation" runat="server" Text=""></asp:Label>
                            </asp:Panel>

                            <%--<asp:Button ID="btnSave_Education" runat="server" Text="Save"
                                CssClass="btn btn-primary" OnClick="btnSave_Education_Click"
                                ValidationGroup="education1" />

                            <asp:Button ID="btnNext" runat="server" Text="Next" Visible="false"
                                CssClass="btn btn-primary" />--%>

                            <span id="validationMsg" class="validationErrorMsg"></span>

                        </ContentTemplate>
                    </asp:UpdatePanel>

                </div>
                <%-- end panel body --%>
            </div>
            <%-- end panel default --%>
        </div>
        <%-- end col-md-12 --%>
    </div>
    <%-- end row --%>



    
<%--    <div class="col-md-15 col-lg-12">
        <asp:UpdatePanel ID="UpdatePanel9" runat="server">
            <ContentTemplate>

                <asp:Button ID="Button1" runat="server" Style="display: none" />
                <ajaxToolkit:ModalPopupExtender ID="modalPopupSubjectWiseResult" runat="server" TargetControlID="Button1" PopupControlID="Panel2"
                    BackgroundCssClass="modalBackground" CancelControlID="btnCancel">
                </ajaxToolkit:ModalPopupExtender>

                <asp:Panel runat="server" ID="Panel2" Style="display: none; padding: 5px; height: 500px; overflow: scroll; overflow-y: scroll"
                    BackColor="White" Width="70%">


                    <div class="panel panel-default">
                        <div class="panel-body">

                            <div class="row">
                                <div class="col-lg-12 col-md-12 col-sm-12" style="text-align: center; color: blue; font-weight: bold">
                                    <b>Subject wise result entry</b>
                                </div>
                                
                            </div>
                            <hr />


                            <div class="row">
                                <div class="col-lg-4 col-md-4 col-sm-4">
                                    <b>Number of subjects appeared</b>
                                    <asp:TextBox ID="txtOlevelSubjetNo" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="col-lg-2 col-md-2 col-sm-2">
                                    <br />
                                    <asp:LinkButton ID="lnkGenerateOlevel" runat="server" OnClick="lnkGenerateOlevel_Click" CssClass="form-control btn-info" Style="text-align: center">Generate</asp:LinkButton>

                                </div>

                            </div>

                            <div class="row" style="margin-top: 10px">
                                <div class="col-lg-12 col-md-12 col-sm-12">

                                    <asp:Panel ID="Panel1" runat="server">

                                        <asp:GridView runat="server" ID="gvOLevelSubjectResult" AutoGenerateColumns="False"
                                            AllowPaging="false" PagerSettings-Mode="NumericFirstLast"
                                            PageSize="20" CellPadding="4" Width="100%"
                                            ShowHeader="true" CssClass="table-bordered" ForeColor="#333333" GridLines="None">

                                            <HeaderStyle BackColor="#4781f0" ForeColor="White" Height="30" Font-Bold="True" />
                                            <FooterStyle BackColor="#4781f0" ForeColor="White" Height="30" Font-Bold="True" />
                                            <AlternatingRowStyle BackColor="White" />

                                            <Columns>
                                                <asp:TemplateField HeaderText="SL#">
                                                    <ItemTemplate>
                                                        <b><%# Container.DataItemIndex + 1 %></b>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="5%" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Subject">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSubjectNo" runat="server" Text='<%#Eval("SubjectNo") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="lblOLevelSubject" runat="server" Text='<%# "Subject " +Eval("SubjectNo") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="left" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Subject Total">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtOLevelMark" runat="server" TextMode="Number" min="1" CssClass="form-control"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="left" Width="10%" />
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <div style="text-align: center">

                                                            <asp:LinkButton ID="lnkOLevelSave" runat="server" OnClick="lnkOLevelSave_Click"
                                                                CssClass="form-control btn-success" Style="text-align: center">Calculate</asp:LinkButton>
                                                            <br />
                                                            <asp:Label ID="lblTitle" runat="server" Text="Obtained Grade"></asp:Label>
                                                        </div>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="ddlOLevelGrade" runat="server" CssClass="form-control">
                                                            <asp:ListItem Selected="True" Text="Select" Value="0"></asp:ListItem>
                                                            <asp:ListItem Text="A*" Value="90"></asp:ListItem>
                                                            <asp:ListItem Text="A" Value="80"></asp:ListItem>
                                                            <asp:ListItem Text="B" Value="70"></asp:ListItem>
                                                            <asp:ListItem Text="C" Value="60"></asp:ListItem>
                                                            <asp:ListItem Text="D" Value="50"></asp:ListItem>
                                                            <asp:ListItem Text="E" Value="40"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="left" Width="15%" />
                                                </asp:TemplateField>
                                            </Columns>

                                        </asp:GridView>

                                    </asp:Panel>

                                </div>
                            </div>

                              <div class="row" style="margin-top:5px">
                                 <div class="col-lg-4 col-md-4 col-sm-4">
                                </div>
                                <div class="col-lg-2 col-md-2 col-sm-2">
                                    <asp:LinkButton ID="btnCancel" runat="server" CssClass="form-control btn-danger" Style="text-align: center">Close</asp:LinkButton>

                                </div>
                            </div>

                        </div>
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>


        <div class="col-md-15 col-lg-12">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>

                <asp:Button ID="Button2" runat="server" Style="display: none" />
                <ajaxToolkit:ModalPopupExtender ID="modalPopupSubjectWiseResultALevel" runat="server" TargetControlID="Button2" PopupControlID="Panel3"
                    BackgroundCssClass="modalBackground" CancelControlID="LinkButton2">
                </ajaxToolkit:ModalPopupExtender>

                <asp:Panel runat="server" ID="Panel3" Style="display: none; padding: 5px; height: 500px; overflow: scroll; overflow-y: scroll"
                    BackColor="White" Width="70%">


                    <div class="panel panel-default">
                        <div class="panel-body">

                            <div class="row">
                                <div class="col-lg-12 col-md-12 col-sm-12" style="text-align: center; color: blue; font-weight: bold">
                                    <b>Subject wise result entry</b>
                                </div>
                                
                            </div>
                            <hr />


                            <div class="row">
                                <div class="col-lg-4 col-md-4 col-sm-4">
                                    <b>Number of subjects appeared</b>
                                    <asp:TextBox ID="txtAlevelSubjetNo" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="col-lg-2 col-md-2 col-sm-2">
                                    <br />
                                     <asp:LinkButton ID="lnkGenerateAlevel" runat="server" OnClick="lnkGenerateAlevel_Click" CssClass="form-control btn-info" Style="text-align: center">Generate</asp:LinkButton>

                                </div>

                            </div>

                            <div class="row" style="margin-top: 10px">
                                <div class="col-lg-12 col-md-12 col-sm-12">

                                    <asp:Panel ID="Panel4" runat="server">

                                        <asp:GridView runat="server" ID="gvALevelSubjectResult" AutoGenerateColumns="False"
                                            AllowPaging="false" PagerSettings-Mode="NumericFirstLast"
                                            PageSize="20" CellPadding="4" Width="100%"
                                            ShowHeader="true" CssClass="table-bordered" ForeColor="#333333" GridLines="None">

                                            <HeaderStyle BackColor="#4781f0" ForeColor="White" Height="30" Font-Bold="True" />
                                            <FooterStyle BackColor="#4781f0" ForeColor="White" Height="30" Font-Bold="True" />
                                            <AlternatingRowStyle BackColor="White" />

                                            <Columns>
                                                <asp:TemplateField HeaderText="SL#">
                                                    <ItemTemplate>
                                                        <b><%# Container.DataItemIndex + 1 %></b>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="5%" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Subject">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblASubjectNo" runat="server" Text='<%#Eval("SubjectNo") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="lblALevelSubject" runat="server" Text='<%# "Subject " +Eval("SubjectNo") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="left" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Subject Total">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtALevelMark" runat="server" TextMode="Number" min="1" CssClass="form-control"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="left" Width="10%" />
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <div style="text-align: center">

                                                            <asp:LinkButton ID="lnkALevelSave" runat="server" OnClick="lnkALevelSave_Click"
                                                                CssClass="form-control btn-success" Style="text-align: center">Calculate</asp:LinkButton>
                                                            <br />
                                                            <asp:Label ID="lblATitle" runat="server" Text="Obtained Grade"></asp:Label>
                                                        </div>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="ddlALevelGrade" runat="server" CssClass="form-control">
                                                            <asp:ListItem Selected="True" Text="Select" Value="0"></asp:ListItem>
                                                            <asp:ListItem Text="A*" Value="90"></asp:ListItem>
                                                            <asp:ListItem Text="A" Value="80"></asp:ListItem>
                                                            <asp:ListItem Text="B" Value="70"></asp:ListItem>
                                                            <asp:ListItem Text="C" Value="60"></asp:ListItem>
                                                            <asp:ListItem Text="D" Value="50"></asp:ListItem>
                                                            <asp:ListItem Text="E" Value="40"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="left" Width="15%" />
                                                </asp:TemplateField>
                                            </Columns>

                                        </asp:GridView>

                                    </asp:Panel>

                                </div>
                            </div>

                              <div class="row" style="margin-top:5px">
                                 <div class="col-lg-4 col-md-4 col-sm-4">
                                </div>
                                <div class="col-lg-2 col-md-2 col-sm-2">
                                    <asp:LinkButton ID="LinkButton2" runat="server" CssClass="form-control btn-danger" Style="text-align: center">Close</asp:LinkButton>

                                </div>
                            </div>

                        </div>
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>--%>

    <ajaxToolkit:UpdatePanelAnimationExtender ID="UpdatePanelAnimationExtender1" TargetControlID="updatePanelEducation" runat="server">
        <Animations>
            <OnUpdating>
                <Parallel duration="0">
                    <ScriptAction Script="InProgress();" />
                    <EnableAction AnimationTarget="btnSave_Education" Enabled="false" />
                    <EnableAction AnimationTarget="btnNext" Enabled="false" />
                </Parallel>
            </OnUpdating>
            <OnUpdated>
                <Parallel duration="0">
                    <ScriptAction Script="onComplete();" />
                    <EnableAction   AnimationTarget="btnSave_Education" Enabled="true" />
                    <EnableAction   AnimationTarget="btnNext" Enabled="true" />
                </Parallel>
            </OnUpdated>
        </Animations>
    </ajaxToolkit:UpdatePanelAnimationExtender>

</asp:Content>
