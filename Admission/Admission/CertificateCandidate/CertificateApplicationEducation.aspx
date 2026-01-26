<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CertificateApplicationEducation.aspx.cs" Inherits="Admission.Admission.CertificateCandidate.CertificateApplicationEducation" %>




<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <link href="../../Content/ApplicationForm.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-heading panelHeaderHeight">
                    <ol class="breadcrumb">
                        <li><asp:HyperLink ID="hrefAppBasic" runat="server">Basic Info</asp:HyperLink></li>
                        <%--<li><asp:HyperLink ID="hrefAppPriority" runat="server">Program Priority</asp:HyperLink></li>--%>
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
                    <asp:HiddenField ID="hfEduCat" runat="server" />


                    <div style="margin-bottom: 15px; margin-left: 20px;">
                        <span class="spanAsterisk">*</span> indicate required fields.<br />
                        <%--<span class="spanAsterisk">Please note that there is no validation for Admin in this form.</span>--%>
                    </div>


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
                                                        <td class="style_td">Marks <span class="spanAsterisk"></span><br />
                                                            <span style="color: cornflowerblue; font-size: 9pt; font-weight: normal; display: none">For older marking system</span></td>
                                                        <td>
                                                            <asp:TextBox ID="txtSec_Marks" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
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
                                                        <td class="style_td">Marks <span class="spanAsterisk"></span><br />
                                                            <span style="color: cornflowerblue; font-size: 9pt; font-weight: normal; display: none">For older marking system</span></td>
                                                        <td>
                                                            <asp:TextBox ID="txtHigherSec_Marks" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
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

                            <asp:Button ID="btnSave_Education" runat="server" Text="Save"
                                CssClass="btn btn-primary" OnClick="btnSave_Education_Click"
                                ValidationGroup="education1" />

                            <asp:Button ID="btnNext" runat="server" Text="Next" Visible="false"
                                CssClass="btn btn-primary" />

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



</asp:Content>
